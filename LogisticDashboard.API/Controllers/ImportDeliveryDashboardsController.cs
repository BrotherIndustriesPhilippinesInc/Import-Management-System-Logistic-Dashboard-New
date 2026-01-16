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
    public class ImportDeliveryDashboardsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public ImportDeliveryDashboardsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/ImportDeliveryDashboards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportDeliveryDashboards>>> GetImportDeliveryDashboards()
        {
            return await _context.ImportDeliveryDashboards.ToListAsync();
        }

        // GET: api/ImportDeliveryDashboards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImportDeliveryDashboards>> GetImportDeliveryDashboards(int id)
        {
            var importDeliveryDashboards = await _context.ImportDeliveryDashboards.FindAsync(id);

            if (importDeliveryDashboards == null)
            {
                return NotFound();
            }

            return importDeliveryDashboards;
        }

        // PUT: api/ImportDeliveryDashboards/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutImportDeliveryDashboards(int id, ImportDeliveryDashboards importDeliveryDashboards)
        {
            if (id != importDeliveryDashboards.Id)
            {
                return BadRequest();
            }

            _context.Entry(importDeliveryDashboards).State = EntityState.Modified;

            try
            {
                importDeliveryDashboards.Original_ETA_Port =
                DateTime.SpecifyKind(importDeliveryDashboards.Original_ETA_Port, DateTimeKind.Utc);

                importDeliveryDashboards.Revised_ETA_Port =
                DateTime.SpecifyKind(importDeliveryDashboards.Revised_ETA_Port, DateTimeKind.Utc);


                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImportDeliveryDashboardsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { status = 200, message = "Record updated successfully." });
        }

        // POST: api/ImportDeliveryDashboards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImportDeliveryDashboards>> PostImportDeliveryDashboards(ImportDeliveryDashboards importDeliveryDashboards)
        {
            importDeliveryDashboards.Original_ETA_Port =
                DateTime.SpecifyKind(importDeliveryDashboards.Original_ETA_Port, DateTimeKind.Utc);

            importDeliveryDashboards.Revised_ETA_Port =
            DateTime.SpecifyKind(importDeliveryDashboards.Revised_ETA_Port, DateTimeKind.Utc);


            _context.ImportDeliveryDashboards.Add(importDeliveryDashboards);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImportDeliveryDashboards", new { id = importDeliveryDashboards.Id }, importDeliveryDashboards);
        }

        // DELETE: api/ImportDeliveryDashboards/5
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteImportDeliveryDashboards(int id)
        {
            var importDeliveryDashboards = await _context.ImportDeliveryDashboards.FindAsync(id);
            if (importDeliveryDashboards == null)
            {
                return NotFound();
            }

            _context.ImportDeliveryDashboards.Remove(importDeliveryDashboards);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImportDeliveryDashboardsExists(int id)
        {
            return _context.ImportDeliveryDashboards.Any(e => e.Id == id);
        }
    }
}
