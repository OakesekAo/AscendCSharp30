namespace ServiceHub.Day13.DTOs.Responses;

public record PaginatedResponse<T>(List<T> Items, int Page, int PageSize, int TotalCount, int TotalPages)
{
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}
