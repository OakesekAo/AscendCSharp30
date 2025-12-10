using ServiceHub.Day08.Models;

namespace ServiceHub.Day08.Repositories;

public interface IWorkOrderRepository
{
    Task AddAsync(WorkOrder order);
    Task<WorkOrder?> GetAsync(int id);
    Task<List<WorkOrder>> GetAllAsync();
}
