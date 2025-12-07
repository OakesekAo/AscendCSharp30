# Day06-Complete — LINQ Fundamentals

This is the **completed, advanced version** of the ServiceHub Customer Manager.

**Builds on Day 05's refactored code with advanced LINQ patterns.**

## 🚀 Quick Start

```bash
cd Day06-Complete
dotnet run
```

## 📋 What New

Day 06 adds **advanced LINQ** to the customer manager:

| Feature | Method | Example |
|---------|--------|---------|
| **Filter** | `.Where()` | Names longer than 12 chars |
| **Transform** | `.Select()` | Create anonymous objects |
| **Aggregates** | `.Sum()`, `.Average()`, `.Min()`, `.Max()` | Statistics |
| **Sorting** | `.OrderBy()`, `.OrderByDescending()` | A-Z or reverse |
| **Multi-sort** | `.ThenBy()` | Sort by multiple fields |
| **Pagination** | `.Take()`, `.Skip()` | First 3, skip 2 |
| **Access** | `.First()`, `.Last()` | Single elements |
| **Predicates** | `.Any()`, `.All()` | Check conditions |
| **Grouping** | `.GroupBy()` | Group by category |
| **Chaining** | Multiple methods | Complex queries |

## 💡 Key Additions

### 1. Work Orders Class
```csharp
class WorkOrder
{
    public int Id { get; set; }
    public string Customer { get; set; }
    public string Description { get; set; }
    public int Hours { get; set; }
    public string Status { get; set; }
}
```

### 2. Real ServiceHub Data
```csharp
List<WorkOrder> workOrders = new()
{
    new() { Id = 1, Customer = "Alice", Description = "Cleaning", Hours = 2, Status = "Completed" },
    // ... more work orders
};
```

### 3. Advanced Queries
```csharp
// Complex multi-condition query
var result = workOrders
    .Where(w => w.Status == "Completed")
    .GroupBy(w => w.Customer)
    .OrderByDescending(g => g.Count())
    .ToList();
```

## 📊 LINQ Methods Demonstrated

| Category | Methods | Purpose |
|----------|---------|---------|
| **Filtering** | `.Where()` | Remove unwanted data |
| **Projection** | `.Select()` | Transform to new shape |
| **Aggregation** | `.Sum()`, `.Average()`, `.Count()` | Calculate statistics |
| **Ordering** | `.OrderBy()`, `.ThenBy()` | Sort results |
| **Selection** | `.First()`, `.Last()`, `.Take()`, `.Skip()` | Get subset |
| **Grouping** | `.GroupBy()` | Organize by category |
| **Logic** | `.Any()`, `.All()` | Check conditions |

## 🎯 Real ServiceHub Scenarios

This code demonstrates **real queries** for ServiceHub:

### Completed Work This Week
```csharp
int completedCount = workOrders.Count(w => w.Status == "Completed");
```

### Total Hours Scheduled
```csharp
double totalHours = workOrders.Sum(w => w.Hours);
```

### Jobs by Customer (with count)
```csharp
var byCustomer = workOrders
    .GroupBy(w => w.Customer)
    .OrderByDescending(g => g.Count());
```

### Average Job Duration
```csharp
double avgHours = workOrders.Average(w => w.Hours);
```

## ✅ Output Example

```
--- High-value customers (name length > 12) (LINQ .Where()) ---
• Charlie Brown
• Diana Prince

--- Top 3 longest-name customers ---
• Charlie Brown (13 chars)
• Diana Prince (12 chars)

--- Work order analytics ---
Completed jobs: 3
Total hours: 17
Average job length: 2.8 hours

Jobs by customer:
• Alice Johnson: 2 jobs, 3h total
• Eve Davis: 1 job, 5h total
```

## 🎯 What Day 07 Will Do

Day 07 **combines everything** into the **ServiceHub Job Scheduler** capstone:
- Uses all Day 01-06 concepts
- Real menu-driven app
- CRUD operations
- Advanced queries
- Week 1 complete

## 🔍 Code Structure

```
Main Program (Show queries)
├── Customer data (lists)
├── Work order data (class instances)
├── Basic LINQ queries (1-6)
├── Filtering & transformation (7-9)
├── Grouping & chaining (10-12)
└── Real ServiceHub scenarios (analytics)
```

## 🎬 Summary

Day 06 shows:
- Advanced LINQ for real problems
- Working with data classes
- Powerful query patterns
- Foundation for API queries (Week 2)
- Foundation for database queries (Week 3)

**LINQ is your super power in C#.**

---

## 🟦 ServiceHub Context

This advanced LINQ is **exactly what happens in production**:
- **Week 2 API:** Endpoints use LINQ to filter/sort customer requests
- **Week 3 Database:** EF Core translates LINQ to SQL
- **Week 4 UI:** Dashboard queries use LINQ aggregates

Learning LINQ well is learning how to query data. That's 80% of real applications.
