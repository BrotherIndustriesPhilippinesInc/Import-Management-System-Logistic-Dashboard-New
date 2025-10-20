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
    public class IncotermsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IncotermsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Incoterms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Incoterms.OrderBy(i => i.Id).ToListAsync());
        }

        // GET: Incoterms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incoterms = await _context.Incoterms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incoterms == null)
            {
                return NotFound();
            }

            return View(incoterms);
        }

        // GET: Incoterms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Incoterms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Seller,OriginTruckings,OriginCustoms,OriginTerminalCharges,OceanFreightAirFreight,DestinationTerminalCharges,DestinationCustoms,DestinationTrucking,Buyer")] Incoterms incoterms)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incoterms);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(incoterms);
        }

        // GET: Incoterms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incoterms = await _context.Incoterms.FindAsync(id);
            if (incoterms == null)
            {
                return NotFound();
            }
            return View(incoterms);
        }

        // POST: Incoterms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Seller,OriginTruckings,OriginCustoms,OriginTerminalCharges,OceanFreightAirFreight,DestinationTerminalCharges,DestinationCustoms,DestinationTrucking,Buyer")] Incoterms incoterms)
        {
            if (id != incoterms.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incoterms);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncotermsExists(incoterms.Id))
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
            return View(incoterms);
        }

        // GET: Incoterms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incoterms = await _context.Incoterms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incoterms == null)
            {
                return NotFound();
            }

            return View(incoterms);
        }

        // POST: Incoterms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incoterms = await _context.Incoterms.FindAsync(id);
            if (incoterms != null)
            {
                _context.Incoterms.Remove(incoterms);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncotermsExists(int id)
        {
            return _context.Incoterms.Any(e => e.Id == id);
        }

        [HttpPost]
        public IActionResult UpdateRole(string fieldName, string fieldValue)
        {
            // Extract the property and the item ID
            var parts = fieldName.Split('_');
            var property = parts[0]; // e.g., "Seller"
            var id = int.Parse(parts[1]);

            var item = _context.Incoterms.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();

            // Set the property dynamically
            switch (property)
            {
                case "Seller":
                    item.Seller = fieldValue;
                    break;
                case "OriginTruckings":
                    item.OriginTruckings = fieldValue;
                    break;
                case "OriginCustoms":
                    item.OriginCustoms = fieldValue;
                    break;
                case "OriginTerminalCharges":
                    item.OriginTerminalCharges = fieldValue;
                    break;
                case "OceanFreightAirFreight":
                    item.OceanFreightAirFreight = fieldValue;
                    break;
                case "DestinationTerminalCharges":
                    item.DestinationTerminalCharges = fieldValue;
                    break;
                case "DestinationCustoms":
                    item.DestinationCustoms = fieldValue;
                    break;
                case "DestinationTrucking":
                    item.DestinationTrucking = fieldValue;
                    break;
                case "Buyer":
                    item.Buyer = fieldValue;
                    break;
                default:
                    return BadRequest();
            }

            _context.SaveChanges();
            return Ok();
        }

    }
}
