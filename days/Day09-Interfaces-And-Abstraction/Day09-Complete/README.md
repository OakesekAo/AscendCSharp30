# Day 09 — DTOs & API Contracts (Complete)

## 🎯 Building on Day 08

This is the **complete, working implementation** of Day 09: **adding DTOs and organizing endpoints**.

**Key difference from Day 08:**
- Day 08: Everything in Program.cs, domain models returned directly
- Day 09: DTOs in separate files, organized endpoints, professional structure

---

## 🏗️ Architecture Evolution

**Day 08 → Day 09 Changes:**

```
Day 08 Complete/
├── Models/
├── Repositories/
├── Services/
└── Program.cs (all endpoints inline)

Day 09 Complete/  (WHAT'S NEW)
├── Models/        (same as Day 08)
├── Repositories/  (same as Day 08)
├── Services/      (WITH mappers)
├── DTOs/          (NEW!)
│   ├── Requests/
│   │   ├── CreateCustomerRequest.cs
│   │   └── CreateWorkOrderRequest.cs
│   └── Responses/
│       ├── CustomerResponse.cs
│       └── WorkOrderResponse.cs
├── Endpoints/     (NEW!)
│   ├── CustomerEndpoints.cs
│   └── WorkOrderEndpoints.cs
└── Program.cs     (CLEAN - just calls MapCustomerEndpoints(), etc.)
```

---

## 🚀 Run This Code

### Prerequisites
- .NET 10 SDK installed
- Terminal open in `days/Day09-Interfaces-And-Abstraction/Day09-Complete/`

### Run It
```bash
dotnet run
```

Open browser to: **https://localhost:5001/swagger/index.html**

---

## 📝 Key Concepts

### 1. DTOs (Data Transfer Objects)

**Request DTOs** - What clients send:
```csharp
// DTOs/Requests/CreateCustomerRequest.cs
public record CreateCustomerRequest(string Name, string Email);
```

**Response DTOs** - What API returns:
```csharp
// DTOs/Responses/CustomerResponse.cs
public record CustomerResponse(int Id, string Name, string Email);
```

**Why?**
- ✅ Control what's exposed (security)
- ✅ Separate API contract from domain
- ✅ Easy to evolve without breaking clients
- ✅ Clear input/output contracts

### 2. Mapper Extension Methods

Convert domain models to DTOs:

```csharp
// In CustomerService.cs
public static class CustomerExtensions
{
    public static CustomerResponse ToResponse(this Customer customer)
        => new(customer.Id, customer.Name, customer.Email);
}

// Usage
var customer = await service.GetAsync(1);
var response = customer.ToResponse();
```

**Benefits:**
- ✅ Clean conversion syntax
- ✅ Reusable across endpoints
- ✅ DRY principle

### 3. Organized Endpoints

**Before (Day 08):** Everything in Program.cs
```csharp
app.MapGet("/customers", ...);
app.MapGet("/customers/{id}", ...);
app.MapPost("/customers", ...);
// (repeat for work orders, analytics, etc.)
```

**After (Day 09):** Organized by resource
```csharp
// Endpoints/CustomerEndpoints.cs
public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/customers");
        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);
        group.MapPost("/", Create);
    }
    
    private static async Task<IResult> GetAll(CustomerService service) { ... }
    private static async Task<IResult> GetById(int id, CustomerService service) { ... }
    private static async Task<IResult> Create(CreateCustomerRequest request, CustomerService service) { ... }
}

// Program.cs (clean!)
app.MapCustomerEndpoints();
app.MapWorkOrderEndpoints();
```

**Benefits:**
- ✅ Program.cs stays clean
- ✅ Each resource in its own file
- ✅ Easy to find and update
- ✅ Professional organization

---

## 🔄 Data Flow Example

**User calls: `POST /customers`**

```
CreateCustomerRequest (DTO)
    ↓
CustomerEndpoints.Create() handler
    ↓
CustomerService.CreateAsync()
    ↓
CustomerRepository.AddAsync()
    ↓
Stores domain Customer model
    ↓
Service returns domain Customer
    ↓
Handler calls customer.ToResponse() (mapper)
    ↓
Returns CustomerResponse DTO
    ↓
Serialized to JSON
    ↓
Client receives JSON response
```

**Key:** Domain model never exposed directly. Only DTOs go over HTTP.

---

## ✅ Endpoints Available

Same endpoints as Day 08, but now using DTOs:

```
GET    /health                 Health check
GET    /customers              List all (returns CustomerResponse[])
GET    /customers/{id}         Get one (returns CustomerResponse)
POST   /customers              Create (accepts CreateCustomerRequest)

GET    /workorders             List all (returns WorkOrderResponse[])
GET    /workorders/{id}        Get one (returns WorkOrderResponse)
POST   /workorders             Create (accepts CreateWorkOrderRequest)
```

---

## 📊 What Changed Since Day 08

| Aspect | Day 08 | Day 09 |
|--------|--------|--------|
| **DTOs** | Inline in Program.cs | Separate files |
| **Endpoints** | All in Program.cs | Organized in Endpoints/ |
| **Mappers** | None | Extension methods |
| **API responses** | Domain models | Response DTOs |
| **Code organization** | One big file | Proper layering |

---

## 🎯 What to Notice

1. **Program.cs is tiny** - Just wiring, no endpoint logic
2. **DTOs are dumb** - Just records, no logic
3. **Mappers are simple** - Just conversion extension methods
4. **Endpoints are organized** - One file per resource
5. **Services unchanged** - Still take repositories, still async

---

## 🚀 Next: Day 10

Day 10 will **build on this code** by:
- Keeping Models, Repositories, Services, DTOs
- Adding error handling and validation
- Response wrapper with error messages
- Professional error responses

**The code will grow, but the pattern stays the same.**

---

## 📖 Professional Patterns Demonstrated

This code shows:

- ✅ **DTO Pattern** - Separate API contracts from domain
- ✅ **Extension Methods** - Clean mapper syntax
- ✅ **Group Endpoints** - Organized by resource
- ✅ **Dependency Injection** - Still foundational
- ✅ **Async/Await** - Ready for any backend
- ✅ **Separation of Concerns** - Each file has one job

**This is how enterprise .NET APIs are built.**

---

## 🔄 Comparison: Day 08 vs Day 09

### Day 08: Foundation
- Models, Repositories, Services (DI foundation)
- Basic endpoints (all in Program.cs)
- Domain models returned directly

### Day 09: Professional Contracts
- Same Models, Repositories, Services
- DTOs for API contracts
- Organized endpoints by resource
- Mappers for conversion

### Days 10-14 Will Add:
- Day 10: Error handling, validation
- Day 11: Advanced features (search, filtering)
- Day 12: Analytics and reporting
- Day 13: More complex business logic
- Day 14: Production-ready polish

---

**Ready to run?** `dotnet run` then visit `https://localhost:5001/swagger` 🚀

**This is what real API code looks like!**

