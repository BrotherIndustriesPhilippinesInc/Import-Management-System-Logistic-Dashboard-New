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
    public class ContainerVisualizationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContainerVisualizationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContainerVisualizations
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContainerVisualization.ToListAsync());
        }

        // GET: ContainerVisualizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var containerVisualization = await _context.ContainerVisualization
                .FirstOrDefaultAsync(m => m.Id == id);
            if (containerVisualization == null)
            {
                return NotFound();
            }

            return View(containerVisualization);
        }

        // GET: ContainerVisualizations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContainerVisualizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Container_Image_Link,LastUpdated,LastUpdatedBy")] ContainerVisualization containerVisualization)
        {
            if (ModelState.IsValid)
            {
                _context.Add(containerVisualization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(containerVisualization);
        }

        // GET: ContainerVisualizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var containerVisualization = await _context.ContainerVisualization.FindAsync(id);
            if (containerVisualization == null)
            {
                return NotFound();
            }
            return View(containerVisualization);
        }

        // POST: ContainerVisualizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Container_Image_Link,LastUpdated,LastUpdatedBy")] ContainerVisualization containerVisualization)
        {
            if (id != containerVisualization.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(containerVisualization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContainerVisualizationExists(containerVisualization.Id))
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
            return View(containerVisualization);
        }

        // GET: ContainerVisualizations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var containerVisualization = await _context.ContainerVisualization
                .FirstOrDefaultAsync(m => m.Id == id);
            if (containerVisualization == null)
            {
                return NotFound();
            }

            return View(containerVisualization);
        }

        // POST: ContainerVisualizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var containerVisualization = await _context.ContainerVisualization.FindAsync(id);
            if (containerVisualization != null)
            {
                _context.ContainerVisualization.Remove(containerVisualization);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContainerVisualizationExists(int id)
        {
            return _context.ContainerVisualization.Any(e => e.Id == id);
        }
    }
}
