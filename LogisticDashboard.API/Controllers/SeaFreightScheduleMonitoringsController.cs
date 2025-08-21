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
            return await _context.SeaFreightScheduleMonitoring.ToListAsync();
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

            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"SeaFreightScheduleMonitoring\"");

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

                var seaFreight = new SeaFreightScheduleMonitoring
                {
                    //SHIPMENT DETAILS
                    ItemCategory = worksheet.Cells[row, startCol].Text,
                    Shipper = worksheet.Cells[row, startCol + 1].Text,
                    Origin = worksheet.Cells[row, startCol + 2].Text,
                    BL = worksheet.Cells[row, startCol + 3].Text,
                    INV = worksheet.Cells[row, startCol + 4].Text,
                    Carrier_Forwarded = worksheet.Cells[row, startCol + 5].Text,
                    Port_Of_Discharge = worksheet.Cells[row, startCol + 6].Text,
                    Vessel_Name = worksheet.Cells[row, startCol + 7].Text,
                    Mode_Of_Shipment = worksheet.Cells[row, startCol + 8].Text,
                    Container_Size_No_Of_PKGS = worksheet.Cells[row, startCol + 9].Text,
                    Container_No = worksheet.Cells[row, startCol + 10].Text,
                    Trucker = worksheet.Cells[row, startCol + 11].Text,

                    //VESSEL STATUS
                    Original_ETD = worksheet.Cells[row, startCol + 12].Text,
                    ATD = worksheet.Cells[row, startCol + 13].Text,
                    Original_ETA = worksheet.Cells[row, startCol + 14].Text,
                    Latest_ETA = worksheet.Cells[row, startCol + 15].Text,
                    ATA = (worksheet.Cells[row, startCol + 16].Value as DateTime?)?.ToString("yyyy-MM-dd") ?? "",
                    ATB_Date = worksheet.Cells[row, startCol + 17].Text,
                    ATB_Time = worksheet.Cells[row, startCol + 18].Text,

                    No_Of_Days_Delayed_ETD_ATD = worksheet.Cells[row, startCol + 19].Text,
                    No_Of_Days_Delayed_ETA_ATA = worksheet.Cells[row, startCol + 20].Text,
                    No_Of_Days_Delayed_ETA_ATB = worksheet.Cells[row, startCol + 21].Text,
                    Transit_Days_ATD_ATA = worksheet.Cells[row, startCol + 22].Text,
                    Vessel_Status = worksheet.Cells[row, startCol + 23].Text,
                    Vessel_Remarks = worksheet.Cells[row, startCol + 24].Text,

                    //SPECIAL REQUIREMENTS
                    Have_Job_Operation = worksheet.Cells[row, startCol + 25].Text,
                    With_Special_Permit = worksheet.Cells[row, startCol + 26].Text,

                    //DELIVERY
                    Based_On_BERTH_BIPH_Leadtime = worksheet.Cells[row, startCol + 27].Text,
                    ETA_BIPH = worksheet.Cells[row, startCol + 28].Text,

                    Orig_RDD = worksheet.Cells[row, startCol + 29].Text,
                    Requested_Del_Date_To_Trucker = worksheet.Cells[row, startCol + 30].Text,
                    Requested_Del_Time_To_Trucker = worksheet.Cells[row, startCol + 31].Text,
                    Actual_Delivery = worksheet.Cells[row, startCol + 32].Text,
                    Actual_Del_Time_To_Trucker = worksheet.Cells[row, startCol + 33].Text,

                    BERTH_Leadtime = worksheet.Cells[row, startCol + 34].Text,
                    Actual_Leadtime_ATA_Port_ATA_BIPH_exclude_weekend = worksheet.Cells[row, startCol + 35].Text,

                    //SHIPMENT PROCESS
                    Step_1 = worksheet.Cells[row, startCol + 36].Text,
                    Step_2 = worksheet.Cells[row, startCol + 37].Text,
                    Step_3 = worksheet.Cells[row, startCol + 38].Text,
                    Step_4 = worksheet.Cells[row, startCol + 39].Text,
                    Step_5 = worksheet.Cells[row, startCol + 40].Text,
                    Step_6 = worksheet.Cells[row, startCol + 41].Text,
                    Actual_Status = worksheet.Cells[row, startCol + 42].Text,
                    Shipment_Processing_Remarks = worksheet.Cells[row, startCol + 43].Text,

                    //BOBTAIL/DETENTION
                    Bobtail_Date = worksheet.Cells[row, startCol + 44].Text,
                    Requested_Pick_Up_Date = worksheet.Cells[row, startCol + 45].Text,
                    Date_Return_of_Empty_Cntr = worksheet.Cells[row, startCol + 46].Text,
                    FreeTime_Valid_Until = worksheet.Cells[row, startCol + 47].Text,
                    No_of_Days_with_Detention_Estimate_Only = worksheet.Cells[row, startCol + 48].Text,
                    No_of_Days_of_Free_Time = worksheet.Cells[row, startCol + 49].Text,

                    //MP/PURCHASING
                    Requested_Del_Date_To_Ship = worksheet.Cells[row, startCol + 50].Text,
                    Priority_Container = worksheet.Cells[row, startCol + 51].Text,
                    Earliest_Shortage_Date = worksheet.Cells[row, startCol + 52].Text,
                    Request_to_Unload_AM_or_PM = worksheet.Cells[row, startCol + 53].Text,

                    Random_Boolean = worksheet.Cells[row, startCol + 54].Text,
                    Final_Remarks = worksheet.Cells[row, startCol + 55].Text
                };

                _context.SeaFreightScheduleMonitoring.Add(seaFreight);
                await _context.SaveChangesAsync(); // save immediately for this row
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



    }
}
