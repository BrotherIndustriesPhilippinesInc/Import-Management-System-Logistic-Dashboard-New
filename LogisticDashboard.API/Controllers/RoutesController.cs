using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogisticDashboard.API.Data;
using LogisticDashboard.Core;
using AutoMapper;
using LogisticDashboard.API.DTO;
using System.Globalization;

namespace LogisticDashboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public RoutesController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/Routes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RouteDto>>> GetRoutes()
        {
            var routes = await _context.Routes
            .Include(r => r.SailingSchedule)
            .Select(r => new RouteDto
            {
                Id = r.Id,
                RouteName = r.RouteName,
                From = r.From,
                To = r.To,
                FiscalYear = r.FiscalYear,
                Schedules = r.SailingSchedule.Select(s => new SailingScheduleNoRoutesDTO
                {
                    Id = s.Id,
                    Week = s.Week,
                    Start = s.Start,
                    End = s.End,
                    VesselName = s.VesselName,
                    VoyNo = s.VoyNo,
                    Origin = s.Origin,
                    OriginalETD = s.OriginalETD,
                    OriginalETAMNL = s.OriginalETAMNL,
                    LatestETD = s.LatestETD,
                    LatestETAMNL = s.LatestETAMNL,
                    TransitDays = s.TransitDays,
                    DelayDeparture = s.DelayDeparture,
                    DelayArrival = s.DelayArrival,
                    Remarks = s.Remarks,
                    RouteId = s.RouteId
                }).ToList()
            })
            .ToListAsync();

            return Ok(routes);
        }

        



        // GET: api/Routes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Routes>> GetRoutes(int id)
        {
            var routes = await _context.Routes.FindAsync(id);

            if (routes == null)
            {
                return NotFound();
            }

            return routes;
        }

        // PUT: api/Routes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoutes(int id, Routes routes)
        {
            if (id != routes.Id)
            {
                return BadRequest();
            }

            _context.Entry(routes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoutesExists(id))
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

        // POST: api/Routes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RouteDto>> PostRoutes(RouteDto routes)
        {
            //check if to and from have value
            if (string.IsNullOrEmpty(routes.From) || string.IsNullOrEmpty(routes.To) || routes.FiscalYear == 0)
            {
                return BadRequest("From and To fields are required.");
            }
            // check if a route with same From and To already exists
            var existingRoute = await _context.Routes
                .FirstOrDefaultAsync(r => r.From == routes.From && r.To == routes.To && r.FiscalYear == routes.FiscalYear);

            if (existingRoute != null)
            {
                return Conflict(new { message = "Route already exists." });
            }

            var newRoute = new Routes
            {
                RouteName = routes.From + " - " + routes.To,
                From = routes.From,
                To = routes.To,
                FiscalYear = routes.FiscalYear
            };

            _context.Routes.Add(newRoute);
            await _context.SaveChangesAsync();

            //Adding of 52 weeks onto the new route
            for (int i = 1; i <= 52; i++)
            {
                var newSailingSchedule = new SailingSchedule
                {
                    Week = "Week " + i,
                    RouteId = newRoute.Id
                };
                _context.SailingSchedule.Add(newSailingSchedule);
            }
            await _context.SaveChangesAsync();

            var routeDto = new RouteDto
            {
                Id = newRoute.Id,
                RouteName = newRoute.RouteName,
                From = newRoute.From,
                To = newRoute.To,
                FiscalYear = newRoute.FiscalYear,
                Schedules = new List<SailingScheduleNoRoutesDTO>() // empty for now (you can query later if you want)
            };

            return CreatedAtAction("GetRoutes", new { id = newRoute.Id }, routeDto);
        }


        // DELETE: api/Routes/5
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteRoutes(int id)
        {
            var routes = await _context.Routes.FindAsync(id);
            if (routes == null)
            {
                return NotFound();
            }

            _context.Routes.Remove(routes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoutesExists(int id)
        {
            return _context.Routes.Any(e => e.Id == id);
        }

        [HttpGet("distinct-routes")]
        public async Task<ActionResult<IEnumerable<RouteYearsDto>>> GetDistinctRoutesWithYears()
        {
            var routes = await _context.Routes
                .GroupBy(r => new { r.From, r.To })
                .Select(g => new RouteYearsDto
                {
                    From = g.Key.From,
                    To = g.Key.To,
                    Years = g.Select(r => r.FiscalYear).Distinct().OrderBy(y => y).ToList()
                })
                .ToListAsync();

            return Ok(routes);
        }

        [HttpGet("ID")]
        public async Task<ActionResult<RouteDto>> GetRoutes([FromQuery] string from, [FromQuery] string to, [FromQuery] int fiscalYear)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to) || fiscalYear == null || fiscalYear == 0)
            {
                return BadRequest(new { status = 400, message = "From, To, and FiscalYear are required." });
            }

            var route = await _context.Routes
                .Include(r => r.SailingSchedule) // ✅ include related entity first
                .Where(r => r.From == from && r.To == to && r.FiscalYear == fiscalYear)
                .Select(r => new RouteDto
                {
                    Id = r.Id,
                    From = r.From,
                    To = r.To,
                    FiscalYear = r.FiscalYear,
                    RouteName = r.RouteName,
                    Schedules = r.SailingSchedule
                        .OrderBy(s => s.Id)
                        .Select(s => new SailingScheduleNoRoutesDTO
                        {
                            Id = s.Id,
                            WeekNumber = EF.Functions.Like(s.Week, "Week%")
                                ? Convert.ToInt32(s.Week.Replace("Week ", ""))
                                : 0,
                            Week = s.Week,
                            Start = s.Start,
                            End = s.End,
                            VesselName = s.VesselName,
                            VoyNo = s.VoyNo,
                            Origin = s.Origin,
                            OriginalETD = s.OriginalETD,
                            OriginalETAMNL = s.OriginalETAMNL,
                            LatestETD = s.LatestETD,
                            LatestETAMNL = s.LatestETAMNL,
                            TransitDays = s.TransitDays,
                            DelayDeparture = s.DelayDeparture,
                            DelayArrival = s.DelayArrival,
                            Remarks = s.Remarks
                        }).ToList()
                })
                .FirstOrDefaultAsync();


            if (route == null)
                return NotFound();

            return Ok(route);
        }



        [HttpGet("ByCurrentWeeks")]
        public async Task<ActionResult<RouteDto>> GetRoutesByCurrentWeeks(
            [FromQuery] string from,
            [FromQuery] string to,
            [FromQuery] int fiscalYear)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to) || fiscalYear == 0)
            {
                return BadRequest(new { status = 400, message = "From, To, and FiscalYear are required." });
            }

            // Get current ISO week number
            var today = DateTime.Now;
            int currentWeek = ISOWeek.GetWeekOfYear(today); // e.g., 36
            int startWeek = currentWeek - 4;
            if (startWeek < 1) startWeek = 1;

            var route = await _context.Routes
                .Include(r => r.SailingSchedule)
                .Where(r => r.From == from && r.To == to && r.FiscalYear == fiscalYear)
                .FirstOrDefaultAsync();

            if (route == null)
                return NotFound();

            // Process schedules in-memory
            var schedules = route.SailingSchedule
                .Where(s => !string.IsNullOrEmpty(s.Week) && s.Week.StartsWith("Week "))
                .Select(s =>
                {
                    int weekNum = 0;
                    int.TryParse(s.Week.Replace("Week ", ""), out weekNum);

                    return new SailingScheduleNoRoutesDTO
                    {
                        Id = s.Id,
                        WeekNumber = weekNum,
                        Week = s.Week,
                        Start = s.Start,
                        End = s.End,
                        VesselName = s.VesselName,
                        VoyNo = s.VoyNo,
                        Origin = s.Origin,
                        OriginalETD = s.OriginalETD,
                        OriginalETAMNL = s.OriginalETAMNL,
                        LatestETD = s.LatestETD,
                        LatestETAMNL = s.LatestETAMNL,
                        TransitDays = s.TransitDays,
                        DelayDeparture = s.DelayDeparture,
                        DelayArrival = s.DelayArrival,
                        Remarks = s.Remarks
                    };
                })
                .Where(s => s.WeekNumber >= startWeek && s.WeekNumber <= currentWeek)
                .OrderBy(s => s.Id)
                .ToList();

            var dto = new RouteDto
            {
                Id = route.Id,
                From = route.From,
                To = route.To,
                FiscalYear = route.FiscalYear,
                RouteName = route.RouteName,
                Schedules = schedules
            };

            return Ok(dto);
        }


    }
}
