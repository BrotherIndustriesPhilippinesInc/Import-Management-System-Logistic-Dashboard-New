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
    public class BerthingStatusController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public BerthingStatusController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/BerthingStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BerthingStatus>>> GetBerthingStatus()
        {
            return await _context.BerthingStatus.ToListAsync();
        }

        // GET: api/BerthingStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BerthingStatus>> GetBerthingStatus(int id)
        {
            var berthingStatus = await _context.BerthingStatus.FindAsync(id);

            if (berthingStatus == null)
            {
                return NotFound();
            }

            return berthingStatus;
        }

        // PUT: api/BerthingStatus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBerthingStatus(int id, BerthingStatus berthingStatus)
        {
            if (id != berthingStatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(berthingStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BerthingStatusExists(id))
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

        // POST: api/BerthingStatus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostBerthingStatus([FromBody] BerthingRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.PortName))
                return BadRequest("Berth Name is required.");

            // Check if existing na
            bool exists = await _context.BerthingStatus
                .AnyAsync(b => b.PortName == request.PortName && b.Year == request.CalendarYear);

            if (exists)
                return Conflict(new { message = $"Records for {request.PortName} in {request.CalendarYear} already exist." });

            var start = new DateTime(request.CalendarYear, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var daysInYear = DateTime.IsLeapYear(request.CalendarYear) ? 366 : 365;

            var list = Enumerable.Range(0, daysInYear)
                                 .Select(i => new BerthingStatus
                                 {
                                     Year = request.CalendarYear,
                                     PortName = request.PortName,
                                     Date = start.AddDays(i),
                                     LastUpdate = DateTime.UtcNow
                                 })
                                 .ToList();

            _context.BerthingStatus.AddRange(list);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"{list.Count} records created for {request.CalendarYear} - {request.PortName}" });
        }

        // DELETE: api/BerthingStatus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBerthingStatus(int id)
        {
            var berthingStatus = await _context.BerthingStatus.FindAsync(id);
            if (berthingStatus == null)
            {
                return NotFound();
            }

            _context.BerthingStatus.Remove(berthingStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BerthingStatusExists(int id)
        {
            return _context.BerthingStatus.Any(e => e.Id == id);
        }

        [HttpGet("SpecificBerth/{name}/{year}")]
        public async Task<ActionResult<BerthingStatus>> GetBerth([FromRoute] string name, [FromRoute] int year)
        {
            var berth = await _context.BerthingStatus
                .Where(b => b.PortName == name && b.Year == year)
                .ToListAsync();

            if (berth == null)
                return NotFound(new { status = 404, message = "Berth not found." });

            return Ok(berth);
        }

        [HttpPost("update/{id}")]
        public async Task<IActionResult> PutBerth(int id, BerthingStatus berthing)
        {
            if (id != berthing.Id)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            var berthingStatus = await _context.BerthingStatus.FindAsync(id);
            if (berthingStatus == null)
            {
                return NotFound();
            }

            // Map DTO -> Entity
            berthingStatus.VesselsAtBerth = berthing.VesselsAtBerth;

            berthingStatus.VesselsAtAnchorage = berthing.VesselsAtAnchorage;

            berthingStatus.LastUpdate = DateTime.UtcNow;


            await _context.SaveChangesAsync();

            return Ok(new { status = 200 });
        }

        [HttpGet("ByCurrentWeek/{calendarYear}/{portName}")]
        public async Task<ActionResult<IEnumerable<BerthingStatus>>> GetBerthsByCurrentWeek(
        int calendarYear, string portName)
        {
            if (calendarYear == 0)
            {
                return BadRequest(new { status = 400, message = "Calendar Year is required." });
            }

            // Get start (Monday) and end (Sunday) of the current week
            var today = DateTime.UtcNow.Date;
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var weekStart = today.AddDays(-1 * diff);          // Monday
            var weekEnd = weekStart.AddDays(6);                // Sunday

            // Query berthing records only within this week
            var ports = await _context.BerthingStatus
                .Where(p => p.Year == calendarYear
                         && p.PortName == portName
                         && p.Date >= weekStart && p.Date <= weekEnd)
                .ToListAsync();

            if (ports == null || !ports.Any())
                return NotFound(new { status = 404, message = "No data for current week." });

            return Ok(ports);
        }

        [HttpGet("distinct-ports")]
        public async Task<ActionResult<IEnumerable<PortsYearsDTO>>> GetDistinctPortsWithYears()
        {
            var ports = await _context.BerthingStatus
                .GroupBy(p => p.PortName)
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
    }
}
