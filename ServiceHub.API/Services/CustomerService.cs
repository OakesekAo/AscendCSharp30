using ServiceHub.API.DTOs.Requests;
using ServiceHub.API.DTOs.Responses;
using ServiceHub.API.Models;
using ServiceHub.API.Repositories;

namespace ServiceHub.API.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerResponse> CreateAsync(CreateCustomerRequest request)
    {
        var customer = new Customer { Name = request.Name, Email = request.Email };
        await _repository.AddAsync(customer);
        return customer.ToResponse();
    }

    public async Task<CustomerResponse?> GetAsync(int id)
    {
        var customer = await _repository.GetAsync(id);
        return customer?.ToResponse();
    }

    public async Task<List<CustomerResponse>> GetAllAsync()
    {
        var customers = await _repository.GetAllAsync();
        return customers.Select(c => c.ToResponse()).ToList();
    }

    public async Task<CustomerResponse?> UpdateAsync(int id, CreateCustomerRequest request)
    {
        var customer = await _repository.GetAsync(id);
        if (customer == null) return null;

        customer.Name = request.Name;
        customer.Email = request.Email;
        await _repository.UpdateAsync(customer);
        return customer.ToResponse();
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}

public static class CustomerExtensions
{
    public static CustomerResponse ToResponse(this Customer customer)
        => new(customer.Id, customer.Name, customer.Email, customer.CreatedAt);
}
