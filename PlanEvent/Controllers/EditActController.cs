using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlanEvent.Data;
using PlanEvent.InputModels;
using PlanEvent.ViewModels;
using PlanEvent.HelperMethods;

namespace PlanEvent.Controllers
{
    public class EditActController : Controller
    {
        private readonly IRepository _repository;

        public EditActController(IRepository repository)
        {
            _repository = repository;
        }

        // GET:
        public async Task<IActionResult> Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                //TODO: a message?
                throw new NotImplementedException();
            }

            var activities = await _repository.GetActivities(this.User.Identity.Name);
            return View(activities);
        }

        // GET:
        public async Task<IActionResult> MainEdit(Guid? guid)
        {
            if (guid == null)
            {
                return NotFound();
            }

            var viewModel = await GuidToActivityEditViewModel((Guid)guid);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // POST:
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MainEdit(Guid guid, [Bind("Guid,Description,ActivityTimeInviteeTableIds")]
        ActivityEditInputModel inputModel)
        {
            if (guid != inputModel.Guid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _repository.UpdateActivityDescription(guid, inputModel.Description);

                //TODO: ensure that invitees cannot change other peoples availabilities by playing around with the HTTP post shennanigans
                //TODO: See above but then for description of activitee and organiser
                if (!await _repository.TryUpdateActivityAtis(inputModel.Guid, inputModel.ActivityTimeInviteeTableIds))
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(MainEdit), new { guid = inputModel.Guid });
            }

            var viewModel = GuidToActivityEditViewModel(guid);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // GET:
        public IActionResult AddTime(Guid? guid)
        {
            if (guid == null || !HasOrganiserPermission((Guid)guid))
            {
                return NotFound();
            }

            return View(new AddTimeViewModel()
            {
                Guid = (Guid)guid
            });
        }


        //TODO: Do I really need to to run some of these action asynchrously, especially since some of the tasks return void
        // POST:
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTime([Bind("Guid,StartTime,EndTime")] AddTimeInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                if (!HasOrganiserPermission(inputModel.Guid))
                {
                    return NotFound();
                }

                var newTimeProposed = new TimeProposed()
                {
                    StartTime = inputModel.StartTime,
                    EndTime = inputModel.EndTime
                };

                _repository.AddActivityTime(inputModel.Guid, newTimeProposed);

                return RedirectToAction(nameof(MainEdit), new { guid = inputModel.Guid });
            }

            if (inputModel == null)
            {
                return NotFound();
            }

            return View(new AddTimeViewModel()
            {
                Guid = inputModel.Guid
            });
        }

        // GET:
        public async Task<IActionResult> DeleteTime(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Get the activity that corresponds with the requested ID
            var guidActivity = _repository.GetGuidFromTimeId((int)id);

            if (!HasOrganiserPermission(guidActivity))
            {
                return NotFound();
            }

            //Check the number Timeproposeds in Activity
            var numberOfTimeProposeds = await _repository.NumberOfTimeProposeds(guidActivity);

            //TODO: maybe add a message that specifies the reason for trying to delete the activity
            if (numberOfTimeProposeds == 1)
            {
                return RedirectToAction(nameof(DeleteActivity), new { guid = guidActivity });
            }

            //Get the data to display
            var timeProposed = await _repository.GetTimeProposedFromTimeId((int)id);

            if (timeProposed == null)
            {
                return NotFound();
            }

            return View(new DeleteTimeViewModel()
            {
                Guid = guidActivity,
                TimeProposedId = timeProposed.TimeProposedId,
                StartTime = timeProposed.StartTime,
                EndTime = timeProposed.EndTime
            });
        }

        // POST:
        [HttpPost, ActionName("DeleteTime")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTimeConfirmed(int timeProposedId)
        {
            //TODO: What if the id is not in the database (entry is already removed)
            var timeProposed = await _repository.GetTimeProposedFromTimeId(timeProposedId);

            if (timeProposed == null)
            {
                return NotFound();
            }

            var guidActivity = _repository.GetGuidFromTimeId((int)timeProposedId);

            if (!HasOrganiserPermission(guidActivity))
            {
                return NotFound();
            }

            _repository.DeleteTimeProposed(timeProposed);

            return RedirectToAction(nameof(MainEdit), new { guid = guidActivity });
        }

        // GET:
        public IActionResult AddInvitee(Guid? guid)
        {
            if (guid == null)
            {
                return NotFound();
            }

            return View(new AddInviteeViewModel()
            {
                Guid = (Guid)guid
            });
        }

        // POST:
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInvitee([Bind("Guid,Name")] AddInviteeInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                //Is Guid valid?
                if (!await _repository.IsGuidValid(inputModel.Guid))
                {
                    return NotFound();
                }

                var invitee = new Invitee()
                {
                    Name = inputModel.Name
                };

                _repository.AddActivityInvitee(inputModel.Guid, invitee);

                if (!this.User.Identity.IsAuthenticated)
                {
                    Response.Cookies.Append(inputModel.Guid.ToString(), invitee.InviteeId.ToString(), new Microsoft.AspNetCore.Http.CookieOptions()
                    {
                        Expires = new DateTimeOffset(DateTime.Now.AddMonths(1))
                    });
                }

                return RedirectToAction(nameof(MainEdit), new { guid = inputModel.Guid });
            }

            if (inputModel == null)
            {
                return NotFound();
            }

            return View(new AddInviteeViewModel()
            {
                Guid = inputModel.Guid
            });
        }


        public async Task<IActionResult> DeleteInvitee(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guidActivity = _repository.GetGuidFromInviteeId((int)id);

            if (guidActivity == null)
            {
                return NotFound();
            }

            var numberOfInvitees = await _repository.NumberOfInvitees(guidActivity);

            //TODO: maybe add a message that specifies the reason for trying to delete the activity
            if (numberOfInvitees == 1)
            {
                return RedirectToAction(nameof(DeleteActivity), new { guid = guidActivity });
            }

            var invitee = await _repository.GetInviteeFromInviteeId((int)id);

            return View(new DeleteInviteeViewModel()
            {
                Guid = guidActivity,
                InviteeId = invitee.InviteeId,
                Name = invitee.Name
            });
        }

        // POST:
        [HttpPost, ActionName("DeleteInvitee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteInviteeConfirmed(int inviteeId)
        {
            var guidActivity = _repository.GetGuidFromInviteeId(inviteeId);

            if (guidActivity == null)
            {
                return NotFound();
            }

            _repository.DeleteInvitee(await _repository.GetInviteeFromInviteeId(inviteeId));

            return RedirectToAction(nameof(MainEdit), new { guid = guidActivity });
        }

        public async Task<IActionResult> DeleteActivity(Guid? guid)
        {
            if (guid == null)
            {
                return NotFound();
            }

            if (!HasOrganiserPermission((Guid)guid))
            {
                return NotFound();
            }

            var activity = await _repository.GuidToActivityAsync((Guid)guid);

            if (activity == null)
            {
                return NotFound();
            }

            return View(new DeleteActivityViewModel()
            {
                Guid = activity.Guid,
                ActivityName = activity.Name
            });
        }

        // POST:
        [HttpPost, ActionName("DeleteActivity")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteActivityConfirmed(Guid guid)
        {
            if (!HasOrganiserPermission(guid))
            {
                return NotFound();
            }

            _repository.DeleteActivity(guid);

            return RedirectToAction(nameof(Index), "Home");
        }

        private bool HasOrganiserPermission(Guid guid)
        {
            var activity = _repository.GuidToActivityAsync(guid).Result;

            if (activity == null)
            {
                return false;
            }

            return this.User.Identity.IsAuthenticated
                && activity.AccountName == this.User.Identity.Name;
        }

        private async Task<ActivityEditViewModel> GuidToActivityEditViewModel(Guid guid)
        {
            if (!await _repository.IsGuidValid(guid))
            {
                return null;
            }

            var atis = await _repository.GetATIsfromGuid(guid);

            var activity = atis.First().Activity;

            var viewModel = new ActivityEditViewModel()
            {
                Guid = activity.Guid,
                Name = activity.Name,
                Description = activity.Description,
                IsOrganiser = (this.User.Identity.IsAuthenticated
                                && this.User.Identity.Name == activity.AccountName)
            };

            foreach (var item in atis)
            {
                viewModel.ActivityTimeInvitees.Add(new ActivityTimeInvitee()
                {
                    TimeProposedId = item.TimeProposedId,
                    InviteeId = item.InviteeId,
                    Availability = item.Availability
                });
            }

            var invitees = atis
               .Select(ati => ati.Invitee)
               .Distinct()
               .ToList();

            foreach (var item in invitees)
            {
                viewModel.Invitees.Add(new Invitee()
                {
                    InviteeId = item.InviteeId,
                    Name = item.Name
                });
            }

            var timeProposeds = atis
                .Select(ati => ati.TimeProposed)
                .Distinct()
                .ToList();

            foreach (var item in timeProposeds)
            {
                viewModel.TimeProposeds.Add(new TimeProposed()
                {
                    TimeProposedId = item.TimeProposedId,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime
                });
            }

            return viewModel;
        }

        public ActionResult SendEmail(Guid guid)
        {
            if (guid == null || !HasOrganiserPermission(guid))
            {
                return NotFound();
            }

            var viewModel = new SendEmailViewModel()
            {
                Guid = guid
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendEmail(SendEmailInputModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Guid == null || !HasOrganiserPermission(model.Guid))
                {
                    return NotFound();
                }

                var activity = await _repository.GuidToActivityAsync(model.Guid);

                var link = $"{this.Request.Scheme}://{this.Request.Host.Value}/EditAct/{nameof(EditActController.MainEdit)}?guid={model.Guid}";

                var body = "<p>You have been invited to {0}</p>  <p>{1}</p> <p>{2}</p> <a href={3}>{3}</a>";

                var message = new MailMessage();
                message.To.Add(new MailAddress(model.ToEmail));  // replace with valid value 
                message.From = new MailAddress(Secrets.emailSending);  // replace with valid value
                message.Subject = "Your email subject";

                message.Body = string.Format(body, activity.Name,
                    activity.Description,
                    model.AdditionalTextMessage,
                    link);

                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = Secrets.emailSending,  // replace with valid value
                        Password = Secrets.emailSendingPassword // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp-mail.outlook.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
        }

        public ActionResult Sent()
        {
            return View();
        }

    }
}