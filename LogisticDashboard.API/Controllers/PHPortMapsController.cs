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
    public class PHPortMapsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public PHPortMapsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/PHPortMaps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PHPortMap>>> GetPHPortMap()
        {
            return await _context.PHPortMap.ToListAsync();
        }

        // GET: api/PHPortMaps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PHPortMap>> GetPHPortMap(int id)
        {
            var pHPortMap = await _context.PHPortMap.FindAsync(id);

            if (pHPortMap == null)
            {
                return NotFound();
            }

            return pHPortMap;
        }

        // PUT: api/PHPortMaps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPHPortMap(int id, PHPortMap pHPortMap)
        {
            if (id != pHPortMap.Id)
            {
                return BadRequest();
            }

            _context.Entry(pHPortMap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PHPortMapExists(id))
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

        // POST: api/PHPortMaps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PHPortMap>> PostPHPortMap(PHPortMap pHPortMap)
        {
            _context.PHPortMap.Add(pHPortMap);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPHPortMap", new { id = pHPortMap.Id }, pHPortMap);
        }

        // DELETE: api/PHPortMaps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePHPortMap(int id)
        {
            var pHPortMap = await _context.PHPortMap.FindAsync(id);
            if (pHPortMap == null)
            {
                return NotFound();
            }

            _context.PHPortMap.Remove(pHPortMap);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PHPortMapExists(int id)
        {
            return _context.PHPortMap.Any(e => e.Id == id);
        }
    }
}
