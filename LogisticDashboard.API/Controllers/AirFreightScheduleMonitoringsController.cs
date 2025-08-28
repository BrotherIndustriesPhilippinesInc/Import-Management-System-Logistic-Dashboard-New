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
            return await _context.AirFreightScheduleMonitoring.ToListAsync();
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
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);

            // Find the worksheet with "shipment" in the name (case-insensitive)
            var worksheet = package.Workbook.Worksheets
                .FirstOrDefault(ws => ws.Name.IndexOf("shipment", StringComparison.OrdinalIgnoreCase) >= 0);

            if (worksheet == null)
                return BadRequest("No worksheet found containing 'shipment'.");

            // Example starting row/column
            int startRow = 17;  // adjust as needed
            int startCol = 2;   // adjust as needed
            int rowCount = worksheet.Dimension.Rows;

            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"AirFreightScheduleMonitoring\"");

            for (int row = startRow; row <= rowCount; row++)
            {
                bool rowHasData = false;
                for (int col = startCol; col <= startCol + 55; col++) // adjust max col as needed
                {
                    if (!string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Text))
                    {
                        rowHasData = true;
                        break;
                    }
                }

                if (!rowHasData)
                    continue; // skip this row

                var itemCategory = worksheet.Cells[row, startCol].Text;
                if (string.IsNullOrWhiteSpace(itemCategory))
                    continue;

                var airFreight = new AirFreightScheduleMonitoring
                {
                    //SHIPMENT DETAILS
                    ItemCategory = worksheet.Cells[row, startCol].Text,
                    Shipper = worksheet.Cells[row, startCol + 1].Text,
                    AWB = worksheet.Cells[row, startCol + 2].Text,
                    Forwarder_Courier = worksheet.Cells[row, startCol + 3].Text,
                    Broker = worksheet.Cells[row, startCol + 4].Text,
                    Flight_Detail = worksheet.Cells[row, startCol + 5].Text,
                    Invoice_No = worksheet.Cells[row, startCol + 6].Text,
                    Freight_Term = worksheet.Cells[row, startCol + 7].Text,
                    No_Of_Pkgs = worksheet.Cells[row, startCol + 8].Text,

                    //FLIGHT STATUS
                    Original_ETD = worksheet.Cells[row, startCol + 9].Text,
                    ATD = worksheet.Cells[row, startCol + 10].Text,
                    Original_ETA = worksheet.Cells[row, startCol + 11].Text,
                    Latest_ETA = worksheet.Cells[row, startCol + 12].Text,
                    ATA = (worksheet.Cells[row, startCol + 13].Value as DateTime?)?.ToString("yyyy-MM-dd") ?? "",
                    Flight_Status_Remarks = worksheet.Cells[row, startCol + 14].Text,

                    //DELIVERY
                    Import_Permit_Status = worksheet.Cells[row, startCol + 15].Text,

                    //SPECIAL REQUIREMENTS
                    Have_Arrangement = worksheet.Cells[row, startCol + 16].Text,
                    With_Special_Permit = worksheet.Cells[row, startCol + 17].Text,

                    //DELIVERY
                    ATA_Port_BIPH_Leadtime = worksheet.Cells[row, startCol + 18].Text,
                    ETA_BIPH_Manual_Computation = worksheet.Cells[row, startCol + 19].Text,
                    Requested_Del_Date_To_Ship = worksheet.Cells[row, startCol + 20].Text,
                    Earliest_Shortage_Date = worksheet.Cells[row, startCol + 21].Text,
                    Actual_Del = worksheet.Cells[row, startCol + 22].Text,
                    Status = worksheet.Cells[row, startCol + 23].Text,
                    Import_Remarks = worksheet.Cells[row, startCol + 24].Text,
                    System_Update = worksheet.Cells[row, startCol + 25].Text,
                };

                _context.AirFreightScheduleMonitoring.Add(airFreight);
                await _context.SaveChangesAsync(); // save immediately for this row
            }

            return Ok("Data imported successfully.");
        }

        [HttpGet("category_status")]
        public async Task<ActionResult<IEnumerable<AirFreightScheduleMonitoring>>> GetAirFreightScheduleMonitoring(
        [FromQuery] string item_category,
        [FromQuery] string actual_status)
        {
            var results = await _context.AirFreightScheduleMonitoring
                .Where(x => x.ItemCategory.ToLower() == item_category.ToLower()
                         && x.Status.ToLower() == actual_status.ToLower())
                .ToListAsync();

            if (!results.Any())
            {
                return NotFound();
            }

            return results;
        }
    }
}
