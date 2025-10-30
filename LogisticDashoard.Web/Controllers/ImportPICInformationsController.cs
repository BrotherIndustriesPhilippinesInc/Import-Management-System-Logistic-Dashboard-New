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
    public class ImportPICInformationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImportPICInformationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ImportPICInformations
        public async Task<IActionResult> Index()
        {
            //data ordered by mode of shipment
            var data = await _context.ImportPICInformation
                .OrderByDescending(i => i.ModeOfShipment)
                .ToListAsync();
            return View(data);
        }

        // GET: ImportPICInformations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var importPICInformation = await _context.ImportPICInformation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (importPICInformation == null)
            {
                return NotFound();
            }

            return View(importPICInformation);
        }

        // GET: ImportPICInformations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ImportPICInformations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImportPICInformation importPICInformation)
        {
            if (ModelState.IsValid)
            {
                importPICInformation.CreatedDate = DateTime.UtcNow; // ✅ Set UTC for creation
                                                                    // UpdatedDate stays null on creation
                _context.Add(importPICInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(importPICInformation);
        }


        // GET: ImportPICInformations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var importPICInformation = await _context.ImportPICInformation.FindAsync(id);
            if (importPICInformation == null)
            {
                return NotFound();
            }
            return View(importPICInformation);
        }

        // POST: ImportPICInformations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModeOfShipment,Supplier,ShippingMainPICStaff,ShippingSubPICStaff,Supervisor,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] ImportPICInformation importPICInformation)
        {
            if (id != importPICInformation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(importPICInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImportPICInformationExists(importPICInformation.Id))
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
            return View(importPICInformation);
        }

        // GET: ImportPICInformations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var importPICInformation = await _context.ImportPICInformation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (importPICInformation == null)
            {
                return NotFound();
            }

            return View(importPICInformation);
        }

        // POST: ImportPICInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var importPICInformation = await _context.ImportPICInformation.FindAsync(id);
            if (importPICInformation != null)
            {
                _context.ImportPICInformation.Remove(importPICInformation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImportPICInformationExists(int id)
        {
            return _context.ImportPICInformation.Any(e => e.Id == id);
        }
    }
}
