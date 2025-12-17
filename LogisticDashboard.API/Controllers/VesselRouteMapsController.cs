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
    public class VesselRouteMapsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public VesselRouteMapsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/VesselRouteMaps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VesselRouteMap>>> GetVesselRouteMap()
        {
            return await _context.VesselRouteMap.ToListAsync();
        }

        // GET: api/VesselRouteMaps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VesselRouteMap>> GetVesselRouteMap(int id)
        {
            var vesselRouteMap = await _context.VesselRouteMap.FindAsync(id);

            if (vesselRouteMap == null)
            {
                return NotFound();
            }

            return vesselRouteMap;
        }

        // PUT: api/VesselRouteMaps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // PUT: api/VesselRouteMaps/5
        [HttpPost("EditVesselRouteMap/{id}")]
        // 💥 Must accept the file and use [FromForm] for the entity, just like Create!
        public async Task<IActionResult> PutVesselRouteMap(int id, IFormFile? file, [FromForm] VesselRouteMap vesselRouteMap)
        {
            if (id != vesselRouteMap.Id)
            {
                return BadRequest("ID mismatch, baka!");
            }

            // 1. Fetch the existing entity from the database
            var existingRouteMap = await _context.VesselRouteMap.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

            if (existingRouteMap == null)
            {
                return NotFound("Original record not found!");
            }

            // 2. Handle File Upload (The modular part you asked for!)
            if (file != null)
            {
                // Delete Old File (Crucial for maintenance and storage space)
                if (!string.IsNullOrEmpty(existingRouteMap.PictureLocation))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingRouteMap.PictureLocation);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Save New File (Using the unique name logic you requested)
                var fileExtension = Path.GetExtension(file.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "vesselRouteMap");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var filePath = Path.Combine(uploadPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Update the location property
                vesselRouteMap.PictureLocation = $"vesselRouteMap/{uniqueFileName}";
            }
            else
            {
                // 3. No new file uploaded: Preserve the existing PictureLocation value
                vesselRouteMap.PictureLocation = existingRouteMap.PictureLocation;
            }

            // Preserve the Creation Date, since this is an update!
            vesselRouteMap.CreatedDate = existingRouteMap.CreatedDate;
            vesselRouteMap.LastUpdate = DateTime.UtcNow; // Add a LastUpdated field if you have one!


            // Update the database entry
            _context.Entry(vesselRouteMap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VesselRouteMapExists(id))
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
        
        // POST: api/VesselRouteMaps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateVesselRouteMap")]
        public async Task<ActionResult<VesselRouteMap>> PostVesselRouteMap(IFormFile file, [FromForm] VesselRouteMap vesselRouteMap)
        {

            if (ModelState.IsValid)
            {
                //Upload Picture
                if (file != null)
                {
                    // 1. Get the file extension from the original filename
                    var fileExtension = Path.GetExtension(file.FileName);

                    // 2. Generate a new unique name using GUID and append the extension
                    var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

                    // 3. Define the full save path
                    var uploadPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "vesselRouteMap"
                    );

                    // Ensure the directory exists (optional, but safer)
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var filePath = Path.Combine(uploadPath, uniqueFileName);

                    // 4. Save the file using the unique path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // 5. Save the unique, web-accessible URL path to the database
                    vesselRouteMap.PictureLocation = $"vesselRouteMap/{uniqueFileName}";
                }

                vesselRouteMap.CreatedDate = DateTime.UtcNow;
                _context.VesselRouteMap.Add(vesselRouteMap);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetVesselRouteMap", new { id = vesselRouteMap.Id }, vesselRouteMap);
        }
        // DELETE: api/VesselRouteMaps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVesselRouteMap(int id)
        {
            var vesselRouteMap = await _context.VesselRouteMap.FindAsync(id);
            if (vesselRouteMap == null)
            {
                return NotFound();
            }

            _context.VesselRouteMap.Remove(vesselRouteMap);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VesselRouteMapExists(int id)
        {
            return _context.VesselRouteMap.Any(e => e.Id == id);
        }

        [HttpGet("GetVesselRouteMap/{id?}")]
        public async Task<IActionResult> GetSpecificVesselRouteMap(int id)
        {
            var data = await _context.VesselRouteMap
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpGet("GetAllVesselRouteMap")]
        public async Task<IActionResult> GetAllVesselRouteMap(int id)
        {
            var data = await _context.VesselRouteMap.ToListAsync();

            return Ok(data);
        }


    }
}
