# Day 13 — JSON & File I/O

## 🚀 Working with Data Persistence

**Maya's Message:**
> "ServiceHub needs to export/import customer data, search capabilities, and advanced filtering. Let's build those features."

Today, you'll learn **JSON serialization** and **file I/O** — working with files and making your API more powerful.

---

## 🎯 Learning Objectives

1. **Serialize to JSON** — Convert objects to JSON strings
2. **Deserialize from JSON** — Convert JSON back to objects
3. **Handle files** — Read and write from disk
4. **Add search** — Find work orders by description
5. **Add filtering** — Get work orders by status or customer
6. **Apply to ServiceHub** — Export/import and search

---

## 📋 Prerequisites

- Days 09-12 complete (async error-handled APIs)
- Comfortable with file I/O concepts
- ~90 minutes

---

## Setup

```bash
mkdir Day13-JsonIO
cd Day13-JsonIO
dotnet new web
```

---

## Step 1: JSON Serialization

**Convert objects to JSON:**

```csharp
using System.Text.Json;

var customer = new Customer { Id = 1, Name = "Alice", Email = "alice@example.com" };

// Serialize to JSON
var options = new JsonSerializerOptions { WriteIndented = true };
var json = JsonSerializer.Serialize(customer, options);
Console.WriteLine(json);
// Output:
// {
//   "id": 1,
//   "name": "Alice",
//   "email": "alice@example.com"
// }
```

---

## Step 2: JSON Deserialization

**Convert JSON back to objects:**

```csharp
string json = """
{
  "name": "Bob",
  "email": "bob@example.com"
}
""";

var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
var customer = JsonSerializer.Deserialize<Customer>(json, options);
// customer.Name = "Bob"
// customer.Email = "bob@example.com"
```

---

## Step 3: Export Endpoint

**Export customers as JSON:**

```csharp
app.MapGet("/export/customers", ExportCustomersAsync)
.WithName("ExportCustomers")
.WithOpenApi();

async Task<IResult> ExportCustomersAsync(CustomerService service)
{
    var customers = await service.ListCustomersAsync();
    var json = JsonSerializer.Serialize(customers, new JsonSerializerOptions { WriteIndented = true });
    
    return Results.Ok(new { exported_at = DateTime.UtcNow, data = json });
}
```

---

## Step 4: Import Endpoint

**Import customers from JSON file:**

```csharp
app.MapPost("/import/customers", ImportCustomersAsync)
.WithName("ImportCustomers")
.WithOpenApi();

async Task<IResult> ImportCustomersAsync(IFormFile file, CustomerService service)
{
    if (file == null || file.Length == 0)
        return Results.BadRequest("No file provided");
    
    try
    {
        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync();
        
        var customers = JsonSerializer.Deserialize<List<Customer>>(json);
        if (customers == null)
            return Results.BadRequest("Invalid JSON format");
        
        foreach (var customer in customers)
            await service.CreateCustomerAsync(customer);
        
        return Results.Ok(new { imported = customers.Count });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}
```

---

## Step 5: Search Functionality

**Search work orders by description:**

```csharp
interface IWorkOrderRepository
{
    // ... existing methods
    Task<List<WorkOrder>> SearchAsync(string term);
}

class WorkOrderRepository : IWorkOrderRepository
{
    public async Task<List<WorkOrder>> SearchAsync(string term)
    {
        await Task.Delay(5);
        return orders
            .Where(o => o.Description.Contains(term, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}

// Endpoint
app.MapGet("/workorders/search/{term}", SearchWorkOrdersAsync)
.WithName("SearchWorkOrders")
.WithOpenApi();

async Task<IResult> SearchWorkOrdersAsync(string term, WorkOrderService service)
{
    var orders = await service.SearchWorkOrdersAsync(term);
    return Results.Ok(new 
    { 
        search_term = term, 
        results = orders.Select(o => o.ToResponse()).ToList() 
    });
}
```

---

## Step 6: Filtering by Customer

**Get all work orders for a specific customer:**

```csharp
interface IWorkOrderRepository
{
    // ... existing methods
    Task<List<WorkOrder>> GetByCustomerAsync(int customerId);
}

// Service method
public async Task<List<WorkOrder>> GetWorkOrdersForCustomerAsync(int customerId)
{
    return await repository.GetByCustomerAsync(customerId);
}

// Endpoint
app.MapGet("/customers/{id}/workorders", GetCustomerWorkOrdersAsync)
.WithName("GetCustomerWorkOrders")
.WithOpenApi();

async Task<IResult> GetCustomerWorkOrdersAsync(int id, CustomerService customerService, WorkOrderService orderService)
{
    var customer = await customerService.GetCustomerAsync(id);
    if (customer == null)
        return Results.NotFound();
    
    var orders = await orderService.GetWorkOrdersForCustomerAsync(id);
    return Results.Ok(new 
    { 
        customer = customer.ToResponse(), 
        workorders = orders.Select(o => o.ToResponse()).ToList() 
    });
}
```

---

## Mini Challenge: Add Search & Export to Day 12

**Extend your Day 12 API with:**

1. Export endpoint (GET /export/customers)
2. Import endpoint (POST /import/customers)
3. Search work orders (GET /workorders/search/{term})
4. Filter by customer (GET /customers/{id}/workorders)
5. Filter by status (GET /workorders/status/{status})

**Example search:**

```csharp
var results = await service.SearchWorkOrdersAsync("cleaning");
// Returns all work orders with "cleaning" in description
```

---

## ✅ Checklist

- [ ] You can serialize objects to JSON
- [ ] You can deserialize JSON to objects
- [ ] You have export endpoint
- [ ] You have import endpoint
- [ ] You can search work orders
- [ ] You can filter by customer
- [ ] You can filter by status

---

## 🔗 Next Steps

Day 14: **Week 2 Capstone** — Complete API with analytics and advanced features.

---

## 📚 Resources

- <a href="https://dotnet.microsoft.com/learn/" target="_blank">Getting Started with .NET</a>
- <a href="https://github.com/OakesekAo/AscendCSharp30" target="_blank">AscendCSharp30 Repository</a>

---

**Export, import, search — these make real-world APIs.** See you on Day 14! 🚀
