using ServiceHub.Day09.DTOs.Responses;
using ServiceHub.Day09.Models;
using ServiceHub.Day09.Repositories;

namespace ServiceHub.Day09.Services;

public class WorkOrderService
{
    private readonly IWorkOrderRepository _repository;

    public WorkOrderService(IWorkOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkOrder> CreateAsync(int customerId, string description)
    {
        var order = new WorkOrder { CustomerId = customerId, Description = description };
        await _repository.AddAsync(order);
        return order;
    }

    public async Task<WorkOrder?> GetAsync(int id)
    {
        return await _repository.GetAsync(id);
    }

    public async Task<List<WorkOrder>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
}

// Mapper extension method
public static class WorkOrderExtensions
{
    public static WorkOrderResponse ToResponse(this WorkOrder order)
        => new(order.Id, order.CustomerId, order.Description, order.Status);
}
