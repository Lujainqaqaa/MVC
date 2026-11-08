
using DBtask5.Models;
using System.ComponentModel.DataAnnotations;

namespace UniversityCafeteria.Models.CodeFirst
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = "";

        [EmailAddress]
        [MaxLength(150)]
        public string? Email { get; set; }

        [MaxLength(30)]
        public string? Phone { get; set; }

        public ICollection<Product> Products { get; set; }
            = new List<Product>();

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
            = new List<PurchaseOrder>();
    }
}
