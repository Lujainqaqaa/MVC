using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityCafeteria.Data;
using UniversityCafeteria.Models;

namespace UniversityCafeteria.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employees = _context.Employees
                .Include(e => e.Orders)
                .ToList();

            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (!ModelState.IsValid)
                return View(employee);

            _context.Employees.Add(employee);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _context.Employees.Find(id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(employee);

            var oldEmployee = _context.Employees.Find(id);

            if (oldEmployee == null)
                return NotFound();

            oldEmployee.FullName = employee.FullName;
            oldEmployee.Phone = employee.Phone;
            oldEmployee.Email = employee.Email;
            oldEmployee.Position = employee.Position;
            oldEmployee.Salary = employee.Salary;
            oldEmployee.HireDate = employee.HireDate;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var employee = _context.Employees
                .Include(e => e.Orders)
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var employee = _context.Employees
                .Include(e => e.Orders)
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
                return NotFound();

            if (employee.Orders.Any())
            {
                TempData["Error"] =
                    "Cannot delete employee with orders.";

                return RedirectToAction("Index");
            }

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}