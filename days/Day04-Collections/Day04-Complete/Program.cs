using System;
using System.Collections.Generic;
using System.Linq;

// Day 04 — Collections: Complete Example
// ServiceHub Customer Manager
// Demonstrates: Arrays, Lists, Dictionaries, LINQ

Console.WriteLine("=== ServiceHub Customer Manager ===\n");

// Initialize collections
List<string> customerNames = new()
{
    "Alice Johnson",
    "Bob Smith",
    "Charlie Brown",
    "Diana Prince",
    "Eve Davis"
};

Dictionary<string, string> customerEmails = new()
{
    { "Alice Johnson", "alice@example.com" },
    { "Bob Smith", "bob@example.com" },
    { "Charlie Brown", "charlie@example.com" },
    { "Diana Prince", "diana@example.com" },
    { "Eve Davis", "eve@example.com" }
};

// 1. Display all customers
Console.WriteLine("--- All Customers ---");
foreach (var name in customerNames)
{
    string email = customerEmails[name];
    Console.WriteLine($"• {name,-20} ({email})");
}
Console.WriteLine();

// 2. Count
Console.WriteLine($"Total customers: {customerNames.Count}");
Console.WriteLine();

// 3. Search by name (LINQ)
Console.WriteLine("--- Customers containing 'a' (LINQ) ---");
var withA = customerNames.Where(n => n.ToLower().Contains("a")).ToList();
foreach (var name in withA)
{
    Console.WriteLine($"• {name}");
}
Console.WriteLine();

// 4. Sort by name (LINQ)
Console.WriteLine("--- Customers sorted A-Z (LINQ) ---");
var sorted = customerNames.OrderBy(n => n).ToList();
foreach (var name in sorted)
{
    Console.WriteLine($"• {name}");
}
Console.WriteLine();

// 5. Statistics (LINQ)
Console.WriteLine("--- Statistics ---");
int totalChars = customerNames.Sum(n => n.Length);
double avgLength = customerNames.Average(n => n.Length);
int longestName = customerNames.Max(n => n.Length);

Console.WriteLine($"Total characters across all names: {totalChars}");
Console.WriteLine($"Average name length: {avgLength:F1} characters");
Console.WriteLine($"Longest name: {longestName} characters");
Console.WriteLine();

// 6. Filter by name length (LINQ)
Console.WriteLine("--- Customers with names > 10 characters (LINQ) ---");
var longNames = customerNames.Where(n => n.Length > 10).ToList();
foreach (var name in longNames)
{
    Console.WriteLine($"• {name} ({name.Length} chars)");
}
Console.WriteLine();

// 7. Array example (fixed size)
Console.WriteLine("--- Customer IDs (Array) ---");
int[] customerIds = new int[customerNames.Count];
for (int i = 0; i < customerIds.Length; i++)
{
    customerIds[i] = i + 1;
    Console.WriteLine($"ID {customerIds[i]}: {customerNames[i]}");
}
Console.WriteLine();

// 8. Group by first letter (advanced LINQ)
Console.WriteLine("--- Customers grouped by first letter ---");
var grouped = customerNames.GroupBy(n => n[0]).OrderBy(g => g.Key);
foreach (var group in grouped)
{
    Console.WriteLine($"{group.Key}: {string.Join(", ", group)}");
}

Console.WriteLine("\n✅ Day 04 Complete!");
