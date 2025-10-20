using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogisticDashboard.API.Data;
using LogisticDashboard.Core;
using LogisticDashboard.API.DTO;

namespace LogisticDashboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortUtilizationsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public PortUtilizationsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/PortUtilizations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PortUtilization>>> GetPortUtilization()
        {
            return await _context.PortUtilization.ToListAsync();
        }

        // GET: api/PortUtilizations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PortUtilization>> GetPortUtilization(int id)
        {
            var portUtilization = await _context.PortUtilization.FindAsync(id);

            if (portUtilization == null)
            {
                return NotFound();
            }

            return portUtilization;
        }

        // PUT: api/PortUtilizations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPortUtilization(int id, PortUtilization portUtilization)
        {
            if (id != portUtilization.Id)
            {
                return BadRequest();
            }

            _context.Entry(portUtilization).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PortUtilizationExists(id))
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

        // POST: api/PortUtilizations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PortUtilization>> PostPortUtilization(PortUtilization portUtilization)
        {
            _context.PortUtilization.Add(portUtilization);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPortUtilization", new { id = portUtilization.Id }, portUtilization);
        }

        // DELETE: api/PortUtilizations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePortUtilization(int id)
        {
            var portUtilization = await _context.PortUtilization.FindAsync(id);
            if (portUtilization == null)
            {
                return NotFound();
            }

            _context.PortUtilization.Remove(portUtilization);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PortUtilizationExists(int id)
        {
            return _context.PortUtilization.Any(e => e.Id == id);
        }


        [HttpPost("update/{id}")]
        public async Task<IActionResult> PutPort(int id, PortUtilizationNoPortBackDTO port)
        {
            if (id != port.Id)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            var portUtil = await _context.PortUtilization.FindAsync(id);
            if (port == null)
            {
                return NotFound();
            }

            // Map DTO -> Entity
            portUtil.Week = port.Week;
            portUtil.StartDate = port.StartDate.HasValue
                ? DateTime.SpecifyKind(port.StartDate.Value, DateTimeKind.Utc)
                : null;

            portUtil.EndDate = port.EndDate.HasValue
                ? DateTime.SpecifyKind(port.EndDate.Value, DateTimeKind.Utc)
                : null;
            portUtil.Overall_Yard_Utilization = port.Overall_Yard_Utilization;
            portUtil.Vessels_At_Berth = port.Vessels_At_Berth;
            portUtil.Vessels_At_Anchorage_Waiting = port.Vessels_At_Anchorage_Waiting;
            portUtil.Last_Update = DateTime.UtcNow;


            await _context.SaveChangesAsync();

            return Ok(new { status = 200 });
        }
    }
}
