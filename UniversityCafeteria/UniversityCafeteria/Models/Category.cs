using Microsoft.CodeAnalysis;

namespace UniversityCafeteria.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }



        // تسمح للصنف الواحد بامتلاك عدة منتجات 
        public ICollection<Product>? Products { get; set; }
    }
}