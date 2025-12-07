using System;
using System.Collections.Generic;
using System.Linq;

// Day 06 — LINQ Fundamentals: Complete Example
// ServiceHub Customer Manager (Advanced LINQ from Day 05)
// Demonstrates: Advanced LINQ queries, method chaining, real-world patterns

Console.WriteLine("=== ServiceHub Customer Manager (LINQ Advanced) ===\n");

// Initialize collections
List<string> customerNames = new()
{
    "Alice Johnson",
    "Bob Smith",
    "Charlie Brown",
    "Diana Prince",
    "Eve Davis",
    "Frank Miller",
    "Grace Lee"
};

Dictionary<string, string> customerEmails = new()
{
    { "Alice Johnson", "alice@example.com" },
    { "Bob Smith", "bob@example.com" },
    { "Charlie Brown", "charlie@example.com" },
    { "Diana Prince", "diana@example.com" },
    { "Eve Davis", "eve@example.com" },
    { "Frank Miller", "frank@example.com" },
    { "Grace Lee", "grace@example.com" }
};

// More realistic: work orders with durations
List<WorkOrder> workOrders = new()
{
    new() { Id = 1, Customer = "Alice Johnson", Description = "Cleaning", Hours = 2, Status = "Completed" },
    new() { Id = 2, Customer = "Bob Smith", Description = "Mowing", Hours = 4, Status = "Scheduled" },
    new() { Id = 3, Customer = "Alice Johnson", Description = "Windows", Hours = 1, Status = "Completed" },
    new() { Id = 4, Customer = "Charlie Brown", Description = "HVAC Service", Hours = 3, Status = "InProgress" },
    new() { Id = 5, Customer = "Diana Prince", Description = "Cleaning", Hours = 2, Status = "Scheduled" },
    new() { Id = 6, Customer = "Eve Davis", Description = "Lawn Care", Hours = 5, Status = "Completed" }
};

// 1. Basic LINQ - Display all customers
Console.WriteLine("--- All Customers ---");
foreach (var name in customerNames)
{
    Console.WriteLine($"• {name}");
}
Console.WriteLine();

// 2. LINQ .Where() - Filter by condition
Console.WriteLine("--- High-value customers (name length > 12) (LINQ .Where()) ---");
var highValue = customerNames.Where(n => n.Length > 12).ToList();
foreach (var name in highValue)
{
    Console.WriteLine($"• {name}");
}
Console.WriteLine();

// 3. LINQ .Select() - Transform data
Console.WriteLine("--- Customers with email (LINQ .Select()) ---");
var withEmail = customerNames.Select(n => new { Name = n, Email = customerEmails[n] }).ToList();
foreach (var item in withEmail)
{
    Console.WriteLine($"• {item.Name}: {item.Email}");
}
Console.WriteLine();

// 4. LINQ Aggregates - Statistics
Console.WriteLine("--- Statistics (LINQ Aggregates) ---");
int totalCustomers = customerNames.Count();
int maxNameLength = customerNames.Max(n => n.Length);
int minNameLength = customerNames.Min(n => n.Length);
double avgNameLength = customerNames.Average(n => n.Length);
Console.WriteLine($"Total: {totalCustomers}, Max: {maxNameLength}, Min: {minNameLength}, Avg: {avgNameLength:F1}");
Console.WriteLine();

// 5. LINQ .OrderBy() & .OrderByDescending() - Sorting
Console.WriteLine("--- Customers sorted by name (LINQ .OrderBy()) ---");
var sorted = customerNames.OrderBy(n => n).ToList();
foreach (var name in sorted.Take(3)) // Show first 3
{
    Console.WriteLine($"• {name}");
}
Console.WriteLine("...\n");

// 6. LINQ .ThenBy() - Multiple sort criteria
Console.WriteLine("--- Work orders sorted by hours then status (LINQ .ThenBy()) ---");
var sortedOrders = workOrders
    .OrderBy(w => w.Hours)
    .ThenBy(w => w.Status)
    .ToList();
foreach (var order in sortedOrders.Take(5))
{
    Console.WriteLine($"• {order.Description}: {order.Hours}h ({order.Status})");
}
Console.WriteLine();

// 7. LINQ .Take() & .Skip() - Pagination
Console.WriteLine("--- First 3 customers (LINQ .Take()) ---");
var first3 = customerNames.Take(3).ToList();
foreach (var name in first3)
{
    Console.WriteLine($"• {name}");
}
Console.WriteLine();

// 8. LINQ .First(), .Last() - Accessing elements
Console.WriteLine("--- First and Last customers (LINQ .First()/.Last()) ---");
string first = customerNames.First();
string last = customerNames.Last();
Console.WriteLine($"First: {first}");
Console.WriteLine($"Last: {last}");
Console.WriteLine();

// 9. LINQ .Any() & .All() - Predicates
Console.WriteLine("--- Checking conditions (LINQ .Any()/.All()) ---");
bool hasShortName = customerNames.Any(n => n.Length < 8);
bool allHaveEmail = customerNames.All(n => customerEmails.ContainsKey(n));
Console.WriteLine($"Has customer with name < 8 chars: {hasShortName}");
Console.WriteLine($"All customers have email: {allHaveEmail}");
Console.WriteLine();

// 10. LINQ .GroupBy() - Grouping
Console.WriteLine("--- Customers grouped by first letter (LINQ .GroupBy()) ---");
var grouped = customerNames.GroupBy(n => n[0]).OrderBy(g => g.Key);
foreach (var group in grouped)
{
    var names = string.Join(", ", group.Select(n => n.Split()[0])); // First names only
    Console.WriteLine($"{group.Key}: {names}");
}
Console.WriteLine();

// 11. LINQ Method Chaining - Complex query
Console.WriteLine("--- Complex query: Top 3 longest-name customers, sorted (LINQ chaining) ---");
var complexQuery = customerNames
    .Where(n => n.Length > 10)              // Filter
    .OrderByDescending(n => n.Length)       // Sort descending
    .Take(3)                                 // Get top 3
    .ToList();
foreach (var name in complexQuery)
{
    Console.WriteLine($"• {name} ({name.Length} chars)");
}
Console.WriteLine();

// 12. LINQ on work orders - Real ServiceHub scenario
Console.WriteLine("--- Work order analytics (LINQ on real data) ---");
int completedCount = workOrders.Count(w => w.Status == "Completed");
double totalHours = workOrders.Sum(w => w.Hours);
double avgHours = workOrders.Average(w => w.Hours);
var byCustomer = workOrders.GroupBy(w => w.Customer);

Console.WriteLine($"Completed jobs: {completedCount}");
Console.WriteLine($"Total hours: {totalHours}");
Console.WriteLine($"Average job length: {avgHours:F1} hours");
Console.WriteLine("\nJobs by customer:");
foreach (var group in byCustomer.OrderByDescending(g => g.Count()))
{
    Console.WriteLine($"• {group.Key}: {group.Count()} jobs, {group.Sum(w => w.Hours)}h total");
}

Console.WriteLine("\n✅ Day 06 Complete!");

// ========== DATA CLASS ==========
class WorkOrder
{
    public int Id { get; set; }
    public string Customer { get; set; }
    public string Description { get; set; }
    public int Hours { get; set; }
    public string Status { get; set; }
}
