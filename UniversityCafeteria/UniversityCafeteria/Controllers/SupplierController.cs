using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public IActionResult Index()
        {
            var suppliers = _context.Suppliers
                .Include(s => s.Products)
                .ToList();

            return View(suppliers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Supplier supplier)
        {
            if (!ModelState.IsValid)
                return View(supplier);

            _context.Suppliers.Add(supplier);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var supplier = _context.Suppliers.Find(id);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Supplier supplier)
        {
            if (id != supplier.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(supplier);

            var oldSupplier = _context.Suppliers.Find(id);

            if (oldSupplier == null)
                return NotFound();

            oldSupplier.Name = supplier.Name;
            oldSupplier.Phone = supplier.Phone;
            oldSupplier.Email = supplier.Email;
            oldSupplier.Address = supplier.Address;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var supplier = _context.Suppliers
                .Include(s => s.Products)
                .FirstOrDefault(s => s.Id == id);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var supplier = _context.Suppliers
                .Include(s => s.Products)
                .FirstOrDefault(s => s.Id == id);

            if (supplier == null)
                return NotFound();

            if (supplier.Products.Any())
            {
                TempData["Error"] =
                    "Cannot delete supplier with products.";

                return RedirectToAction("Index");
            }

            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}