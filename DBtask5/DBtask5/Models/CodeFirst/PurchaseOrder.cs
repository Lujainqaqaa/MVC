
using System.ComponentModel.DataAnnotations;

namespace UniversityCafeteria.Models.CodeFirst
{
    public class PurchaseOrder
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "Pending";

        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public ICollection<PurchaseOrderItem> Items { get; set; }
            = new List<PurchaseOrderItem>();
    }
}
