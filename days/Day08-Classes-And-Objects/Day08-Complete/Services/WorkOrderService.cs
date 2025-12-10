using ServiceHub.Day08.Models;
using ServiceHub.Day08.Repositories;

namespace ServiceHub.Day08.Services;

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
