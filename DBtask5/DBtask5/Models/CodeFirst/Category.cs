
using System.ComponentModel.DataAnnotations;

namespace UniversityCafeteria.Models.CodeFirst
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(500)]
        public string? Description { get; set; }

        public ICollection<Product> Products { get; set; }
            = new List<Product>();
    }
}

