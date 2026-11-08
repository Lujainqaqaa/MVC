using System;
using System.Collections.Generic;

namespace UniversityCafeteria.Models
{
    public class SupplyItem
    {
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }

    public class SupplyOrder
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public List<SupplyItem> Items { get; set; } = new();
        public string Priority { get; set; } = "Normal";
        public decimal Subtotal { get; set; }
        public decimal Fee { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = "Confirmed";
    }
}