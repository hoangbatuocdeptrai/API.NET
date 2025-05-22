using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MyApiProject.Entity;
using MyApiProject.IService;
using MyApiProject.Service;

namespace MyApiProject.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        //  API ĐĂNG NHẬP
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Email và mật khẩu không được để trống.");

            var hashedPassword = PasswordHasher.HashPassword(request.Password);

            var customer = await _customerService.LoginAsync(request.Email, hashedPassword);
            if (customer == null)
                return Unauthorized("Email hoặc mật khẩu không đúng.");

            return Ok(customer);
        }


        // ✅ API ĐĂNG KÝ
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Email) || string.IsNullOrWhiteSpace(customer.Password))
                return BadRequest("Email và mật khẩu không được để trống.");

            try
            {
                var created = await _customerService.RegisterAsync(customer);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
