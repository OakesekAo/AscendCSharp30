using ServiceHub.API.DTOs.Requests;
using ServiceHub.API.DTOs.Responses;
using ServiceHub.API.Models;

namespace ServiceHub.API.Services;

public interface IWorkOrderService
{
    Task<WorkOrderResponse> CreateAsync(CreateWorkOrderRequest request);
    Task<WorkOrderResponse?> GetAsync(int id);
    Task<List<WorkOrderResponse>> GetAllAsync();
    Task<List<WorkOrderResponse>> GetByCustomerIdAsync(int customerId);
    Task<WorkOrderResponse?> UpdateStatusAsync(int id, UpdateWorkOrderStatusRequest request);
    Task DeleteAsync(int id);
}
