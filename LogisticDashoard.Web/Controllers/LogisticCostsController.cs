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
    public class LogisticCostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LogisticCostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LogisticCosts
        public async Task<IActionResult> Index()
        {
            return View(await _context.LogisticCost.ToListAsync());
        }

        // GET: LogisticCosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logisticCost = await _context.LogisticCost
                .FirstOrDefaultAsync(m => m.Id == id);
            if (logisticCost == null)
            {
                return NotFound();
            }

            return View(logisticCost);
        }

        // GET: LogisticCosts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LogisticCosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Value")] LogisticCost logisticCost)
        {
            if (ModelState.IsValid)
            {
                _context.Add(logisticCost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(logisticCost);
        }

        // GET: LogisticCosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logisticCost = await _context.LogisticCost.FindAsync(id);
            if (logisticCost == null)
            {
                return NotFound();
            }
            return View(logisticCost);
        }

        // POST: LogisticCosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Value")] LogisticCost logisticCost)
        {
            if (id != logisticCost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(logisticCost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogisticCostExists(logisticCost.Id))
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
            return View(logisticCost);
        }

        // GET: LogisticCosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logisticCost = await _context.LogisticCost
                .FirstOrDefaultAsync(m => m.Id == id);
            if (logisticCost == null)
            {
                return NotFound();
            }

            return View(logisticCost);
        }

        // POST: LogisticCosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var logisticCost = await _context.LogisticCost.FindAsync(id);
            if (logisticCost != null)
            {
                _context.LogisticCost.Remove(logisticCost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LogisticCostExists(int id)
        {
            return _context.LogisticCost.Any(e => e.Id == id);
        }
    }
}
