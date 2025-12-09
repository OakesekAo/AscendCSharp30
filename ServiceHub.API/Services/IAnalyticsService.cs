namespace ServiceHub.API.Services;

public interface IAnalyticsService
{
    Task<object> GetSummaryAsync();
}
