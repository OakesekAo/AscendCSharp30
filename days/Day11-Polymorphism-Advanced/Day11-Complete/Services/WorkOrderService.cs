using ServiceHub.Day11.DTOs.Responses;
using ServiceHub.Day11.Models;
using ServiceHub.Day11.Repositories;

namespace ServiceHub.Day11.Services;

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

    public async Task<List<WorkOrder>> GetByCustomerIdAsync(int customerId)
    {
        return await _repository.GetByCustomerIdAsync(customerId);
    }

    public async Task<List<WorkOrder>> GetByStatusAsync(string status)
    {
        return await _repository.GetByStatusAsync(status);
    }

    public async Task<List<WorkOrder>> SearchAsync(string searchTerm)
    {
        return await _repository.SearchAsync(searchTerm);
    }

    public async Task<WorkOrder?> UpdateStatusAsync(int id, string status)
    {
        var order = await _repository.GetAsync(id);
        if (order != null)
        {
            order.Status = status;
            await _repository.UpdateAsync(order);
        }
        return order;
    }
}

public static class WorkOrderExtensions
{
    public static WorkOrderResponse ToResponse(this WorkOrder order)
        => new(order.Id, order.CustomerId, order.Description, order.Status);
}
