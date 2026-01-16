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
    public class ImportBerthingStatusManilasController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public ImportBerthingStatusManilasController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/ImportBerthingStatusManilas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportBerthingStatusManila>>> GetImportBerthingStatusManila()
        {
            return await _context.ImportBerthingStatusManila.ToListAsync();
        }

        // GET: api/ImportBerthingStatusManilas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImportBerthingStatusManila>> GetImportBerthingStatusManila(int id)
        {
            var importBerthingStatusManila = await _context.ImportBerthingStatusManila.FindAsync(id);

            if (importBerthingStatusManila == null)
            {
                return NotFound();
            }

            return importBerthingStatusManila;
        }

        // PUT: api/ImportBerthingStatusManilas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutImportBerthingStatusManila(int id, ImportBerthingStatusManila importBerthingStatusManila)
        {
            if (id != importBerthingStatusManila.Id)
            {
                return BadRequest();
            }

            _context.Entry(importBerthingStatusManila).State = EntityState.Modified;

            try
            {
                importBerthingStatusManila.Original_ETA_Port =
                DateTime.SpecifyKind(importBerthingStatusManila.Original_ETA_Port, DateTimeKind.Utc);

                importBerthingStatusManila.Revised_ETA_Port =
                DateTime.SpecifyKind(importBerthingStatusManila.Revised_ETA_Port, DateTimeKind.Utc);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImportBerthingStatusManilaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetImportBerthingStatusManila", new { id = importBerthingStatusManila.Id }, importBerthingStatusManila);
        }

        // POST: api/ImportBerthingStatusManilas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImportBerthingStatusManila>> PostImportBerthingStatusManila(ImportBerthingStatusManila importBerthingStatusManila)
        {
            importBerthingStatusManila.Original_ETA_Port =
            DateTime.SpecifyKind(importBerthingStatusManila.Original_ETA_Port, DateTimeKind.Utc);

            importBerthingStatusManila.Revised_ETA_Port =
            DateTime.SpecifyKind(importBerthingStatusManila.Revised_ETA_Port, DateTimeKind.Utc);


            _context.ImportBerthingStatusManila.Add(importBerthingStatusManila);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImportBerthingStatusManila", new { id = importBerthingStatusManila.Id }, importBerthingStatusManila);
        }

        // DELETE: api/ImportBerthingStatusManilas/5
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteImportBerthingStatusManila(int id)
        {
            var importBerthingStatusManila = await _context.ImportBerthingStatusManila.FindAsync(id);
            if (importBerthingStatusManila == null)
            {
                return NotFound();
            }

            _context.ImportBerthingStatusManila.Remove(importBerthingStatusManila);
            await _context.SaveChangesAsync();

            return Ok(new { status = 200, message = "Record deleted successfully." });
        }

        private bool ImportBerthingStatusManilaExists(int id)
        {
            return _context.ImportBerthingStatusManila.Any(e => e.Id == id);
        }
    }
}
