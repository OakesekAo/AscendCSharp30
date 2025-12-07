# Day13-Complete — JSON & File I/O

This is the **feature-rich API with search, filtering, and export/import**.

**Builds on Day 12 with JSON serialization, file I/O, and advanced queries.**

## 🚀 Quick Start

```bash
cd Day13-Complete
dotnet run
```

## 📋 What This Program Does

Enhanced API from Day 12:
- ✅ Search work orders by description
- ✅ Filter by customer
- ✅ Export customers as JSON
- ✅ Import customers from JSON file
- ✅ JSON serialization configuration
- ✅ Advanced filtering

## 🔍 Search & Filter Endpoints

```
GET  /customers/{id}/workorders     - Get customer's jobs
GET  /workorders/search/{term}      - Search jobs by description
GET  /workorders/status/{status}    - Filter by status
POST /import/customers              - Import from JSON file
GET  /export/customers              - Export as JSON
```

## 💾 Export Example

**Request:**
```bash
curl http://localhost:5000/export/customers
```

**Response:**
```json
{
  "exported_at": "2024-12-04T10:30:00Z",
  "data": "[{\"id\":1,\"name\":\"Alice\",\"email\":\"alice@example.com\"}]"
}
```

## 📤 Import Example

**Request:**
```bash
curl -X POST http://localhost:5000/import/customers \
  -F "file=@customers.json"
```

**Response:**
```json
{
  "imported": 5
}
```

## 🔎 Search Example

**Request:**
```bash
curl http://localhost:5000/workorders/search/cleaning
```

**Response:**
```json
{
  "search_term": "cleaning",
  "results": [
    {
      "id": 1,
      "customerId": 1,
      "description": "Gutter Cleaning",
      "status": "Scheduled"
    }
  ]
}
```

## 📊 Repository Enhancements

New repository methods:
- `SearchAsync(string term)` - Find by description
- `GetByStatusAsync(string status)` - Filter by status
- `GetByCustomerAsync(int customerId)` - Get customer's jobs

## ✅ Complete Endpoint List

```
CUSTOMERS
  GET  /customers
  POST /customers
  GET  /customers/{id}

WORK ORDERS
  GET  /workorders
  POST /workorders
  GET  /workorders/{id}

FILTERING
  GET  /customers/{id}/workorders
  GET  /workorders/search/{term}
  GET  /workorders/status/{status}

DATA OPERATIONS
  GET  /export/customers
  POST /import/customers

SYSTEM
  GET  /health
```

## 🎯 JSON Configuration

```csharp
var jsonOptions = new JsonSerializerOptions 
{ 
    PropertyNameCaseInsensitive = true,
    WriteIndented = true 
};
```

## 🎬 What Day 14 Will Do

Day 14 is the **capstone** — add analytics, update operations, and complete the API.

## 🟦 ServiceHub Context

Real applications need to export/import data, search, and filter. You now have those capabilities. Day 14 adds the final piece: analytics and reporting.

---

## 🟦 ServiceHub Context  
ServiceHub's dashboard and work order lists rely on LINQ filtering, projections, and sorting.  
Today's LINQ work prepares you to build dynamic views like "today's schedule" and "overdue jobs."

