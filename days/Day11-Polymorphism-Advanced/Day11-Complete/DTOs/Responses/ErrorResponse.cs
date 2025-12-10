namespace ServiceHub.Day11.DTOs.Responses;

public record ErrorResponse(string Error, string? Details = null);
