namespace ServiceHub.Day13.DTOs.Responses;

public record ErrorResponse(string Error, string? Details = null);
