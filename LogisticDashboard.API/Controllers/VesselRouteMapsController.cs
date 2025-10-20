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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVesselRouteMap(int id, VesselRouteMap vesselRouteMap)
        {
            if (id != vesselRouteMap.Id)
            {
                return BadRequest();
            }

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
        [HttpPost]
        public async Task<ActionResult<VesselRouteMap>> PostVesselRouteMap(VesselRouteMap vesselRouteMap)
        {
            _context.VesselRouteMap.Add(vesselRouteMap);
            await _context.SaveChangesAsync();

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
    }
}
