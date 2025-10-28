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
    public class PhilippineHolidaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhilippineHolidaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PhilippineHolidays
        public async Task<IActionResult> Index()
        {
            return View(await _context.PhilippineHolidays.ToListAsync());
        }

        // GET: PhilippineHolidays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var philippineHolidays = await _context.PhilippineHolidays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (philippineHolidays == null)
            {
                return NotFound();
            }

            return View(philippineHolidays);
        }

        // GET: PhilippineHolidays/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PhilippineHolidays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhilippineHolidays philippineHolidays)
        {
            ModelState.Remove("LastUpdated");
            ModelState.Remove("LastUpdatedBy");

            if (ModelState.IsValid)
            {
                // ✅ Convert to UTC before saving
                philippineHolidays.Date = DateTime.SpecifyKind(philippineHolidays.Date, DateTimeKind.Local).ToUniversalTime();
                philippineHolidays.LastUpdated = DateTime.UtcNow;
                philippineHolidays.LastUpdatedBy = "Admin";

                _context.Add(philippineHolidays);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(philippineHolidays);
        }


        // GET: PhilippineHolidays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Console.WriteLine(id);
            if (id == null)
            {
                return NotFound();
            }

            var philippineHolidays = await _context.PhilippineHolidays.FindAsync(id);
            if (philippineHolidays == null)
            {
                return NotFound();
            }
            return View(philippineHolidays);
        }

        // POST: PhilippineHolidays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PhilippineHolidays philippineHolidays)
        {
            if (id != philippineHolidays.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // ✅ Ensure Date is UTC
                    if (philippineHolidays.Date.Kind == DateTimeKind.Unspecified)
                        philippineHolidays.Date = DateTime.SpecifyKind(philippineHolidays.Date, DateTimeKind.Utc);
                    else if (philippineHolidays.Date.Kind == DateTimeKind.Local)
                        philippineHolidays.Date = philippineHolidays.Date.ToUniversalTime();

                    // Optional audit fields
                    philippineHolidays.LastUpdated = DateTime.UtcNow;
                    philippineHolidays.LastUpdatedBy = "Admin";

                    _context.Update(philippineHolidays);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhilippineHolidaysExists(philippineHolidays.Id))
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

            return View(philippineHolidays);
        }


        // GET: PhilippineHolidays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var philippineHolidays = await _context.PhilippineHolidays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (philippineHolidays == null)
            {
                return NotFound();
            }

            return View(philippineHolidays);
        }

        // POST: PhilippineHolidays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var philippineHolidays = await _context.PhilippineHolidays.FindAsync(id);
            if (philippineHolidays != null)
            {
                _context.PhilippineHolidays.Remove(philippineHolidays);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhilippineHolidaysExists(int id)
        {
            return _context.PhilippineHolidays.Any(e => e.Id == id);
        }

        public async Task<IActionResult> GetHolidays()
        {
            return Json(await _context.PhilippineHolidays.ToListAsync());
        }
    }
}
