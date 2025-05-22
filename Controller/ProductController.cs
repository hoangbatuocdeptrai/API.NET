using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApiProject.Entity;
using MyApiProject.IService;
using MyApiProject.Service;

namespace MyApiProject.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        public ProductController(AppDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var product = _productService.GetAllProduct();
            return Ok(product);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] Product model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.ProductName) || string.IsNullOrWhiteSpace(model.Price))
                return BadRequest("Dữ liệu sản phẩm không hợp lệ.");

            var created = await _productService.AddProductAsync(model, model.ImageFile);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        //  SỬA SẢN PHẨM
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] Product model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.ProductName))
                return BadRequest("Dữ liệu sản phẩm không hợp lệ.");

            var result = await _productService.UpdateProductAsync(id, model, model.ImageFile);
            if (!result)
                return NotFound("Không tìm thấy sản phẩm để cập nhật.");

            return NoContent();
        }

        // XOÁ SẢN PHẨM
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success)
                return NotFound("Không tìm thấy sản phẩm để xoá.");

            return NoContent();
        }

        // LẤY 1 SẢN PHẨM (dùng trong CreatedAtAction)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }
    }
}
