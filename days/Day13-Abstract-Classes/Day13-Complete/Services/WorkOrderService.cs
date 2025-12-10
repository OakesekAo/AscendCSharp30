using ServiceHub.Day13.DTOs.Responses;
using ServiceHub.Day13.Models;
using ServiceHub.Day13.Repositories;

namespace ServiceHub.Day13.Services;

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

    public async Task<PaginatedResponse<WorkOrderResponse>> GetPaginatedAsync(int page = 1, int pageSize = 10, string? sortBy = null)
    {
        var all = await _repository.GetAllAsync();

        // Apply sorting
        var sorted = sortBy switch
        {
            "status" => all.OrderBy(o => o.Status).ToList(),
            "customer" => all.OrderBy(o => o.CustomerId).ToList(),
            "description" => all.OrderBy(o => o.Description).ToList(),
            _ => all
        };

        var total = sorted.Count;
        var totalPages = (int)Math.Ceiling((double)total / pageSize);
        
        var items = sorted
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => o.ToResponse())
            .ToList();

        return new PaginatedResponse<WorkOrderResponse>(items, page, pageSize, total, totalPages);
    }
}

public static class WorkOrderExtensions
{
    public static WorkOrderResponse ToResponse(this WorkOrder order)
        => new(order.Id, order.CustomerId, order.Description, order.Status);
}
