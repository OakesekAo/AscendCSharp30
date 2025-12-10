using ServiceHub.Day12.Services;

namespace ServiceHub.Day12.Endpoints;

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

    private static async Task<IResult> GetSummary(AnalyticsService service)
    {
        try
        {
            var summary = await service.GetSummaryAsync();
            return Results.Ok(summary);
        }
        catch
        {
            return Results.StatusCode(500);
        }
    }
}
