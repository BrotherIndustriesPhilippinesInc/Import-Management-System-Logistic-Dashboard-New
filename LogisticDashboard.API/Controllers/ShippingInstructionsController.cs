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
    public class ShippingInstructionsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public ShippingInstructionsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/ShippingInstructions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShippingInstruction>>> GetShippingInstruction()
        {
            return await _context.ShippingInstruction.ToListAsync();
        }

        // GET: api/ShippingInstructions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShippingInstruction>> GetShippingInstruction(int id)
        {
            var shippingInstruction = await _context.ShippingInstruction.FindAsync(id);

            if (shippingInstruction == null)
            {
                return NotFound();
            }

            return shippingInstruction;
        }

        // PUT: api/ShippingInstructions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShippingInstruction(int id, ShippingInstruction shippingInstruction)
        {
            if (id != shippingInstruction.Id)
            {
                return BadRequest();
            }

            _context.Entry(shippingInstruction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShippingInstructionExists(id))
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

        // POST: api/ShippingInstructions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ShippingInstruction>> PostShippingInstruction(ShippingInstruction shippingInstruction)
        {
            _context.ShippingInstruction.Add(shippingInstruction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShippingInstruction", new { id = shippingInstruction.Id }, shippingInstruction);
        }

        // DELETE: api/ShippingInstructions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShippingInstruction(int id)
        {
            var shippingInstruction = await _context.ShippingInstruction.FindAsync(id);
            if (shippingInstruction == null)
            {
                return NotFound();
            }

            _context.ShippingInstruction.Remove(shippingInstruction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShippingInstructionExists(int id)
        {
            return _context.ShippingInstruction.Any(e => e.Id == id);
        }
    }
}
