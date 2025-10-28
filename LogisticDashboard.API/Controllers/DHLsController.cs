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
    public class DHLsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public DHLsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/DHLs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DHL>>> GetDHL()
        {
            return await _context.DHL.ToListAsync();
        }

        // GET: api/DHLs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DHL>> GetDHL(int id)
        {
            var dHL = await _context.DHL.FindAsync(id);

            if (dHL == null)
            {
                return NotFound();
            }

            return dHL;
        }

        // PUT: api/DHLs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDHL(int id, DHL dHL)
        {
            if (id != dHL.Id)
            {
                return BadRequest();
            }

            _context.Entry(dHL).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DHLExists(id))
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

        // POST: api/DHLs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> Create(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                ModelState.AddModelError("Image", "Please upload an image.");
            }

            // Save file first
            var fileName = Path.GetFileName(image.FileName);
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resources", "dhl");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var courierInformation = new DHL
            {
                ImageLink = "resources/dhl/" + fileName,
                LastUpdated = DateTime.UtcNow,
                LastUpdateBy = "Admin"
            };

            _context.Add(courierInformation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // DELETE: api/DHLs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDHL(int id)
        {
            var dHL = await _context.DHL.FindAsync(id);
            if (dHL == null)
            {
                return NotFound();
            }

            _context.DHL.Remove(dHL);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DHLExists(int id)
        {
            return _context.DHL.Any(e => e.Id == id);
        }
    }
}
