using Microsoft.AspNetCore.Mvc;

namespace LogisticDashboard.Web.Controllers
{
    public class LogisticInformation : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
