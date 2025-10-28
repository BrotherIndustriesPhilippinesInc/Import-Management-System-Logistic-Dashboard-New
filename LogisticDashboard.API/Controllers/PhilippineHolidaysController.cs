using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogisticDashboard.API.Data;
using LogisticDashboard.Core;

namespace LogisticDashboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhilippineHolidaysController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public PhilippineHolidaysController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/PhilippineHolidays
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhilippineHolidays>>> GetPhilippineHolidays()
        {
            return await _context.PhilippineHolidays.ToListAsync();
        }

        // GET: api/PhilippineHolidays/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhilippineHolidays>> GetPhilippineHolidays(int id)
        {
            var philippineHolidays = await _context.PhilippineHolidays.FindAsync(id);

            if (philippineHolidays == null)
            {
                return NotFound();
            }

            return philippineHolidays;
        }

        // PUT: api/PhilippineHolidays/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhilippineHolidays(int id, PhilippineHolidays philippineHolidays)
        {
            if (id != philippineHolidays.Id)
            {
                return BadRequest();
            }

            _context.Entry(philippineHolidays).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhilippineHolidaysExists(id))
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

        // POST: api/PhilippineHolidays
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PhilippineHolidays>> PostPhilippineHolidays(PhilippineHolidays philippineHolidays)
        {
            _context.PhilippineHolidays.Add(philippineHolidays);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPhilippineHolidays", new { id = philippineHolidays.Id }, philippineHolidays);
        }

        // DELETE: api/PhilippineHolidays/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhilippineHolidays(int id)
        {
            var philippineHolidays = await _context.PhilippineHolidays.FindAsync(id);
            if (philippineHolidays == null)
            {
                return NotFound();
            }

            _context.PhilippineHolidays.Remove(philippineHolidays);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PhilippineHolidaysExists(int id)
        {
            return _context.PhilippineHolidays.Any(e => e.Id == id);
        }
    }
}
