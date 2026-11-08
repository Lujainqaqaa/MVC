using Microsoft.AspNetCore.Mvc;
using UniversityCafeteria.Data;
using UniversityCafeteria.Models;

namespace UniversityCafeteria.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Category/Index
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();

            return View(categories);
        }

        // GET: Category/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public IActionResult Create(Category category)
        {
            _context.Categories.Add(category);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
