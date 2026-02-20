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
using System.Text.RegularExpressions;

namespace LogisticDashboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogisticCostsController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        public LogisticCostsController(LogisticDashboardAPIContext context)
        {
            _context = context;
        }

        // GET: api/LogisticCosts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogisticCost>>> GetLogisticCost()
        {
            return await _context.LogisticCost.ToListAsync();
        }

        // GET: api/LogisticCosts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LogisticCost>> GetLogisticCost(int id)
        {
            var logisticCost = await _context.LogisticCost.FindAsync(id);

            if (logisticCost == null)
            {
                return NotFound();
            }

            return logisticCost;
        }

        // PUT: api/LogisticCosts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogisticCost(int id, LogisticCost logisticCost)
        {
            if (id != logisticCost.Id)
            {
                return BadRequest();
            }

            _context.Entry(logisticCost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogisticCostExists(id))
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

        // POST: api/LogisticCosts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LogisticCost>> PostLogisticCost(LogisticCost logisticCost)
        {
            _context.LogisticCost.Add(logisticCost);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLogisticCost", new { id = logisticCost.Id }, logisticCost);
        }

        // DELETE: api/LogisticCosts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogisticCost(int id)
        {
            var logisticCost = await _context.LogisticCost.FindAsync(id);
            if (logisticCost == null)
            {
                return NotFound();
            }

            _context.LogisticCost.Remove(logisticCost);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LogisticCostExists(int id)
        {
            return _context.LogisticCost.Any(e => e.Id == id);
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadLogisticCost(IFormFile file)
        {
            // Fix your license! This property is the correct way in modern EPPlus (5+). 
            // The method you used might be deprecated or throw a warning.
            ExcelPackage.License.SetNonCommercialOrganization("BPS");

            // 1. Get the list of expected origins from the DB
            var allOrigin = await _context.LogisticCostCourierAF.ToListAsync();

            using (var package = new ExcelPackage(file.OpenReadStream()))
            {
                // ... (Outer loop and sheet finding logic is unchanged)

                foreach (var originItem in allOrigin)
                {
                    string sheetName = originItem.OriginName;
                    var worksheet = package.Workbook.Worksheets[sheetName];

                    if (worksheet == null || worksheet.Dimension == null)
                    {
                        continue;
                    }

                    int startingRow = 0;
                    int startColIndex = 0;
                    int endColIndex = 0;

                    // FIND START ROW
                    for (int r = 1; r <= worksheet.Dimension.End.Row; r++)
                    {
                        var text = worksheet.Cells[r, 1].Text.Trim();
                        if (text.Equals("KGS", StringComparison.OrdinalIgnoreCase))
                        {
                            startingRow = r;
                            break;
                        }
                    }

                    if (startingRow == 0)
                        continue; // no header found, skip sheet

                    // FIND START COLUMN (Scan Forward)
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        if (!string.IsNullOrWhiteSpace(worksheet.Cells[startingRow, col].Text))
                        {
                            startColIndex = col;
                            break;
                        }
                    }

                    // FIND END COLUMN (Scan Backward) - You needed this
                    for (int col = worksheet.Dimension.End.Column; col >= startColIndex; col--)
                    {
                        if (!string.IsNullOrWhiteSpace(worksheet.Cells[startingRow, col].Text))
                        {
                            endColIndex = col;
                            break;
                        }
                    }

                    if (startColIndex == 0 || endColIndex == 0)
                    {
                        continue; // Header row was empty, skip sheet.
                    }

                    // --- START OF NEW LOGIC ---

                    // 7. Loop through data rows (starting at 6, below the header)
                    // End.Row is the last row used anywhere in the sheet.
                    for (int row = startingRow + 1; row <= worksheet.Dimension.End.Row; row++)
                    {
                        // We assume the first column in your dynamic range (startColIndex) 
                        // holds the primary identifier (e.g., Destination Name).
                        string kgs = worksheet.Cells[row, startColIndex].Text.Trim();

                        // If the key column is empty, we assume we've hit the end of the data block.
                        if (string.IsNullOrWhiteSpace(kgs))
                        {
                            break;
                        }

                        string totalUSD = worksheet.Cells[row, endColIndex].Text.Trim();
                        //Remove any words from the totalUSD string
                        totalUSD = Regex.Replace(totalUSD, @"[^\d\.]", "", RegexOptions.IgnoreCase);

                        try
                        {
                            // 8. Create a new LogisticCost object
                            var logisticCost = new LogisticCost
                            {
                                KGS = Convert.ToString(kgs),
                                TotalUSD = Convert.ToDecimal(totalUSD),
                                Origin = originItem.OriginName
                            };

                            // 9. Add the LogisticCost object to the context
                        
                            _context.LogisticCost.Add(logisticCost);
                        }
                        catch (Exception ex)
                        {
                            // Handle the exception here
                            Console.WriteLine($"Error adding LogisticCost: {ex.Message}");
                            //show the variable before error
                            Console.WriteLine($"Origin: {originItem.OriginName}");
                            Console.WriteLine($"KGS: {kgs}");
                            Console.WriteLine($"TotalUSD: {totalUSD}");

                            throw;
                        }
                        
                    }

                    // --- END OF NEW LOGIC ---
                }

                //Truncate table before saving
                await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"LogisticCost\"");

                // 10. Save all changes to the database AFTER processing all sheets
                await _context.SaveChangesAsync();
            }

            return Ok("Matching sheets processed and database updated.");
        }

        [HttpGet("CalculateCost")]
        public async Task<IActionResult> CalculateCost([FromQuery] string origin, [FromQuery] string weight)
        {
            if (!decimal.TryParse(weight, out decimal parsedWeight))
            {
                return BadRequest("Weight must be a valid number.");
            }

            // Notice we compare the parsedWeight directly to the converted DB column
            var result = await _context.LogisticCost
                .FirstOrDefaultAsync(l => l.Origin == origin && Convert.ToDecimal(l.KGS) == parsedWeight);

            return result == null ? NotFound() : Ok(result);
        }
    }
}
