using ServiceHub.API.DTOs.Requests;
using ServiceHub.API.Services;

namespace ServiceHub.API.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/customers")
            .WithTags("Customers");

        group.MapGet("/", GetAll)
            .WithName("GetCustomers")
            .WithOpenApi();

        group.MapGet("/{id}", GetById)
            .WithName("GetCustomer")
            .WithOpenApi();

        group.MapPost("/", Create)
            .WithName("CreateCustomer")
            .WithOpenApi();

        group.MapPut("/{id}", Update)
            .WithName("UpdateCustomer")
            .WithOpenApi();

        group.MapDelete("/{id}", Delete)
            .WithName("DeleteCustomer")
            .WithOpenApi();
    }

    private static async Task<IResult> GetAll(ICustomerService service)
    {
        var customers = await service.GetAllAsync();
        return Results.Ok(customers);
    }

    private static async Task<IResult> GetById(int id, ICustomerService service)
    {
        var customer = await service.GetAsync(id);
        return customer != null
            ? Results.Ok(customer)
            : Results.NotFound(new { error = $"Customer {id} not found" });
    }

    private static async Task<IResult> Create(CreateCustomerRequest request, ICustomerService service)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
            return Results.BadRequest(new { error = "Name and Email are required" });

        var customer = await service.CreateAsync(request);
        return Results.Created($"/customers/{customer.Id}", customer);
    }

    private static async Task<IResult> Update(int id, CreateCustomerRequest request, ICustomerService service)
    {
        var customer = await service.UpdateAsync(id, request);
        return customer != null
            ? Results.Ok(customer)
            : Results.NotFound(new { error = $"Customer {id} not found" });
    }

    private static async Task<IResult> Delete(int id, ICustomerService service)
    {
        await service.DeleteAsync(id);
        return Results.NoContent();
    }
}
