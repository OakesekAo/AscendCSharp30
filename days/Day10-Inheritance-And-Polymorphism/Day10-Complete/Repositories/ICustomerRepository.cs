using ServiceHub.Day10.Models;

namespace ServiceHub.Day10.Repositories;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer);
    Task<Customer?> GetAsync(int id);
    Task<List<Customer>> GetAllAsync();
}
