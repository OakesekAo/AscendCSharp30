# ServiceHub.API — Professional N-Tier REST API

This is the **complete, runnable ServiceHub API** with production-ready architecture that evolves through **Days 08-14** of the AscendCSharp30 curriculum.

## 🚀 Quick Start

### Run the API
```bash
cd ServiceHub.API
dotnet run
```

The API will start on `https://localhost:5001`.

### Open Swagger
Browser: **https://localhost:5001/swagger/index.html**

## 🏗️ Architecture: N-Tier Pattern

ServiceHub.API uses **proper layering** to separate concerns and prepare for Blazor frontend integration:

```
ServiceHub.API/
│
├── Models/                          DOMAIN LAYER
│   ├── Customer.cs                 (Business entities)
│   └── WorkOrder.cs
│
├── DTOs/                            API CONTRACT LAYER
│   ├── Requests/                   (What clients send)
│   │   ├── CreateCustomerRequest.cs
│   │   ├── CreateWorkOrderRequest.cs
│   │   └── UpdateWorkOrderStatusRequest.cs
│   └── Responses/                  (What API returns)
│       ├── CustomerResponse.cs
│       └── WorkOrderResponse.cs
│
├── Repositories/                    DATA ACCESS LAYER
│   ├── ICustomerRepository.cs      (Interfaces)
│   ├── IWorkOrderRepository.cs
│   ├── CustomerRepository.cs       (In-memory implementations)
│   └── WorkOrderRepository.cs
│
├── Services/                        BUSINESS LOGIC LAYER
│   ├── ICustomerService.cs         (Interfaces)
│   ├── IWorkOrderService.cs
│   ├── IAnalyticsService.cs
│   ├── CustomerService.cs          (Implementations)
│   ├── WorkOrderService.cs
│   └── AnalyticsService.cs
│
├── Endpoints/                       PRESENTATION LAYER
│   ├── CustomerEndpoints.cs        (Route handlers)
│   ├── WorkOrderEndpoints.cs
│   └── AnalyticsEndpoints.cs
│
├── Extensions/                      HELPERS
│   └── ServiceCollectionExtensions.cs  (DI configuration)
│
├── Program.cs                       APPLICATION BOOTSTRAP
└── appsettings.json
```

### Layer Responsibilities

**Models** → Domain entities (Customer, WorkOrder)
**DTOs** → API contracts (request/response shapes)
**Repositories** → Data access (in-memory, but ready for SQL)
**Services** → Business logic (validation, calculations)
**Endpoints** → HTTP handlers (routing, validation)

---

## 📚 Why This Architecture?

### ✅ SOLID Principles
- **S**ingle Responsibility: Each class has one reason to change
- **O**pen/Closed: Open for extension, closed for modification
- **L**iskov Substitution: Services depend on interfaces
- **I**nterface Segregation: Small, focused interfaces
- **D**ependency Inversion: Depend on abstractions, not implementations

### ✅ DRY (Don't Repeat Yourself)
- Extension methods for DTO mapping (`.ToResponse()`)
- Centralized DI configuration
- Consistent validation patterns

### ✅ Testable
- Repositories behind interfaces (easy to mock)
- Services receive dependencies via constructor
- Clear input/output contracts

### ✅ Scalable
- Easy to swap repositories (add EF Core in Week 3)
- Easy to add caching, logging, validation
- Ready for Blazor frontend integration

---

## 🔌 API Endpoints

All endpoints are organized by domain resource:

### Customers
```
GET    /customers              List all
GET    /customers/{id}         Get one
POST   /customers              Create
PUT    /customers/{id}         Update
DELETE /customers/{id}         Delete
```

### Work Orders
```
GET    /workorders             List all
GET    /workorders/{id}        Get one
POST   /workorders             Create
GET    /workorders/customer/{customerId}  By customer
PUT    /workorders/{id}/status Update status
DELETE /workorders/{id}        Delete
```

### Analytics
```
GET    /analytics/summary      Statistics & reporting
```

### System
```
GET    /                       API info
GET    /health                 Health check
```

