using System;
using System.Collections.Generic;
using System.Linq;

// Day 07 — Week 1 Capstone: ServiceHub Job Scheduler
// Complete, production-style console application
// Uses: Variables, collections, control flow, methods, LINQ

Console.WriteLine("╔════════════════════════════════════════╗");
Console.WriteLine("║   ServiceHub Job Scheduler - MVP v1.0  ║");
Console.WriteLine("╚════════════════════════════════════════╝\n");

// Initialize data
var workOrders = InitializeWorkOrders();
var running = true;

while (running)
{
    DisplayMenu();
    string choice = Console.ReadLine() ?? "";
    
    switch (choice)
    {
        case "1":
            DisplayAllWorkOrders(workOrders);
            break;
        case "2":
            SearchByCustomer(workOrders);
            break;
        case "3":
            FilterByStatus(workOrders);
            break;
        case "4":
            ShowStatistics(workOrders);
            break;
        case "5":
            CreateNewWorkOrder(workOrders);
            break;
        case "6":
            UpdateWorkOrderStatus(workOrders);
            break;
        case "7":
            running = false;
            Console.WriteLine("\n👋 Thank you for using ServiceHub!");
            break;
        default:
            Console.WriteLine("❌ Invalid choice. Try again.\n");
            break;
    }
}

// ========== MENU ==========
void DisplayMenu()
{
    Console.WriteLine("\n┌─ Menu ─────────────────────────────────┐");
    Console.WriteLine("│ 1. View all work orders                 │");
    Console.WriteLine("│ 2. Search by customer                   │");
    Console.WriteLine("│ 3. Filter by status                     │");
    Console.WriteLine("│ 4. View statistics                      │");
    Console.WriteLine("│ 5. Create new work order                │");
    Console.WriteLine("│ 6. Update work order status             │");
    Console.WriteLine("│ 7. Exit                                 │");
    Console.WriteLine("└─────────────────────────────────────────┘");
    Console.Write("Choose an option (1-7): ");
}

// ========== DATA INITIALIZATION ==========
List<WorkOrder> InitializeWorkOrders()
{
    return new()
    {
        new() { Id = 1, Customer = "Alice Johnson", Description = "Gutter Cleaning", ScheduledDate = DateTime.Now, DurationHours = 2, Status = "Scheduled" },
        new() { Id = 2, Customer = "Bob Smith", Description = "Lawn Mowing", ScheduledDate = DateTime.Now.AddDays(1), DurationHours = 4, Status = "Scheduled" },
        new() { Id = 3, Customer = "Charlie Brown", Description = "HVAC Service", ScheduledDate = DateTime.Now.AddDays(-1), DurationHours = 3, Status = "Completed" },
        new() { Id = 4, Customer = "Alice Johnson", Description = "Window Washing", ScheduledDate = DateTime.Now.AddDays(2), DurationHours = 1, Status = "Scheduled" },
        new() { Id = 5, Customer = "Diana Prince", Description = "Appliance Repair", ScheduledDate = DateTime.Now, DurationHours = 2, Status = "InProgress" },
    };
}

// ========== DISPLAY OPERATIONS ==========
void DisplayAllWorkOrders(List<WorkOrder> orders)
{
    if (!orders.Any())
    {
        Console.WriteLine("\n❌ No work orders found.");
        return;
    }
    
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║                         ALL WORK ORDERS                           ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════╝");
    Console.WriteLine();
    
    var sorted = orders.OrderBy(o => o.ScheduledDate).ToList();
    foreach (var order in sorted)
    {
        DisplayWorkOrder(order);
    }
}

void DisplayWorkOrder(WorkOrder order)
{
    string statusEmoji = order.Status switch
    {
        "Scheduled" => "📅",
        "InProgress" => "🔧",
        "Completed" => "✅",
        _ => "❓"
    };
    
    Console.WriteLine($"╭─ Job #{order.Id} {statusEmoji}");
    Console.WriteLine($"│ Customer: {order.Customer}");
    Console.WriteLine($"│ Task: {order.Description}");
    Console.WriteLine($"│ Scheduled: {order.ScheduledDate:M/d/yyyy h:mm tt}");
    Console.WriteLine($"│ Duration: {order.DurationHours}h");
    Console.WriteLine($"│ Status: {order.Status}");
    Console.WriteLine("╰─────────────────────────────────────────────────────────────────────");
    Console.WriteLine();
}

// ========== SEARCH & FILTER ==========
void SearchByCustomer(List<WorkOrder> orders)
{
    Console.Write("\n🔍 Enter customer name to search: ");
    string search = Console.ReadLine() ?? "";
    
    var results = orders.Where(o => o.Customer.ToLower().Contains(search.ToLower())).ToList();
    
    if (!results.Any())
    {
        Console.WriteLine($"❌ No work orders found for '{search}'.");
        return;
    }
    
    Console.WriteLine($"\n📋 Found {results.Count} job(s) for '{search}':");
    foreach (var order in results)
    {
        Console.WriteLine($"  • {order.Description} ({order.Status})");
    }
}

