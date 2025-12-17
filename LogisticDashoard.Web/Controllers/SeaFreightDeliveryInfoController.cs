using LogisticDashboard.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LogisticDashboard.Web.Controllers
{
    public class SeaFreightDeliveryInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SeaFreightDeliveryInfoController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var SeaFreightDeliveryInfo = await _context.SeaFreightScheduleMonitoring
                .OrderBy(s => s.Id)
                .ToListAsync();

            return View(SeaFreightDeliveryInfo);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok();
        }

        [HttpPost("UploadFile")]
        public IActionResult UploadFile()
        {
            return Ok();
        }

        


    }
}
