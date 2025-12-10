using ServiceHub.Day09.Models;

namespace ServiceHub.Day09.Repositories;

public interface IWorkOrderRepository
{
    Task AddAsync(WorkOrder order);
    Task<WorkOrder?> GetAsync(int id);
    Task<List<WorkOrder>> GetAllAsync();
}
