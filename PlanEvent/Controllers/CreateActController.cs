using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlanEvent.Data;
using PlanEvent.ViewModels;
using PlanEvent.InputModels;

namespace PlanEvent.Controllers
{
    public class CreateActController : Controller
    {
        //private readonly EventDbContext _context;
        //public CreateActController(EventDbContext context)
        //{
        //    _context = context;
        //}

        private readonly IRepository _repository;

        public CreateActController(IRepository repository)
        {
            _repository = repository;
        }

        // GET:
        public IActionResult CreateOrganiser()
        {
            if(!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(RedirectToLogin));
            }

            return View(new FullActivityViewModel());
        }

        // POST:
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrganiser([Bind("OrganiserName")] CreateActOrganiserInputModel InputModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(
                    nameof(CreateActivity),
                    new { InputModel.OrganiserName });
            }

            return View();
        }


        public IActionResult CreateActivity(string organiserName)
        {
            return View(new FullActivityViewModel()
            {
                OrganiserName = organiserName
            });
        }

        // POST:
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateActivity([Bind("OrganiserName,Description,ActivityName")] CreateActActivityInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(CreateTime), new FullActivityViewModel()
                {
                    OrganiserName = inputModel.OrganiserName,
                    ActivityName = inputModel.ActivityName,
                    Description = inputModel.Description,
                    StartTime = DateTime.Today,
                    EndTime = DateTime.Today
                });
            }

            return View(new FullActivityViewModel()
            {
                OrganiserName = inputModel.OrganiserName
            });
        }

        // GET:
        public IActionResult CreateTime(FullActivityViewModel viewModel)
        {
            return View(viewModel);
        }

        // POST:
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTime([Bind("OrganiserName,Description,ActivityName,StartTime,EndTime")] CreateActAllInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                if(!this.User.Identity.IsAuthenticated)
                {
                    return RedirectToAction(nameof(RedirectToLogin));
                }

                var newAti = new ActivityTimeInvitee()
                {
                    Activity = new Activity()
                    {
                        Name = inputModel.ActivityName,
                        Description = inputModel.Description,
                        AccountName = this.User.Identity.Name,
                        Guid = Guid.NewGuid()
                    },
                    Invitee = new Invitee()
                    {
                        Name = inputModel.OrganiserName
                    },
                    TimeProposed = new TimeProposed()
                    {
                        StartTime = inputModel.StartTime,
                        EndTime = inputModel.EndTime
                    }
                };

                _repository.InsertActivityTimeInvitee(newAti);

                return RedirectToAction(nameof(EditActController.MainEdit),"EditAct",new { guid = newAti.Activity.Guid });
            }

            return View(new FullActivityViewModel()
            {
                OrganiserName = inputModel.OrganiserName,
                ActivityName = inputModel.ActivityName,
                Description = inputModel.Description,
                StartTime = DateTime.Today,
                EndTime = DateTime.Today
            });
        }
        public IActionResult RedirectToLogin()
        {
            return View();
        }
    }
}