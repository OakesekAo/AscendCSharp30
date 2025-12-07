# Day10-Complete — DTOs & API Contracts

This is the **refactored ServiceHub API with professional DTOs**.

**Builds on Day 09 with clean separation of API contract from domain models.**

## 🚀 Quick Start

```bash
cd Day10-Complete
dotnet run
```

## 📋 What This Program Does

Same endpoints as Day 09, but with **professional API design**:
- ✅ Request DTOs (CreateCustomerRequest)
- ✅ Response DTOs (CustomerResponse)
- ✅ Mapper extension methods
- ✅ Clean separation of concerns
- ✅ Only expose what clients need
- ✅ Version-safe API contract

## 💡 Key Differences from Day 09

**Day 09 (Direct domain models):**
```csharp
app.MapPost("/customers", (Customer customer, Service service) => 
    service.Create(customer)
);
```

**Day 10 (Using DTOs):**
```csharp
app.MapPost("/customers", (CreateCustomerRequest request, Service service) =>
{
    var customer = request.ToCustomer();
    service.Create(customer);
    return Results.Created(..., customer.ToResponse());
});
```

## 📊 DTOs Included

```csharp
// Requests (what clients send)
record CreateCustomerRequest(string Name, string Email);
record CreateWorkOrderRequest(int CustomerId, string Description, string Status);

// Responses (what API returns)
record CustomerResponse(int Id, string Name, string Email);
record WorkOrderResponse(int Id, int CustomerId, string Description, string Status);
```

## 🎯 Mappers

Clean extension methods for conversion:

```csharp
customer.ToResponse()           // Domain → DTO
request.ToCustomer()            // DTO → Domain
order.ToResponse()
request.ToWorkOrder()
```

## ✅ Endpoints (Same as Day 09)

```
GET  /customers
POST /customers
GET  /customers/{id}
GET  /workorders
POST /workorders
GET  /workorders/{id}
GET  /health
```

## 🎬 What Day 11 Will Do

Day 11 refactors to **async/await** — making repositories and services properly asynchronous.

## 🟦 ServiceHub Context

Now you have professional API design. The API contract is clean, versioned, and separated from implementation. This is the pattern used in enterprise applications worldwide.

---

## 🟦 ServiceHub Context  
ServiceHub will expose data for customers and work orders using DTOs.  
These contracts define how the frontend and API communicate.  
Your DTO choices today will influence how easily Blazor integrates in Week 3.

