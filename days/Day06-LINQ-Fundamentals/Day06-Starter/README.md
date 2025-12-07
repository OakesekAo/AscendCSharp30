# Day 06 — LINQ Fundamentals

Welcome to Day 6! Today you'll master **LINQ** — Language Integrated Query, a powerful way to filter, transform, and aggregate data.

By the end of this day, you will have:
- ✅ Understood LINQ syntax and methods
- ✅ Filtered collections with `.Where()`
- ✅ Transformed data with `.Select()`
- ✅ Aggregated with `.Sum()`, `.Average()`, `.Count()`
- ✅ Sorted with `.OrderBy()` and `.OrderByDescending()`
- ✅ Built ServiceHub queries

---

## 🎯 Learning Objectives

1. **Understand LINQ** — What it is and why it matters
2. **Use `.Where()`** — Filter collections
3. **Use `.Select()`** — Transform/map data
4. **Use aggregates** — `.Sum()`, `.Average()`, `.Count()`
5. **Sort data** — `.OrderBy()`, `.OrderByDescending()`, `.ThenBy()`
6. **Chain methods** — Combine LINQ operations
7. **Apply to ServiceHub** — Query customers and work orders

---

## 📋 Prerequisites

Before you start:
- Days 01-05 complete (comfortable with collections and methods)
- Your editor open and ready
- ~60 minutes of uninterrupted time

---

## Setup

```bash
mkdir Day06-LINQ
cd Day06-LINQ
dotnet new console
```

---

## Step 1: What is LINQ?

**LINQ** = Language Integrated Query. SQL-like syntax for C# data.

```csharp
List<int> scores = new() { 95, 87, 92, 78, 85, 90 };

var result = scores
    .Where(s => s >= 85)
    .OrderByDescending(s => s)
    .ToList();
// [95, 92, 90, 85]
```

---

## Step 2: `.Where()` — Filter

```csharp
List<int> numbers = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

var evens = numbers.Where(n => n % 2 == 0).ToList();
// [2, 4, 6, 8, 10]

var big = numbers.Where(n => n > 5).ToList();
// [6, 7, 8, 9, 10]
```

---

## Step 3: `.Select()` — Transform

```csharp
List<int> numbers = new() { 1, 2, 3, 4, 5 };

var doubled = numbers.Select(n => n * 2).ToList();
// [2, 4, 6, 8, 10]

var strings = numbers.Select(n => n.ToString()).ToList();
// ["1", "2", "3", "4", "5"]
```

---

## Step 4: Aggregates

```csharp
List<int> scores = new() { 95, 87, 92, 78, 85, 90 };

int total = scores.Sum();           // 527
double avg = scores.Average();      // 87.83
int count = scores.Count();         // 6
int max = scores.Max();             // 95
int excellent = scores.Count(s => s >= 90); // 3
```

---

## Step 5: Sorting

```csharp
List<int> numbers = new() { 3, 1, 4, 1, 5, 9 };

var sorted = numbers.OrderBy(n => n).ToList();
// [1, 1, 3, 4, 5, 9]

var reverse = numbers.OrderByDescending(n => n).ToList();
// [9, 5, 4, 3, 1, 1]
```

---

## Step 6: Chaining

```csharp
List<int> scores = new() { 95, 87, 92, 78, 85, 90, 88, 91 };

var top3 = scores
    .Where(s => s >= 85)
    .OrderByDescending(s => s)
    .Take(3)
    .ToList();
// [95, 92, 91]
```

---

## Mini Challenge: ServiceHub Analytics

**Build a program that:**
1. Has a list of job durations
2. Calculates using LINQ:
   - Total hours
   - Average duration
   - Longest job
   - High-priority jobs (>3 hours)

**Hints:**
```csharp
var durations = new List<int> { 2, 4, 1, 3, 2, 5 };

int total = durations.Sum();
int highCount = durations.Count(d => d > 3);
var topDuration = durations.OrderByDescending(d => d).First();
```

---

## ✅ Checklist

- [ ] You understand LINQ
- [ ] You can use `.Where()` to filter
- [ ] You can use `.Select()` to transform
- [ ] You can use aggregates (Sum, Average, Count)
- [ ] You can sort with `.OrderBy()`
- [ ] You can chain LINQ methods
- [ ] You completed the analytics challenge

---

## 🔗 Next Steps

Day 07: **Mini Console Project** — ServiceHub Job Scheduler!

---

## 📚 Resources

- <a href="https://learn.microsoft.com/en-us/dotnet/csharp/linq/" target="_blank">LINQ Overview</a>
- <a href="https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable" target="_blank">LINQ Methods</a>

---

**You've got this.** See you on Day 07! 🚀
