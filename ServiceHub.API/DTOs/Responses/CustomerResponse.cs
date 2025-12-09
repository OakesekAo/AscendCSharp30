namespace ServiceHub.API.DTOs.Responses;

public record CustomerResponse(int Id, string Name, string Email, DateTime CreatedAt);
