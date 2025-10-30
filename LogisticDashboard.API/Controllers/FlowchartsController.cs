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
    public class FlowchartsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public FlowchartsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/Flowcharts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flowchart>>> GetFlowchart()
        {
            return await _context.Flowchart.ToListAsync();
        }

        // GET: api/Flowcharts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Flowchart>> GetFlowchart(string id)
        {
            var flowchart = await _context.Flowchart.FindAsync(id);

            if (flowchart == null)
            {
                return NotFound();
            }

            return flowchart;
        }

        // PUT: api/Flowcharts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlowchart(string id, Flowchart flowchart)
        {
            if (id != flowchart.Id)
            {
                return BadRequest();
            }

            _context.Entry(flowchart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlowchartExists(id))
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

        // POST: api/Flowcharts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Flowchart>> PostFlowchart(Flowchart flowchart)
        {
            _context.Flowchart.Add(flowchart);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FlowchartExists(flowchart.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFlowchart", new { id = flowchart.Id }, flowchart);
        }

        // DELETE: api/Flowcharts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlowchart(string id)
        {
            var flowchart = await _context.Flowchart.FindAsync(id);
            if (flowchart == null)
            {
                return NotFound();
            }

            _context.Flowchart.Remove(flowchart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FlowchartExists(string id)
        {
            return _context.Flowchart.Any(e => e.Id == id);
        }
    }
}
