using ServiceHub.API.Services;

namespace ServiceHub.API.Endpoints;

public static class AnalyticsEndpoints
{
    public static void MapAnalyticsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/analytics")
            .WithTags("Analytics");

        group.MapGet("/summary", GetSummary)
            .WithName("GetAnalyticsSummary")
            .WithOpenApi();
    }

    private static async Task<IResult> GetSummary(IAnalyticsService service)
    {
        var summary = await service.GetSummaryAsync();
        return Results.Ok(summary);
    }
}
