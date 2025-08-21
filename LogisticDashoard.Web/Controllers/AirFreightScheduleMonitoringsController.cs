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
    public class AirFreightScheduleMonitoringsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AirFreightScheduleMonitoringsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AirFreightScheduleMonitorings
        public async Task<IActionResult> Index()
        {
            return View(await _context.AirFreightScheduleMonitoring.ToListAsync());
        }

        // GET: AirFreightScheduleMonitorings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airFreightScheduleMonitoring = await _context.AirFreightScheduleMonitoring
                .FirstOrDefaultAsync(m => m.Id == id);
            if (airFreightScheduleMonitoring == null)
            {
                return NotFound();
            }

            return View(airFreightScheduleMonitoring);
        }

        // GET: AirFreightScheduleMonitorings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AirFreightScheduleMonitorings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemCategory,Shipper,AWB,Forwarder_Courier,Broker,Flight_Detail,Invoice_No,Freight_Term,No_Of_Pkgs,Original_ETD,ATD,Original_ETA,Latest_ETA,ATA,Flight_Status_Remarks,Import_Permit_Status,Have_Arrangement,With_Special_Permit,ATA_Port_BIPH_Leadtime,ETA_BIPH_Manual_Computation,Requested_Del_Date_To_Ship,Earliest_Shortage_Date,Actual_Del,Status,Import_Remarks,System_Update")] AirFreightScheduleMonitoring airFreightScheduleMonitoring)
        {
            if (ModelState.IsValid)
            {
                _context.Add(airFreightScheduleMonitoring);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(airFreightScheduleMonitoring);
        }

        // GET: AirFreightScheduleMonitorings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airFreightScheduleMonitoring = await _context.AirFreightScheduleMonitoring.FindAsync(id);
            if (airFreightScheduleMonitoring == null)
            {
                return NotFound();
            }
            return View(airFreightScheduleMonitoring);
        }

        // POST: AirFreightScheduleMonitorings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemCategory,Shipper,AWB,Forwarder_Courier,Broker,Flight_Detail,Invoice_No,Freight_Term,No_Of_Pkgs,Original_ETD,ATD,Original_ETA,Latest_ETA,ATA,Flight_Status_Remarks,Import_Permit_Status,Have_Arrangement,With_Special_Permit,ATA_Port_BIPH_Leadtime,ETA_BIPH_Manual_Computation,Requested_Del_Date_To_Ship,Earliest_Shortage_Date,Actual_Del,Status,Import_Remarks,System_Update")] AirFreightScheduleMonitoring airFreightScheduleMonitoring)
        {
            if (id != airFreightScheduleMonitoring.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(airFreightScheduleMonitoring);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirFreightScheduleMonitoringExists(airFreightScheduleMonitoring.Id))
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
            return View(airFreightScheduleMonitoring);
        }

        // GET: AirFreightScheduleMonitorings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airFreightScheduleMonitoring = await _context.AirFreightScheduleMonitoring
                .FirstOrDefaultAsync(m => m.Id == id);
            if (airFreightScheduleMonitoring == null)
            {
                return NotFound();
            }

            return View(airFreightScheduleMonitoring);
        }

        // POST: AirFreightScheduleMonitorings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var airFreightScheduleMonitoring = await _context.AirFreightScheduleMonitoring.FindAsync(id);
            if (airFreightScheduleMonitoring != null)
            {
                _context.AirFreightScheduleMonitoring.Remove(airFreightScheduleMonitoring);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AirFreightScheduleMonitoringExists(int id)
        {
            return _context.AirFreightScheduleMonitoring.Any(e => e.Id == id);
        }
    }
}
