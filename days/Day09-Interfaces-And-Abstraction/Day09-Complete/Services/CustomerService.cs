using ServiceHub.Day09.DTOs.Responses;
using ServiceHub.Day09.Models;
using ServiceHub.Day09.Repositories;

namespace ServiceHub.Day09.Services;

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
}

// Mapper extension method
public static class CustomerExtensions
{
    public static CustomerResponse ToResponse(this Customer customer)
        => new(customer.Id, customer.Name, customer.Email);
}
