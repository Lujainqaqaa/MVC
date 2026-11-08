using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using UniversityCafeteria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace UniversityCafeteria.Controllers
{
    public class SupplyRequestController : Controller
    {
        private static readonly List<SupplyOrder> Orders = new();

        // قراءة المسودة
        private SupplyOrder ReadDraft()
        {
            string? data = HttpContext.Session.GetString("Draft");

            if (data == null)
                return new SupplyOrder();

            return JsonSerializer.Deserialize<SupplyOrder>(data)
                   ?? new SupplyOrder();
        }

        // حفظ المسودة
        private void SaveDraft(SupplyOrder order)
        {
            HttpContext.Session.SetString(
                "Draft", JsonSerializer.Serialize(order));
        }

        private bool ValidPriority(string priority)
        {
            return priority == "Normal" ||
                   priority == "Urgent" ||
                   priority == "Emergency";
        }

        // حساب التكلفة
        private void Calculate(SupplyOrder order)
        {
            order.Subtotal = 0;

            foreach (var item in order.Items)
            {
                item.Total = item.Quantity * item.Price;
                order.Subtotal += item.Total;
            }

            switch (order.Priority)
            {
                case "Urgent":
                    order.Fee = 5;
                    break;
                case "Emergency":
                    order.Fee = 10;
                    break;
                default:
                    order.Fee = 2;
                    break;
            }

            if (order.Subtotal > 200)
                order.Discount = order.Subtotal * 0.15m;
            else if (order.Subtotal > 100)
                order.Discount = order.Subtotal * 0.10m;
            else
                order.Discount = 0;

            order.Total =
                order.Subtotal + order.Fee - order.Discount;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var order = ReadDraft();

            string preferred =
                Request.Cookies["PreferredPriority"] ?? "Normal";

            if (!ValidPriority(preferred))
                preferred = "Normal";

            if (order.Items.Count == 0)
                order.Priority = preferred;

            ViewBag.Priority = order.Priority;
            ViewBag.Preferred = preferred;
            ViewBag.Count = order.Items.Count;

            return View();
        }

        // استقبال المنتجات باستخدام Model Binding
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddItem(
            string name, string category,
            int quantity, decimal price, string priority)
        {
            if (!ModelState.IsValid ||
                string.IsNullOrWhiteSpace(name) ||
                quantity <= 0 || price <= 0 ||
                !ValidPriority(priority))
            {
                TempData["Message"] = "Enter valid product details.";
                return RedirectToAction("Index");
            }

            var order = ReadDraft();

            order.Priority = priority;

            order.Items.Add(new SupplyItem
            {
                Name = name,
                Category = category ?? "",
                Quantity = quantity,
                Price = price
            });

            SaveDraft(order);

            TempData["Message"] = "Product added.";
            return RedirectToAction("Index");
        }

        public IActionResult Review()
        {
            var order = ReadDraft();

            if (order.Items.Count == 0)
            {
                TempData["Message"] = "Add products first.";
                return RedirectToAction("Index");
            }

            Calculate(order);
            ViewBag.Order = order;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmRequest()
        {
            var order = ReadDraft();

            if (order.Items.Count == 0)
            {
                TempData["Message"] = "Cannot confirm an empty request.";
                return RedirectToAction("Index");
            }

            Calculate(order);

            lock (Orders)
            {
                Orders.Add(order);
            }

            HttpContext.Session.Remove("Draft");
            TempData["Message"] = "Request confirmed.";

            return RedirectToAction("MyRequests");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearDraft()
        {
            HttpContext.Session.Remove("Draft");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SavePreference(string priority)
        {
            if (ValidPriority(priority))
            {
                Response.Cookies.Append(
                    "PreferredPriority",
                    priority,
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(7),
                        HttpOnly = true
                    });

                TempData["Message"] = "Preference saved.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult MyRequests()
        {
            lock (Orders)
            {
                ViewBag.Orders = Orders.ToList();
            }

            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}