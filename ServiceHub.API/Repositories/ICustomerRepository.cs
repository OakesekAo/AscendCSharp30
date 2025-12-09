using ServiceHub.API.Models;

namespace ServiceHub.API.Repositories;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer);
    Task<Customer?> GetAsync(int id);
    Task<List<Customer>> GetAllAsync();
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
}
