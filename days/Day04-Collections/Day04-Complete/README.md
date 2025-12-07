# Day04-Complete — Collections

This is the **completed, polished version** of the Day 04 ServiceHub Customer Manager.

## 🚀 Quick Start

```bash
cd Day04-Complete
dotnet run
```

## 📋 What This Program Does

A **ServiceHub Customer Manager** that:
- ✅ Stores customers in a list
- ✅ Stores emails in a dictionary
- ✅ Displays all customers
- ✅ Searches by name (LINQ `.Where()`)
- ✅ Sorts customers (LINQ `.OrderBy()`)
- ✅ Calculates statistics (LINQ `.Sum()`, `.Average()`, `.Max()`)
- ✅ Filters by name length (LINQ)
- ✅ Groups by first letter (advanced LINQ `.GroupBy()`)

## 💡 Key Concepts Demonstrated

| Concept | Example |
|---------|---------|
| **List<T>** | `customerNames` list |
| **Dictionary<K,V>** | `customerEmails` dictionary |
| **Array** | `customerIds` array |
| **LINQ .Where()** | Customers containing 'a' |
| **LINQ .OrderBy()** | Sorted A-Z |
| **LINQ .Sum()/.Average()/.Max()** | Statistics |
| **LINQ .GroupBy()** | Group by first letter |
| **Foreach loops** | Display all items |

## 🎯 What Day 05 Will Do

Day 05 refactors this exact program into **clean, reusable methods**.

Instead of inline code:
```csharp
var withA = customerNames.Where(n => n.ToLower().Contains("a")).ToList();
```

We'll have methods:
```csharp
var results = SearchCustomers(customerNames, "a");
DisplayCustomers(results);
```

## ✅ Expected Output

```
=== ServiceHub Customer Manager ===

--- All Customers ---
• Alice Johnson          (alice@example.com)
• Bob Smith              (bob@example.com)
...

Total customers: 5

--- Customers containing 'a' (LINQ) ---
• Alice Johnson
• Diana Prince
• Charlie Brown

--- Customers sorted A-Z (LINQ) ---
• Alice Johnson
• Bob Smith
• Charlie Brown
...

--- Statistics ---
Total characters across all names: 87
Average name length: 17.4 characters
Longest name: 15 characters
```

## 🔍 Code Structure

1. **Initialize collections** — Lists, dictionaries, arrays
2. **Display all** — Show complete data
3. **Count** — Basic statistics
4. **Search** — LINQ `.Where()`
5. **Sort** — LINQ `.OrderBy()`
6. **Advanced stats** — `.Sum()`, `.Average()`, `.Max()`
7. **Filter** — By criteria
8. **Group** — `.GroupBy()`

## 🎬 Summary

This program demonstrates:
- Multiple collection types working together
- Powerful LINQ operations on real data
- Clean, formatted console output
- Foundation for Day 05 refactoring

**Tomorrow:** We organize this into methods for better code structure.

---

## 🟦 ServiceHub Context

This customer manager is the **data foundation** for ServiceHub:
- **Week 1:** Learn to manage data (this week)
- **Week 2:** Build an API to expose this data
- **Week 3:** Connect to a database
- **Week 4:** Add a web UI

This is the core pattern repeated throughout 30 days.
