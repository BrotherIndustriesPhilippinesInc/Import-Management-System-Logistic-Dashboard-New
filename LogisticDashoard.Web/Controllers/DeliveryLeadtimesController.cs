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
    public class DeliveryLeadtimesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeliveryLeadtimesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DeliveryLeadtimes
        public async Task<IActionResult> Index()
        {
            var deliveryLeadtimes = await _context.DeliveryLeadtime.ToListAsync();
            var deliveryLeadtimeData = await _context.DeliveryLeadtimeData.ToListAsync();

            var combinedModel = new DeliveryLeadtimeCombined
            {
                DeliveryLeadtime = deliveryLeadtimes,
                DeliveryLeadtimeData = deliveryLeadtimeData
            };

            return View(combinedModel);
        }



        // GET: DeliveryLeadtimes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryLeadtime = await _context.DeliveryLeadtime
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deliveryLeadtime == null)
            {
                return NotFound();
            }

            return View(deliveryLeadtime);
        }

        // GET: DeliveryLeadtimes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DeliveryLeadtimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("CreateLeadtime")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeliveryLeadtime deliveryLeadtime)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deliveryLeadtime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deliveryLeadtime);
        }

        // GET: DeliveryLeadtimes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryLeadtime = await _context.DeliveryLeadtime.FindAsync(id);
            if (deliveryLeadtime == null)
            {
                return NotFound();
            }
            return View(deliveryLeadtime);
        }

        // POST: DeliveryLeadtimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Year,Carrier,OriginPort,DestinationPort,VesselTransitLeadtime,CustomsClearanceLeadtime,TotalLeadtime")] DeliveryLeadtime deliveryLeadtime)
        {
            if (id != deliveryLeadtime.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliveryLeadtime);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryLeadtimeExists(deliveryLeadtime.Id))
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
            return View(deliveryLeadtime);
        }

        // GET: DeliveryLeadtimes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryLeadtime = await _context.DeliveryLeadtime
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deliveryLeadtime == null)
            {
                return NotFound();
            }

            return View(deliveryLeadtime);
        }

        // POST: DeliveryLeadtimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deliveryLeadtime = await _context.DeliveryLeadtime.FindAsync(id);
            if (deliveryLeadtime != null)
            {
                _context.DeliveryLeadtime.Remove(deliveryLeadtime);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveryLeadtimeExists(int id)
        {
            return _context.DeliveryLeadtime.Any(e => e.Id == id);
        }
    }
}
