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
    public class ImportPortUtilizationManilasController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public ImportPortUtilizationManilasController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/ImportPortUtilizationManilas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportPortUtilizationManila>>> GetImportPortUtilizationManila()
        {
            return await _context.ImportPortUtilizationManila.ToListAsync();
        }

        // GET: api/ImportPortUtilizationManilas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImportPortUtilizationManila>> GetImportPortUtilizationManila(int id)
        {
            var importPortUtilizationManila = await _context.ImportPortUtilizationManila.FindAsync(id);

            if (importPortUtilizationManila == null)
            {
                return NotFound();
            }

            return importPortUtilizationManila;
        }

        // PUT: api/ImportPortUtilizationManilas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutImportPortUtilizationManila(int id, ImportPortUtilizationManila importPortUtilizationManila)
        {
            if (id != importPortUtilizationManila.Id)
            {
                return BadRequest();
            }

            _context.Entry(importPortUtilizationManila).State = EntityState.Modified;

            try
            {
                importPortUtilizationManila.Original_ETA_Port =
                DateTime.SpecifyKind(importPortUtilizationManila.Original_ETA_Port, DateTimeKind.Utc);

                importPortUtilizationManila.Revised_ETA_Port =
                DateTime.SpecifyKind(importPortUtilizationManila.Revised_ETA_Port, DateTimeKind.Utc);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImportPortUtilizationManilaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetImportPortUtilizationManila", new { id = importPortUtilizationManila.Id }, importPortUtilizationManila);
        }

        // POST: api/ImportPortUtilizationManilas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImportPortUtilizationManila>> PostImportPortUtilizationManila(ImportPortUtilizationManila importPortUtilizationManila)
        {
            importPortUtilizationManila.Original_ETA_Port =
            DateTime.SpecifyKind(importPortUtilizationManila.Original_ETA_Port, DateTimeKind.Utc);

            importPortUtilizationManila.Revised_ETA_Port =
            DateTime.SpecifyKind(importPortUtilizationManila.Revised_ETA_Port, DateTimeKind.Utc);

            _context.ImportPortUtilizationManila.Add(importPortUtilizationManila);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImportPortUtilizationManila", new { id = importPortUtilizationManila.Id }, importPortUtilizationManila);
        }

        // DELETE: api/ImportPortUtilizationManilas/5
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteImportPortUtilizationManila(int id)
        {
            var importPortUtilizationManila = await _context.ImportPortUtilizationManila.FindAsync(id);
            if (importPortUtilizationManila == null)
            {
                return NotFound();
            }

            _context.ImportPortUtilizationManila.Remove(importPortUtilizationManila);
            await _context.SaveChangesAsync();

            return Ok(new { status = 200, message = "Record deleted successfully." });
        }

        private bool ImportPortUtilizationManilaExists(int id)
        {
            return _context.ImportPortUtilizationManila.Any(e => e.Id == id);
        }
    }
}
