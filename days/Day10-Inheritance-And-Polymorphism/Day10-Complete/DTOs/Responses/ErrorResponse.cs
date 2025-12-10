namespace ServiceHub.Day10.DTOs.Responses;

public record ErrorResponse(string Error, string? Details = null);
