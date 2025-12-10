namespace ServiceHub.Day12.DTOs.Responses;

public record AnalyticsSummary(
    DateTime Timestamp,
    int TotalCustomers,
    int TotalWorkOrders,
    AnalyticsBreakdown ByStatus,
    double CompletionRatePercent
);

public record AnalyticsBreakdown(
    int Scheduled,
    int InProgress,
    int Completed,
    int Canceled
);
