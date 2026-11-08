using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    public class caffeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
