using ServiceHub.Day10.Models;

namespace ServiceHub.Day10.Repositories;

public interface IWorkOrderRepository
{
    Task AddAsync(WorkOrder order);
    Task<WorkOrder?> GetAsync(int id);
    Task<List<WorkOrder>> GetAllAsync();
    Task<List<WorkOrder>> GetByCustomerIdAsync(int customerId);
    Task UpdateAsync(WorkOrder order);
}
