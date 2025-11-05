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
    public class LogisticCostsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public LogisticCostsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/LogisticCosts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogisticCost>>> GetLogisticCost()
        {
            return await _context.LogisticCost.ToListAsync();
        }

        // GET: api/LogisticCosts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LogisticCost>> GetLogisticCost(int id)
        {
            var logisticCost = await _context.LogisticCost.FindAsync(id);

            if (logisticCost == null)
            {
                return NotFound();
            }

            return logisticCost;
        }

        // PUT: api/LogisticCosts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogisticCost(int id, LogisticCost logisticCost)
        {
            if (id != logisticCost.Id)
            {
                return BadRequest();
            }

            _context.Entry(logisticCost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogisticCostExists(id))
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

        // POST: api/LogisticCosts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LogisticCost>> PostLogisticCost(LogisticCost logisticCost)
        {
            _context.LogisticCost.Add(logisticCost);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLogisticCost", new { id = logisticCost.Id }, logisticCost);
        }

        // DELETE: api/LogisticCosts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogisticCost(int id)
        {
            var logisticCost = await _context.LogisticCost.FindAsync(id);
            if (logisticCost == null)
            {
                return NotFound();
            }

            _context.LogisticCost.Remove(logisticCost);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LogisticCostExists(int id)
        {
            return _context.LogisticCost.Any(e => e.Id == id);
        }
    }
}