---

## 🧪 Testing

### Swagger UI (Easiest)
1. Run `dotnet run`
2. Open `https://localhost:5001/swagger`
3. Click "Try it out" on any endpoint
4. Execute and see responses

### Example: Create Customer
```bash
curl -X POST https://localhost:5001/customers \
  -H "Content-Type: application/json" \
  -k \
  -d '{"name":"Diana","email":"diana@example.com"}'
```

### Example: Get Analytics
```bash
curl -k https://localhost:5001/analytics/summary
```

---

## 🔄 Data Flow Example

**User creates a customer via POST /customers:**

1. **Endpoint** receives `CreateCustomerRequest`
2. **Endpoint** validates input
3. **Endpoint** calls `ICustomerService.CreateAsync()`
4. **Service** creates `Customer` domain model
5. **Service** calls `ICustomerRepository.AddAsync()`
6. **Repository** stores in memory (or future: database)
7. **Service** converts to `CustomerResponse` DTO
8. **Endpoint** returns 201 with DTO

**Key Pattern:**
```
Request DTO → Service → Domain Model → Repository → Service → Response DTO
```

---

## 💾 Seed Data

Pre-loaded data ready for testing:

**Customers:**
- Alice Johnson (alice@example.com)
- Bob Smith (bob@example.com)
- Charlie Brown (charlie@example.com)

**Work Orders:**
- Gutter Cleaning (Scheduled)
- Lawn Mowing (Scheduled)
- Window Washing (In Progress)
- Pressure Washing (Scheduled)

---

## 🎓 Learning Through Code

By studying this API, you see:

### Dependency Injection
```csharp
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    
    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;  // Injected!
    }
}
```

### DTO Mapping Pattern
```csharp
// Extension method for clean conversion
public static CustomerResponse ToResponse(this Customer customer)
    => new(customer.Id, customer.Name, customer.Email, customer.CreatedAt);
```

### Async Foundation
```csharp
public async Task<CustomerResponse?> GetAsync(int id)
{
    var customer = await _repository.GetAsync(id);
    return customer?.ToResponse();
}
```

### Endpoint Organization
```csharp
public static void MapCustomerEndpoints(this WebApplication app)
{
    var group = app.MapGroup("/customers")
        .WithTags("Customers");
    
    group.MapGet("/", GetAll);
    group.MapGet("/{id}", GetById);
    group.MapPost("/", Create);
}
```

---

## 🚀 Week 2 Progression

This API represents **Days 08-14 concepts combined**:

| Day | Concept | Evidence |
|-----|---------|----------|
| 08 | Dependency Injection | `ServiceCollectionExtensions.AddServiceHubServices()` |
| 09 | Minimal API | `MapCustomerEndpoints()`, route handlers |
| 10 | DTOs | Request/Response DTOs, mappers |
| 11 | Async/Await | `async Task`, `await` throughout |
| 12 | Error Handling | Status codes, validation, error messages |
| 13 | Search/Filter | `/workorders/customer/{id}`, repositories filter |
| 14 | Analytics | `IAnalyticsService`, summary endpoint |

---

## 🔮 Ready for Week 3

When you add EF Core (Week 3):

1. Replace `CustomerRepository` with EF Core implementation
2. Swap `WorkOrderRepository` to use `DbContext`
3. Everything else stays the same ✅
4. Services, endpoints, DTOs all work unchanged ✅

That's the power of N-tier architecture.

---

## 📝 Professional Patterns

This codebase demonstrates:

- ✅ **SOLID principles** (Single Responsibility, Interface Segregation, DI)
- ✅ **Clean Architecture** (layered, testable, maintainable)
- ✅ **Repository Pattern** (abstract data access)
- ✅ **DTO Pattern** (decouple API from domain)
- ✅ **Extension Methods** (clean mapping)
- ✅ **Async/Await** (non-blocking I/O)
- ✅ **Swagger/OpenAPI** (API documentation)

**This is production-ready code structure.**

---

**Ready to run?** `cd ServiceHub.API && dotnet run` 🚀

