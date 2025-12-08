# Day 10 — DTOs & API Contracts

## 🚀 Building Professional APIs

**Maya's Message:**
> "Your endpoints work. Now let's design them properly. Clean API contracts matter — they're your promise to clients."

Today, you'll learn **Data Transfer Objects (DTOs)** — separating your API contract from internal domain models. This is **professional software design**.

---

## 🎯 Learning Objectives

1. **Understand the problem** — Why exposing domain models is bad
2. **Learn DTOs** — What they are and when to use them
3. **Create request/response types** — Shape your API contract
4. **Design clean APIs** — Version-safe, maintainable endpoints
5. **Use mappers** — Convert between domain and DTO layers
6. **Apply to ServiceHub** — Customer and work order DTOs

---

## 📋 Prerequisites

- Day 09 complete (understand Minimal APIs)
- Comfortable with C# records and classes
- ~90 minutes

---

## Setup

```bash
mkdir Day10-DTOs
cd Day10-DTOs
dotnet new web
```

---

## Step 1: The Problem Without DTOs

**Your domain model (internal, complex):**

```csharp
class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }           // Internal!
    public byte[] PasswordHash { get; set; }          // Never expose!
    public string InternalNotes { get; set; }         // Private!
    public List<Order> Orders { get; set; }           // Heavyweight!
}
```

**Exposing it directly (BAD):**

```csharp
app.MapPost("/customers", (Customer customer) => 
{
    // Now client can:
    // - Set CreatedAt to whatever they want
    // - Send PasswordHash directly
    // - See internal notes
    // - Send huge Orders collection
    // Result: Security holes, bloated responses
});
```

**Problems:**
- ❌ Expose internal fields
- ❌ Security vulnerabilities
- ❌ Can't validate input
- ❌ Can't version API
- ❌ Tight coupling to domain

---

## Step 2: The Solution: DTOs

**Request DTO (what client sends):**

```csharp
record CreateCustomerRequest(string Name, string Email);
```

**Response DTO (what API returns):**

```csharp
record CustomerResponse(int Id, string Name, string Email);
```

**Benefits:**
- ✅ Only expose what you want
- ✅ Clear API contract
- ✅ Input validation
- ✅ API versioning
- ✅ Loose coupling

---

## Step 3: Using DTOs in Endpoints

**Before (exposing domain directly):**

```csharp
app.MapPost("/customers", (Customer customer, Service service) =>
{
    service.Create(customer);
    return Results.Created($"/customers/{customer.Id}", customer);
});
```

**After (using DTOs):**

```csharp
app.MapPost("/customers", (CreateCustomerRequest request, CustomerService service) =>
{
    // Convert request to domain
    var customer = new Customer { Name = request.Name, Email = request.Email };
    
    // Create in service
    service.CreateCustomer(customer);
    
    // Convert response to DTO
    var response = new CustomerResponse(customer.Id, customer.Name, customer.Email);
    
    return Results.Created($"/customers/{customer.Id}", response);
});
```

---

## Step 4: Clean Mapping with Extension Methods

**Create mapper extension methods:**

```csharp
static class Mappers
{
    // Request DTO → Domain Model
    public static Customer ToCustomer(this CreateCustomerRequest request) =>
        new() { Name = request.Name, Email = request.Email };
    
    // Domain Model → Response DTO
    public static CustomerResponse ToResponse(this Customer customer) =>
        new(customer.Id, customer.Name, customer.Email);
}
```

**Now endpoints are clean:**

```csharp
app.MapPost("/customers", (CreateCustomerRequest request, CustomerService service) =>
{
    var customer = request.ToCustomer();           // ← Mapper
    service.CreateCustomer(customer);
    return Results.Created($"/customers/{customer.Id}", customer.ToResponse()); // ← Mapper
});
```

---

## Step 5: ServiceHub DTOs

**For this course, use C# records (concise, clean):**

```csharp
// Requests (what clients send)
record CreateCustomerRequest(string Name, string Email);
record CreateWorkOrderRequest(int CustomerId, string Description, string Status);

// Responses (what API returns)
record CustomerResponse(int Id, string Name, string Email);
record WorkOrderResponse(int Id, int CustomerId, string Description, string Status);
```

**Or use classes if you need more control:**

```csharp
class CreateCustomerRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
}
```

---

## Step 6: Validation with DTOs

**DTOs make validation easy:**

```csharp
app.MapPost("/customers", (CreateCustomerRequest request, CustomerService service) =>
{
    // Validate at API boundary
    if (string.IsNullOrWhiteSpace(request.Name))
        return Results.BadRequest("Name is required");
    
    if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@"))
        return Results.BadRequest("Valid email is required");
    
    var customer = request.ToCustomer();
    service.CreateCustomer(customer);
    return Results.Created($"/customers/{customer.Id}", customer.ToResponse());
});
```

---

## Mini Challenge: Refactor Day 09 to Use DTOs

**Refactor your Day 09 endpoints:**

1. Create request DTOs:
   - `CreateCustomerRequest`
   - `CreateWorkOrderRequest`

2. Create response DTOs:
   - `CustomerResponse`
   - `WorkOrderResponse`

3. Create mapper extension methods

4. Update all endpoints to use DTOs

5. Add validation in endpoints

**Result:**
- Clean API contract
- Separated domain from API
- Professional, versioned API

---

## Step 7: API Versioning with DTOs

With DTOs, versioning is clean:

```csharp
// Version 1.0
record CustomerResponse(int Id, string Name, string Email);

// Version 2.0 (added CreatedDate)
record CustomerResponseV2(int Id, string Name, string Email, DateTime CreatedDate);

// Both endpoints exist:
app.MapGet("/v1/customers", GetCustomersV1);
app.MapGet("/v2/customers", GetCustomersV2);
```

---

## ✅ Checklist

- [ ] You understand the problem without DTOs
- [ ] You can create request DTOs
- [ ] You can create response DTOs
- [ ] You use mappers for conversion
- [ ] You validate input in endpoints
- [ ] Your endpoints use DTOs consistently
- [ ] You refactored Day 09 endpoints

---

## 🔗 Next Steps

Day 11: **Async/Await** — Make your repositories and services truly asynchronous.

---

## 📚 Resources

- <a href="https://dotnet.microsoft.com/learn/" target="_blank">Getting Started with .NET</a>
- <a href="https://github.com/OakesekAo/AscendCSharp30" target="_blank">AscendCSharp30 Repository</a>

---

## 🎯 Why This Matters for ServiceHub

**Week 2:**
- Day 09: Build endpoints that expose domain models
- Day 10: Refactor to use DTOs (clean API contract)
- Days 11+: Advanced patterns (async, error handling, etc.)

**Week 3+:**
- When you add database (EF Core), DTOs protect you from exposing schema
- When you add Blazor UI, DTOs define the contract between frontend and API
- When you deploy, DTOs make versioning safe

**DTOs are how enterprise apps stay flexible and maintainable.**

---

**This is the difference between "it works" and "it's professional."** See you on Day 11! 🚀
