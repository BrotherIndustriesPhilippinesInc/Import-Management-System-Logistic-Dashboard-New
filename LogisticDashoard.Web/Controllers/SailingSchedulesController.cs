using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LogisticDashboard.Core;
using LogisticDashboard.Web.Data;

namespace LogisticDashboard.Web.Controllers
{
    public class SailingSchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SailingSchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SailingSchedules
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: SailingSchedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sailingSchedule = await _context.SailingSchedule
                .Include(s => s.Route)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sailingSchedule == null)
            {
                return NotFound();
            }

            return View(sailingSchedule);
        }

        // GET: SailingSchedules/Create
        public IActionResult Create()
        {
            ViewData["RouteId"] = new SelectList(_context.Set<Routes>(), "Id", "From");
            return View();
        }

        // POST: SailingSchedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Week,Start,End,VesselName,VoyNo,Origin,OriginalETD,OriginalETAMNL,LatestETD,LatestETAMNL,TransitDays,DelayDeparture,DelayArrival,Remarks,RouteId")] SailingSchedule sailingSchedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sailingSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RouteId"] = new SelectList(_context.Set<Routes>(), "Id", "From", sailingSchedule.RouteId);
            return View(sailingSchedule);
        }

        // GET: SailingSchedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sailingSchedule = await _context.SailingSchedule.FindAsync(id);
            if (sailingSchedule == null)
            {
                return NotFound();
            }
            ViewData["RouteId"] = new SelectList(_context.Set<Routes>(), "Id", "From", sailingSchedule.RouteId);
            return View(sailingSchedule);
        }

        // POST: SailingSchedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Week,Start,End,VesselName,VoyNo,Origin,OriginalETD,OriginalETAMNL,LatestETD,LatestETAMNL,TransitDays,DelayDeparture,DelayArrival,Remarks,RouteId")] SailingSchedule sailingSchedule)
        {
            if (id != sailingSchedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sailingSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SailingScheduleExists(sailingSchedule.Id))
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
            ViewData["RouteId"] = new SelectList(_context.Set<Routes>(), "Id", "From", sailingSchedule.RouteId);
            return View(sailingSchedule);
        }

        // GET: SailingSchedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sailingSchedule = await _context.SailingSchedule
                .Include(s => s.Route)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sailingSchedule == null)
            {
                return NotFound();
            }

            return View(sailingSchedule);
        }

        // POST: SailingSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sailingSchedule = await _context.SailingSchedule.FindAsync(id);
            if (sailingSchedule != null)
            {
                _context.SailingSchedule.Remove(sailingSchedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SailingScheduleExists(int id)
        {
            return _context.SailingSchedule.Any(e => e.Id == id);
        }
    }
}
