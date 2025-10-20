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
    public class IncotermsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public IncotermsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/Incoterms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Incoterms>>> GetIncoterms()
        {
            return await _context.Incoterms.OrderBy(x => x.Id ).ToListAsync();
        }

        // GET: api/Incoterms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Incoterms>> GetIncoterms(int id)
        {
            var incoterms = await _context.Incoterms.FindAsync(id);

            if (incoterms == null)
            {
                return NotFound();
            }

            return incoterms;
        }

        // PUT: api/Incoterms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncoterms(int id, Incoterms incoterms)
        {
            if (id != incoterms.Id)
            {
                return BadRequest();
            }

            _context.Entry(incoterms).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncotermsExists(id))
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

        // POST: api/Incoterms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Incoterms>> PostIncoterms(Incoterms incoterms)
        {
            _context.Incoterms.Add(incoterms);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIncoterms", new { id = incoterms.Id }, incoterms);
        }

        // DELETE: api/Incoterms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncoterms(int id)
        {
            var incoterms = await _context.Incoterms.FindAsync(id);
            if (incoterms == null)
            {
                return NotFound();
            }

            _context.Incoterms.Remove(incoterms);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IncotermsExists(int id)
        {
            return _context.Incoterms.Any(e => e.Id == id);
        }
    }
}
