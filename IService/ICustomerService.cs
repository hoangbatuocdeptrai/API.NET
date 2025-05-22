using MyApiProject.Entity;

namespace MyApiProject.IService
{
    public interface ICustomerService
    {
        Task<Customer?> LoginAsync(string email, string password);
        Task<Customer> RegisterAsync(Customer customer);

    }
}
