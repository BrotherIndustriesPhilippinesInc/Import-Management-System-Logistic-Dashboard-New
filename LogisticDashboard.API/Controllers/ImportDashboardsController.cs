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
    public class ImportDashboardsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public ImportDashboardsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/ImportDashboards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportDashboards>>> GetImportDashboards()
        {
            return await _context.ImportDashboards.ToListAsync();
        }

        // GET: api/ImportDashboards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImportDashboards>> GetImportDashboards(int id)
        {
            var importDashboards = await _context.ImportDashboards.FindAsync(id);

            if (importDashboards == null)
            {
                return NotFound();
            }

            return importDashboards;
        }

        // PUT: api/ImportDashboards/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImportDashboards(int id, ImportDashboards importDashboards)
        {
            if (id != importDashboards.Id)
            {
                return BadRequest();
            }

            _context.Entry(importDashboards).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImportDashboardsExists(id))
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

        // POST: api/ImportDashboards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImportDashboards>> PostImportDashboards(ImportDashboards importDashboards)
        {
            _context.ImportDashboards.Add(importDashboards);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImportDashboards", new { id = importDashboards.Id }, importDashboards);
        }

        // DELETE: api/ImportDashboards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImportDashboards(int id)
        {
            var importDashboards = await _context.ImportDashboards.FindAsync(id);
            if (importDashboards == null)
            {
                return NotFound();
            }

            _context.ImportDashboards.Remove(importDashboards);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImportDashboardsExists(int id)
        {
            return _context.ImportDashboards.Any(e => e.Id == id);
        }
    }
}
