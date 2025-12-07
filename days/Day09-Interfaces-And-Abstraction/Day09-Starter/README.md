# Day 09 — Minimal API Foundations

## 🚀 Week 2: Building ServiceHub API v0.1

**Maya's Message:**
> "You have solid DI foundations. Now let's expose the data layer via REST API. Your first endpoints."

Today, you'll build your **first Minimal API endpoints** using ASP.NET Core — exposing your repositories and services via HTTP.

---

## 🎯 Learning Objectives

1. **Understand Minimal APIs** — Lightweight REST endpoint building
2. **Create route handlers** — GET, POST endpoints
3. **Inject services** — Use DI in endpoints
4. **Return JSON** — Serialize responses
5. **Accept input** — Query params, body data
6. **Apply to ServiceHub** — Customer and work order endpoints

---

## 📋 Prerequisites

Before you start:
- Day 08 complete (understand DI)
- Familiar with REST concepts (GET, POST, HTTP status codes)
- ~90 minutes

---

## Setup

```bash
mkdir Day09-MinimalAPI
cd Day09-MinimalAPI
dotnet new web
```

This creates an ASP.NET Core web project (not console).

---

## Step 1: What Are Minimal APIs?

Minimal APIs are a lightweight way to build REST endpoints without controllers:

```csharp
// Traditional controller (verbose)
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    public IActionResult GetAll() => Ok(data);
}

// Minimal API (concise)
app.MapGet("/api/customers", () => data);
```

**Minimal APIs are modern, clean, and perfect for learning.**

---

## Step 2: Basic Endpoints

```csharp
var builder = WebApplicationBuilder.CreateBuilder(args);
var app = builder.Build();

// GET endpoint
app.MapGet("/", () => "Hello from ServiceHub API!");

app.Run();
```

**Result:**
- GET `http://localhost:5000/` returns `"Hello from ServiceHub API!"`

---

## Step 3: Return JSON

```csharp
app.MapGet("/customers", () => new
{
    id = 1,
    name = "Alice",
    email = "alice@example.com"
});
```

**Result:** Automatically serialized to JSON
```json
{"id":1,"name":"Alice","email":"alice@example.com"}
```

---

## Step 4: Inject Services

```csharp
// Register services
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<CustomerService>();

var app = builder.Build();

// Use injected service
app.MapGet("/customers", (CustomerService service) => 
    service.ListCustomers()
);
```

**DI container automatically provides the service!**

---

## Step 5: POST Endpoint (Create)

```csharp
app.MapPost("/customers", (Customer customer, CustomerService service) =>
{
    service.CreateCustomer(customer.Id, customer.Name, customer.Email);
    return Results.Created($"/customers/{customer.Id}", customer);
});
```

---

## Step 6: Route Parameters

```csharp
app.MapGet("/customers/{id}", (int id, CustomerService service) =>
{
    var customer = service.GetCustomer(id);
    return customer != null ? Results.Ok(customer) : Results.NotFound();
});
```

---

## Mini Challenge: ServiceHub API v0.1

**Build these endpoints:**

1. `GET /customers` — List all customers
2. `GET /customers/{id}` — Get one customer
3. `POST /customers` — Create customer
4. `GET /workorders` — List all work orders
5. `GET /workorders/{id}` — Get one work order
6. `POST /workorders` — Create work order

**Example requests:**
```bash
# Get all customers
GET http://localhost:5000/customers

# Create customer
POST http://localhost:5000/customers
Content-Type: application/json
{"id":1,"name":"Alice","email":"alice@example.com"}
```

---

## ✅ Checklist

- [ ] You understand Minimal APIs
- [ ] You can create GET endpoints
- [ ] You can create POST endpoints
- [ ] You can inject services into endpoints
- [ ] You return JSON responses
- [ ] You handle route parameters
- [ ] You built the customer/work order endpoints

---

## 🔗 Next Steps

Day 10: **DTOs & API Contracts** — Separate API models from domain models.

---

## 📚 Resources

- <a href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis" target="_blank">Minimal APIs Overview</a>
- <a href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing" target="_blank">Routing in ASP.NET Core</a>

---

**You're building ServiceHub's backbone.** See you on Day 10! 🚀
