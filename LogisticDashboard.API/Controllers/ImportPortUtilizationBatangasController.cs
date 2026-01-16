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
    public class ImportPortUtilizationBatangasController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public ImportPortUtilizationBatangasController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/ImportPortUtilizationBatangas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportPortUtilizationBatangas>>> GetImportPortUtilizationBatangas()
        {
            return await _context.ImportPortUtilizationBatangas.ToListAsync();
        }

        // GET: api/ImportPortUtilizationBatangas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImportPortUtilizationBatangas>> GetImportPortUtilizationBatangas(int id)
        {
            var importPortUtilizationBatangas = await _context.ImportPortUtilizationBatangas.FindAsync(id);

            if (importPortUtilizationBatangas == null)
            {
                return NotFound();
            }

            return importPortUtilizationBatangas;
        }

        // PUT: api/ImportPortUtilizationBatangas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutImportPortUtilizationBatangas(int id, ImportPortUtilizationBatangas importPortUtilizationBatangas)
        {
            if (id != importPortUtilizationBatangas.Id)
            {
                return BadRequest();
            }

            _context.Entry(importPortUtilizationBatangas).State = EntityState.Modified;

            try
            {
                importPortUtilizationBatangas.Original_ETA_Port =
                DateTime.SpecifyKind(importPortUtilizationBatangas.Original_ETA_Port, DateTimeKind.Utc);

                importPortUtilizationBatangas.Revised_ETA_Port =
                DateTime.SpecifyKind(importPortUtilizationBatangas.Revised_ETA_Port, DateTimeKind.Utc);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImportPortUtilizationBatangasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetImportPortUtilizationBatangas", new { id = importPortUtilizationBatangas.Id }, importPortUtilizationBatangas);
        }

        // POST: api/ImportPortUtilizationBatangas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImportPortUtilizationBatangas>> PostImportPortUtilizationBatangas(ImportPortUtilizationBatangas importPortUtilizationBatangas)
        {
            importPortUtilizationBatangas.Original_ETA_Port =
            DateTime.SpecifyKind(importPortUtilizationBatangas.Original_ETA_Port, DateTimeKind.Utc);

            importPortUtilizationBatangas.Revised_ETA_Port =
            DateTime.SpecifyKind(importPortUtilizationBatangas.Revised_ETA_Port, DateTimeKind.Utc);

            _context.ImportPortUtilizationBatangas.Add(importPortUtilizationBatangas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImportPortUtilizationBatangas", new { id = importPortUtilizationBatangas.Id }, importPortUtilizationBatangas);
        }

        // DELETE: api/ImportPortUtilizationBatangas/5
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteImportPortUtilizationBatangas(int id)
        {
            var importPortUtilizationBatangas = await _context.ImportPortUtilizationBatangas.FindAsync(id);
            if (importPortUtilizationBatangas == null)
            {
                return NotFound();
            }

            _context.ImportPortUtilizationBatangas.Remove(importPortUtilizationBatangas);
            await _context.SaveChangesAsync();

            return Ok(new { status = 200, message = "Record deleted successfully." });
        }

        private bool ImportPortUtilizationBatangasExists(int id)
        {
            return _context.ImportPortUtilizationBatangas.Any(e => e.Id == id);
        }
    }
}
