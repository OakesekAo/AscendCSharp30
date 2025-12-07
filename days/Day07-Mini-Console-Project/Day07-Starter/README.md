# Day 07 — Mini Console Project: ServiceHub Job Scheduler

## 🚀 Week 1 Capstone

**Maya's Message:**
> "You've learned the fundamentals. Now let's build something real together. A simple job scheduler for ServiceHub. This is what Week 1 looks like: solid foundations."

Today, you'll combine **everything from Days 01-06** into a real, working application: **ServiceHub Job Scheduler**.

---

## 📌 Project Overview

You're building a **console-based work order management system** where:
- Maya can view her work orders
- She can schedule new jobs
- She can filter and search
- She can see analytics

**Tech used:**
- Variables & types
- Collections (lists, dictionaries)
- Control flow (loops, conditionals)
- Methods
- LINQ queries
- String interpolation

---

## Setup

```bash
mkdir Day07-ServiceHubScheduler
cd Day07-ServiceHubScheduler
dotnet new console
```

---

## Requirements

Your application must:

1. **Store work orders** with:
   - ID
   - Customer name
   - Description
   - Scheduled date
   - Duration (hours)
   - Status ("Scheduled", "InProgress", "Completed")

2. **Display all work orders** in a formatted list

3. **Search work orders** by customer name

4. **Filter by status** (show only "Scheduled" jobs)

5. **Calculate statistics:**
   - Total hours scheduled
   - Average job duration
   - Count of jobs by status

6. **Create a new work order** (get user input)

7. **Update job status** (mark as "InProgress" or "Completed")

---

## Example Output

```
================================
  ServiceHub Job Scheduler
================================

--- All Work Orders ---
ID: 1 | Alice    | Cleaning     | 12/5 | 2h | Scheduled
ID: 2 | Bob      | Mowing       | 12/5 | 4h | InProgress
ID: 3 | Charlie  | HVAC Service | 12/6 | 3h | Scheduled

--- Statistics ---
Total work orders: 3
Total hours: 9
Average duration: 3h

Status breakdown:
- Scheduled: 2
- InProgress: 1

--- Alice's Jobs ---
ID: 1 | Cleaning | 12/5 | 2h | Scheduled
```

---

## Build It Step by Step

### Step 1: Create Data Structure

```csharp
// Option 1: Use a class
class WorkOrder
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public string Description { get; set; }
    public DateTime ScheduledDate { get; set; }
    public int DurationHours { get; set; }
    public string Status { get; set; }
}

// Option 2: Use parallel lists
List<int> ids = new();
List<string> customers = new();
List<string> descriptions = new();
```

### Step 2: Initialize Sample Data

```csharp
var workOrders = new List<WorkOrder>
{
    new() { Id = 1, CustomerName = "Alice", Description = "Cleaning", 
            ScheduledDate = new DateTime(2024, 12, 5), DurationHours = 2, Status = "Scheduled" },
    new() { Id = 2, CustomerName = "Bob", Description = "Mowing", 
            ScheduledDate = new DateTime(2024, 12, 5), DurationHours = 4, Status = "InProgress" }
};
```

### Step 3: Create Methods

```csharp
void DisplayAllOrders(List<WorkOrder> orders) { }
void DisplayByStatus(List<WorkOrder> orders, string status) { }
List<WorkOrder> SearchByCustomer(List<WorkOrder> orders, string name) { }
void ShowStatistics(List<WorkOrder> orders) { }
void CreateNewOrder(List<WorkOrder> orders) { }
void UpdateOrderStatus(List<WorkOrder> orders, int id, string newStatus) { }
```

### Step 4: Build the Main Menu

```csharp
while (true)
{
    Console.WriteLine("\n=== ServiceHub Job Scheduler ===");
    Console.WriteLine("1. View all work orders");
    Console.WriteLine("2. Search by customer");
    Console.WriteLine("3. View by status");
    Console.WriteLine("4. View statistics");
    Console.WriteLine("5. Create new work order");
    Console.WriteLine("6. Update work order status");
    Console.WriteLine("7. Exit");
    Console.Write("Choose: ");
    
    string choice = Console.ReadLine();
    
    switch (choice)
    {
        case "1":
            DisplayAllOrders(workOrders);
            break;
        case "2":
            // Get customer name, search, display
            break;
        // ... etc
        case "7":
            return;
    }
}
```

---

## 🎯 Pro Tips

1. **Use LINQ for filtering:**
   ```csharp
   var scheduled = orders.Where(o => o.Status == "Scheduled").ToList();
   ```

2. **Format output:**
   ```csharp
   Console.WriteLine($"ID: {id,-3} | {customer,-10} | {hours}h");
   ```

3. **Validate input:**
   ```csharp
   if (!int.TryParse(Console.ReadLine(), out int id))
   {
       Console.WriteLine("Invalid input");
       continue;
   }
   ```

4. **Keep methods focused** — One job each

5. **Test as you go** — Don't wait until the end

---

## ✅ Checklist

- [ ] You have a data structure for work orders
- [ ] You can display all work orders
- [ ] You can search by customer (LINQ)
- [ ] You can filter by status (LINQ)
- [ ] You can calculate statistics
- [ ] You can create new work orders
- [ ] You can update status
- [ ] Your code is organized into methods
- [ ] You have a working menu loop

---

## 🎬 Reflection

When you're done, think about:
- How could this save to a file? (Day 13)
- How could this connect to a database? (Day 15)
- How could this become a web UI? (Day 19)
- How could this be deployed? (Day 28)

**This is Week 1 complete. You've built something real.**

---

## 🔗 Next Week

Day 08 onwards: **Week 2 — Building the ServiceHub API**

You'll take these foundations and build a professional ASP.NET Core API that other apps can use.

---

**Congratulations on finishing Week 1!** 🚀
