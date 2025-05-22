using System.ComponentModel.DataAnnotations.Schema;

namespace MyApiProject.Entity
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public bool? Status { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
