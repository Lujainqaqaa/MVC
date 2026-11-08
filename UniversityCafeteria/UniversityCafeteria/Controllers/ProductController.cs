
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityCafeteria.Data;
using UniversityCafeteria.Models;

namespace UniversityCafeteria.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product/Index
        public IActionResult Index()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .ToList();

            return View(products);
        }

        // GET: Product/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(
                _context.Categories.ToList(),
                "Id",
                "Name"
            );

            ViewBag.Suppliers = new SelectList(
                _context.Suppliers.ToList(),
                "Id",
                "Name"
            );

            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public IActionResult Create(Product product)
        {
            _context.Products.Add(product);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}