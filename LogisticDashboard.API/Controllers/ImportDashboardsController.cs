using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogisticDashboard.API.Data;
using LogisticDashboard.Core;
using Microsoft.AspNetCore.Http.HttpResults;

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
        [HttpPost("{id}")]
        public async Task<IActionResult> PutImportDashboards(int id, ImportDashboards importDashboards)
        {
            if (id != importDashboards.Id)
            {
                return BadRequest();
            }

            _context.Entry(importDashboards).State = EntityState.Modified;

            try
            {
                importDashboards.Original_ETA_Port =
                DateTime.SpecifyKind(importDashboards.Original_ETA_Port, DateTimeKind.Utc);

                importDashboards.Revised_ETA_Port =
                DateTime.SpecifyKind(importDashboards.Revised_ETA_Port, DateTimeKind.Utc);

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

            return CreatedAtAction("GetImportDashboards", new { id = importDashboards.Id }, importDashboards);
        }

        // POST: api/ImportDashboards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImportDashboards>> PostImportDashboards(ImportDashboards importDashboards)
        {
            importDashboards.Original_ETA_Port =
            DateTime.SpecifyKind(importDashboards.Original_ETA_Port, DateTimeKind.Utc);

            importDashboards.Revised_ETA_Port =
            DateTime.SpecifyKind(importDashboards.Revised_ETA_Port, DateTimeKind.Utc);

            _context.ImportDashboards.Add(importDashboards);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImportDashboards", new { id = importDashboards.Id }, importDashboards);
        }

        // DELETE: api/ImportDashboards/5
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteImportDashboards(int id)
        {
            var importDashboards = await _context.ImportDashboards.FindAsync(id);
            if (importDashboards == null)
            {
                return NotFound();
            }

            _context.ImportDashboards.Remove(importDashboards);
            await _context.SaveChangesAsync();

            return Ok(new { status = 200, message = "Record deleted successfully." });
        }

        private bool ImportDashboardsExists(int id)
        {
            return _context.ImportDashboards.Any(e => e.Id == id);
        }
    }
}
