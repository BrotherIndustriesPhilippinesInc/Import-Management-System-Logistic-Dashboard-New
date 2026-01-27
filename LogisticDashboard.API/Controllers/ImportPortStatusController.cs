using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogisticDashboard.API.Data;
using LogisticDashboard.Core;
using LogisticDashboard.Web.DTO;

namespace LogisticDashboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportPortStatusController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public ImportPortStatusController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/ImportPortStatus
        [HttpGet]
        public async Task<IActionResult> GetPortStatuses()
        {
            // Use AsNoTracking() because we are only reading data, not modifying it here.
            // This is a critical optimization in EF Core.
            var ports = await _context.ImportPortStatus
                .AsNoTracking()
                .Select(p => new PortStatusUpdateDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    PortUtilizationStatus = p.PortUtilizationStatus, // e.g., "Normal"
                    BerthingStatus = p.BerthingStatus // e.g., "Critical"
                })
                .ToListAsync();

            return Ok(ports);
        }

        // GET: api/ImportPortStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImportPortStatus>> GetImportPortStatus(int id)
        {
            var importPortStatus = await _context.ImportPortStatus.FindAsync(id);

            if (importPortStatus == null)
            {
                return NotFound();
            }

            return importPortStatus;
        }

        // PUT: api/ImportPortStatus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImportPortStatus(int id, ImportPortStatus importPortStatus)
        {
            if (id != importPortStatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(importPortStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImportPortStatusExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ImportPortStatus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] List<PortStatusUpdateDTO> portUpdates)
        {
            if (portUpdates == null || !portUpdates.Any())
            {
                return BadRequest("No data received.");
            }

            foreach (var update in portUpdates)
            {
                // 1. Find the existing port in DB
                var port = await _context.ImportPortStatus.FindAsync(update.Id);

                if (port != null)
                {
                    // 2. Update properties
                    // NOTE: In a real app, use an Enum for Status, not magic strings like "Normal"
                    port.PortUtilizationStatus = update.PortUtilizationStatus;
                    port.BerthingStatus = update.BerthingStatus;
                    port.LastUpdated = DateTime.Now; // Good practice
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Port statuses updated successfully", data = portUpdates });
        }

        // DELETE: api/ImportPortStatus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImportPortStatus(int id)
        {
            var importPortStatus = await _context.ImportPortStatus.FindAsync(id);
            if (importPortStatus == null)
            {
                return NotFound();
            }

            _context.ImportPortStatus.Remove(importPortStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImportPortStatusExists(int id)
        {
            return _context.ImportPortStatus.Any(e => e.Id == id);
        }
    }
}
