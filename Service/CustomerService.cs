using Microsoft.EntityFrameworkCore;
using MyApiProject.Entity;
using MyApiProject.IService;

namespace MyApiProject.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> LoginAsync(string email, string hashedPassword)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(x => x.Email == email && x.Password == hashedPassword);

            return customer;
        }

        public async Task<Customer> RegisterAsync(Customer customer)
        {
            // Kiểm tra email đã tồn tại chưa
            var exists = await _context.Customers.AnyAsync(x => x.Email == customer.Email);
            if (exists)
                throw new Exception("Email đã tồn tại.");

            // Mã hoá password
            customer.Password = PasswordHasher.HashPassword(customer.Password);

            // Nếu có ảnh thì xử lý ảnh (nếu bạn muốn)
            // Còn không thì cứ lưu Image = null

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return customer;
        }
    }
}
