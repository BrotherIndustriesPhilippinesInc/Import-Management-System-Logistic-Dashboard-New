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
    public class PortUtilizationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PortUtilizationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PortUtilizations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PortUtilization.Include(p => p.Port);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PortUtilizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portUtilization = await _context.PortUtilization
                .Include(p => p.Port)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (portUtilization == null)
            {
                return NotFound();
            }

            return View(portUtilization);
        }

        // GET: PortUtilizations/Create
        public IActionResult Create()
        {
            ViewData["PortId"] = new SelectList(_context.Set<Ports>(), "Id", "Name");
            return View();
        }

        // POST: PortUtilizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PortId,Week,Overall_Yard_Utilization,Vessels_At_Berth,Vessels_At_Anchorage_Waiting,Last_Update")] PortUtilization portUtilization)
        {
            if (ModelState.IsValid)
            {
                _context.Add(portUtilization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PortId"] = new SelectList(_context.Set<Ports>(), "Id", "Name", portUtilization.PortId);
            return View(portUtilization);
        }

        // GET: PortUtilizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portUtilization = await _context.PortUtilization.FindAsync(id);
            if (portUtilization == null)
            {
                return NotFound();
            }
            ViewData["PortId"] = new SelectList(_context.Set<Ports>(), "Id", "Name", portUtilization.PortId);
            return View(portUtilization);
        }

        // POST: PortUtilizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PortId,Week,Overall_Yard_Utilization,Vessels_At_Berth,Vessels_At_Anchorage_Waiting,Last_Update")] PortUtilization portUtilization)
        {
            if (id != portUtilization.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(portUtilization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PortUtilizationExists(portUtilization.Id))
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
            ViewData["PortId"] = new SelectList(_context.Set<Ports>(), "Id", "Name", portUtilization.PortId);
            return View(portUtilization);
        }

        // GET: PortUtilizations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portUtilization = await _context.PortUtilization
                .Include(p => p.Port)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (portUtilization == null)
            {
                return NotFound();
            }

            return View(portUtilization);
        }

        // POST: PortUtilizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var portUtilization = await _context.PortUtilization.FindAsync(id);
            if (portUtilization != null)
            {
                _context.PortUtilization.Remove(portUtilization);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PortUtilizationExists(int id)
        {
            return _context.PortUtilization.Any(e => e.Id == id);
        }
    }
}
