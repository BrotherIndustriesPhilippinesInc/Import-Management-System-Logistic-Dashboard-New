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
    public class PHPortMapsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PHPortMapsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PHPortMaps
        public async Task<IActionResult> Index()
        {
            return View(await _context.PHPortMap.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetPorts()
        {
            var ports = await _context.PHPortMap
                .Select(p => new {
                    p.Id,
                    p.Carrier,
                    p.SailingLeadtime,
                    p.OriginPort,
                    p.OriginPortCoordinates,
                    p.PictureLocation,
                    p.Description
                })
                .ToListAsync();

            return Json(ports);
        }

        // GET: PHPortMaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pHPortMap = await _context.PHPortMap
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pHPortMap == null)
            {
                return NotFound();
            }

            return View(pHPortMap);
        }

        // GET: PHPortMaps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PHPortMaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PHPortMap pHPortMap, IFormFile pictureFile)
        {
            if (ModelState.IsValid)
            {
                // Handle image upload
                if (pictureFile != null && pictureFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resources", "ports");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(pictureFile.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await pictureFile.CopyToAsync(stream);
                    }

                    pHPortMap.PictureLocation = Path.Combine("resources", "ports", uniqueFileName).Replace("\\", "/");
                }

                pHPortMap.CreatedDate = DateTime.UtcNow;
                _context.Add(pHPortMap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(pHPortMap);
        }



        // GET: PHPortMaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pHPortMap = await _context.PHPortMap.FindAsync(id);
            if (pHPortMap == null)
            {
                return NotFound();
            }
            return View(pHPortMap);
        }

        // POST: PHPortMaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PHPortMap pHPortMap, IFormFile pictureFile)
        {
            if (id != pHPortMap.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle picture upload
                    if (pictureFile != null && pictureFile.Length > 0)
                    {
                        // Ensure folder exists
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resources", "ports");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Create unique file name
                        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(pictureFile.FileName)}";
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        // Save file to wwwroot/resources/ports/
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await pictureFile.CopyToAsync(stream);
                        }

                        // Update the database field with relative path
                        pHPortMap.PictureLocation = $"resources/ports/{fileName}";
                    }

                    // Update CreatedDate or other properties if necessary
                    pHPortMap.LastUpdate = DateTime.UtcNow; // Optional if you track updates

                    _context.Update(pHPortMap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PHPortMapExists(pHPortMap.Id))
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

            return View(pHPortMap);
        }


        // GET: PHPortMaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pHPortMap = await _context.PHPortMap
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pHPortMap == null)
            {
                return NotFound();
            }

            return View(pHPortMap);
        }

        // POST: PHPortMaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pHPortMap = await _context.PHPortMap.FindAsync(id);
            if (pHPortMap != null)
            {
                _context.PHPortMap.Remove(pHPortMap);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PHPortMapExists(int id)
        {
            return _context.PHPortMap.Any(e => e.Id == id);
        }
    }
}
