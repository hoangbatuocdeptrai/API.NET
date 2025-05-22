using MyApiProject.Entity;

namespace MyApiProject.IService
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProduct();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddProductAsync(Product product, IFormFile? imageFile);
        Task<bool> UpdateProductAsync(int id, Product product, IFormFile? imageFile);
        Task<bool> DeleteProductAsync(int id);
    }
}
