

using DBtask5.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityCafeteria.Data;
using UniversityCafeteria.Models.DatabaseFirst;
using UniversityCafeteria.ViewModels;

namespace UniversityCafeteria.Controllers
{
    public class PurchaseOrdersController : Controller
    {
        private readonly CafeteriaDbContext _context;

        public PurchaseOrdersController(CafeteriaDbContext context)
        {
            _context = context;
        }

        // Display all orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.PurchaseOrders
                .Include(o => o.Supplier)
                .Include(o => o.PurchaseOrderItems)
                .ToListAsync();

            return View(orders);
        }

        // Display create form
        public async Task<IActionResult> Create()
        {
            await LoadSuppliers();
            await LoadProducts();

            return View(new PurchaseOrderCreateVM());
        }

        // Save order
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseOrderCreateVM vm)
        {
            vm.Items ??= new List<PurchaseOrderItemVM>();

            if (vm.Items.Count == 0)
                ModelState.AddModelError("", "Add at least one product.");

            if (!ModelState.IsValid)
            {
                await LoadSuppliers();
                await LoadProducts();
                return View(vm);
            }

            var productIds = vm.Items.Select(i => i.ProductId).ToList();

            if (productIds.Count != productIds.Distinct().Count())
            {
                ModelState.AddModelError("", "Do not add the same product twice.");
                await LoadSuppliers();
                await LoadProducts();
                return View(vm);
            }

            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            if (products.Count != productIds.Count ||
                products.Any(p => p.SupplierId != vm.SupplierId))
            {
                ModelState.AddModelError("", "Invalid product or supplier selection.");
                await LoadSuppliers();
                await LoadProducts();
                return View(vm);
            }

            using var transaction = await _context.Database
                .BeginTransactionAsync();

            try
            {
                var order = new PurchaseOrder
                {
                    SupplierId = vm.SupplierId,
                    OrderDate = DateTime.Now,
                    Status = "Pending"
                };

                _context.PurchaseOrders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in vm.Items)
                {
                    var product = products.First(p => p.Id == item.ProductId);

                    _context.PurchaseOrderItems.Add(
                        new PurchaseOrderItem
                        {
                            PurchaseOrderId = order.Id,
                            ProductId = product.Id,
                            Quantity = item.Quantity,
                            UnitPrice = product.Price
                        });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Display order details
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.PurchaseOrders
                .Include(o => o.Supplier)
                .Include(o => o.PurchaseOrderItems)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            ViewBag.Total = order.PurchaseOrderItems
                .Sum(i => i.Quantity * i.UnitPrice);

            return View(order);
        }

        // Receive order and increase inventory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Receive(int id)
        {
            var order = await _context.PurchaseOrders
                .Include(o => o.PurchaseOrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            if (order.Status != "Pending")
            {
                return BadRequest("This order cannot be received again.");
            }

            using var transaction = await _context.Database
                .BeginTransactionAsync();

            try
            {
                foreach (var item in order.PurchaseOrderItems)
                {
                    var product = await _context.Products
                        .FirstOrDefaultAsync(p => p.Id == item.ProductId);

                    if (product == null)
                        throw new InvalidOperationException("Product not found.");

                    product.Quantity += item.Quantity;
                }

                order.Status = "Received";

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task LoadSuppliers()
        {
            ViewBag.Suppliers = new SelectList(
                await _context.Suppliers.ToListAsync(),
                "Id", "Name");
        }

        private async Task LoadProducts()
        {
            ViewBag.Products = await _context.Products
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name + " - " + p.Price
                })
                .ToListAsync();
        }
    }
}
