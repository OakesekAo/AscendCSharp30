using ServiceHub.API.Models;
using ServiceHub.API.Repositories;

namespace ServiceHub.API.Data;

public class CustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers = new();
    private int _nextId = 1;

    public CustomerRepository()
    {
        // Seed data
        _customers.Add(new Customer { Id = _nextId++, Name = "Alice Johnson", Email = "alice@example.com" });
        _customers.Add(new Customer { Id = _nextId++, Name = "Bob Smith", Email = "bob@example.com" });
        _customers.Add(new Customer { Id = _nextId++, Name = "Charlie Brown", Email = "charlie@example.com" });
    }

    public Task AddAsync(Customer customer)
    {
        customer.Id = _nextId++;
        _customers.Add(customer);
        return Task.CompletedTask;
    }

    public Task<Customer?> GetAsync(int id)
    {
        return Task.FromResult(_customers.FirstOrDefault(c => c.Id == id));
    }

    public Task<List<Customer>> GetAllAsync()
    {
        return Task.FromResult(new List<Customer>(_customers));
    }

    public Task UpdateAsync(Customer customer)
    {
        var existing = _customers.FirstOrDefault(c => c.Id == customer.Id);
        if (existing != null)
        {
            existing.Name = customer.Name;
            existing.Email = customer.Email;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == id);
        if (customer != null)
        {
            _customers.Remove(customer);
        }
        return Task.CompletedTask;
    }
}
