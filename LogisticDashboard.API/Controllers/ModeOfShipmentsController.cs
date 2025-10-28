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
    public class ModeOfShipmentsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public ModeOfShipmentsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/ModeOfShipments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModeOfShipment>>> GetModeOfShipment()
        {
            return await _context.ModeOfShipment.ToListAsync();
        }

        // GET: api/ModeOfShipments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModeOfShipment>> GetModeOfShipment(int id)
        {
            var modeOfShipment = await _context.ModeOfShipment.FindAsync(id);

            if (modeOfShipment == null)
            {
                return NotFound();
            }

            return modeOfShipment;
        }

        // PUT: api/ModeOfShipments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModeOfShipment(int id, ModeOfShipment modeOfShipment)
        {
            if (id != modeOfShipment.Id)
            {
                return BadRequest();
            }

            _context.Entry(modeOfShipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModeOfShipmentExists(id))
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

        // POST: api/ModeOfShipments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ModeOfShipment>> PostModeOfShipment(ModeOfShipment modeOfShipment)
        {
            _context.ModeOfShipment.Add(modeOfShipment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModeOfShipment", new { id = modeOfShipment.Id }, modeOfShipment);
        }

        // DELETE: api/ModeOfShipments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModeOfShipment(int id)
        {
            var modeOfShipment = await _context.ModeOfShipment.FindAsync(id);
            if (modeOfShipment == null)
            {
                return NotFound();
            }

            _context.ModeOfShipment.Remove(modeOfShipment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModeOfShipmentExists(int id)
        {
            return _context.ModeOfShipment.Any(e => e.Id == id);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file, string type)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded!");

            // Ensure uploads folder exists
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "modeOfShipmentUploads");
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
            var shipment = await _context.ModeOfShipment.FirstOrDefaultAsync();
            if (shipment == null)
            {
                shipment = new ModeOfShipment();
                _context.ModeOfShipment.Add(shipment);
            }

            // Update based on type
            if (type.Equals("FCL", StringComparison.OrdinalIgnoreCase))
                shipment.FCLProcessFlow_Image_Link = "modeOfShipmentUploads/" + uniqueFileName;
            else if (type.Equals("LCL", StringComparison.OrdinalIgnoreCase))
                shipment.LCLProcessFlow_Image_Link = "modeOfShipmentUploads/" + uniqueFileName;
            else
                return BadRequest("Invalid type. Must be 'FCL' or 'LCL'.");

            shipment.LastUpdated = DateTime.UtcNow;
            shipment.LastUpdatedBy = User?.Identity?.Name ?? "System";

            await _context.SaveChangesAsync();

            return Ok(new { fileName = uniqueFileName });
        }

        [HttpGet("get_picture")]
        public async Task<IActionResult> GetPicture()
        {
            //return the only record of the ModeOfShipment table
            var shipment = await _context.ModeOfShipment.FirstOrDefaultAsync();
            return Ok(shipment);

        }

    }
}
