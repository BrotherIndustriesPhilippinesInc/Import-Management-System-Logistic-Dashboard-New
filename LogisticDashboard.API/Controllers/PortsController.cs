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
using System.Globalization;

namespace LogisticDashboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public PortsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/Ports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PortsDTO>>> GetPorts()
        {
            var ports = await _context.Ports
                .Include(p => p.PortUtilizations)
                .Select(p => new PortsDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Year = p.Year,
                    Normal_Range_Overall_Yard_Utilization = p.Normal_Range_Overall_Yard_Utilization,
                    Normal_Range_Vessels_At_Berth = p.Normal_Range_Vessels_At_Berth,
                    Normal_Range_Vessels_At_Anchorage = p.Normal_Range_Vessels_At_Anchorage,
                    PortUtilizations = p.PortUtilizations.Select(pu => new PortUtilizationNoPortBackDTO
                    {
                        Id = pu.Id,
                        PortId = pu.PortId,
                        Week = pu.Week,
                        Overall_Yard_Utilization = pu.Overall_Yard_Utilization,
                        Vessels_At_Berth = pu.Vessels_At_Berth,
                        Vessels_At_Anchorage_Waiting = pu.Vessels_At_Anchorage_Waiting,
                        Last_Update = pu.Last_Update
                    }).ToList()
                })
                .ToListAsync();

            return Ok(ports);
        }

        // GET: api/Ports/5
        [HttpGet("SpecificPorts/{name}/{year}")]
        public async Task<ActionResult<PortsDTO>> GetPorts([FromRoute] string name, [FromRoute] int year)
        {
            var port = await _context.Ports
                .Include(p => p.PortUtilizations)
                .Where(p => p.Name == name)
                .Where(p => p.Year == year)
                .Select(p => new PortsDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Year = p.Year,
                    Normal_Range_Overall_Yard_Utilization = p.Normal_Range_Overall_Yard_Utilization,
                    Normal_Range_Vessels_At_Berth = p.Normal_Range_Vessels_At_Berth,
                    Normal_Range_Vessels_At_Anchorage = p.Normal_Range_Vessels_At_Anchorage,
                    PortUtilizations = p.PortUtilizations.Select(pu => new PortUtilizationNoPortBackDTO
                    {
                        Id = pu.Id,
                        PortId = pu.PortId,
                        Week = pu.Week,
                        Overall_Yard_Utilization = pu.Overall_Yard_Utilization,
                        Vessels_At_Berth = pu.Vessels_At_Berth,
                        Vessels_At_Anchorage_Waiting = pu.Vessels_At_Anchorage_Waiting,
                        Last_Update = pu.Last_Update,
                        EndDate = pu.EndDate,
                        StartDate = pu.StartDate

                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (port == null)
                return NotFound(new { status = 404, message = "Port not found." });

            return Ok(port);
        }


        // PUT: api/Ports/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPorts(int id, Ports ports)
        {
            if (id != ports.Id)
            {
                return BadRequest();
            }

            _context.Entry(ports).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PortsExists(id))
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

        // POST: api/Ports
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PortsNoUtilizationDTO>> PostPorts(PortsNoUtilizationDTO ports)
        {
            if (PortsExists(ports.Id))
            {
                return BadRequest();
            }

            var newPort = new Ports
            {
                Name = ports.Name,
                Year = ports.Year,
                Normal_Range_Overall_Yard_Utilization = ports.Normal_Range_Overall_Yard_Utilization,
                Normal_Range_Vessels_At_Berth = ports.Normal_Range_Vessels_At_Berth,
                Normal_Range_Vessels_At_Anchorage = ports.Normal_Range_Vessels_At_Anchorage
            };

            _context.Ports.Add(newPort);
            await _context.SaveChangesAsync();

            //Adding of 52 weeks onto the new route
            for (int i = 1; i <= 52; i++)
            {
                var newPortUtilization = new PortUtilization()
                {
                    PortId = newPort.Id,
                    Week = i,
                    Overall_Yard_Utilization = ports.Normal_Range_Overall_Yard_Utilization,
                    Vessels_At_Berth = ports.Normal_Range_Vessels_At_Berth,
                    Vessels_At_Anchorage_Waiting = ports.Normal_Range_Vessels_At_Anchorage,
                    Last_Update = DateTime.UtcNow
                };
                _context.PortUtilization.Add(newPortUtilization);
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPorts", new { id = newPort.Id }, ports);
        }

        // DELETE: api/Ports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePorts(int id)
        {
            var ports = await _context.Ports.FindAsync(id);
            if (ports == null)
            {
                return NotFound();
            }

            _context.Ports.Remove(ports);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PortsExists(int id)
        {
            return _context.Ports.Any(e => e.Id == id);
        }

        [HttpGet("distinct-ports")]
        public async Task<ActionResult<IEnumerable<PortsYearsDTO>>> GetDistinctPortsWithYears()
        {
            var ports = await _context.Ports
                .GroupBy(p => p.Name)
                .Select(g => new PortsYearsDTO
                {
                    Name = g.Key,
                    Years = g.Select(r => r.Year)
                             .Distinct()
                             .OrderBy(y => y)
                             .ToList()
                })
                .ToListAsync();

            return Ok(ports);
        }

        [HttpGet("ByCurrentWeeks/{calendarYear}/{portName}")]
        public async Task<ActionResult<PortsDTO>> GetPortsByCurrentWeeks(
            int calendarYear, string portName)
        {
            if (calendarYear == 0)
            {
                return BadRequest(new { status = 400, message = "Fiscal Year are required." });
            }

            // Get current ISO week number
            var today = DateTime.Now;
            int currentWeek = ISOWeek.GetWeekOfYear(today); // e.g., 36
            int startWeek = currentWeek - 4;
            if (startWeek < 1) startWeek = 1;

            var ports = await _context.Ports
                .Include(p => p.PortUtilizations)
                .Where(p => p.Year == calendarYear && p.Name == portName)
                .FirstOrDefaultAsync();

            if (ports == null)
                return NotFound();

            // Process schedules in-memory
            var utilizations = ports.PortUtilizations
                .Where(u => u.Week >= startWeek && u.Week <= currentWeek)
                .Select(s =>
                {
                    return new PortUtilizationNoPortBackDTO
                    {
                        Id = s.Id,
                        PortId = s.PortId,
                        Week = s.Week,
                        Overall_Yard_Utilization = s.Overall_Yard_Utilization,
                        Vessels_At_Berth = s.Vessels_At_Berth,
                        Vessels_At_Anchorage_Waiting = s.Vessels_At_Anchorage_Waiting,
                        Last_Update = s.Last_Update,
                        EndDate = s.EndDate,
                        StartDate = s.StartDate
                    };
                })
                .OrderBy(s => s.Id)
                .ToList();

            var dto = new PortsDTO
            {
                Name = ports.Name,
                Year = ports.Year,
                Normal_Range_Overall_Yard_Utilization = ports.Normal_Range_Overall_Yard_Utilization,
                Normal_Range_Vessels_At_Berth = ports.Normal_Range_Vessels_At_Berth,
                Normal_Range_Vessels_At_Anchorage = ports.Normal_Range_Vessels_At_Anchorage,
                PortUtilizations = utilizations
            };

            return Ok(dto);
        }
    }
}
