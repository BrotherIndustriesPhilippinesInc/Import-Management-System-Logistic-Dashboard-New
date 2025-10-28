using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogisticDashboard.API.Data;
using LogisticDashboard.Core;
using System.Net.Http.Headers;

namespace LogisticDashboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerVisualizationsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public ContainerVisualizationsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/ContainerVisualizations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContainerVisualization>>> GetContainerVisualization()
        {
            return await _context.ContainerVisualization.ToListAsync();
        }

        // GET: api/ContainerVisualizations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContainerVisualization>> GetContainerVisualization(int id)
        {
            var containerVisualization = await _context.ContainerVisualization.FindAsync(id);

            if (containerVisualization == null)
            {
                return NotFound();
            }

            return containerVisualization;
        }

        // PUT: api/ContainerVisualizations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContainerVisualization(int id, ContainerVisualization containerVisualization)
        {
            if (id != containerVisualization.Id)
            {
                return BadRequest();
            }

            _context.Entry(containerVisualization).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContainerVisualizationExists(id))
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

        // POST: api/ContainerVisualizations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ContainerVisualization>> PostContainerVisualization(ContainerVisualization containerVisualization)
        {
            _context.ContainerVisualization.Add(containerVisualization);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContainerVisualization", new { id = containerVisualization.Id }, containerVisualization);
        }

        // DELETE: api/ContainerVisualizations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContainerVisualization(int id)
        {
            var containerVisualization = await _context.ContainerVisualization.FindAsync(id);
            if (containerVisualization == null)
            {
                return NotFound();
            }

            _context.ContainerVisualization.Remove(containerVisualization);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContainerVisualizationExists(int id)
        {
            return _context.ContainerVisualization.Any(e => e.Id == id);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded!");

            // Ensure uploads folder exists
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "containerVisuals");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Generate a unique filename to avoid overwriting
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileExtension = Path.GetExtension(originalFileName);
            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(originalFileName)}_{Guid.NewGuid():N}{fileExtension}";
            var filePath = Path.Combine(folderPath, uniqueFileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Get or create the single row
            var shipment = await _context.ContainerVisualization.FirstOrDefaultAsync();
            if (shipment == null)
            {
                shipment = new ContainerVisualization();
                _context.ContainerVisualization.Add(shipment);
            }

            // Update
            shipment.Container_Image_Link = "containerVisuals/" + uniqueFileName;

            shipment.LastUpdated = DateTime.UtcNow;
            shipment.LastUpdatedBy = User?.Identity?.Name ?? "System";

            await _context.SaveChangesAsync();

            return Ok(new { fileName = uniqueFileName });
        }

        [HttpGet("get_picture")]
        public async Task<IActionResult> GetPicture()
        {
            //return the only record of the ModeOfShipment table
            var shipment = await _context.ContainerVisualization.FirstOrDefaultAsync();
            return Ok(shipment);
        }
    }
}
