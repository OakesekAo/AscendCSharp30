using ServiceHub.API.Models;

namespace ServiceHub.API.Repositories;

public interface IWorkOrderRepository
{
    Task AddAsync(WorkOrder order);
    Task<WorkOrder?> GetAsync(int id);
    Task<List<WorkOrder>> GetAllAsync();
    Task<List<WorkOrder>> GetByCustomerIdAsync(int customerId);
    Task UpdateAsync(WorkOrder order);
    Task DeleteAsync(int id);
}
