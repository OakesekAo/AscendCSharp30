using ServiceHub.API.Models;
using ServiceHub.API.Repositories;

namespace ServiceHub.API.Data;

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
        _orders.Add(new WorkOrder { Id = _nextId++, CustomerId = 3, Description = "Pressure Washing", Status = "Scheduled" });
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

    public Task<List<WorkOrder>> GetByCustomerIdAsync(int customerId)
    {
        return Task.FromResult(_orders.Where(o => o.CustomerId == customerId).ToList());
    }

    public Task UpdateAsync(WorkOrder order)
    {
        var existing = _orders.FirstOrDefault(o => o.Id == order.Id);
        if (existing != null)
        {
            existing.Description = order.Description;
            existing.Status = order.Status;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order != null)
        {
            _orders.Remove(order);
        }
        return Task.CompletedTask;
    }
}
