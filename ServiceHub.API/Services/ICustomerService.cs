using ServiceHub.API.DTOs.Requests;
using ServiceHub.API.DTOs.Responses;
using ServiceHub.API.Models;

namespace ServiceHub.API.Services;

public interface ICustomerService
{
    Task<CustomerResponse> CreateAsync(CreateCustomerRequest request);
    Task<CustomerResponse?> GetAsync(int id);
    Task<List<CustomerResponse>> GetAllAsync();
    Task<CustomerResponse?> UpdateAsync(int id, CreateCustomerRequest request);
    Task DeleteAsync(int id);
}
