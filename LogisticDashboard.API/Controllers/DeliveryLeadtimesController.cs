using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogisticDashboard.API.Data;
using LogisticDashboard.Core;
using System.Drawing.Printing;

namespace LogisticDashboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryLeadtimesController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public DeliveryLeadtimesController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/DeliveryLeadtimes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryLeadtime>>> GetDeliveryLeadtime()
        {
            return await _context.DeliveryLeadtime.ToListAsync();
        }

        // GET: api/DeliveryLeadtimes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryLeadtime>> GetDeliveryLeadtime(int id)
        {
            var deliveryLeadtime = await _context.DeliveryLeadtime.FindAsync(id);

            if (deliveryLeadtime == null)
            {
                return NotFound();
            }

            return deliveryLeadtime;
        }

        // PUT: api/DeliveryLeadtimes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeliveryLeadtime(int id, DeliveryLeadtime deliveryLeadtime)
        {
            if (id != deliveryLeadtime.Id)
            {
                return BadRequest();
            }

            _context.Entry(deliveryLeadtime).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryLeadtimeExists(id))
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

        // POST: api/DeliveryLeadtimes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DeliveryLeadtime>> PostDeliveryLeadtime(DeliveryLeadtime deliveryLeadtime)
        {
            _context.DeliveryLeadtime.Add(deliveryLeadtime);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeliveryLeadtime", new { id = deliveryLeadtime.Id }, deliveryLeadtime);
        }

        // DELETE: api/DeliveryLeadtimes/5
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteDeliveryLeadtime(int id)
        {
            var deliveryLeadtime = await _context.DeliveryLeadtime.FindAsync(id);
            if (deliveryLeadtime == null)
            {
                return NotFound();
            }

            _context.DeliveryLeadtime.Remove(deliveryLeadtime);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeliveryLeadtimeExists(int id)
        {
            return _context.DeliveryLeadtime.Any(e => e.Id == id);
        }

        [HttpPost("UpdateCell")]
        public IActionResult UpdateCell([FromBody] DeliveryLeadtimeUpdateDto update)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var record = _context.DeliveryLeadtime
                .FirstOrDefault(r => r.Id == update.Id);

            if (record == null)
                return NotFound("Record not found.");

            switch (update.Field)
            {
                case "Carrier": record.Carrier = update.Value; break;
                case "OriginPort": record.OriginPort = update.Value; break;
                case "DestinationPort": record.DestinationPort = update.Value; break;
                case "VesselTransitLeadtime": record.VesselTransitLeadtime = update.Value; break;
                case "CustomsClearanceLeadtime": record.CustomsClearanceLeadtime = update.Value; break;
                case "TotalLeadtime": record.TotalLeadtime = update.Value; break;
            }

            _context.SaveChanges();
            return Ok(new { message = "Saved successfully" });
        }

        public class DeliveryLeadtimeUpdateDto
        {
            public int Id { get; set; }
            public string Carrier { get; set; }
            public string Origin { get; set; }
            public string Destination { get; set; }
            public string Field { get; set; }
            public string Value { get; set; }
        }


    }
}
