# Day 09 — DTOs & API Contracts

## 🚀 Building on Day 08

**Maya's Message:**
> "Your Day 08 API works, but we need to be more careful about what we expose. DTOs separate our internal data from what we show clients."

Today, you'll learn **DTOs (Data Transfer Objects)** — a professional pattern for separating API contracts from domain models.

---

## 🎯 Learning Objectives

1. **Understand the problem:** Exposing domain models directly is brittle
2. **Learn DTOs:** Separate API contracts from domain models
3. **Create mappers:** Convert between domain and API models
4. **Organize endpoints:** Group endpoints by resource
5. **Handle validation:** Validate inputs with proper error responses
6. **Apply to ServiceHub:** Professional API contracts

---

## 📋 Prerequisites

Before you start:
- Day 08 complete and working
- Understand the N-tier architecture from Day 08
- ~90 minutes

---

## What Changed Since Day 08

Day 08 had:
```
Program.cs
├── DI Container setup
├── Basic endpoints (GET /customers, POST /customers, etc.)
├── Inline DTOs (record types at bottom)
└── All in one file
```

Day 09 adds:
```
DTOs/                     (NEW!)
├── Requests/
│   ├── CreateCustomerRequest.cs
│   └── CreateWorkOrderRequest.cs
└── Responses/
    ├── CustomerResponse.cs
    └── WorkOrderResponse.cs

Endpoints/                (NEW!)
├── CustomerEndpoints.cs
├── WorkOrderEndpoints.cs
└── AnalyticsEndpoints.cs

(Models, Repositories, Services stay the same)
```

---

## Step 1: Why DTOs?

**Problem: Exposing domain models directly**

```csharp
// Day 08: Domain model directly in API response
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string InternalNotes { get; set; }    // Should be hidden!
    public DateTime CreatedAt { get; set; }      // Might expose too much
}

// Client sees InternalNotes - security issue!
```

**Solution: DTOs for API contracts**

```csharp
// Request (what client sends)
record CreateCustomerRequest(string Name, string Email);

// Response (what API returns)
record CustomerResponse(int Id, string Name, string Email);

// Domain model (internal only)
class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string InternalNotes { get; set; }    // Client never sees this
}
```

**Benefits:**
- ✅ Control what's exposed
- ✅ Separate API from domain
- ✅ Easy to change domain without breaking clients
- ✅ Professional, secure API design

---

## Step 2: Create Request DTOs

```csharp
// DTOs/Requests/CreateCustomerRequest.cs
namespace ServiceHub.DTOs.Requests;

public record CreateCustomerRequest(string Name, string Email);
```

```csharp
// DTOs/Requests/CreateWorkOrderRequest.cs
namespace ServiceHub.DTOs.Requests;

public record CreateWorkOrderRequest(int CustomerId, string Description);
```

```csharp
// DTOs/Requests/UpdateWorkOrderStatusRequest.cs
namespace ServiceHub.DTOs.Requests;

public record UpdateWorkOrderStatusRequest(string Status);
```

---

## Step 3: Create Response DTOs

```csharp
// DTOs/Responses/CustomerResponse.cs
namespace ServiceHub.DTOs.Responses;

public record CustomerResponse(int Id, string Name, string Email);
```

```csharp
// DTOs/Responses/WorkOrderResponse.cs
namespace ServiceHub.DTOs.Responses;

public record WorkOrderResponse(int Id, int CustomerId, string Description, string Status);
```

---

## Step 4: Create Mappers (Extension Methods)

**Convert domain models to DTOs:**

```csharp
// In CustomerService.cs
public static class CustomerExtensions
{
    public static CustomerResponse ToResponse(this Customer customer)
        => new(customer.Id, customer.Name, customer.Email);
}

// Usage
var customer = await _repository.GetAsync(1);
var response = customer.ToResponse();  // Domain → DTO
```

---

## Step 5: Organize Endpoints

**Instead of everything in Program.cs:**

```csharp
// Endpoints/CustomerEndpoints.cs
namespace ServiceHub.Endpoints;

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
    }

    private static async Task<IResult> GetAll(CustomerService service)
    {
        var customers = await service.GetAllAsync();
        return Results.Ok(customers.Select(c => c.ToResponse()));
    }

    private static async Task<IResult> GetById(int id, CustomerService service)
    {
        var customer = await service.GetAsync(id);
        return customer != null
            ? Results.Ok(customer.ToResponse())
            : Results.NotFound();
    }

    private static async Task<IResult> Create(CreateCustomerRequest request, CustomerService service)
    {
        var customer = await service.CreateAsync(request.Name, request.Email);
        return Results.Created($"/customers/{customer.Id}", customer.ToResponse());
    }
}

// Program.cs
app.MapCustomerEndpoints();
app.MapWorkOrderEndpoints();
```

**Benefits:**
- ✅ Program.cs stays clean
- ✅ Each endpoint file handles one resource
- ✅ Easy to find and update endpoints
- ✅ Professional code organization

---

## Step 6: Mini Challenge

**Refactor your Day 08 code to add:**

1. Create `DTOs/Requests/` folder with CreateCustomerRequest, CreateWorkOrderRequest
2. Create `DTOs/Responses/` folder with CustomerResponse, WorkOrderResponse
3. Add mapper extension methods in Services
4. Create `Endpoints/CustomerEndpoints.cs` and `Endpoints/WorkOrderEndpoints.cs`
5. Call `app.MapCustomerEndpoints()` and `app.MapWorkOrderEndpoints()` in Program.cs
6. Update endpoints to use DTOs instead of domain models
7. Test in Swagger - should work exactly the same!

---

## ✅ Checklist

- [ ] Created DTOs/Requests/ folder with request records
- [ ] Created DTOs/Responses/ folder with response records
- [ ] Created mapper extension methods
- [ ] Organized endpoints into separate files
- [ ] Updated Program.cs to call MapCustomerEndpoints(), etc.
- [ ] All endpoints use DTOs for requests/responses
- [ ] Code compiles without errors
- [ ] Tested in Swagger - endpoints work correctly
- [ ] Compared to Day 09 Complete example

---

## 🔗 Next Steps

Day 10: **Error Handling & Validation** — Professional error responses.

---

## 📚 Resources

- <a href="https://docs.microsoft.com/dotnet/csharp/language-reference/builtin-types/record" target="_blank">C# Records</a>
- <a href="https://github.com/OakesekAo/AscendCSharp30" target="_blank">AscendCSharp30 Repository</a>

---

**You're building a professional API.** See you on Day 10! 🚀
