using ServiceHub.Day12.Repositories;

namespace ServiceHub.Day12.Services;

public class AnalyticsService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IWorkOrderRepository _workOrderRepository;

    public AnalyticsService(ICustomerRepository customerRepository, IWorkOrderRepository workOrderRepository)
    {
        _customerRepository = customerRepository;
        _workOrderRepository = workOrderRepository;
    }

    public async Task<object> GetSummaryAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        var orders = await _workOrderRepository.GetAllAsync();

        var scheduled = orders.Count(o => o.Status == "Scheduled");
        var inProgress = orders.Count(o => o.Status == "InProgress");
        var completed = orders.Count(o => o.Status == "Completed");
        var canceled = orders.Count(o => o.Status == "Canceled");

        var completionRate = orders.Count > 0 
            ? Math.Round((double)completed / orders.Count * 100, 2) 
            : 0;

        return new
        {
            timestamp = DateTime.UtcNow,
            total_customers = customers.Count,
            total_workorders = orders.Count,
            by_status = new
            {
                scheduled,
                in_progress = inProgress,
                completed,
                canceled
            },
            completion_rate_percent = completionRate
        };
    }
}
