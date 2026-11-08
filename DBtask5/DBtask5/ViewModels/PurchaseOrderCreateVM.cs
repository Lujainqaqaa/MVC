
using System.ComponentModel.DataAnnotations;

namespace UniversityCafeteria.ViewModels
{
    public class PurchaseOrderCreateVM
    {
        [Required]
        public int SupplierId { get; set; }

        public List<PurchaseOrderItemVM> Items { get; set; }
            = new List<PurchaseOrderItemVM>();
    }

    public class PurchaseOrderItemVM
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, 100000)]
        public int Quantity { get; set; }
    }
}

