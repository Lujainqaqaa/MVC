using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UniversityCafeteria.Models;

namespace UniversityCafeteria.Controllers
{
    public class HomeController : Controller
    {
        // الصفحة الرئيسية
        public IActionResult Index()
        {
            return View();
        }

        // عرض المنتجات
        public IActionResult Products()
        {
            ViewBag.Products = new[]
            {
                "Water",
                "Orange Juice",
                "Coffee",
                "Sandwich",
                "Chocolate"
            };

            return View();
        }

        // عرض الموردين
        public IActionResult Suppliers()
        {
            ViewBag.Suppliers = new[]
            {
                "Fresh Food Company",
                "Happy Drinks Company",
                "Daily Supplies Company"
            };

            return View();
        }

        // صفحة الخصوصية
        public IActionResult Privacy()
        {
            return View();
        }

        // صفحة الأخطاء
        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                            ?? HttpContext.TraceIdentifier
            });
        }
    }
}