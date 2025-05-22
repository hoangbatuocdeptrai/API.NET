using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApiProject.Entity;
using MyApiProject.IService;

namespace MyApiProject.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context, ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {

            var categories = _categoryService.GetAllCategories();
            return Ok(categories);

        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromForm] Category model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.CategoryName))
            {
                return BadRequest("Invalid category data.");
            }

            // Gọi service để thêm danh mục
            var createdCategory = await _categoryService.AddCategoryAsync(new Category
            {
                CategoryName = model.CategoryName,
                Status = model.Status
            }, model.ImageFile);

            if (createdCategory == null)
            {
                return Conflict("Danh mục đã tồn tại trong hệ thống.");
            }

            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound("Không tìm thấy danh mục.");

            return Ok(category);
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] Category model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.CategoryName))
                return BadRequest("Invalid category data.");

            var success = await _categoryService.UpdateCategoryAsync(id, new Category
            {
                CategoryName = model.CategoryName,
                Status = model.Status
            }, model.ImageFile);

            if (!success)
                return Conflict("Không sửa được danh mục. Có thể do không tồn tại hoặc tên bị trùng.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result)
                return NotFound("Không tìm thấy danh mục cần xoá.");

            return Ok("Xoá thành công.");
        }

    }
}
