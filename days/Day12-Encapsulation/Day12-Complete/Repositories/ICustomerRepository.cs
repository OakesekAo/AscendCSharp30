using ServiceHub.Day12.Models;

namespace ServiceHub.Day12.Repositories;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer);
    Task<Customer?> GetAsync(int id);
    Task<List<Customer>> GetAllAsync();
    Task<List<Customer>> SearchAsync(string searchTerm);
}
