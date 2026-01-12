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
    public class ImportBerthingStatusController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public ImportBerthingStatusController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/ImportBerthingStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportBerthingStatus>>> GetImportBerthingStatus()
        {
            return await _context.ImportBerthingStatus.ToListAsync();
        }

        // GET: api/ImportBerthingStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImportBerthingStatus>> GetImportBerthingStatus(int id)
        {
            var importBerthingStatus = await _context.ImportBerthingStatus.FindAsync(id);

            if (importBerthingStatus == null)
            {
                return NotFound();
            }

            return importBerthingStatus;
        }

        // PUT: api/ImportBerthingStatus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImportBerthingStatus(int id, ImportBerthingStatus importBerthingStatus)
        {
            if (id != importBerthingStatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(importBerthingStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImportBerthingStatusExists(id))
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

        // POST: api/ImportBerthingStatus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImportBerthingStatus>> PostImportBerthingStatus(ImportBerthingStatus importBerthingStatus)
        {
            _context.ImportBerthingStatus.Add(importBerthingStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImportBerthingStatus", new { id = importBerthingStatus.Id }, importBerthingStatus);
        }

        // DELETE: api/ImportBerthingStatus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImportBerthingStatus(int id)
        {
            var importBerthingStatus = await _context.ImportBerthingStatus.FindAsync(id);
            if (importBerthingStatus == null)
            {
                return NotFound();
            }

            _context.ImportBerthingStatus.Remove(importBerthingStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImportBerthingStatusExists(int id)
        {
            return _context.ImportBerthingStatus.Any(e => e.Id == id);
        }
    }
}
