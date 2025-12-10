using ServiceHub.Day09.DTOs.Requests;
using ServiceHub.Day09.Services;

namespace ServiceHub.Day09.Endpoints;

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
            .WithName("GetWorkOrderById")
            .WithOpenApi();

        group.MapPost("/", Create)
            .WithName("CreateWorkOrder")
            .WithOpenApi();
    }

    private static async Task<IResult> GetAll(WorkOrderService service)
    {
        var orders = await service.GetAllAsync();
        return Results.Ok(orders.Select(o => o.ToResponse()).ToList());
    }

    private static async Task<IResult> GetById(int id, WorkOrderService service)
    {
        var order = await service.GetAsync(id);
        return order != null
            ? Results.Ok(order.ToResponse())
            : Results.NotFound(new { error = $"Work order {id} not found" });
    }

    private static async Task<IResult> Create(CreateWorkOrderRequest request, WorkOrderService service)
    {
        if (request.CustomerId <= 0 || string.IsNullOrWhiteSpace(request.Description))
            return Results.BadRequest(new { error = "CustomerId and Description are required" });

        var order = await service.CreateAsync(request.CustomerId, request.Description);
        return Results.Created($"/workorders/{order.Id}", order.ToResponse());
    }
}
