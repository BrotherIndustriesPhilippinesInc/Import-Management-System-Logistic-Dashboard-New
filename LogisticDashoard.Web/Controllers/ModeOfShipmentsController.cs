using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LogisticDashboard.Core;
using LogisticDashboard.Web.Data;
using System.Net.Http.Headers;

namespace LogisticDashboard.Web.Controllers
{
    public class ModeOfShipmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModeOfShipmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ModeOfShipments
        public async Task<IActionResult> Index()
        {
            return View(await _context.ModeOfShipment.ToListAsync());
        }

        // GET: ModeOfShipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modeOfShipment = await _context.ModeOfShipment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modeOfShipment == null)
            {
                return NotFound();
            }

            return View(modeOfShipment);
        }

        // GET: ModeOfShipments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ModeOfShipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FCLProcessFlow_Image_Link,LCLProcessFlow_Image_Link,LastUpdated,LastUpdatedBy")] ModeOfShipment modeOfShipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modeOfShipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modeOfShipment);
        }

        // GET: ModeOfShipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modeOfShipment = await _context.ModeOfShipment.FindAsync(id);
            if (modeOfShipment == null)
            {
                return NotFound();
            }
            return View(modeOfShipment);
        }

        // POST: ModeOfShipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FCLProcessFlow_Image_Link,LCLProcessFlow_Image_Link,LastUpdated,LastUpdatedBy")] ModeOfShipment modeOfShipment)
        {
            if (id != modeOfShipment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modeOfShipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModeOfShipmentExists(modeOfShipment.Id))
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
            return View(modeOfShipment);
        }

        // GET: ModeOfShipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modeOfShipment = await _context.ModeOfShipment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modeOfShipment == null)
            {
                return NotFound();
            }

            return View(modeOfShipment);
        }

        // POST: ModeOfShipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modeOfShipment = await _context.ModeOfShipment.FindAsync(id);
            if (modeOfShipment != null)
            {
                _context.ModeOfShipment.Remove(modeOfShipment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModeOfShipmentExists(int id)
        {
            return _context.ModeOfShipment.Any(e => e.Id == id);
        }

        

    }
}
