using ServiceHub.Day08.Models;
using ServiceHub.Day08.Repositories;

namespace ServiceHub.Day08.Data;

public class WorkOrderRepository : IWorkOrderRepository
{
    private readonly List<WorkOrder> _orders = new();
    private int _nextId = 1;

    public WorkOrderRepository()
    {
        // Seed data
        _orders.Add(new WorkOrder { Id = _nextId++, CustomerId = 1, Description = "Gutter Cleaning", Status = "Scheduled" });
        _orders.Add(new WorkOrder { Id = _nextId++, CustomerId = 2, Description = "Lawn Mowing", Status = "Scheduled" });
        _orders.Add(new WorkOrder { Id = _nextId++, CustomerId = 1, Description = "Window Washing", Status = "InProgress" });
    }

    public Task AddAsync(WorkOrder order)
    {
        order.Id = _nextId++;
        _orders.Add(order);
        return Task.CompletedTask;
    }

    public Task<WorkOrder?> GetAsync(int id)
    {
        return Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));
    }

    public Task<List<WorkOrder>> GetAllAsync()
    {
        return Task.FromResult(new List<WorkOrder>(_orders));
    }
}
