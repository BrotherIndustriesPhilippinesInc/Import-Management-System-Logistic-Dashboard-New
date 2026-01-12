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
    public class ImportPortUtilizationsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public ImportPortUtilizationsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/ImportPortUtilizations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportPortUtilization>>> GetImportPortUtilization()
        {
            return await _context.ImportPortUtilization.ToListAsync();
        }

        // GET: api/ImportPortUtilizations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImportPortUtilization>> GetImportPortUtilization(int id)
        {
            var importPortUtilization = await _context.ImportPortUtilization.FindAsync(id);

            if (importPortUtilization == null)
            {
                return NotFound();
            }

            return importPortUtilization;
        }

        // PUT: api/ImportPortUtilizations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImportPortUtilization(int id, ImportPortUtilization importPortUtilization)
        {
            if (id != importPortUtilization.Id)
            {
                return BadRequest();
            }

            _context.Entry(importPortUtilization).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImportPortUtilizationExists(id))
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

        // POST: api/ImportPortUtilizations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImportPortUtilization>> PostImportPortUtilization(ImportPortUtilization importPortUtilization)
        {
            _context.ImportPortUtilization.Add(importPortUtilization);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImportPortUtilization", new { id = importPortUtilization.Id }, importPortUtilization);
        }

        // DELETE: api/ImportPortUtilizations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImportPortUtilization(int id)
        {
            var importPortUtilization = await _context.ImportPortUtilization.FindAsync(id);
            if (importPortUtilization == null)
            {
                return NotFound();
            }

            _context.ImportPortUtilization.Remove(importPortUtilization);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImportPortUtilizationExists(int id)
        {
            return _context.ImportPortUtilization.Any(e => e.Id == id);
        }
    }
}
