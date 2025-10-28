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
    public class DeliveryLeadtimeDatasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeliveryLeadtimeDatasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DeliveryLeadtimeDatas
        public async Task<IActionResult> Index()
        {
            return View(await _context.DeliveryLeadtimeData.ToListAsync());
        }

        // GET: DeliveryLeadtimeDatas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryLeadtimeData = await _context.DeliveryLeadtimeData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deliveryLeadtimeData == null)
            {
                return NotFound();
            }

            return View(deliveryLeadtimeData);
        }

        // GET: DeliveryLeadtimeDatas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DeliveryLeadtimeDatas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LeadtimeName,Month,Actual_Ave,Target_Max,Target_Min,No_Of_BL,ActualFYAverage,TargetFYMax,TargetFYMin,NoOfBLFY")] DeliveryLeadtimeData deliveryLeadtimeData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deliveryLeadtimeData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deliveryLeadtimeData);
        }

        // GET: DeliveryLeadtimeDatas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryLeadtimeData = await _context.DeliveryLeadtimeData.FindAsync(id);
            if (deliveryLeadtimeData == null)
            {
                return NotFound();
            }
            return View(deliveryLeadtimeData);
        }

        // POST: DeliveryLeadtimeDatas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LeadtimeName,Month,Actual_Ave,Target_Max,Target_Min,No_Of_BL,ActualFYAverage,TargetFYMax,TargetFYMin,NoOfBLFY")] DeliveryLeadtimeData deliveryLeadtimeData)
        {
            if (id != deliveryLeadtimeData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliveryLeadtimeData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryLeadtimeDataExists(deliveryLeadtimeData.Id))
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
            return View(deliveryLeadtimeData);
        }

        // GET: DeliveryLeadtimeDatas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryLeadtimeData = await _context.DeliveryLeadtimeData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deliveryLeadtimeData == null)
            {
                return NotFound();
            }

            return View(deliveryLeadtimeData);
        }

        // POST: DeliveryLeadtimeDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deliveryLeadtimeData = await _context.DeliveryLeadtimeData.FindAsync(id);
            if (deliveryLeadtimeData != null)
            {
                _context.DeliveryLeadtimeData.Remove(deliveryLeadtimeData);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveryLeadtimeDataExists(int id)
        {
            return _context.DeliveryLeadtimeData.Any(e => e.Id == id);
        }
    }
}
