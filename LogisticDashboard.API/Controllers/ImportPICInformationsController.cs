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
    public class ImportPICInformationsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public ImportPICInformationsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/ImportPICInformations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportPICInformation>>> GetImportPICInformation()
        {
            return await _context.ImportPICInformation.ToListAsync();
        }

        // GET: api/ImportPICInformations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImportPICInformation>> GetImportPICInformation(int id)
        {
            var importPICInformation = await _context.ImportPICInformation.FindAsync(id);

            if (importPICInformation == null)
            {
                return NotFound();
            }

            return importPICInformation;
        }

        // PUT: api/ImportPICInformations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImportPICInformation(int id, ImportPICInformation importPICInformation)
        {
            if (id != importPICInformation.Id)
            {
                return BadRequest();
            }

            _context.Entry(importPICInformation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImportPICInformationExists(id))
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

        // POST: api/ImportPICInformations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImportPICInformation>> PostImportPICInformation(ImportPICInformation importPICInformation)
        {
            _context.ImportPICInformation.Add(importPICInformation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImportPICInformation", new { id = importPICInformation.Id }, importPICInformation);
        }

        // DELETE: api/ImportPICInformations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImportPICInformation(int id)
        {
            var importPICInformation = await _context.ImportPICInformation.FindAsync(id);
            if (importPICInformation == null)
            {
                return NotFound();
            }

            _context.ImportPICInformation.Remove(importPICInformation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImportPICInformationExists(int id)
        {
            return _context.ImportPICInformation.Any(e => e.Id == id);
        }
    }
}
