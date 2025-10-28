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
    public class DeliveryLeadtimeDatasController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;
        public DeliveryLeadtimeDatasController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/DeliveryLeadtimeDatas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryLeadtimeData>>> GetDeliveryLeadtimeData()
        {
            return await _context.DeliveryLeadtimeData.ToListAsync();
        }

        // GET: api/DeliveryLeadtimeDatas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryLeadtimeData>> GetDeliveryLeadtimeData(int id)
        {
            var deliveryLeadtimeData = await _context.DeliveryLeadtimeData.FindAsync(id);

            if (deliveryLeadtimeData == null)
            {
                return NotFound();
            }

            return deliveryLeadtimeData;
        }

        // PUT: api/DeliveryLeadtimeDatas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeliveryLeadtimeData(int id, DeliveryLeadtimeData deliveryLeadtimeData)
        {
            if (id != deliveryLeadtimeData.Id)
            {
                return BadRequest();
            }

            _context.Entry(deliveryLeadtimeData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryLeadtimeDataExists(id))
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

        // POST: api/DeliveryLeadtimeDatas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DeliveryLeadtimeData>> PostDeliveryLeadtimeData(DeliveryLeadtimeData deliveryLeadtimeData)
        {
            _context.DeliveryLeadtimeData.Add(deliveryLeadtimeData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeliveryLeadtimeData", new { id = deliveryLeadtimeData.Id }, deliveryLeadtimeData);
        }

        // DELETE: api/DeliveryLeadtimeDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryLeadtimeData(int id)
        {
            var deliveryLeadtimeData = await _context.DeliveryLeadtimeData.FindAsync(id);
            if (deliveryLeadtimeData == null)
            {
                return NotFound();
            }

            _context.DeliveryLeadtimeData.Remove(deliveryLeadtimeData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeliveryLeadtimeDataExists(int id)
        {
            return _context.DeliveryLeadtimeData.Any(e => e.Id == id);
        }

        [HttpPost("Upload")]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded!");

            // --- 🔥 Truncate existing data for these sections ---
            var sections = new[]
            {
        "CN-BIPH Leadtime via Maersk",
        "HK-BIPH Leadtime via Maersk",
        "JPN-BIPH Leadtime via ONE"
    };

            var existingData = _context.DeliveryLeadtimeData
                .Where(d => sections.Contains(d.LeadtimeName));

            _context.DeliveryLeadtimeData.RemoveRange(existingData);
            await _context.SaveChangesAsync();

            // --- Continue reading Excel ---
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);

            var worksheet = package.Workbook.Worksheets["Data"];
            if (worksheet == null)
                return BadRequest("No worksheet found in Excel.");

            var months = new[]
            {
        "Apr", "May", "Jun", "Jul", "Aug", "Sep",
        "Oct", "Nov", "Dec", "Jan", "Feb", "Mar", "Ave. Fy2025"
    };

            var allLeadtimeData = new List<DeliveryLeadtimeData>();

            foreach (var section in new[]
            {
        new { Name = "CN-BIPH Leadtime via Maersk", RowStart = 16 },
        new { Name = "HK-BIPH Leadtime via Maersk", RowStart = 23 },
        new { Name = "JPN-BIPH Leadtime via ONE",   RowStart = 30 }
    })
            {
                int dataStartCol = 3; // Column C
                int actualRow = section.RowStart + 1;
                int targetMaxRow = section.RowStart + 2;
                int targetMinRow = section.RowStart + 3;
                int noOfBLRow = section.RowStart + 4;

                for (int i = 0; i < months.Length; i++)
                {
                    int col = dataStartCol + i;

                    var actualAve = TryParseDecimal(worksheet.Cells[actualRow, col].Value);
                    var targetMax = TryParseDecimal(worksheet.Cells[targetMaxRow, col].Value);
                    var targetMin = TryParseDecimal(worksheet.Cells[targetMinRow, col].Value);
                    var noOfBL = TryParseDecimal(worksheet.Cells[noOfBLRow, col].Value);

                    allLeadtimeData.Add(new DeliveryLeadtimeData
                    {
                        LeadtimeName = section.Name,
                        Month = months[i],
                        Actual_Ave = actualAve,
                        Target_Max = targetMax,
                        Target_Min = targetMin,
                        No_Of_BL = noOfBL
                    });
                }
            }

            await _context.DeliveryLeadtimeData.AddRangeAsync(allLeadtimeData);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Data imported successfully.",
                Count = allLeadtimeData.Count,
                Data = allLeadtimeData
            });
        }


        private int? TryParseInt(object value)
        {
            if (value == null) return null;
            if (int.TryParse(value.ToString(), out int result))
                return result;
            return null;
        }

        private decimal? TryParseDecimal(object value)
        {
            if (value == null) return null;

            // EPPlus sometimes gives doubles or strings, so handle both
            if (decimal.TryParse(value.ToString(), out decimal result))
                return result;

            return null;
        }

        [HttpGet("GetChartData")]
        public async Task<ActionResult> GetChartData([FromQuery] string leadtimeName)
        {
            var data = await _context.DeliveryLeadtimeData
                .Where(d => d.LeadtimeName == leadtimeName)
                .OrderBy(d => d.Id) // optional, ensure order of months
                .ToListAsync();

            if (!data.Any())
                return NotFound();

            // Map properties exactly
            var labels = data.Select(d => d.Month).ToList();
            var actualAve = data.Select(d => d.Actual_Ave).ToList();
            var targetMax = data.Select(d => d.Target_Max).ToList();
            var targetMin = data.Select(d => d.Target_Min).ToList();
            var noOfBL = data.Select(d => d.No_Of_BL).ToList();

            var chartData = new
            {
                labels,
                datasets = new
                {
                    actualAve,
                    targetMax,
                    targetMin,
                    noOfBL
                }
            };

            return Ok(chartData);
        }


    }
}