using Microsoft.AspNetCore.Mvc;
using UniversityCafeteria.Data;
using UniversityCafeteria.Models;

namespace UniversityCafeteria.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupplierController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Supplier/Index
        public IActionResult Index()
        {
            var suppliers = _context.Suppliers.ToList();

            return View(suppliers);
        }

        // GET: Supplier/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Supplier/Create
        [HttpPost]
        public IActionResult Create(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}