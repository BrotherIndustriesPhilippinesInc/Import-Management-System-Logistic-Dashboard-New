using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogisticDashboard.API.Data;
using LogisticDashboard.Core;
using LogisticDashboard.API.DTO;
using AutoMapper;

namespace LogisticDashboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SailingSchedulesController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;
        private readonly IMapper _mapper;

        public SailingSchedulesController(IMapper mapper, LogisticDashboardAPIContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/SailingSchedules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SailingScheduleNoRoutesDTO>>> GetSailingSchedule()
        {
            var schedules = await _context.SailingSchedule.ToListAsync();

            // Map to DTO
            var dtoList = _mapper.Map<List<SailingScheduleNoRoutesDTO>>(schedules);

            return Ok(dtoList);
        }

        // GET: api/SailingSchedules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SailingSchedule>> GetSailingSchedule(int id)
        {
            var sailingSchedule = await _context.SailingSchedule.FindAsync(id);

            if (sailingSchedule == null)
            {
                return NotFound();
            }

            return sailingSchedule;
        }

        // PUT: api/SailingSchedules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSailingSchedule(int id, SailingSchedule sailingSchedule)
        {
            if (id != sailingSchedule.Id)
            {
                return BadRequest();
            }

            _context.Entry(sailingSchedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SailingScheduleExists(id))
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

        // POST: api/SailingSchedules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SailingScheduleNoRoutesDTO>> PostSailingSchedule(SailingScheduleNoRoutesDTO sailingScheduleDto)
        {
            // Map DTO to entity
            var sailingSchedule = _mapper.Map<SailingSchedule>(sailingScheduleDto);

            // Optional: load the Route from DB to assign navigation property
            var route = await _context.Routes.FindAsync(sailingScheduleDto.RouteId);
            if (route == null)
                return BadRequest("Route not found");

            sailingSchedule.Route = route;

            // Add and save
            _context.SailingSchedule.Add(sailingSchedule);
            await _context.SaveChangesAsync();

            // Map back to DTO for response
            var resultDto = _mapper.Map<SailingScheduleNoRoutesDTO>(sailingSchedule);

            return CreatedAtAction(nameof(GetSailingSchedule), new { id = sailingSchedule.Id }, resultDto);
        }


        // DELETE: api/SailingSchedules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSailingSchedule(int id)
        {
            var sailingSchedule = await _context.SailingSchedule.FindAsync(id);
            if (sailingSchedule == null)
            {
                return NotFound();
            }

            _context.SailingSchedule.Remove(sailingSchedule);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SailingScheduleExists(int id)
        {
            return _context.SailingSchedule.Any(e => e.Id == id);
        }
    }
}
