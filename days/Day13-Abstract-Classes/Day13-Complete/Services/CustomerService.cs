using ServiceHub.Day13.DTOs.Responses;
using ServiceHub.Day13.Models;
using ServiceHub.Day13.Repositories;

namespace ServiceHub.Day13.Services;

public class CustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Customer> CreateAsync(string name, string email)
    {
        var customer = new Customer { Name = name, Email = email };
        await _repository.AddAsync(customer);
        return customer;
    }

    public async Task<Customer?> GetAsync(int id)
    {
        return await _repository.GetAsync(id);
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<List<Customer>> SearchAsync(string searchTerm)
    {
        return await _repository.SearchAsync(searchTerm);
    }

    public async Task<PaginatedResponse<CustomerResponse>> GetPaginatedAsync(int page = 1, int pageSize = 10)
    {
        var all = await _repository.GetAllAsync();
        var total = all.Count;
        var totalPages = (int)Math.Ceiling((double)total / pageSize);
        
        var items = all
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => c.ToResponse())
            .ToList();

        return new PaginatedResponse<CustomerResponse>(items, page, pageSize, total, totalPages);
    }
}

public static class CustomerExtensions
{
    public static CustomerResponse ToResponse(this Customer customer)
        => new(customer.Id, customer.Name, customer.Email);
}
