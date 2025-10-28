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
    public class DHLsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DHLsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DHLs
        public async Task<IActionResult> Index()
        {
            return View(await _context.DHL.ToListAsync());
        }

        // GET: DHLs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dHL = await _context.DHL
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dHL == null)
            {
                return NotFound();
            }

            return View(dHL);
        }

        // GET: DHLs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DHLs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile ImageLink)
        {
            if (ImageLink == null || ImageLink.Length == 0)
            {
                ModelState.AddModelError("CourierImage", "Please upload an image.");
                return View();
            }

            // Save file first
            var fileName = Path.GetFileName(ImageLink.FileName);
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resources", "dhl");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ImageLink.CopyToAsync(stream);
            }

            var dhlInformation = new DHL
            {
                ImageLink = "resources/dhl/" + fileName,
                LastUpdated = DateTime.UtcNow,
                LastUpdateBy = "Admin"
            };

            _context.Add(dhlInformation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: DHLs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dHL = await _context.DHL.FindAsync(id);
            if (dHL == null)
            {
                return NotFound();
            }
            return View(dHL);
        }

        // POST: DHLs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImageLink,LastUpdated,LastUpdateBy")] DHL dHL)
        {
            if (id != dHL.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dHL);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DHLExists(dHL.Id))
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
            return View(dHL);
        }

        // GET: DHLs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dHL = await _context.DHL
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dHL == null)
            {
                return NotFound();
            }

            return View(dHL);
        }

        // POST: DHLs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dHL = await _context.DHL.FindAsync(id);
            if (dHL != null)
            {
                _context.DHL.Remove(dHL);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DHLExists(int id)
        {
            return _context.DHL.Any(e => e.Id == id);
        }
    }
}
