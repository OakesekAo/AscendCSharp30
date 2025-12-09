using ServiceHub.API.Data;
using ServiceHub.API.Repositories;
using ServiceHub.API.Services;

namespace ServiceHub.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceHubServices(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();

        // Services
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IWorkOrderService, WorkOrderService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();

        return services;
    }
}
