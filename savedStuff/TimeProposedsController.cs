using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlanEvent.Data;

namespace PlanEvent.Controllers
{
    public class TimeProposedsController : Controller
    {
        private readonly EventDbContext _context;

        public TimeProposedsController(EventDbContext context)
        {
            _context = context;
        }

        // GET: TimeProposeds
        public async Task<IActionResult> Index()
        {
            return View(await _context.TimeProposeds.ToListAsync());
        }

        // GET: TimeProposeds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeProposed = await _context.TimeProposeds
                .FirstOrDefaultAsync(m => m.TimeProposedId == id);
            if (timeProposed == null)
            {
                return NotFound();
            }

            return View(timeProposed);
        }

        // GET: TimeProposeds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TimeProposeds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TimeProposedId,StartTime,EndTime")] TimeProposed timeProposed)
        {
            if (ModelState.IsValid)
            {
                _context.Add(timeProposed);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(timeProposed);
        }

        // GET: TimeProposeds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeProposed = await _context.TimeProposeds.FindAsync(id);
            if (timeProposed == null)
            {
                return NotFound();
            }
            return View(timeProposed);
        }

        // POST: TimeProposeds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TimeProposedId,StartTime,EndTime")] TimeProposed timeProposed)
        {
            if (id != timeProposed.TimeProposedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeProposed);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeProposedExists(timeProposed.TimeProposedId))
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
            return View(timeProposed);
        }

        // GET: TimeProposeds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeProposed = await _context.TimeProposeds
                .FirstOrDefaultAsync(m => m.TimeProposedId == id);
            if (timeProposed == null)
            {
                return NotFound();
            }

            return View(timeProposed);
        }

        // POST: TimeProposeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timeProposed = await _context.TimeProposeds.FindAsync(id);
            _context.TimeProposeds.Remove(timeProposed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimeProposedExists(int id)
        {
            return _context.TimeProposeds.Any(e => e.TimeProposedId == id);
        }
    }
}
