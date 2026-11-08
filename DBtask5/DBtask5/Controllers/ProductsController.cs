
using DBtask5.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityCafeteria.Data;
using UniversityCafeteria.Models.DatabaseFirst;

namespace UniversityCafeteria.Controllers
{
    public class ProductsController : Controller
    {
        private readonly CafeteriaDbContext _context;

        public ProductsController(CafeteriaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier);

            return View(await products.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            await LoadLists();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            await LoadLists();
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            await LoadLists();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            await LoadLists();
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                bool used = await _context.PurchaseOrderItems
                    .AnyAsync(i => i.ProductId == id);

                if (used)
                {
                    ModelState.AddModelError("",
                        "This product is used in purchase orders.");

                    return RedirectToAction(nameof(Index));
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadLists()
        {
            ViewBag.Categories = new SelectList(
                await _context.Categories.ToListAsync(),
                "Id", "Name");

            ViewBag.Suppliers = new SelectList(
                await _context.Suppliers.ToListAsync(),
                "Id", "Name");
        }
    }
}

