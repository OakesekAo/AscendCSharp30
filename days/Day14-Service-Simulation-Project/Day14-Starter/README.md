# Day 14 — Week 2 Capstone: Complete API Project

## 🎉 Week 2 Complete!

**Maya's Message:**
> "You've learned everything: APIs, DTOs, async, error handling, JSON, search, filtering. Now build the complete system. ServiceHub API v1.0."

This week, you've grown from learning Minimal APIs to building production-quality REST APIs. Today, you'll **integrate everything into a complete, professional API**.

---

## 🎯 What You're Building

A **complete REST API for ServiceHub** with:

✅ **Full CRUD operations** (Create, Read, Update, Delete)
✅ **Search & filtering** (by status, customer, description)
✅ **Analytics dashboard** (statistics and insights)
✅ **Proper error handling** (status codes, validation)
✅ **Async throughout** (repositories, services, endpoints)
✅ **Clean architecture** (DTOs, services, repositories)

---

## 📋 Prerequisites

- Days 09-13 complete (understand all patterns)
- Comfortable building endpoints
- ~120 minutes (this is bigger!)

---

## Setup

```bash
mkdir Day14-ApiCapstone
cd Day14-ApiCapstone
dotnet new web
```

---

## Building Blocks

**You have all the pieces from Days 09-13:**

1. ✅ **Minimal API endpoints** (Day 09)
2. ✅ **DTOs & mappers** (Day 10)
3. ✅ **Async repositories & services** (Day 11)
4. ✅ **Error handling & status codes** (Day 12)
5. ✅ **Search, filtering, export** (Day 13)

**Today you combine them into ONE coherent API.**

---

## Architecture

```
ServiceHub API v1.0
├── Endpoints
│   ├── Customer endpoints (CRUD)
│   ├── Work order endpoints (CRUD)
│   ├── Filter & search endpoints
│   └── Analytics endpoints
│
├── Services
│   ├── CustomerService
│   ├── WorkOrderService
│   └── AnalyticsService ← NEW
│
├── Repositories
│   ├── ICustomerRepository (async)
│   ├── IWorkOrderRepository (async)
│   └── Error handling middleware
│
└── DTOs & Models
    ├── Requests (Create*)
    ├── Responses (CustomerResponse, etc.)
    └── Domain models (Customer, WorkOrder)
```

---

## Feature: Analytics Service

**NEW for the capstone: Analytics**

```csharp
class AnalyticsService
{
    public async Task<object> GetSummaryAsync(List<Customer> customers, List<WorkOrder> orders)
    {
        await Task.Delay(10);
        return new
        {
            total_customers = customers.Count,
            total_workorders = orders.Count,
            scheduled = orders.Count(o => o.Status == "Scheduled"),
            in_progress = orders.Count(o => o.Status == "InProgress"),
            completed = orders.Count(o => o.Status == "Completed"),
            completion_rate = orders.Count > 0 
                ? Math.Round((double)orders.Count(o => o.Status == "Completed") / orders.Count * 100, 2)
                : 0
        };
    }
}
```

**Analytics endpoints:**

```csharp
app.MapGet("/analytics/summary", GetAnalyticsSummaryAsync);
app.MapGet("/analytics/by-status", GetAnalyticsByStatusAsync);
app.MapGet("/analytics/customer/{id}", GetCustomerAnalyticsAsync);
```

---

## Feature: Update Work Order Status

**NEW: PUT endpoint for status updates**

```csharp
record UpdateWorkOrderStatusRequest(string Status);

app.MapPut("/workorders/{id}/status", UpdateWorkOrderStatusAsync)
.WithName("UpdateWorkOrderStatus")
.WithOpenApi();

async Task<IResult> UpdateWorkOrderStatusAsync(
    int id, 
    UpdateWorkOrderStatusRequest request, 
    WorkOrderService service)
{
    var order = await service.GetWorkOrderAsync(id);
    if (order == null)
        return Results.NotFound();
    
    // Validate status
    if (!new[] { "Scheduled", "InProgress", "Completed" }.Contains(request.Status))
        return Results.BadRequest("Invalid status");
    
    order.Status = request.Status;
    await service.UpdateWorkOrderAsync(order);
    return Results.Ok(order.ToResponse());
}
```

