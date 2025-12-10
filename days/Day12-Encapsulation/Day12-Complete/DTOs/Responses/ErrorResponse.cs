namespace ServiceHub.Day12.DTOs.Responses;

public record ErrorResponse(string Error, string? Details = null);
