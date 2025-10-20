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
using System.Globalization;

namespace LogisticDashboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SailingSchedulesController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;


        public SailingSchedulesController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/SailingSchedules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SailingScheduleNoRoutesDTO>>> GetSailingSchedule()
        {
            var schedules = await _context.SailingSchedule.ToListAsync();

            // Map to DTO
            var dtoList = new SailingScheduleNoRoutesDTO() { 
                
            };

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
        [HttpPost("update/{id}")]
        public async Task<IActionResult> PutSailingSchedule(int id, SailingScheduleUpdateDTO sailingScheduleDto)
        {
            if (id != sailingScheduleDto.Id)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            var schedule = await _context.SailingSchedule.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            // Map DTO -> Entity
            schedule.Week = sailingScheduleDto.Week;
            schedule.Start = sailingScheduleDto.Start.HasValue
                ? DateTime.SpecifyKind(sailingScheduleDto.Start.Value, DateTimeKind.Utc)
                : null;

            schedule.End = sailingScheduleDto.End.HasValue
                ? DateTime.SpecifyKind(sailingScheduleDto.End.Value, DateTimeKind.Utc)
                : null;
            schedule.VesselName = sailingScheduleDto.VesselName;
            schedule.VoyNo = sailingScheduleDto.VoyNo;
            schedule.Origin = sailingScheduleDto.Origin;
            schedule.OriginalETD = sailingScheduleDto.OriginalETD;
            schedule.OriginalETAMNL = sailingScheduleDto.OriginalETAMNL;
            schedule.LatestETD = sailingScheduleDto.LatestETD;
            schedule.LatestETAMNL = sailingScheduleDto.LatestETAMNL;
            schedule.TransitDays = sailingScheduleDto.TransitDays;
            schedule.DelayDeparture = sailingScheduleDto.DelayDeparture;
            schedule.DelayArrival = sailingScheduleDto.DelayArrival;
            schedule.Remarks = sailingScheduleDto.Remarks;

            await _context.SaveChangesAsync();

            return Ok(new {status= 200});
        }


        // POST: api/SailingSchedules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<SailingScheduleNoRoutesDTO>> PostSailingSchedule(SailingScheduleNoRoutesDTO sailingScheduleDto)
        //{
        //    // Map DTO to entity
        //    //var sailingSchedule = _mapper.Map<SailingSchedule>(sailingScheduleDto);

        //    // Optional: load the Route from DB to assign navigation property
        //    var route = await _context.Routes.FindAsync(sailingScheduleDto.RouteId);
        //    if (route == null)
        //        return BadRequest("Route not found");

        //    sailingSchedule.Route = route;

        //    // Add and save
        //    _context.SailingSchedule.Add(sailingSchedule);
        //    await _context.SaveChangesAsync();

        //    // Map back to DTO for response
        //    //var resultDto = _mapper.Map<SailingScheduleNoRoutesDTO>(sailingSchedule);

        //    return CreatedAtAction(nameof(GetSailingSchedule), new { id = sailingSchedule.Id }, sa);
        //}


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