---

## Feature: Info Endpoint

**NEW: API information endpoint**

```csharp
app.MapGet("/info", InfoAsync)
.WithName("Info")
.WithOpenApi();

async Task<IResult> InfoAsync()
{
    return Results.Ok(new 
    { 
        name = "ServiceHub API",
        version = "1.0.0",
        description = "Complete REST API for service business management",
        endpoints = new 
        {
            customers = "GET /customers, POST /customers",
            workorders = "GET /workorders, POST /workorders, PUT /workorders/{id}/status",
            analytics = "GET /analytics/summary, GET /analytics/by-status",
            health = "GET /health",
            info = "GET /info"
        }
    });
}
```

---

## Complete Endpoint List

**By the end of Day 14, you should have:**

```
CUSTOMERS
  GET    /customers              - List all customers
  GET    /customers/{id}         - Get one customer
  POST   /customers              - Create customer
  
WORK ORDERS
  GET    /workorders             - List all work orders
  GET    /workorders/{id}        - Get one work order
  POST   /workorders             - Create work order
  PUT    /workorders/{id}/status - Update status
  
FILTERING & SEARCH
  GET    /workorders/status/{status}     - By status
  GET    /customers/{id}/workorders      - Customer's jobs
  GET    /workorders/search/{term}       - Search jobs
  
ANALYTICS
  GET    /analytics/summary              - Overall stats
  GET    /analytics/by-status            - By status
  GET    /analytics/customer/{id}        - Customer stats
  
SYSTEM
  GET    /health                 - Health check
  GET    /info                   - API information
```

---

## Challenge: Build the Complete API

**Take all code from Days 09-13 and integrate it:**

1. ✅ Copy your endpoints from Day 13
2. ✅ Add the analytics service
3. ✅ Add the PUT status update endpoint
4. ✅ Add the info endpoint
5. ✅ Test all endpoints manually with curl or Postman

**Example: Testing with curl**

```bash
# Get all customers
curl http://localhost:5000/customers

# Create a customer
curl -X POST http://localhost:5000/customers \
  -H "Content-Type: application/json" \
  -d '{"name":"Alice","email":"alice@example.com"}'

# Get analytics
curl http://localhost:5000/analytics/summary

# Search work orders
curl http://localhost:5000/workorders/search/cleaning
```

---

## ✅ Checklist

- [ ] You have all customer CRUD endpoints
- [ ] You have all work order CRUD endpoints
- [ ] You can filter and search
- [ ] You have analytics endpoints
- [ ] You have error handling throughout
- [ ] You can update work order status
- [ ] You have health & info endpoints
- [ ] All endpoints are async
- [ ] All endpoints return proper status codes
- [ ] You tested endpoints manually

---

## 🎉 Week 2 Complete!

You've built a **complete, professional REST API** with:
- Clean architecture (DI, services, repositories)
- Professional patterns (DTOs, mappers, async)
- Production features (error handling, search, analytics)
- Real-world capabilities (CRUD, filtering, export)

---

## 🔗 Next: Week 3

Starting Day 15: **Database Layer (EF Core)**

Your API will move from in-memory to a real database, connecting to SQL Server with Entity Framework Core.

---

## 📚 Resources

- <a href="https://dotnet.microsoft.com/learn/" target="_blank">Getting Started with .NET</a>
- <a href="https://github.com/OakesekAo/AscendCSharp30" target="_blank">AscendCSharp30 Repository</a>

---

## 🎬 Reflection

Take a moment to appreciate what you've built:
- **Day 1:** Hello, World!
- **Day 7:** Console app with CRUD
- **Day 14:** Production REST API

**That's incredible progress. You should be proud.** 🚀

---

**Next week, we add persistence.** See you on Day 15! 🗄️
