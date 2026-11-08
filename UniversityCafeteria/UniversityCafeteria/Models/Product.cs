namespace UniversityCafeteria.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        // Foreign Keys
        public int CategoryId { get; set; }

        public int SupplierId { get; set; }

        // Navigation Properties
        public Category? Category { get; set; }

        public Supplier? Supplier { get; set; }
    }
}