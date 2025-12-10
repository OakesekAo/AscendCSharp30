using ServiceHub.Day11.Models;
using ServiceHub.Day11.Repositories;

namespace ServiceHub.Day11.Data;

public class CustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers = new();
    private int _nextId = 1;

    public CustomerRepository()
    {
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

    public Task<List<Customer>> SearchAsync(string searchTerm)
    {
        var results = _customers.Where(c =>
            c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            c.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
        ).ToList();

        return Task.FromResult(results);
    }
}