void FilterByStatus(List<WorkOrder> orders)
{
    Console.WriteLine("\n📊 Available statuses:");
    Console.WriteLine("  1. Scheduled");
    Console.WriteLine("  2. InProgress");
    Console.WriteLine("  3. Completed");
    Console.Write("Choose status (1-3): ");
    
    string statusChoice = Console.ReadLine() ?? "";
    string status = statusChoice switch
    {
        "1" => "Scheduled",
        "2" => "InProgress",
        "3" => "Completed",
        _ => "Invalid"
    };
    
    if (status == "Invalid")
    {
        Console.WriteLine("❌ Invalid choice.");
        return;
    }
    
    var results = orders.Where(o => o.Status == status).ToList();
    
    Console.WriteLine($"\n📋 {results.Count} job(s) with status '{status}':");
    foreach (var order in results.OrderBy(o => o.ScheduledDate))
    {
        Console.WriteLine($"  • {order.Customer}: {order.Description} ({order.DurationHours}h)");
    }
}

// ========== STATISTICS ==========
void ShowStatistics(List<WorkOrder> orders)
{
    if (!orders.Any())
    {
        Console.WriteLine("\n❌ No work orders to analyze.");
        return;
    }
    
    Console.WriteLine("\n╔════════════════════════════════════════╗");
    Console.WriteLine("║         WORK ORDER STATISTICS           ║");
    Console.WriteLine("╚════════════════════════════════════════╝\n");
    
    int total = orders.Count();
    int completed = orders.Count(o => o.Status == "Completed");
    int scheduled = orders.Count(o => o.Status == "Scheduled");
    int inProgress = orders.Count(o => o.Status == "InProgress");
    int totalHours = orders.Sum(o => o.DurationHours);
    double avgHours = orders.Average(o => o.DurationHours);
    int maxHours = orders.Max(o => o.DurationHours);
    
    Console.WriteLine($"📊 Total work orders: {total}");
    Console.WriteLine($"   ✅ Completed: {completed}");
    Console.WriteLine($"   🔧 In Progress: {inProgress}");
    Console.WriteLine($"   📅 Scheduled: {scheduled}");
    Console.WriteLine();
    Console.WriteLine($"⏱️  Total hours: {totalHours}h");
    Console.WriteLine($"   Average: {avgHours:F1}h per job");
    Console.WriteLine($"   Longest job: {maxHours}h");
    Console.WriteLine();
    
    Console.WriteLine("👥 Top customers:");
    var topCustomers = orders
        .GroupBy(o => o.Customer)
        .OrderByDescending(g => g.Count())
        .Take(3);
    
    foreach (var group in topCustomers)
    {
        int customerTotal = group.Sum(o => o.DurationHours);
        Console.WriteLine($"   • {group.Key}: {group.Count()} job(s), {customerTotal}h total");
    }
}

// ========== CREATE & UPDATE ==========
void CreateNewWorkOrder(List<WorkOrder> orders)
{
    Console.WriteLine("\n➕ Create New Work Order");
    
    Console.Write("  Customer name: ");
    string customer = Console.ReadLine() ?? "";
    if (string.IsNullOrWhiteSpace(customer))
    {
        Console.WriteLine("❌ Customer name is required.");
        return;
    }
    
    Console.Write("  Description: ");
    string description = Console.ReadLine() ?? "";
    if (string.IsNullOrWhiteSpace(description))
    {
        Console.WriteLine("❌ Description is required.");
        return;
    }
    
    Console.Write("  Duration (hours): ");
    if (!int.TryParse(Console.ReadLine(), out int hours) || hours <= 0)
    {
        Console.WriteLine("❌ Please enter a valid number of hours.");
        return;
    }
    
    int newId = orders.Max(o => o.Id) + 1;
    var newOrder = new WorkOrder
    {
        Id = newId,
        Customer = customer,
        Description = description,
        ScheduledDate = DateTime.Now,
        DurationHours = hours,
        Status = "Scheduled"
    };
    
    orders.Add(newOrder);
    Console.WriteLine($"\n✅ Work order #{newId} created successfully!");
}

void UpdateWorkOrderStatus(List<WorkOrder> orders)
{
    Console.Write("\n🔄 Enter work order ID to update: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("❌ Invalid ID.");
        return;
    }
    
    var order = orders.FirstOrDefault(o => o.Id == id);
    if (order == null)
    {
        Console.WriteLine("❌ Work order not found.");
        return;
    }
    
    Console.WriteLine($"\n  Current status: {order.Status}");
    Console.WriteLine("  New status:");
    Console.WriteLine("    1. Scheduled");
    Console.WriteLine("    2. InProgress");
    Console.WriteLine("    3. Completed");
    Console.Write("  Choose (1-3): ");
    
    string newStatus = Console.ReadLine() switch
    {
        "1" => "Scheduled",
        "2" => "InProgress",
        "3" => "Completed",
        _ => "Invalid"
    };
    
    if (newStatus == "Invalid")
    {
        Console.WriteLine("❌ Invalid choice.");
        return;
    }
    
    order.Status = newStatus;
    Console.WriteLine($"✅ Status updated to '{newStatus}'!");
}

// ========== DATA CLASS ==========
class WorkOrder
{
    public int Id { get; set; }
    public string Customer { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime ScheduledDate { get; set; }
    public int DurationHours { get; set; }
    public string Status { get; set; } = "Scheduled";
}
