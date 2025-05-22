using Microsoft.EntityFrameworkCore;
using MyApiProject.Entity;
using MyApiProject.IService;

namespace MyApiProject.Service
{
    public class CategoryService: ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

     
        public async Task<Category?> AddCategoryAsync(Category category, IFormFile? imageFile)
        {
            // Kiểm tra xem danh mục có trùng tên trong hệ thống không
            var normalizedName = category.CategoryName;//.Trim().ToLower()
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.CategoryName == normalizedName);//.Trim().ToLower()

            if (categoryExists)
                return null; // Trùng tên, không cho thêm

            // Xử lý upload ảnh 
            string imagePath = null;
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine("wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder); // Tạo folder nếu chưa có

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Lưu file vào thư mục
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                imagePath = "/uploads/" + fileName;
            }

            // Thêm danh mục vào cơ sở dữ liệu
            category.Image = imagePath;
            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return category; // Trả về danh mục vừa thêm
        }

        public async Task<bool> UpdateCategoryAsync(int id, Category category, IFormFile? imageFile)
        {
            var existing = await _context.Categories.FindAsync(id);
            if (existing == null)
                return false;

            var normalizedName = category.CategoryName.Trim().ToLower();
            var isDuplicate = await _context.Categories
                .AnyAsync(c => c.Id != id && c.CategoryName.Trim().ToLower() == normalizedName);

            if (isDuplicate)
                return false;

            existing.CategoryName = category.CategoryName;
            existing.Status = category.Status;

            // Nếu có ảnh mới thì xử lý lưu ảnh
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine("wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                existing.Image = "/uploads/" + fileName;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id); // Tìm bài viết theo ID

        }

    }
}
