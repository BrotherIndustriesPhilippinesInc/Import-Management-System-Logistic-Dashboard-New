using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LogisticDashboard.Core;
using LogisticDashboard.Web.Data;
using LogisticDashboard.Web.DTO;

namespace LogisticDashboard.Web.Controllers
{
    public class CourierInformationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourierInformationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CourierInformations
        public async Task<IActionResult> Index()
        {
            var couriers = await _context.CourierInformation.ToListAsync();
            var dhl = await _context.DHL.FirstOrDefaultAsync(); // only one DHL record

            var combined = new CourierInformationDHLCombined
            {
                DeliveryInformation = couriers,
                Dhl = dhl
            };

            return View(combined);
        }


        // GET: CourierInformations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courierInformation = await _context.CourierInformation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courierInformation == null)
            {
                return NotFound();
            }

            return View(courierInformation);
        }

        // GET: CourierInformations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CourierInformations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string courierName, IFormFile courierImage, string bIPH_Account_No)
        {
            if (courierImage == null || courierImage.Length == 0)
            {
                ModelState.AddModelError("CourierImage", "Please upload an image.");
                return View();
            }

            // Save file first
            var fileName = Path.GetFileName(courierImage.FileName);
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resources", "courier");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await courierImage.CopyToAsync(stream);
            }

            var courierInformation = new CourierInformation
            {
                CourierName = courierName,
                CourierImage = "resources/courier/" + fileName,
                BIPH_Account_No = bIPH_Account_No,
                LastUpdated = DateTime.UtcNow,
                LastUpdateBy = "Admin"
            };

            _context.Add(courierInformation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: CourierInformations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courierInformation = await _context.CourierInformation.FindAsync(id);
            if (courierInformation == null)
            {
                return NotFound();
            }
            return View(courierInformation);
        }

        // POST: CourierInformations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourierInformation courierInformation)
        {

            if (id != courierInformation.Id)
            {
                return NotFound();
            }

            courierInformation.LastUpdated = DateTime.UtcNow;
            courierInformation.LastUpdateBy = "Admin";

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courierInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourierInformationExists(courierInformation.Id))
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
            return View(courierInformation);
        }

        // GET: CourierInformations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courierInformation = await _context.CourierInformation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courierInformation == null)
            {
                return NotFound();
            }

            return View(courierInformation);
        }

        // POST: CourierInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courierInformation = await _context.CourierInformation.FindAsync(id);
            if (courierInformation != null)
            {
                _context.CourierInformation.Remove(courierInformation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourierInformationExists(int id)
        {
            return _context.CourierInformation.Any(e => e.Id == id);
        }
    }
}
