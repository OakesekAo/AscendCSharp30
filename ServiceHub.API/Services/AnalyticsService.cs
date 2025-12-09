using ServiceHub.API.Repositories;

namespace ServiceHub.API.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IWorkOrderRepository _orderRepository;

    public AnalyticsService(ICustomerRepository customerRepository, IWorkOrderRepository orderRepository)
    {
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
    }

    public async Task<object> GetSummaryAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        var orders = await _orderRepository.GetAllAsync();

        var completed = orders.Count(o => o.Status == "Completed");
        var total = orders.Count;
        var completionRate = total > 0 ? Math.Round((double)completed / total * 100, 2) : 0;

        return new
        {
            timestamp = DateTime.UtcNow,
            total_customers = customers.Count,
            total_workorders = total,
            by_status = new
            {
                scheduled = orders.Count(o => o.Status == "Scheduled"),
                in_progress = orders.Count(o => o.Status == "InProgress"),
                completed = completed,
                canceled = orders.Count(o => o.Status == "Canceled")
            },
            completion_rate_percent = completionRate
        };
    }
}
