using ServiceHub.Day09.Models;

namespace ServiceHub.Day09.Repositories;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer);
    Task<Customer?> GetAsync(int id);
    Task<List<Customer>> GetAllAsync();
}
