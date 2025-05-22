using System.ComponentModel.DataAnnotations.Schema;

namespace MyApiProject.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string? Image { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public Category? Category { get; set; }

    }
}
