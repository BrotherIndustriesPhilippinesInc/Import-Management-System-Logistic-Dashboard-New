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
    public class VesselRouteMapsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VesselRouteMapsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VesselRouteMaps
        public async Task<IActionResult> Index()
        {
            return View(await _context.VesselRouteMap.ToListAsync());
        }

        [HttpGet("VesselRouteMaps/RouteView/{id?}")]
        public async Task<IActionResult> RouteView(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vesselRouteMap = await _context.VesselRouteMap
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vesselRouteMap == null)
            {
                return NotFound();
            }

            return View(vesselRouteMap);
        }

        // GET: VesselRouteMaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vesselRouteMap = await _context.VesselRouteMap
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vesselRouteMap == null)
            {
                return NotFound();
            }

            return View(vesselRouteMap);
        }

        // GET: VesselRouteMaps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VesselRouteMaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VesselRouteMap vesselRouteMap)

        {
            if (ModelState.IsValid)
            {
                vesselRouteMap.CreatedDate = DateTime.UtcNow;
                _context.Add(vesselRouteMap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vesselRouteMap);
        }

        // GET: VesselRouteMaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vesselRouteMap = await _context.VesselRouteMap.FindAsync(id);
            if (vesselRouteMap == null)
            {
                return NotFound();
            }
            return View(vesselRouteMap);
        }

        // POST: VesselRouteMaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VesselRouteMap vesselRouteMap)
        {
            if (id != vesselRouteMap.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vesselRouteMap.LastUpdate = DateTime.UtcNow;
                    _context.Update(vesselRouteMap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VesselRouteMapExists(vesselRouteMap.Id))
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
            return View(vesselRouteMap);
        }

        // GET: VesselRouteMaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vesselRouteMap = await _context.VesselRouteMap
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vesselRouteMap == null)
            {
                return NotFound();
            }

            return View(vesselRouteMap);
        }

        // POST: VesselRouteMaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vesselRouteMap = await _context.VesselRouteMap.FindAsync(id);
            if (vesselRouteMap != null)
            {
                _context.VesselRouteMap.Remove(vesselRouteMap);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool VesselRouteMapExists(int id)
        {
            return _context.VesselRouteMap.Any(e => e.Id == id);
        }

        [HttpGet("VesselRouteMaps/GetVesselRouteMap/{id?}")]
        public async Task<IActionResult> GetVesselRouteMap(int id)
        {
            var data = await _context.VesselRouteMap
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return Json(data);
        }

        [HttpGet("VesselRouteMaps/GetAllVesselRouteMap")]
        public async Task<IActionResult> GetAllVesselRouteMap(int id)
        {
            var data = await _context.VesselRouteMap.ToListAsync();

            return Json(data);
        }

    }
}
