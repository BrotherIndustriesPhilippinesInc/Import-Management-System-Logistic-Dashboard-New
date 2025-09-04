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

namespace LogisticDashboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;
        private readonly IMapper _mapper;

        public RoutesController(IMapper mapper, LogisticDashboardAPIContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Routes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RouteDto>>> GetRoutes()
        {
            var routes = await _context.Routes
                .Include(r => r.SailingSchedule)
                .ToListAsync();

            var routeDto = _mapper.Map<List<RouteDto>>(routes);

            return Ok(routeDto);
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
        public async Task<ActionResult<Routes>> PostRoutes(Routes routes)
        {
            _context.Routes.Add(routes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoutes", new { id = routes.Id }, routes);
        }

        // DELETE: api/Routes/5
        [HttpDelete("{id}")]
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
    }
}
