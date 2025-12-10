using ServiceHub.Day09.DTOs.Requests;
using ServiceHub.Day09.Services;

namespace ServiceHub.Day09.Endpoints;

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
            .WithName("GetCustomerById")
            .WithOpenApi();

        group.MapPost("/", Create)
            .WithName("CreateCustomer")
            .WithOpenApi();
    }

    private static async Task<IResult> GetAll(CustomerService service)
    {
        var customers = await service.GetAllAsync();
        return Results.Ok(customers.Select(c => c.ToResponse()).ToList());
    }

    private static async Task<IResult> GetById(int id, CustomerService service)
    {
        var customer = await service.GetAsync(id);
        return customer != null
            ? Results.Ok(customer.ToResponse())
            : Results.NotFound(new { error = $"Customer {id} not found" });
    }

    private static async Task<IResult> Create(CreateCustomerRequest request, CustomerService service)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
            return Results.BadRequest(new { error = "Name and Email are required" });

        var customer = await service.CreateAsync(request.Name, request.Email);
        return Results.Created($"/customers/{customer.Id}", customer.ToResponse());
    }
}
