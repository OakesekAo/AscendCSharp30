namespace ServiceHub.API.DTOs.Responses;

public record WorkOrderResponse(int Id, int CustomerId, string Description, string Status, DateTime CreatedAt);
