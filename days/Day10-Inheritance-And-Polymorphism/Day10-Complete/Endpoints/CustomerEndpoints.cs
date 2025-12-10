using ServiceHub.Day10.DTOs.Requests;
using ServiceHub.Day10.DTOs.Responses;
using ServiceHub.Day10.Services;

namespace ServiceHub.Day10.Endpoints;

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
        try
        {
            var customers = await service.GetAllAsync();
            return Results.Ok(customers.Select(c => c.ToResponse()).ToList());
        }
        catch (Exception ex)
        {
            return Results.StatusCode(500);
        }
    }

    private static async Task<IResult> GetById(int id, CustomerService service)
    {
        if (id <= 0)
            return Results.BadRequest(new ErrorResponse("Invalid customer ID"));

        try
        {
            var customer = await service.GetAsync(id);
            if (customer == null)
                return Results.NotFound(new ErrorResponse($"Customer {id} not found"));

            return Results.Ok(customer.ToResponse());
        }
        catch (Exception ex)
        {
            return Results.StatusCode(500);
        }
    }

    private static async Task<IResult> Create(CreateCustomerRequest request, CustomerService service)
    {
        // Day 10: Professional validation
        var errors = ValidateCreateCustomerRequest(request);
        if (errors.Any())
            return Results.BadRequest(new ErrorResponse("Validation failed", string.Join(", ", errors)));

        try
        {
            var customer = await service.CreateAsync(request.Name, request.Email);
            return Results.Created($"/customers/{customer.Id}", customer.ToResponse());
        }
        catch (Exception ex)
        {
            return Results.StatusCode(500);
        }
    }

    private static List<string> ValidateCreateCustomerRequest(CreateCustomerRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Name))
            errors.Add("Name is required");
        else if (request.Name.Length < 2)
            errors.Add("Name must be at least 2 characters");

        if (string.IsNullOrWhiteSpace(request.Email))
            errors.Add("Email is required");
        else if (!request.Email.Contains("@"))
            errors.Add("Email must be valid");

        return errors;
    }
}
