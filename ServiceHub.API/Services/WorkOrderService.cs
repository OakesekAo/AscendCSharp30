using ServiceHub.API.DTOs.Requests;
using ServiceHub.API.DTOs.Responses;
using ServiceHub.API.Models;
using ServiceHub.API.Repositories;

namespace ServiceHub.API.Services;

public class WorkOrderService : IWorkOrderService
{
    private readonly IWorkOrderRepository _repository;

    public WorkOrderService(IWorkOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkOrderResponse> CreateAsync(CreateWorkOrderRequest request)
    {
        var order = new WorkOrder { CustomerId = request.CustomerId, Description = request.Description };
        await _repository.AddAsync(order);
        return order.ToResponse();
    }

    public async Task<WorkOrderResponse?> GetAsync(int id)
    {
        var order = await _repository.GetAsync(id);
        return order?.ToResponse();
    }

    public async Task<List<WorkOrderResponse>> GetAllAsync()
    {
        var orders = await _repository.GetAllAsync();
        return orders.Select(o => o.ToResponse()).ToList();
    }

    public async Task<List<WorkOrderResponse>> GetByCustomerIdAsync(int customerId)
    {
        var orders = await _repository.GetByCustomerIdAsync(customerId);
        return orders.Select(o => o.ToResponse()).ToList();
    }

    public async Task<WorkOrderResponse?> UpdateStatusAsync(int id, UpdateWorkOrderStatusRequest request)
    {
        var order = await _repository.GetAsync(id);
        if (order == null) return null;

        order.Status = request.Status;
        await _repository.UpdateAsync(order);
        return order.ToResponse();
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}

public static class WorkOrderExtensions
{
    public static WorkOrderResponse ToResponse(this WorkOrder order)
        => new(order.Id, order.CustomerId, order.Description, order.Status, order.CreatedAt);
}
