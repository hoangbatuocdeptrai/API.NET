using MyApiProject.Entity;

namespace MyApiProject.IService
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> AddCategoryAsync(Category category, IFormFile? imageFile);
        Task<bool> UpdateCategoryAsync(int id, Category category, IFormFile? imageFile);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
