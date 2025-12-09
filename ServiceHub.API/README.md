# ServiceHub.API — Complete REST API

This is the **complete, runnable ServiceHub API** that evolves through **Days 08-14** of the AscendCSharp30 curriculum.

## 🚀 Quick Start

### Run the API
```bash
cd ServiceHub.API
dotnet run
```

The API will start on `https://localhost:5001` (or `http://localhost:5000` in development).

### View Swagger/OpenAPI Documentation
Open your browser to:
- `https://localhost:5001/swagger/index.html`
- Try endpoints directly in the browser

### Test with curl
```bash
# Get all customers
curl https://localhost:5001/customers

# Create a customer
curl -X POST https://localhost:5001/customers \
  -H "Content-Type: application/json" \
  -d '{"name":"Diana","email":"diana@example.com"}'

# Get analytics
curl https://localhost:5001/analytics/summary
```

## 📚 Architecture

This project demonstrates the progression from **Day 08 → Day 14**:

### Day 08: Dependency Injection Foundation
- Interfaces: `ICustomerRepository`, `IWorkOrderRepository`
- Services: `CustomerService`, `WorkOrderService`
- DI Container registration
- In-memory repositories with seed data

### Day 09: Minimal API Endpoints
- Convert to `WebApplicationBuilder`
- Map GET, POST endpoints
- Basic CRUD operations
- Swagger integration

### Day 10: DTOs & API Contracts
- Request DTOs: `CreateCustomerRequest`, `CreateWorkOrderRequest`, `UpdateWorkOrderStatusRequest`
- Response DTOs: `CustomerResponse`, `WorkOrderResponse`
- Mapper extension methods (`.ToResponse()`)

### Day 11: Async/Await Patterns
- Services support async operations
- `AnalyticsService` with async methods
- Ready for async database calls

### Day 12: Error Handling
- Status codes: 200, 201, 400, 404
- Input validation
- Error response objects
- Proper HTTP semantics

### Day 13: Search & Filtering
- `/workorders/by-customer/{customerId}` endpoint
- List filtering in repositories
- Advanced queries

### Day 14: Analytics & Advanced Features
- `/analytics/summary` endpoint
- Status breakdown reporting
- Completion rate calculation
- Update work order status

## 🔌 API Endpoints

### Customers
```
GET    /customers              List all customers
GET    /customers/{id}         Get one customer
POST   /customers              Create customer
```

### Work Orders
```
GET    /workorders             List all work orders
GET    /workorders/{id}        Get one work order
POST   /workorders             Create work order
GET    /workorders/by-customer/{customerId}  Get orders for customer
PUT    /workorders/{id}/status Update work order status
```

### Analytics
```
GET    /analytics/summary      Get dashboard statistics
```

### System
```
GET    /                       API info
GET    /health                 Health check
```

## 📊 Seed Data

The API comes with sample data pre-loaded:

**Customers:**
- Alice Johnson (alice@example.com)
- Bob Smith (bob@example.com)
- Charlie Brown (charlie@example.com)

**Work Orders:**
- Gutter Cleaning (Customer 1, Scheduled)
- Lawn Mowing (Customer 2, Scheduled)
- Window Washing (Customer 1, In Progress)
- Pressure Washing (Customer 3, Scheduled)

## 🧪 Testing Endpoints

### With Swagger (Easiest)
1. Run `dotnet run`
2. Open `https://localhost:5001/swagger/index.html`
3. Click "Try it out" on any endpoint
4. Execute and see responses

### With curl
```bash
# List customers
curl -k https://localhost:5001/customers

# Create work order
curl -k -X POST https://localhost:5001/workorders \
  -H "Content-Type: application/json" \
  -d '{"customerId":1,"description":"New Job"}'

# Update status
curl -k -X PUT https://localhost:5001/workorders/1/status \
  -H "Content-Type: application/json" \
  -d '{"status":"Completed"}'

# Get analytics
curl -k https://localhost:5001/analytics/summary
```

### With Postman
1. Create a new request
2. Set URL: `https://localhost:5001/customers`
3. Set method: GET
4. Send

## 🏗️ Code Structure

```
ServiceHub.API/
├── Program.cs                    Main application file (everything is here)
├── ServiceHub.API.csproj        Project file
├── bin/                         Compiled output
└── obj/                         Build artifacts
```

**Note:** This is intentionally monolithic for learning purposes. In production, you'd separate concerns into:
- `Models/` - Domain entities
- `DTOs/` - API contracts
- `Services/` - Business logic
- `Repositories/` - Data access
- `Endpoints/` - Route handlers

## 🔄 Progression Through Days

Each day builds on the previous:

| Day | Focus | Changes |
|-----|-------|---------|
| 08 | DI Foundation | Repositories, Services, Interfaces |
| 09 | Minimal API | Convert to web, add endpoints |
| 10 | DTOs | Separate API models from domain |
| 11 | Async/Await | Ready for async databases |
| 12 | Error Handling | Validation, status codes, error responses |
| 13 | Search/Filter | Advanced queries, filtering |
| 14 | Analytics | Reporting, aggregations, advanced features |

## 📖 Learning from This Code

### Dependency Injection Pattern
See how services receive dependencies via constructor injection:
```csharp
public CustomerService(ICustomerRepository repo) => repository = repo;
```

### DTO Pattern
Notice the separation between domain models and API contracts:
```csharp
// Domain model
class Customer { public int Id { get; set; } ... }

// API response
record CustomerResponse(int Id, string Name, string Email);
```

### Async Foundation
Methods are structured for async:
```csharp
public async Task<object> GetSummaryAsync(...) { ... }
```

### Error Handling
Proper HTTP semantics and validation:
```csharp
if (string.IsNullOrWhiteSpace(request.Name))
    return Results.BadRequest(new { error = "Name required" });
```

## 🚀 Next Steps

After exploring this API:

1. **Test all endpoints** using Swagger
2. **Create new customers and work orders** through the API
3. **Understand each layer:**
   - How repositories store data
   - How services orchestrate logic
   - How endpoints expose functionality
4. **Read Days 08-14 Starter guides** to understand each concept
5. **Modify the code** - Add new endpoints, features, validation

## 📝 Notes

- This API uses **in-memory storage** (data is lost on restart)
- All dates/times use UTC
- Seed data is auto-loaded on startup
- CORS is not enabled (for local testing only)
- HTTPS is enforced in production (disabled in development)

## 🎓 What You're Learning

By studying and testing this API, you're learning:
- ✅ Dependency Injection patterns
- ✅ REST API design
- ✅ DTOs and API contracts
- ✅ Async/await patterns
- ✅ Error handling
- ✅ API documentation with Swagger
- ✅ Minimal API framework
- ✅ Data validation

**This is professional-grade code structure.**

---

**Ready to test?** `cd ServiceHub.API && dotnet run` 🚀
