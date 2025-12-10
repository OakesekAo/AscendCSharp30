using ServiceHub.Day13.Models;

namespace ServiceHub.Day13.Repositories;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer);
    Task<Customer?> GetAsync(int id);
    Task<List<Customer>> GetAllAsync();
    Task<List<Customer>> SearchAsync(string searchTerm);
}
