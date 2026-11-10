using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityCafeteria.Data;
using UniversityCafeteria.Models;

namespace UniversityCafeteria.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        private void LoadEmployees()
        {
            ViewBag.Employees = new SelectList(
                _context.Employees.ToList(), "Id", "FullName");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.Employee)
                .ToList();

            return View(orders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadEmployees();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order order)
        {
            if (!ModelState.IsValid)
            {
                LoadEmployees();
                return View(order);
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
                return NotFound();

            LoadEmployees();
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Order order)
        {
            if (id != order.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                LoadEmployees();
                return View(order);
            }

            var oldOrder = _context.Orders.Find(id);

            if (oldOrder == null)
                return NotFound();

            oldOrder.CustomerName = order.CustomerName;
            oldOrder.OrderDate = order.OrderDate;
            oldOrder.TotalPrice = order.TotalPrice;
            oldOrder.Status = order.Status;
            oldOrder.EmployeeId = order.EmployeeId;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Include(o => o.Employee)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}