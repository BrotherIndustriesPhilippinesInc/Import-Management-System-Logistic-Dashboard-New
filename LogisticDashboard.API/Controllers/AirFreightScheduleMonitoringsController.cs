using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogisticDashboard.API.Data;
using LogisticDashboard.Core;
using OfficeOpenXml;

namespace LogisticDashboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirFreightScheduleMonitoringsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public AirFreightScheduleMonitoringsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/AirFreightScheduleMonitorings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AirFreightScheduleMonitoring>>> GetAirFreightScheduleMonitoring()
        {
            return await _context.AirFreightScheduleMonitoring.OrderByDescending(x => x.Id).ToListAsync();
        }

        // GET: api/AirFreightScheduleMonitorings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AirFreightScheduleMonitoring>> GetAirFreightScheduleMonitoring(int id)
        {
            var airFreightScheduleMonitoring = await _context.AirFreightScheduleMonitoring.FindAsync(id);

            if (airFreightScheduleMonitoring == null)
            {
                return NotFound();
            }

            return airFreightScheduleMonitoring;
        }

        // PUT: api/AirFreightScheduleMonitorings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAirFreightScheduleMonitoring(int id, AirFreightScheduleMonitoring airFreightScheduleMonitoring)
        {
            if (id != airFreightScheduleMonitoring.Id)
            {
                return BadRequest();
            }

            _context.Entry(airFreightScheduleMonitoring).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AirFreightScheduleMonitoringExists(id))
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

        // POST: api/AirFreightScheduleMonitorings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AirFreightScheduleMonitoring>> PostAirFreightScheduleMonitoring(AirFreightScheduleMonitoring airFreightScheduleMonitoring)
        {
            _context.AirFreightScheduleMonitoring.Add(airFreightScheduleMonitoring);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAirFreightScheduleMonitoring", new { id = airFreightScheduleMonitoring.Id }, airFreightScheduleMonitoring);
        }

        // DELETE: api/AirFreightScheduleMonitorings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAirFreightScheduleMonitoring(int id)
        {
            var airFreightScheduleMonitoring = await _context.AirFreightScheduleMonitoring.FindAsync(id);
            if (airFreightScheduleMonitoring == null)
            {
                return NotFound();
            }

            _context.AirFreightScheduleMonitoring.Remove(airFreightScheduleMonitoring);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AirFreightScheduleMonitoringExists(int id)
        {
            return _context.AirFreightScheduleMonitoring.Any(e => e.Id == id);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded!");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);

            var worksheet = package.Workbook.Worksheets
                .FirstOrDefault(ws => ws.Name.IndexOf("shipment", StringComparison.OrdinalIgnoreCase) >= 0);

            if (worksheet == null)
                return BadRequest("No worksheet found containing 'shipment'.");

            int startRow = 17;  // adjust as needed
            int startCol = 2;   // adjust as needed
            int rowCount = worksheet.Dimension.Rows;

            for (int row = startRow; row <= rowCount; row++)
            {
                bool rowHasData = false;
                for (int col = startCol; col <= startCol + 55; col++)
                {
                    if (!string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Text))
                    {
                        rowHasData = true;
                        break;
                    }
                }

                if (!rowHasData)
                    continue;

                var awb = worksheet.Cells[row, startCol + 2].Text; // AWB column
                if (string.IsNullOrWhiteSpace(awb))
                    continue;

                var existing = await _context.AirFreightScheduleMonitoring
                    .FirstOrDefaultAsync(x => x.AWB == awb);

                if (existing == null)
                {
                    existing = new AirFreightScheduleMonitoring();
                    existing.AWB = awb;
                    _context.AirFreightScheduleMonitoring.Add(existing);
                }

                //SHIPMENT DETAILS
                existing.ItemCategory = worksheet.Cells[row, startCol].Text;
                existing.Shipper = worksheet.Cells[row, startCol + 1].Text;
                existing.Forwarder_Courier = worksheet.Cells[row, startCol + 3].Text;
                existing.Broker = worksheet.Cells[row, startCol + 4].Text;
                existing.Flight_Detail = worksheet.Cells[row, startCol + 5].Text;
                existing.Invoice_No = worksheet.Cells[row, startCol + 6].Text;
                existing.Freight_Term = worksheet.Cells[row, startCol + 7].Text;
                existing.No_Of_Pkgs = worksheet.Cells[row, startCol + 8].Text;

                //FLIGHT STATUS
                existing.Original_ETD = worksheet.Cells[row, startCol + 9].Text;
                existing.ATD = worksheet.Cells[row, startCol + 10].Text;
                existing.Original_ETA = worksheet.Cells[row, startCol + 11].Text;
                existing.Latest_ETA = worksheet.Cells[row, startCol + 12].Text;
                existing.ATA = (worksheet.Cells[row, startCol + 13].Value as DateTime?)?.ToString("yyyy-MM-dd") ?? "";
                existing.Flight_Status_Remarks = worksheet.Cells[row, startCol + 14].Text;

                //PERMITS
                existing.Import_Permit_Status = worksheet.Cells[row, startCol + 15].Text;
                existing.Have_Arrangement = worksheet.Cells[row, startCol + 16].Text;
                existing.With_Special_Permit = worksheet.Cells[row, startCol + 17].Text;

                //DELIVERY & STATUS
                existing.ATA_Port_BIPH_Leadtime = worksheet.Cells[row, startCol + 18].Text;
                existing.ETA_BIPH_Manual_Computation = worksheet.Cells[row, startCol + 19].Text;
                existing.Requested_Del_Date_To_Ship = worksheet.Cells[row, startCol + 20].Text;
                existing.Earliest_Shortage_Date = worksheet.Cells[row, startCol + 21].Text;
                existing.Actual_Del = worksheet.Cells[row, startCol + 22].Text;
                existing.Status = worksheet.Cells[row, startCol + 23].Text;
                existing.Import_Remarks = worksheet.Cells[row, startCol + 24].Text;
                existing.System_Update = worksheet.Cells[row, startCol + 25].Text;

                await _context.SaveChangesAsync(); // per row, or move outside for batch
            }

            return Ok("Data imported successfully.");
        }


        [HttpGet("category_status")]
        public async Task<ActionResult<IEnumerable<AirFreightScheduleMonitoring>>> GetAirFreightScheduleMonitoring(
        [FromQuery] string item_category,
        [FromQuery] string actual_status)
        {
            IQueryable<AirFreightScheduleMonitoring> query = _context.AirFreightScheduleMonitoring;

            if (!string.IsNullOrEmpty(actual_status))
            {
                query = query.Where(x => x.Status.ToLower() == actual_status.ToLower());
            }

            if (!string.IsNullOrEmpty(item_category))
            {
                if (item_category.Equals("direct", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(x => x.ItemCategory.ToLower() == "direct parts");
                }
                else
                {
                    query = query.Where(x => x.ItemCategory.ToLower() != "direct parts");
                }
            }

            var results = await query.ToListAsync();

            if (!results.Any())
            {
                return NotFound();
            }

            return results;
        }

    }
}
