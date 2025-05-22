using System.ComponentModel.DataAnnotations.Schema;

namespace MyApiProject.Entity
{
    public class Customer
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime Birthday { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public string? Description { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
