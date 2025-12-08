# Day 04 — Collections

Welcome to Day 4! Today you'll learn about **collections** — ways to store and work with groups of data instead of individual variables.

By the end of this day, you will have:
- ✅ Understood arrays and their limitations
- ✅ Used lists for dynamic collections
- ✅ Worked with dictionaries for key-value pairs
- ✅ Iterated through collections with loops
- ✅ Used LINQ for basic querying

---

## 🎯 Learning Objectives

1. **Understand arrays** — Fixed-size collections
2. **Use lists** — Dynamic collections that grow/shrink
3. **Work with dictionaries** — Key-value storage
4. **Iterate collections** — Loops and foreach
5. **Query with LINQ** — Basic filtering and selection
6. **Apply to ServiceHub** — Store multiple customers and work orders

---

## 📋 Prerequisites

Before you start:
- Days 01-03 complete (comfortable with variables, types, control flow)
- Your editor open and ready
- ~60 minutes of uninterrupted time

---

## Setup: Create Your Day 04 Project

```bash
mkdir Day04-Collections
cd Day04-Collections
dotnet new console
```

---

## Step 1: Understanding Arrays

An **array** is a fixed-size collection of elements of the same type.

```csharp
int[] scores = new int[5];
scores[0] = 95;
scores[1] = 87;

Console.WriteLine(scores[0]); // Output: 95
Console.WriteLine(scores.Length); // Output: 5
```

**Key points:**
- Arrays have a **fixed size** (can't grow)
- Access by **index** (0, 1, 2, ...)
- All elements are the **same type**

---

## Step 2: Lists — Dynamic Collections

A **list** is a dynamic collection that grows and shrinks.

```csharp
using System.Collections.Generic;

List<string> customers = new List<string>();
customers.Add("Alice");
customers.Add("Bob");
customers.Add("Charlie");

Console.WriteLine(customers[0]); // "Alice"
Console.WriteLine(customers.Count); // 3

customers.Remove("Bob");
customers.Clear();
```

**Key points:**
- Lists are **generic** — `List<T>`
- They **grow and shrink** automatically
- Use `.Add()` to add items
- Use `.Count` to get size

---

## Step 3: Dictionaries — Key-Value Pairs

A **dictionary** stores pairs of keys and values.

```csharp
Dictionary<string, string> customers = new Dictionary<string, string>();

customers["alice@example.com"] = "Alice Johnson";
customers["bob@example.com"] = "Bob Smith";

Console.WriteLine(customers["alice@example.com"]); // "Alice Johnson"

if (customers.ContainsKey("alice@example.com"))
{
    Console.WriteLine("Customer found!");
}

foreach (var entry in customers)
{
    Console.WriteLine($"{entry.Key}: {entry.Value}");
}
```

**Key points:**
- Store **key-value pairs**
- Access by **key** (not index)
- Check with `.ContainsKey()`
- Iterate with `foreach`

---

## Step 4: Iterating Collections

```csharp
List<string> customers = new List<string> { "Alice", "Bob", "Charlie" };

// Foreach loop
foreach (string customer in customers)
{
    Console.WriteLine(customer);
}

// For loop with index
for (int i = 0; i < customers.Count; i++)
{
    Console.WriteLine($"{i}: {customers[i]}");
}
```

---

## Step 5: LINQ — Query Collections

LINQ lets you filter, map, and aggregate collections:

```csharp
List<int> scores = new List<int> { 95, 87, 92, 78, 85, 90 };

// Filter
var excellent = scores.Where(s => s >= 90).ToList();
// [95, 92, 90]

// Map
var doubled = scores.Select(s => s * 2).ToList();
// [190, 174, 184, 156, 170, 180]

// Count
int count = scores.Count(s => s >= 85);
// 4

// Sum
int total = scores.Sum();
// 527

// Average
double avg = scores.Average();
// 87.83...
```

---

## Step 6: Mini Challenge — ServiceHub Customer Manager

**Build a program that:**
1. Manages a list of customer names
2. Manages a dictionary of email addresses
3. Displays all customers
4. Searches customers by name (LINQ)
5. Shows statistics (count, average name length)

**Example output:**
```
=== ServiceHub Customer Manager ===

All Customers:
- Alice Johnson (alice@example.com)
- Bob Smith (bob@example.com)
- Charlie Brown (charlie@example.com)

Total: 3

Customers with 'a' in name:
- Charlie Brown

Average name length: 15 characters
```

**Steps:**
1. Create `List<string>` of customer names
2. Create `Dictionary<string, string>` of name → email
3. Display all customers
4. Use LINQ `.Where()` to search
5. Use LINQ `.Average()` for stats

**Hints:**
```csharp
List<string> names = new List<string> { "Alice", "Bob", "Charlie" };
Dictionary<string, string> emails = new Dictionary<string, string>
{
    { "Alice", "alice@example.com" },
    { "Bob", "bob@example.com" }
};

// Search
var search = names.Where(n => n.ToLower().Contains("a")).ToList();

// Average
var avgLength = names.Average(n => n.Length);
```

---

## ✅ Checklist

- [ ] You understand arrays vs lists
- [ ] You can create and use lists
- [ ] You can create and use dictionaries
- [ ] You understand foreach loops
- [ ] You can use basic LINQ (Where, Count, Average)
- [ ] You completed the mini challenge

---

## 🔗 Next Steps

Day 05: **Methods & Parameters** — Organize code into reusable functions.

---

## 📚 Resources

- <a href="https://dotnet.microsoft.com/learn/" target="_blank">Getting Started with .NET</a>
- <a href="https://github.com/OakesekAo/AscendCSharp30" target="_blank">AscendCSharp30 Repository</a>

---

**You've got this.** See you on Day 05! 🚀
