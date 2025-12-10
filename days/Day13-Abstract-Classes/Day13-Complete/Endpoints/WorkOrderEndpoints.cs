using ServiceHub.Day13.DTOs.Requests;
using ServiceHub.Day13.DTOs.Responses;
using ServiceHub.Day13.Services;

namespace ServiceHub.Day13.Endpoints;

public static class WorkOrderEndpoints
{
    public static void MapWorkOrderEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/workorders")
            .WithTags("Work Orders");

        group.MapGet("/", GetAll)
            .WithName("GetWorkOrders")
            .WithOpenApi();

        group.MapGet("/paginated", GetPaginated)
            .WithName("GetWorkOrdersPaginated")
            .WithOpenApi();

        group.MapGet("/{id}", GetById)
            .WithName("GetWorkOrderById")
            .WithOpenApi();

        group.MapPost("/", Create)
            .WithName("CreateWorkOrder")
            .WithOpenApi();

        group.MapGet("/customer/{customerId}", GetByCustomerId)
            .WithName("GetWorkOrdersByCustomer")
            .WithOpenApi();

        group.MapGet("/status/{status}", GetByStatus)
            .WithName("GetWorkOrdersByStatus")
            .WithOpenApi();

        group.MapGet("/search/{searchTerm}", Search)
            .WithName("SearchWorkOrders")
            .WithOpenApi();

        group.MapPut("/{id}/status", UpdateStatus)
            .WithName("UpdateWorkOrderStatus")
            .WithOpenApi();
    }

    private static async Task<IResult> GetAll(WorkOrderService service)
    {
        try
        {
            var orders = await service.GetAllAsync();
            return Results.Ok(orders.Select(o => o.ToResponse()).ToList());
        }
        catch
        {
            return Results.StatusCode(500);
        }
    }

    private static async Task<IResult> GetPaginated(WorkOrderService service, int page = 1, int pageSize = 10, string? sortBy = null)
    {
        if (page < 1 || pageSize < 1)
            return Results.BadRequest(new ErrorResponse("Page and pageSize must be greater than 0"));

        try
        {
            var result = await service.GetPaginatedAsync(page, pageSize, sortBy);
            return Results.Ok(result);
        }
        catch
        {
            return Results.StatusCode(500);
        }
    }

    private static async Task<IResult> GetById(int id, WorkOrderService service)
    {
        if (id <= 0)
            return Results.BadRequest(new ErrorResponse("Invalid work order ID"));

        try
        {
            var order = await service.GetAsync(id);
            if (order == null)
                return Results.NotFound(new ErrorResponse($"Work order {id} not found"));

            return Results.Ok(order.ToResponse());
        }
        catch
        {
            return Results.StatusCode(500);
        }
    }

    private static async Task<IResult> Create(CreateWorkOrderRequest request, WorkOrderService service)
    {
        var errors = ValidateCreateWorkOrderRequest(request);
        if (errors.Any())
            return Results.BadRequest(new ErrorResponse("Validation failed", string.Join(", ", errors)));

        try
        {
            var order = await service.CreateAsync(request.CustomerId, request.Description);
            return Results.Created($"/workorders/{order.Id}", order.ToResponse());
        }
        catch
        {
            return Results.StatusCode(500);
        }
    }

    private static async Task<IResult> GetByCustomerId(int customerId, WorkOrderService service)
    {
        if (customerId <= 0)
            return Results.BadRequest(new ErrorResponse("Invalid customer ID"));

        try
        {
            var orders = await service.GetByCustomerIdAsync(customerId);
            return Results.Ok(orders.Select(o => o.ToResponse()).ToList());
        }
        catch
        {
            return Results.StatusCode(500);
        }
    }

    private static async Task<IResult> GetByStatus(string status, WorkOrderService service)
    {
        var validStatuses = new[] { "Scheduled", "InProgress", "Completed", "Canceled" };
        if (!validStatuses.Contains(status))
            return Results.BadRequest(new ErrorResponse($"Invalid status. Must be one of: {string.Join(", ", validStatuses)}"));

        try
        {
            var orders = await service.GetByStatusAsync(status);
            return Results.Ok(orders.Select(o => o.ToResponse()).ToList());
        }
        catch
        {
            return Results.StatusCode(500);
        }
    }

    private static async Task<IResult> Search(string searchTerm, WorkOrderService service)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return Results.BadRequest(new ErrorResponse("Search term is required"));

        try
        {
            var results = await service.SearchAsync(searchTerm);
            return Results.Ok(results.Select(o => o.ToResponse()).ToList());
        }
        catch
        {
            return Results.StatusCode(500);
        }
    }

    private static async Task<IResult> UpdateStatus(int id, UpdateWorkOrderStatusRequest request, WorkOrderService service)
    {
        if (id <= 0)
            return Results.BadRequest(new ErrorResponse("Invalid work order ID"));

        var validStatuses = new[] { "Scheduled", "InProgress", "Completed", "Canceled" };
        if (!validStatuses.Contains(request.Status))
            return Results.BadRequest(new ErrorResponse($"Invalid status. Must be one of: {string.Join(", ", validStatuses)}"));

        try
        {
            var order = await service.UpdateStatusAsync(id, request.Status);
            if (order == null)
                return Results.NotFound(new ErrorResponse($"Work order {id} not found"));

            return Results.Ok(order.ToResponse());
        }
        catch
        {
            return Results.StatusCode(500);
        }
    }

    private static List<string> ValidateCreateWorkOrderRequest(CreateWorkOrderRequest request)
    {
        var errors = new List<string>();

        if (request.CustomerId <= 0)
            errors.Add("CustomerId must be greater than 0");

        if (string.IsNullOrWhiteSpace(request.Description))
            errors.Add("Description is required");
        else if (request.Description.Length < 3)
            errors.Add("Description must be at least 3 characters");

        return errors;
    }
}
