namespace ServiceHub.Day11.DTOs.Responses;

public record WorkOrderResponse(int Id, int CustomerId, string Description, string Status);
