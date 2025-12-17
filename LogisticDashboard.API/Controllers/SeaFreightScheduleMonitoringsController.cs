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
    public class SeaFreightScheduleMonitoringsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public SeaFreightScheduleMonitoringsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/SeaFreightScheduleMonitorings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeaFreightScheduleMonitoring>>> GetSeaFreightScheduleMonitoring()
        {
            return await _context.SeaFreightScheduleMonitoring.OrderByDescending(x => x.Id).ToListAsync();
        }

        // GET: api/SeaFreightScheduleMonitorings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SeaFreightScheduleMonitoring>> GetSeaFreightScheduleMonitoring(string id)
        {
            var seaFreightScheduleMonitoring = await _context.SeaFreightScheduleMonitoring.FindAsync(id);

            if (seaFreightScheduleMonitoring == null)
            {
                return NotFound();
            }

            return seaFreightScheduleMonitoring;
        }

        // PUT: api/SeaFreightScheduleMonitorings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeaFreightScheduleMonitoring(int id, SeaFreightScheduleMonitoring seaFreightScheduleMonitoring)
        {
            if (id != seaFreightScheduleMonitoring.Id)
            {
                return BadRequest();
            }

            _context.Entry(seaFreightScheduleMonitoring).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeaFreightScheduleMonitoringExists(id))
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

        // POST: api/SeaFreightScheduleMonitorings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SeaFreightScheduleMonitoring>> PostSeaFreightScheduleMonitoring(SeaFreightScheduleMonitoring seaFreightScheduleMonitoring)
        {
            _context.SeaFreightScheduleMonitoring.Add(seaFreightScheduleMonitoring);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSeaFreightScheduleMonitoring", new { id = seaFreightScheduleMonitoring.Id }, seaFreightScheduleMonitoring);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded!");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);

            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
                return BadRequest("No worksheet found in Excel.");

            int startRow = 11;  // Change this
            int startCol = 2;  // Change this
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

                var containerNo = worksheet.Cells[row, startCol + 10].Text; // Container_No column
                if (string.IsNullOrWhiteSpace(containerNo))
                    continue;

                var existing = await _context.SeaFreightScheduleMonitoring
                    .FirstOrDefaultAsync(x => x.Container_No == containerNo);

                if (existing == null)
                {
                    existing = new SeaFreightScheduleMonitoring();
                    _context.SeaFreightScheduleMonitoring.Add(existing);
                }

                //SHIPMENT DETAILS
                existing.ItemCategory = worksheet.Cells[row, startCol].Text;
                existing.Shipper = worksheet.Cells[row, startCol + 1].Text;
                existing.Origin = worksheet.Cells[row, startCol + 2].Text;
                existing.BL = worksheet.Cells[row, startCol + 3].Text;
                existing.INV = worksheet.Cells[row, startCol + 4].Text;
                existing.Carrier_Forwarded = worksheet.Cells[row, startCol + 5].Text;
                existing.Port_Of_Discharge = worksheet.Cells[row, startCol + 6].Text;
                existing.Vessel_Name = worksheet.Cells[row, startCol + 7].Text;
                existing.Mode_Of_Shipment = worksheet.Cells[row, startCol + 8].Text;
                existing.Container_Size_No_Of_PKGS = worksheet.Cells[row, startCol + 9].Text;
                existing.Container_No = containerNo;
                existing.Trucker = worksheet.Cells[row, startCol + 11].Text;

                //VESSEL STATUS
                existing.Original_ETD = worksheet.Cells[row, startCol + 12].Text;
                existing.ATD = worksheet.Cells[row, startCol + 13].Text;
                existing.Original_ETA = worksheet.Cells[row, startCol + 14].Value?.ToString() ?? "";
                existing.Latest_ETA = worksheet.Cells[row, startCol + 15].Value?.ToString() ?? "";
                existing.ATA = (worksheet.Cells[row, startCol + 16].Value as DateTime?)?.ToString("yyyy-MM-dd") ?? "";
                existing.ATB_Date = worksheet.Cells[row, startCol + 17].Text;
                existing.ATB_Time = worksheet.Cells[row, startCol + 18].Text;

                existing.No_Of_Days_Delayed_ETD_ATD = worksheet.Cells[row, startCol + 19].Text;
                existing.No_Of_Days_Delayed_ETA_ATA = worksheet.Cells[row, startCol + 20].Text;
                existing.No_Of_Days_Delayed_ETA_ATB = worksheet.Cells[row, startCol + 21].Text;
                existing.Transit_Days_ATD_ATA = worksheet.Cells[row, startCol + 22].Text;
                existing.Vessel_Status = worksheet.Cells[row, startCol + 23].Text;
                existing.Vessel_Remarks = worksheet.Cells[row, startCol + 24].Text;

                //SPECIAL REQUIREMENTS
                existing.Have_Job_Operation = worksheet.Cells[row, startCol + 25].Text;
                existing.With_Special_Permit = worksheet.Cells[row, startCol + 26].Text;

                //DELIVERY
                existing.Based_On_BERTH_BIPH_Leadtime = worksheet.Cells[row, startCol + 27].Text;
                existing.ETA_BIPH = worksheet.Cells[row, startCol + 28].Text;
                existing.Orig_RDD = worksheet.Cells[row, startCol + 29].Text;
                existing.Requested_Del_Date_To_Trucker = worksheet.Cells[row, startCol + 30].Text;
                existing.Requested_Del_Time_To_Trucker = worksheet.Cells[row, startCol + 31].Text;
                existing.Actual_Delivery = worksheet.Cells[row, startCol + 32].Text;
                existing.Actual_Del_Time_To_Trucker = worksheet.Cells[row, startCol + 33].Text;

                existing.BERTH_Leadtime = worksheet.Cells[row, startCol + 34].Text;
                existing.Actual_Leadtime_ATA_Port_ATA_BIPH_exclude_weekend = worksheet.Cells[row, startCol + 35].Text;

                //SHIPMENT PROCESS
                existing.Step_1 = worksheet.Cells[row, startCol + 36].Text;
                existing.Step_2 = worksheet.Cells[row, startCol + 37].Text;
                existing.Step_3 = worksheet.Cells[row, startCol + 38].Text;
                existing.Step_4 = worksheet.Cells[row, startCol + 39].Text;
                existing.Step_5 = worksheet.Cells[row, startCol + 40].Text;
                existing.Step_6 = worksheet.Cells[row, startCol + 41].Text;
                existing.Actual_Status = worksheet.Cells[row, startCol + 42].Text;
                existing.Shipment_Processing_Remarks = worksheet.Cells[row, startCol + 43].Text;

                //BOBTAIL/DETENTION
                existing.Bobtail_Date = worksheet.Cells[row, startCol + 44].Text;
                existing.Requested_Pick_Up_Date = worksheet.Cells[row, startCol + 45].Text;
                existing.Date_Return_of_Empty_Cntr = worksheet.Cells[row, startCol + 46].Text;
                existing.FreeTime_Valid_Until = worksheet.Cells[row, startCol + 47].Text;
                existing.No_of_Days_with_Detention_Estimate_Only = worksheet.Cells[row, startCol + 48].Text;
                existing.No_of_Days_of_Free_Time = worksheet.Cells[row, startCol + 49].Text;

                //MP/PURCHASING
                existing.Requested_Del_Date_To_Ship = worksheet.Cells[row, startCol + 50].Text;
                existing.Priority_Container = worksheet.Cells[row, startCol + 51].Text;
                existing.Earliest_Shortage_Date = worksheet.Cells[row, startCol + 52].Text;
                existing.Request_to_Unload_AM_or_PM = worksheet.Cells[row, startCol + 53].Text;

                existing.Random_Boolean = worksheet.Cells[row, startCol + 54].Text;
                existing.Final_Remarks = worksheet.Cells[row, startCol + 55].Text;

                existing.Vessel_Status_BIPH_Action = "";
                await _context.SaveChangesAsync(); // save per row, or batch outside loop if large file
            }

            return Ok("Data imported successfully.");
        }

        // DELETE: api/SeaFreightScheduleMonitorings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeaFreightScheduleMonitoring(string id)
        {
            var seaFreightScheduleMonitoring = await _context.SeaFreightScheduleMonitoring.FindAsync(id);
            if (seaFreightScheduleMonitoring == null)
            {
                return NotFound();
            }

            _context.SeaFreightScheduleMonitoring.Remove(seaFreightScheduleMonitoring);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SeaFreightScheduleMonitoringExists(int id)
        {
            return _context.SeaFreightScheduleMonitoring.Any(e => e.Id == id);
        }

        [HttpGet("category_status")]
        public async Task<ActionResult<IEnumerable<SeaFreightScheduleMonitoring>>> GetSeaFreightScheduleMonitoring(
        [FromQuery] string item_category,
        [FromQuery] string actual_status)
        {
            var results = await _context.SeaFreightScheduleMonitoring
                .Where(x => x.ItemCategory.ToLower() == item_category.ToLower()
                         && x.Actual_Status.ToLower() == actual_status.ToLower())
                .ToListAsync();

            if (!results.Any())
            {
                return NotFound();
            }

            return results;
        }

        [HttpGet("ImportInfo")]
        public async Task<IActionResult> ImportInfo()
        {
            var seaFreightDeliveryInfo = await _context.SeaFreightScheduleMonitoring
                .Select(x => new { x.Id, x.BL, x.Shipper, x.Original_ETA, x.Latest_ETA, x.Vessel_Remarks, x.Vessel_Status_BIPH_Action })
                .ToListAsync();
            return Ok(seaFreightDeliveryInfo);
        }

        [HttpGet("VesselTransitDays")]
        public async Task<IActionResult> VesselTransitDays()
        {
            var seaFreightDeliveryInfo = await _context.SeaFreightScheduleMonitoring.ToListAsync();

            return Ok(seaFreightDeliveryInfo);
        }
    }
}
