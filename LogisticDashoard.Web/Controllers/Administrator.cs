using Microsoft.AspNetCore.Mvc;

namespace LogisticDashboard.Web.Controllers
{
    public class Administrator : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
