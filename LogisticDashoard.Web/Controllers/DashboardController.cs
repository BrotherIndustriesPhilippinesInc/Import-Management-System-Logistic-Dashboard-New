using Microsoft.AspNetCore.Mvc;

namespace LogisticDashboard.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
