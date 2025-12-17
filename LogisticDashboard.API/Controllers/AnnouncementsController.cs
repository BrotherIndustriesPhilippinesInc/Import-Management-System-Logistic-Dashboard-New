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
    public class AnnouncementsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public AnnouncementsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/Announcements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Announcements>>> GetAnnouncements()
        {
            return await _context.Announcements.OrderByDescending(x => x.Id).ToListAsync();
        }

        // GET: api/Announcements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Announcements>> GetAnnouncements(int id)
        {
            var announcements = await _context.Announcements.FindAsync(id);

            if (announcements == null)
            {
                return NotFound();
            }

            return announcements;
        }

        // PUT: api/Announcements/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnnouncements(int id, Announcements announcements)
        {
            if (id != announcements.Id)
            {
                return BadRequest();
            }

            _context.Entry(announcements).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnnouncementsExists(id))
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

        // POST: api/Announcements
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Announcements>> PostAnnouncements(Announcements announcements)
        {
            announcements.CreatedDate = DateTime.UtcNow;

            _context.Announcements.Add(announcements);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnnouncements", new { id = announcements.Id }, announcements);
        }

        // DELETE: api/Announcements/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnouncements(int id)
        {
            var announcements = await _context.Announcements.FindAsync(id);
            if (announcements == null)
            {
                return NotFound();
            }

            _context.Announcements.Remove(announcements);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnnouncementsExists(int id)
        {
            return _context.Announcements.Any(e => e.Id == id);
        }
    }
}
