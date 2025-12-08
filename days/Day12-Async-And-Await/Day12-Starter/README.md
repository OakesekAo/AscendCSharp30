# Day 12 — Error Handling & Status Codes

## 🚀 Building Robust APIs

**Maya's Message:**
> "Things go wrong. APIs that handle errors gracefully are professional APIs. Learn to respond with proper status codes and error details."

Today, you'll learn **error handling** — catching exceptions, returning appropriate HTTP status codes, and providing useful error information to clients.

---

## 🎯 Learning Objectives

1. **Understand HTTP status codes** — What each code means
2. **Handle exceptions** — Catch and respond appropriately
3. **Use middleware for errors** — Centralized error handling
4. **Return ProblemDetails** — RFC 7807 standard error format
5. **Validate input** — Return 400 Bad Request appropriately
6. **Apply to ServiceHub** — Proper error responses

---

## 📋 Prerequisites

- Days 09-11 complete (async APIs)
- Understand HTTP status codes
- ~90 minutes

---

## Setup

```bash
mkdir Day12-ErrorHandling
cd Day12-ErrorHandling
dotnet new web
```

---

## Step 1: HTTP Status Codes

**Common status codes you'll use:**

```
2xx Success
  200 OK           - Request succeeded
  201 Created      - Resource created
  204 No Content   - Success, no response body

4xx Client Error
  400 Bad Request        - Invalid input
  401 Unauthorized       - Not authenticated
  403 Forbidden          - Not authorized
  404 Not Found          - Resource doesn't exist
  409 Conflict           - Request conflicts with state

5xx Server Error
  500 Internal Server Error - Unexpected error
  503 Service Unavailable   - Service down
```

---

## Step 2: Returning Status Codes

**With Results helper:**

```csharp
app.MapGet("/customers/{id}", async (int id, CustomerService service) =>
{
    var customer = await service.GetCustomerAsync(id);
    return customer != null 
        ? Results.Ok(customer.ToResponse())              // 200
        : Results.NotFound();                             // 404
});

app.MapPost("/customers", async (CreateCustomerRequest request, CustomerService service) =>
{
    if (string.IsNullOrWhiteSpace(request.Name))
        return Results.BadRequest("Name required");       // 400
    
    var customer = request.ToCustomer();
    await service.CreateCustomerAsync(customer);
    return Results.Created(
        $"/customers/{customer.Id}", 
        customer.ToResponse()                             // 201
    );
});
```

---

## Step 3: Error Handling Middleware

**Centralized error handling:**

```csharp
app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (Exception ex)
    {
        // Handle uncaught exceptions
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        
        await context.Response.WriteAsJsonAsync(new 
        { 
            error = "Internal server error",
            message = ex.Message
        });
    }
});
```

---

## Step 4: ProblemDetails (RFC 7807)

**Standard error response format:**

```csharp
record ProblemDetails(
    string Title,
    string Detail, 
    int Status,
    string Instance
);

// Usage
app.MapPost("/customers", async (CreateCustomerRequest request, CustomerService service) =>
{
    if (string.IsNullOrWhiteSpace(request.Name))
        return Results.BadRequest(
            new ProblemDetails(
                Title: "Validation Error",
                Detail: "Name is required",
                Status: 400,
                Instance: "/customers"
            )
        );
    
    // ...
});
```

---

## Step 5: Validation at API Boundary

**Validate input before service calls:**

```csharp
app.MapPost("/workorders", async (CreateWorkOrderRequest request, WorkOrderService service) =>
{
    // Validate synchronously first
    var validationError = ValidateWorkOrderRequest(request);
    if (validationError != null)
        return Results.BadRequest(validationError);
    
    // If valid, call service
    var order = request.ToWorkOrder();
    await service.CreateWorkOrderAsync(order);
    return Results.Created($"/workorders/{order.Id}", order.ToResponse());
});

// Helper validation method
ProblemDetails? ValidateWorkOrderRequest(CreateWorkOrderRequest request)
{
    if (request.CustomerId <= 0)
        return new("Validation Error", "CustomerId must be > 0", 400, "/workorders");
    
    if (string.IsNullOrWhiteSpace(request.Description))
        return new("Validation Error", "Description required", 400, "/workorders");
    
    if (!new[] { "Scheduled", "InProgress", "Completed" }.Contains(request.Status))
        return new("Validation Error", "Invalid status", 400, "/workorders");
    
    return null;
}
```

---

## Step 6: Not Found Errors

**Handle missing resources:**

```csharp
app.MapGet("/customers/{id}", async (int id, CustomerService service) =>
{
    var customer = await service.GetCustomerAsync(id);
    
    if (customer == null)
        return Results.NotFound(
            new ProblemDetails(
                Title: "Not Found",
                Detail: $"Customer {id} not found",
                Status: 404,
                Instance: $"/customers/{id}"
            )
        );
    
    return Results.Ok(customer.ToResponse());
});
```

---

## Mini Challenge: Add Error Handling to Day 11

**Add error handling to your async API:**

1. Add error handling middleware
2. Return proper status codes (200, 201, 400, 404)
3. Use ProblemDetails for errors
4. Validate input on POST endpoints
5. Handle not found scenarios

**Example validation:**

```csharp
if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
    return Results.BadRequest("Name and Email required");
```

---

## ✅ Checklist

- [ ] You understand HTTP status codes
- [ ] You return appropriate status codes
- [ ] You have error handling middleware
- [ ] You use ProblemDetails format
- [ ] You validate input on endpoints
- [ ] You handle not found (404) scenarios
- [ ] Your API responds with proper errors

---

## 🔗 Next Steps

Day 13: **JSON & I/O** — Working with files and JSON serialization.

---

## 📚 Resources

- <a href="https://dotnet.microsoft.com/learn/" target="_blank">Getting Started with .NET</a>
- <a href="https://github.com/OakesekAo/AscendCSharp30" target="_blank">AscendCSharp30 Repository</a>

---

**Professional error handling makes confident APIs.** See you on Day 13! 🚀
