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
    public class LogisticCostCourierAFsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public LogisticCostCourierAFsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/LogisticCostCourierAFs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogisticCostCourierAF>>> GetLogisticCostCourierAF()
        {
            return await _context.LogisticCostCourierAF.ToListAsync();
        }

        // GET: api/LogisticCostCourierAFs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LogisticCostCourierAF>> GetLogisticCostCourierAF(int id)
        {
            var logisticCostCourierAF = await _context.LogisticCostCourierAF.FindAsync(id);

            if (logisticCostCourierAF == null)
            {
                return NotFound();
            }

            return logisticCostCourierAF;
        }

        // PUT: api/LogisticCostCourierAFs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogisticCostCourierAF(int id, LogisticCostCourierAF logisticCostCourierAF)
        {
            if (id != logisticCostCourierAF.Id)
            {
                return BadRequest();
            }

            _context.Entry(logisticCostCourierAF).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogisticCostCourierAFExists(id))
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

        // POST: api/LogisticCostCourierAFs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LogisticCostCourierAF>> PostLogisticCostCourierAF(LogisticCostCourierAF logisticCostCourierAF)
        {
            _context.LogisticCostCourierAF.Add(logisticCostCourierAF);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLogisticCostCourierAF", new { id = logisticCostCourierAF.Id }, logisticCostCourierAF);
        }

        // DELETE: api/LogisticCostCourierAFs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogisticCostCourierAF(int id)
        {
            var logisticCostCourierAF = await _context.LogisticCostCourierAF.FindAsync(id);
            if (logisticCostCourierAF == null)
            {
                return NotFound();
            }

            _context.LogisticCostCourierAF.Remove(logisticCostCourierAF);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LogisticCostCourierAFExists(int id)
        {
            return _context.LogisticCostCourierAF.Any(e => e.Id == id);
        }
    }
}
