using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlanEvent.Data;
using PlanEvent.InputModels;
using PlanEvent.ViewModels;

namespace PlanEvent.Controllers
{
    public class InviteesController : Controller
    {
        private readonly EventDbContext _context;

        public InviteesController(EventDbContext context)
        {
            _context = context;
        }

        // GET: Invitees/Create
        public IActionResult ActivityCreate(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View(new ActivityCreateInviteeViewModel()
            {
                ActivityId = (int)Id
            });
        }

        // POST: Invitees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivityCreate([Bind("InviteeId,Name,ActivityId")] ActivityCreateInviteeInputModel InputModel)
        {
            if (ModelState.IsValid)
            {
                _context.Invitees.Add(new Invitee
                {
                    Name = InputModel.Name,
                    ActivityTimeInvitees = //new List<ActivityTimeInvitee>()
                    {
                        new ActivityTimeInvitee()
                        {
                            ActivityId = InputModel.ActivityId
                        }
                    }
                });


                //_context.ActivityTimeInvitees.Add(new ActivityTimeInvitee()
                //{
                //    ActivityId = InputModel.ActivityId,
                //    InviteeId = InputModel.InviteeId
                //});

                await _context.SaveChangesAsync();
                return RedirectToAction(
                    nameof(ActivitiesController.ActivityEdit),
                    "Activities",
                    new { id = InputModel.ActivityId });
            }

            return View(new ActivityCreateInviteeViewModel()
            {
                ActivityId = InputModel.ActivityId
            });
        }

        // GET: Invitees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitee = await _context.Invitees
                .FirstOrDefaultAsync(m => m.InviteeId == id);
            if (invitee == null)
            {
                return NotFound();
            }

            return View(invitee);
        }

        // POST: Invitees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invitee = await _context.Invitees.FindAsync(id);

            var activityId = _context.ActivityTimeInvitees
    .Where(ati => ati.InviteeId == id)
    .FirstOrDefault()
    .ActivityId;

            _context.Invitees.Remove(invitee);

            _context.ActivityTimeInvitees.RemoveRange(
                _context.ActivityTimeInvitees
                .Where(ati => ati.InviteeId == id));

            await _context.SaveChangesAsync();
            return RedirectToAction(
                    nameof(ActivitiesController.ActivityEdit),
                    "Activities",
                    new { id = activityId });
        }



















        // GET: Invitees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Invitees.ToListAsync());
        }

        // GET: Invitees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitee = await _context.Invitees
                .FirstOrDefaultAsync(m => m.InviteeId == id);
            if (invitee == null)
            {
                return NotFound();
            }

            return View(invitee);
        }

        // GET: Invitees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Invitees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InviteeId,Name")] Invitee invitee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invitee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(invitee);
        }

        // GET: Invitees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitee = await _context.Invitees.FindAsync(id);
            if (invitee == null)
            {
                return NotFound();
            }
            return View(invitee);
        }

        // POST: Invitees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InviteeId,Name")] Invitee invitee)
        {
            if (id != invitee.InviteeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invitee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InviteeExists(invitee.InviteeId))
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
            return View(invitee);
        }

        private bool InviteeExists(int id)
        {
            return _context.Invitees.Any(e => e.InviteeId == id);
        }
    }
}
