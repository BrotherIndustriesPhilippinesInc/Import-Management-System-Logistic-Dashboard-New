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
    public class ImportDeliveriesController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public ImportDeliveriesController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/ImportDeliveries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportDelivery>>> GetImportDelivery()
        {
            return await _context.ImportDelivery.ToListAsync();
        }

        // GET: api/ImportDeliveries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImportDelivery>> GetImportDelivery(int id)
        {

            var importDelivery = await _context.ImportDelivery.FindAsync(id);

            if (importDelivery == null)
            {
                return NotFound();
            }

            return importDelivery;

        }

        // PUT: api/ImportDeliveries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutImportDelivery(int id, ImportDelivery importDelivery)
        {
            if (id != importDelivery.Id)
                return BadRequest("ID mismatch.");

            var original = await _context.ImportDelivery
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            importDelivery.Original_ETA_Port =
            DateTime.SpecifyKind(importDelivery.Original_ETA_Port, DateTimeKind.Utc);

            importDelivery.Revised_ETA_Port =
            DateTime.SpecifyKind(importDelivery.Revised_ETA_Port, DateTimeKind.Utc);


            if (original == null)
                return NotFound();

            // Preserve immutable fields
            importDelivery.CreatedBy = original.CreatedBy;
            importDelivery.CreatedDate = original.CreatedDate;
            importDelivery.UpdatedDate = DateTime.UtcNow;

            _context.Entry(importDelivery).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(importDelivery);
        }


        // POST: api/ImportDeliveries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImportDelivery>> PostImportDelivery(ImportDelivery importDelivery)
        {
            importDelivery.Original_ETA_Port =
            DateTime.SpecifyKind(importDelivery.Original_ETA_Port, DateTimeKind.Utc);

            importDelivery.Revised_ETA_Port =
            DateTime.SpecifyKind(importDelivery.Revised_ETA_Port, DateTimeKind.Utc);

            importDelivery.CreatedDate = DateTime.UtcNow;
            _context.ImportDelivery.Add(importDelivery);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImportDelivery", new { id = importDelivery.Id }, importDelivery);
        }

        // DELETE: api/ImportDeliveries/5
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteImportDelivery(int id)
        {
            var importDelivery = await _context.ImportDelivery.FindAsync(id);
            if (importDelivery == null)
            {
                return NotFound();
            }

            _context.ImportDelivery.Remove(importDelivery);
            await _context.SaveChangesAsync();

            return Ok(importDelivery);
        }

        private bool ImportDeliveryExists(int id)
        {
            return _context.ImportDelivery.Any(e => e.Id == id);
        }
    }
}
