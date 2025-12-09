using ServiceHub.API.DTOs.Requests;
using ServiceHub.API.Services;

namespace ServiceHub.API.Endpoints;

public static class WorkOrderEndpoints
{
    public static void MapWorkOrderEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/workorders")
            .WithTags("Work Orders");

        group.MapGet("/", GetAll)
            .WithName("GetWorkOrders")
            .WithOpenApi();

        group.MapGet("/{id}", GetById)
            .WithName("GetWorkOrder")
            .WithOpenApi();

        group.MapPost("/", Create)
            .WithName("CreateWorkOrder")
            .WithOpenApi();

        group.MapPut("/{id}/status", UpdateStatus)
            .WithName("UpdateWorkOrderStatus")
            .WithOpenApi();

        group.MapDelete("/{id}", Delete)
            .WithName("DeleteWorkOrder")
            .WithOpenApi();

        group.MapGet("/customer/{customerId}", GetByCustomerId)
            .WithName("GetWorkOrdersByCustomer")
            .WithOpenApi();
    }

    private static async Task<IResult> GetAll(IWorkOrderService service)
    {
        var orders = await service.GetAllAsync();
        return Results.Ok(orders);
    }

    private static async Task<IResult> GetById(int id, IWorkOrderService service)
    {
        var order = await service.GetAsync(id);
        return order != null
            ? Results.Ok(order)
            : Results.NotFound(new { error = $"Work order {id} not found" });
    }

    private static async Task<IResult> Create(CreateWorkOrderRequest request, IWorkOrderService service)
    {
        if (request.CustomerId <= 0 || string.IsNullOrWhiteSpace(request.Description))
            return Results.BadRequest(new { error = "CustomerId and Description are required" });

        var order = await service.CreateAsync(request);
        return Results.Created($"/workorders/{order.Id}", order);
    }

    private static async Task<IResult> UpdateStatus(int id, UpdateWorkOrderStatusRequest request, IWorkOrderService service)
    {
        var validStatuses = new[] { "Scheduled", "InProgress", "Completed", "Canceled" };
        if (!validStatuses.Contains(request.Status))
            return Results.BadRequest(new { error = "Invalid status" });

        var order = await service.UpdateStatusAsync(id, request);
        return order != null
            ? Results.Ok(order)
            : Results.NotFound(new { error = $"Work order {id} not found" });
    }

    private static async Task<IResult> Delete(int id, IWorkOrderService service)
    {
        await service.DeleteAsync(id);
        return Results.NoContent();
    }

    private static async Task<IResult> GetByCustomerId(int customerId, IWorkOrderService service)
    {
        var orders = await service.GetByCustomerIdAsync(customerId);
        return Results.Ok(orders);
    }
}
