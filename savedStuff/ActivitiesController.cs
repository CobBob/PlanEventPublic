using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlanEvent.Data;
using PlanEvent.InputModels;
using PlanEvent.ViewModels;

namespace PlanEvent.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly EventDbContext _context;

        public ActivitiesController(EventDbContext context)
        {
            _context = context;
        }

        // GET: Activities/Create
        public IActionResult ActivityCreate()
        {
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivityCreate([Bind("ActivityId,Name,Description")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ActivityEdit), "Activities", new { id = activity.ActivityId });
            }
            return View(activity);
        }


        // GET: Activities/Edit/5
        public async Task<IActionResult> ActivityEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            var viewModel = new ActivityEditViewModel()
            {
                ActivityId = activity.ActivityId,
                Name = activity.Name,
                Description = activity.Description
            };

            viewModel.ActivityTimeInvitees = await _context.ActivityTimeInvitees
                .Where(ati => ati.ActivityId == viewModel.ActivityId)
                .Select(ati => new ActivityTimeInvitee()
                {
                    TimeProposedId = ati.TimeProposedId,
                    InviteeId = ati.InviteeId
                }).ToListAsync();

            var inviteeKeys = viewModel.ActivityTimeInvitees
                .Where(ati => ati.InviteeId != null)
                .Select(ati => (int)ati.InviteeId)
                .Distinct()
                .ToList();

            Invitee tempInvitee;
            foreach (int key in inviteeKeys)
            {
                tempInvitee = _context.Invitees
                    .FirstOrDefault(i => i.InviteeId == key);
                viewModel.Invitees.Add(new Invitee()
                {
                    InviteeId = tempInvitee.InviteeId,
                    Name = tempInvitee.Name
                });
            }

            var timeKeys = viewModel.ActivityTimeInvitees
                .Where(ati => ati.TimeProposedId != null)
                .Select(ati => (int)ati.TimeProposedId)
                .Distinct()
                .ToList();

            TimeProposed tempTimeProposed;
            foreach (int key in timeKeys)
            {
                tempTimeProposed = _context.TimeProposeds
                    .FirstOrDefault(tp => tp.TimeProposedId == key);
                viewModel.TimeProposeds.Add(new TimeProposed()
                {
                    TimeProposedId = tempTimeProposed.TimeProposedId,
                    StartTime = tempTimeProposed.StartTime,
                    EndTime = tempTimeProposed.EndTime
                });
            }

            return View(viewModel);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivityEdit(int id, [Bind("ActivityId,Name,Description,ActivityTimeInviteeTableIds")]
        ActivityEditInputModel inputModel)
        {
            if (id != inputModel.ActivityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var activity = new Activity()
                    {
                        ActivityId = inputModel.ActivityId,
                        Name = inputModel.Name,
                        Description = inputModel.Description
                    };
                    _context.Activities.Update(activity);

                    //What are the ati's that are recieved form Post?
                    var activityTimeInviteesFromPost = new List<ActivityTimeInvitee>();

                    //Gather the ati's from Post
                    int tempInviteeId;
                    int tempTimeId;
                    for (int i = 0; i < inputModel.ActivityTimeInviteeTableIds.Length; i++)
                    {
                        if(TryParseStringActivityTimeInviteeTableIds(
                            inputModel.ActivityTimeInviteeTableIds[i],
                            out tempInviteeId,
                            out tempTimeId))
                        {
                            activityTimeInviteesFromPost.Add(new ActivityTimeInvitee()
                            {ActivityId = activity.ActivityId,
                            InviteeId = tempInviteeId,
                            TimeProposedId = tempTimeId});
                        }
                        else
                        {
                            return RedirectToAction();
                        }
                    }

                    //What are the ati's that are already in the database that connect a timeproposed whith an invitee.
                    var activityTimeInviteesInDb = await _context.ActivityTimeInvitees
                        .Where(ati => ati.ActivityId == inputModel.ActivityId
                        && !(ati.TimeProposedId == null || ati.InviteeId == null))
                        .ToListAsync();

                    //Remove all ati's from dataBase from above list that are not in the post 
                    for (int i = 0; i < activityTimeInviteesInDb.Count; i++)
                    {
                        if (!activityTimeInviteesFromPost.Exists(ati =>
                         ati.TimeProposedId == activityTimeInviteesInDb[i].TimeProposedId
                         && ati.InviteeId == activityTimeInviteesInDb[i].InviteeId))
                        {
                            _context.ActivityTimeInvitees.Remove(activityTimeInviteesInDb[i]);
                        }
                    }

                    //Add all ati's from the Post that are not already in the database
                    for (int i = 0; i < activityTimeInviteesFromPost.Count; i++)
                    {
                        if(!activityTimeInviteesInDb.Exists(ati => 
                        ati.TimeProposedId == activityTimeInviteesFromPost[i].TimeProposedId
                            && ati.InviteeId == activityTimeInviteesFromPost[i].InviteeId))
                        {
                            _context.ActivityTimeInvitees.Add(new ActivityTimeInvitee()
                            {
                                ActivityId = inputModel.ActivityId,
                                InviteeId = activityTimeInviteesFromPost[i].InviteeId,
                                TimeProposedId = activityTimeInviteesFromPost[i].TimeProposedId
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(inputModel.ActivityId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction();
            }
            return RedirectToAction(nameof(Index));

            //The function below is used to parse the string output from the POST back to keys
            bool TryParseStringActivityTimeInviteeTableIds(string str, out int InviteeIdnum, out int timeIdnum)
            {
                InviteeIdnum = 0;
                timeIdnum = 0;
                var sb = new StringBuilder();

                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] != '+')
                    {
                        sb.Append(str[i]);
                    }
                    else
                    {
                        if (int.TryParse(sb.ToString(),out InviteeIdnum))
                        {
                            sb.Clear();
                        }
                    }
                }
                int.TryParse(sb.ToString(), out timeIdnum);
                return !(InviteeIdnum == 0 || timeIdnum ==0);
            }
        }














        // GET: Activities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Activities.ToListAsync());
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActivityId,Name,Description")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activity);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActivityId,Name,Description")] Activity activity)
        {
            if (id != activity.ActivityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.ActivityId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(activity);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.ActivityId == id);
        }
    }
}
