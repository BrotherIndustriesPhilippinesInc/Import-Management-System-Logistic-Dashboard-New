using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LogisticDashboard.Core;
using LogisticDashboard.Web.Data;

namespace LogisticDashboard.Web.Controllers
{
    public class FlowchartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlowchartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Flowcharts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Flowchart.ToListAsync());
        }

        // GET: Flowcharts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flowchart = await _context.Flowchart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flowchart == null)
            {
                return NotFound();
            }

            return View(flowchart);
        }

        // GET: Flowcharts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flowcharts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DrawflowData,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] Flowchart flowchart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flowchart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flowchart);
        }

        // GET: Flowcharts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flowchart = await _context.Flowchart.FindAsync(id);
            if (flowchart == null)
            {
                return NotFound();
            }
            return View(flowchart);
        }

        // POST: Flowcharts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,DrawflowData,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] Flowchart flowchart)
        {
            if (id != flowchart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flowchart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlowchartExists(flowchart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(flowchart);
        }

        // GET: Flowcharts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flowchart = await _context.Flowchart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flowchart == null)
            {
                return NotFound();
            }

            return View(flowchart);
        }

        // POST: Flowcharts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var flowchart = await _context.Flowchart.FindAsync(id);
            if (flowchart != null)
            {
                _context.Flowchart.Remove(flowchart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlowchartExists(string id)
        {
            return _context.Flowchart.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var chart = await _context.Flowchart.OrderByDescending(f => f.CreatedDate).FirstOrDefaultAsync();
            return Json(chart?.DrawflowData ?? "{}");
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Flowchart flowchart)
        {
            if (flowchart == null || string.IsNullOrEmpty(flowchart.DrawflowData))
                return BadRequest("Invalid flowchart data.");

            var existing = await _context.Flowchart.OrderByDescending(f => f.CreatedDate).FirstOrDefaultAsync();
            if (existing != null)
            {
                existing.DrawflowData = flowchart.DrawflowData;
                existing.UpdatedBy = "System";
                existing.UpdatedDate = DateTime.UtcNow;
            }
            else
            {
                flowchart.Id = Guid.NewGuid().ToString();
                flowchart.CreatedBy = "System";
                flowchart.CreatedDate = DateTime.UtcNow;
                _context.Flowchart.Add(flowchart);
            }

            await _context.SaveChangesAsync();
            return Ok("Saved successfully.");
        }
    }
}
