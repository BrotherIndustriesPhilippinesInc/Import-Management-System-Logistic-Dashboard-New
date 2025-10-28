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
    public class CourierInformationsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public CourierInformationsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/CourierInformations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourierInformation>>> GetCourierInformation()
        {
            return await _context.CourierInformation.ToListAsync();
        }

        // GET: api/CourierInformations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourierInformation>> GetCourierInformation(int id)
        {
            var courierInformation = await _context.CourierInformation.FindAsync(id);

            if (courierInformation == null)
            {
                return NotFound();
            }

            return courierInformation;
        }

        // PUT: api/CourierInformations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourierInformation(int id, CourierInformation courierInformation)
        {
            if (id != courierInformation.Id)
            {
                return BadRequest();
            }

            _context.Entry(courierInformation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourierInformationExists(id))
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

        // POST: api/CourierInformations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CourierInformation>> PostCourierInformation(CourierInformation courierInformation)
        {
            _context.CourierInformation.Add(courierInformation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourierInformation", new { id = courierInformation.Id }, courierInformation);
        }

        // DELETE: api/CourierInformations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourierInformation(int id)
        {
            var courierInformation = await _context.CourierInformation.FindAsync(id);
            if (courierInformation == null)
            {
                return NotFound();
            }

            _context.CourierInformation.Remove(courierInformation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourierInformationExists(int id)
        {
            return _context.CourierInformation.Any(e => e.Id == id);
        }
    }
}
