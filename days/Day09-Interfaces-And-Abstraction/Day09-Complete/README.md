# Day09-Complete — Minimal API Foundations

This is the **completed, working ServiceHub API v0.1** with first endpoints.

## 🚀 Quick Start

```bash
cd Day09-Complete
dotnet run
```

Then test endpoints:

```bash
# Get all customers
curl http://localhost:5000/customers

# Create customer
curl -X POST http://localhost:5000/customers \
  -H "Content-Type: application/json" \
  -d '{"id":1,"name":"Alice","email":"alice@example.com"}'

# Get all work orders
curl http://localhost:5000/workorders

# Health check
curl http://localhost:5000/health
```

## 📋 What This Program Does

A **working Minimal API** that demonstrates:
- ✅ GET/POST endpoints for customers
- ✅ GET/POST endpoints for work orders
- ✅ Dependency injection in action
- ✅ Repository pattern
- ✅ Service layer pattern
- ✅ JSON request/response
- ✅ Route parameters
- ✅ Health check endpoint

## 🏗️ Architecture

```
Program.cs
├── DI Setup (ServiceCollection)
├── Endpoints (MapGet, MapPost)
├── Repositories (DI-injected)
├── Services (DI-injected)
└── Models (Customer, WorkOrder)
```

## 📊 Endpoints Available

```
CUSTOMERS
  GET  /customers              - List all
  GET  /customers/{id}         - Get one
  POST /customers              - Create

WORK ORDERS
  GET  /workorders             - List all
  GET  /workorders/{id}        - Get one
  POST /workorders             - Create

SYSTEM
  GET  /health                 - Health check
```

## ✅ Output Example

Hitting `GET /customers`:

```json
[
  {
    "id": 1,
    "name": "Alice",
    "email": "alice@example.com"
  },
  {
    "id": 2,
    "name": "Bob",
    "email": "bob@example.com"
  }
]
```

## 🎯 What Day 10 Will Do

Day 10 refactors this to use **DTOs** — separating API contracts from domain models.

## 🟦 ServiceHub Context

This is **Week 2, Day 1**: The API foundation is set. Clean, working endpoints with DI and repositories. Starting point for adding DTOs, async, error handling, and more throughout the week.

**By Day 14:** A complete, professional API with analytics, search, filtering, and production-ready error handling.

