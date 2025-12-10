using ServiceHub.Day11.Models;

namespace ServiceHub.Day11.Repositories;

public interface IWorkOrderRepository
{
    Task AddAsync(WorkOrder order);
    Task<WorkOrder?> GetAsync(int id);
    Task<List<WorkOrder>> GetAllAsync();
    Task<List<WorkOrder>> GetByCustomerIdAsync(int customerId);
    Task<List<WorkOrder>> GetByStatusAsync(string status);
    Task<List<WorkOrder>> SearchAsync(string searchTerm);
    Task UpdateAsync(WorkOrder order);
}
