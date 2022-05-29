using Microsoft.AspNetCore.Mvc;

namespace Template_for_ASP.NET___entity_framework.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
