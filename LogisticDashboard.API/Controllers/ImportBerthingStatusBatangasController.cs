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
    public class ImportBerthingStatusBatangasController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public ImportBerthingStatusBatangasController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/ImportBerthingStatusBatangas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportBerthingStatusBatangas>>> GetImportBerthingStatusBatangas()
        {
            return await _context.ImportBerthingStatusBatangas.ToListAsync();
        }

        // GET: api/ImportBerthingStatusBatangas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImportBerthingStatusBatangas>> GetImportBerthingStatusBatangas(int id)
        {
            var importBerthingStatusBatangas = await _context.ImportBerthingStatusBatangas.FindAsync(id);

            if (importBerthingStatusBatangas == null)
            {
                return NotFound();
            }

            return importBerthingStatusBatangas;
        }

        // PUT: api/ImportBerthingStatusBatangas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutImportBerthingStatusBatangas(int id, ImportBerthingStatusBatangas importBerthingStatusBatangas)
        {
            if (id != importBerthingStatusBatangas.Id)
            {
                return BadRequest();
            }

            _context.Entry(importBerthingStatusBatangas).State = EntityState.Modified;

            try
            {
                importBerthingStatusBatangas.Original_ETA_Port =
                DateTime.SpecifyKind(importBerthingStatusBatangas.Original_ETA_Port, DateTimeKind.Utc);

                importBerthingStatusBatangas.Revised_ETA_Port =
                DateTime.SpecifyKind(importBerthingStatusBatangas.Revised_ETA_Port, DateTimeKind.Utc);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImportBerthingStatusBatangasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { status = 200, message = "Successfully Updated." });
        }

        // POST: api/ImportBerthingStatusBatangas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImportBerthingStatusBatangas>> PostImportBerthingStatusBatangas(ImportBerthingStatusBatangas importBerthingStatusBatangas)
        {
            importBerthingStatusBatangas.Original_ETA_Port =
            DateTime.SpecifyKind(importBerthingStatusBatangas.Original_ETA_Port, DateTimeKind.Utc);

            importBerthingStatusBatangas.Revised_ETA_Port =
            DateTime.SpecifyKind(importBerthingStatusBatangas.Revised_ETA_Port, DateTimeKind.Utc);


            _context.ImportBerthingStatusBatangas.Add(importBerthingStatusBatangas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImportBerthingStatusBatangas", new { id = importBerthingStatusBatangas.Id }, importBerthingStatusBatangas);
        }

        // DELETE: api/ImportBerthingStatusBatangas/5
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteImportBerthingStatusBatangas(int id)
        {
            var importBerthingStatusBatangas = await _context.ImportBerthingStatusBatangas.FindAsync(id);
            if (importBerthingStatusBatangas == null)
            {
                return NotFound();
            }

            _context.ImportBerthingStatusBatangas.Remove(importBerthingStatusBatangas);
            await _context.SaveChangesAsync();

            return Ok(new { status = 200, message = "Record deleted successfully." });
        }

        private bool ImportBerthingStatusBatangasExists(int id)
        {
            return _context.ImportBerthingStatusBatangas.Any(e => e.Id == id);
        }
    }
}
