using Microsoft.EntityFrameworkCore;
using MyApiProject.Entity;
using MyApiProject.IService;

namespace MyApiProject.Service
{
    public class ProductService: IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProduct()
        {
            return _context.Products.ToList();
        }

     
        public async Task<Product?> AddProductAsync(Product product, IFormFile? imageFile)
        {
            // Lưu ảnh nếu có
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var uploadPath = Path.Combine("wwwroot", "uploads");
                Directory.CreateDirectory(uploadPath);
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                product.Image = "/uploads/" + fileName;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<bool> UpdateProductAsync(int id, Product product, IFormFile? imageFile)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null)
                return false;

            existing.ProductName = product.ProductName;
            existing.Price = product.Price;
            existing.Description = product.Description;
            existing.CategoryId = product.CategoryId;

            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var uploadPath = Path.Combine("wwwroot", "uploads");
                Directory.CreateDirectory(uploadPath);
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                existing.Image = "/uploads/" + fileName;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null)
                return false;

            _context.Products.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id); // Tìm bài viết theo ID

        }

    }
}
