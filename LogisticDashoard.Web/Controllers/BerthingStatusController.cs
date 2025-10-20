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
    public class BerthingStatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BerthingStatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BerthingStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.BerthingStatus.ToListAsync());
        }

        // GET: BerthingStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var berthingStatus = await _context.BerthingStatus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (berthingStatus == null)
            {
                return NotFound();
            }

            return View(berthingStatus);
        }

        // GET: BerthingStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BerthingStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PortName,Normal_Range_VesselsAtBerth,VesselsAtBerth,Normal_Range_VesselsAtAnchorage,VesselsAtAnchorage,Year,Date,LastUpdate")] BerthingStatus berthingStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(berthingStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(berthingStatus);
        }

        // GET: BerthingStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var berthingStatus = await _context.BerthingStatus.FindAsync(id);
            if (berthingStatus == null)
            {
                return NotFound();
            }
            return View(berthingStatus);
        }

        // POST: BerthingStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PortName,Normal_Range_VesselsAtBerth,VesselsAtBerth,Normal_Range_VesselsAtAnchorage,VesselsAtAnchorage,Year,Date,LastUpdate")] BerthingStatus berthingStatus)
        {
            if (id != berthingStatus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(berthingStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BerthingStatusExists(berthingStatus.Id))
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
            return View(berthingStatus);
        }

        // GET: BerthingStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var berthingStatus = await _context.BerthingStatus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (berthingStatus == null)
            {
                return NotFound();
            }

            return View(berthingStatus);
        }

        // POST: BerthingStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var berthingStatus = await _context.BerthingStatus.FindAsync(id);
            if (berthingStatus != null)
            {
                _context.BerthingStatus.Remove(berthingStatus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BerthingStatusExists(int id)
        {
            return _context.BerthingStatus.Any(e => e.Id == id);
        }
    }
}
