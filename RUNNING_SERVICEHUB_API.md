# 🚀 Running ServiceHub.API — Week 2 In Action

This guide shows you how to **run the complete REST API** that you'll build throughout **Days 08-14** of AscendCSharp30.

## ⚡ Quick Start (2 minutes)

### Step 1: Open Terminal
```bash
cd C:\Users\oakes\source\repos\AscendCSharp30
```

### Step 2: Navigate to API Project
```bash
cd ServiceHub.API
```

### Step 3: Run the API
```bash
dotnet run
```

You should see output like:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
      Now listening on: http://localhost:5000
```

### Step 4: Open in Browser
Go to: **https://localhost:5001/swagger/index.html**

You'll see the Swagger UI with all available endpoints!

---

## 📚 What You'll See

### Swagger Interface
The Swagger UI shows all endpoints organized by resource:
- **Customer endpoints** (GET, POST, GET by ID)
- **Work Order endpoints** (GET, POST, PUT, filters)
- **Analytics endpoints** (summary, statistics)
- **Health checks**

### Try It Out
1. Click on any endpoint (e.g., `GET /customers`)
2. Click **"Try it out"**
3. Click **"Execute"**
4. See the live response!

---

## 🧪 Test Scenarios

### Scenario 1: Get All Customers
```
Method: GET
URL: https://localhost:5001/customers
Response: List of 3 pre-loaded customers
```

**Result:**
```json
[
  {"id":1,"name":"Alice Johnson","email":"alice@example.com"},
  {"id":2,"name":"Bob Smith","email":"bob@example.com"},
  {"id":3,"name":"Charlie Brown","email":"charlie@example.com"}
]
```

### Scenario 2: Create a New Customer
```
Method: POST
URL: https://localhost:5001/customers
Body:
{
  "name": "Diana Prince",
  "email": "diana@example.com"
}
```

**Result:**
```json
{
  "id": 4,
  "name": "Diana Prince",
  "email": "diana@example.com"
}
```

### Scenario 3: Get Work Orders for a Customer
```
Method: GET
URL: https://localhost:5001/workorders/by-customer/1
Response: All work orders for customer ID 1
```

### Scenario 4: Update Work Order Status
```
Method: PUT
URL: https://localhost:5001/workorders/1/status
Body:
{
  "status": "Completed"
}
```

### Scenario 5: View Analytics Dashboard
```
Method: GET
URL: https://localhost:5001/analytics/summary
Response: Statistics (customer count, work order breakdown, completion rate)
```

**Result:**
```json
{
  "timestamp": "2024-12-04T15:30:00Z",
  "total_customers": 4,
  "total_workorders": 4,
  "by_status": {
    "scheduled": 2,
    "in_progress": 1,
    "completed": 1,
    "canceled": 0
  },
  "completion_rate_percent": 25.0
}
```

---

## 🔌 All Available Endpoints

### Customer Management
| Method | Endpoint | Purpose |
|--------|----------|---------|
| GET | `/customers` | List all customers |
| GET | `/customers/{id}` | Get one customer |
| POST | `/customers` | Create new customer |

### Work Order Management
| Method | Endpoint | Purpose |
|--------|----------|---------|
| GET | `/workorders` | List all work orders |
| GET | `/workorders/{id}` | Get one work order |
| POST | `/workorders` | Create work order |
| GET | `/workorders/by-customer/{customerId}` | Orders for customer |
| PUT | `/workorders/{id}/status` | Update status |

### Analytics
| Method | Endpoint | Purpose |
|--------|----------|---------|
| GET | `/analytics/summary` | Dashboard statistics |

### System
| Method | Endpoint | Purpose |
|--------|----------|---------|
| GET | `/` | API information |
| GET | `/health` | Health status |

---

## 💻 Using curl (Command Line)

If you prefer command line testing:

### List customers
```bash
curl -k https://localhost:5001/customers
```

### Create customer
```bash
curl -k -X POST https://localhost:5001/customers \
  -H "Content-Type: application/json" \
  -d '{"name":"Eve","email":"eve@example.com"}'
```

### Get work orders for customer 1
```bash
curl -k https://localhost:5001/workorders/by-customer/1
```

### Update work order status
```bash
curl -k -X PUT https://localhost:5001/workorders/1/status \
  -H "Content-Type: application/json" \
  -d '{"status":"Completed"}'
```

### Get analytics
```bash
curl -k https://localhost:5001/analytics/summary
```

---

## 🐛 Troubleshooting

### "Port 5001 already in use"
**Solution:** Another app is using port 5001
```bash
# Stop the API:
# Press Ctrl+C in the terminal
# Or use a different port:
dotnet run --urls "https://localhost:5002"
```

### "SSL Certificate Error"
**For curl:**
```bash
curl -k https://localhost:5001/customers  # -k ignores SSL warnings
```

**For browser:**
- Click "Advanced" → "Proceed anyway" (development only)
- Or visit `http://localhost:5000/swagger` (unencrypted)

### No response from API
**Check:**
1. Is terminal still running? (Should say "Now listening")
2. Did you run `dotnet run` from `ServiceHub.API/` folder?
3. Try refreshing the browser
4. Check browser console (F12 → Console tab)

---

## 🎯 Learning Goals

By testing this API, you're exploring:

### ✅ Day 08: Dependency Injection
- See how services are registered and injected
- Repositories handle data storage
- Services coordinate logic

### ✅ Day 09: Minimal API Endpoints
- REST endpoints with MapGet/MapPost/MapPut
- HTTP methods and status codes
- Route parameters and query strings

### ✅ Day 10: DTOs & API Contracts
- Request/response models separate from domain
- `.ToResponse()` mappers converting models
- Clean API contracts

### ✅ Day 11: Async/Await
- API is structured for async operations
- Ready for database integration
- Non-blocking request handling

### ✅ Day 12: Error Handling
- Validation on input
- Proper HTTP status codes (200, 201, 400, 404)
- Error responses with context

### ✅ Day 13: Search & Filtering
- `/workorders/by-customer/{id}` filters by customer
- List endpoints with LINQ queries
- Advanced filtering

### ✅ Day 14: Analytics & Advanced Features
- `/analytics/summary` aggregates data
- Statistics and reporting
- Status breakdowns

---

## 📖 Next Steps

1. **Explore all endpoints** in Swagger UI
2. **Test creating data** - add customers and work orders
3. **Update statuses** - mark work orders as complete
4. **Check analytics** - see dashboard update automatically
5. **Read Days 08-14 READMEs** to understand each concept
6. **Modify the code** - add new endpoints, features, validation

---

## 🚀 What You're Experiencing

This **working API** demonstrates:
- Professional REST API design
- Dependency injection in action
- Clean separation of concerns
- Proper error handling
- Real-time data operations

**This is Week 2 of AscendCSharp30 in a runnable form.**

---

**Ready to test?** Open https://localhost:5001/swagger/index.html and start experimenting! 🎉
