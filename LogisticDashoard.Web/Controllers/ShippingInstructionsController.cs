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
    public class ShippingInstructionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShippingInstructionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShippingInstructions
        public async Task<IActionResult> Index()
        {
            var data = await _context.ShippingInstruction
                .Where(x => x.Type == "Instruction")
                .ToListAsync();
            return View(data);
        }

        public async Task<IActionResult> RequestForms()
        {
            var data = await _context.ShippingInstruction
                .Where(x => x.Type == "Request Form")
                .ToListAsync();
            return View("~/Views/RequestForms/Index.cshtml", data);
        }

        // GET: ShippingInstructions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingInstruction = await _context.ShippingInstruction
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shippingInstruction == null)
            {
                return NotFound();
            }

            return View(shippingInstruction);
        }

        // GET: ShippingInstructions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShippingInstructions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShippingInstruction shippingInstruction, IFormFile? FileEnglish, IFormFile? FileJapanese)
        {
            ModelState.Remove("UpdatedBy");
            if (ModelState.IsValid)
            {
                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resources", "shippingInstructions");

                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                if (FileEnglish != null && FileEnglish.Length > 0)
                {
                    string englishFileName = Guid.NewGuid().ToString() + Path.GetExtension(FileEnglish.FileName);
                    string englishPath = Path.Combine(uploadDir, englishFileName);

                    using (var stream = new FileStream(englishPath, FileMode.Create))
                    {
                        await FileEnglish.CopyToAsync(stream);
                    }

                    // ✅ Corrected web-accessible path
                    shippingInstruction.FileLinkEnglish = "/resources/shippingInstructions/" + englishFileName;
                }

                if (FileJapanese != null && FileJapanese.Length > 0)
                {
                    string japaneseFileName = Guid.NewGuid().ToString() + Path.GetExtension(FileJapanese.FileName);
                    string japanesePath = Path.Combine(uploadDir, japaneseFileName);

                    using (var stream = new FileStream(japanesePath, FileMode.Create))
                    {
                        await FileJapanese.CopyToAsync(stream);
                    }

                    // ✅ Corrected web-accessible path
                    shippingInstruction.FileLinkJapanese = "/resources/shippingInstructions/" + japaneseFileName;
                }

                shippingInstruction.LastUpdated = DateTime.UtcNow;
                shippingInstruction.UpdatedBy = User.Identity?.Name ?? "System";

                _context.Add(shippingInstruction);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(shippingInstruction);
        }


        // GET: ShippingInstructions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingInstruction = await _context.ShippingInstruction.FindAsync(id);
            if (shippingInstruction == null)
            {
                return NotFound();
            }
            return View(shippingInstruction);
        }

        // POST: ShippingInstructions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ShippingInstructionName,FileLinkEnglish,FileLinkJapanese,Type,LastUpdated,UpdatedBy")] ShippingInstruction shippingInstruction)
        {
            if (id != shippingInstruction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shippingInstruction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShippingInstructionExists(shippingInstruction.Id))
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
            return View(shippingInstruction);
        }

        // GET: ShippingInstructions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingInstruction = await _context.ShippingInstruction
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shippingInstruction == null)
            {
                return NotFound();
            }

            return View(shippingInstruction);
        }

        // POST: ShippingInstructions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shippingInstruction = await _context.ShippingInstruction.FindAsync(id);
            if (shippingInstruction != null)
            {
                _context.ShippingInstruction.Remove(shippingInstruction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShippingInstructionExists(int id)
        {
            return _context.ShippingInstruction.Any(e => e.Id == id);
        }
    }
}
