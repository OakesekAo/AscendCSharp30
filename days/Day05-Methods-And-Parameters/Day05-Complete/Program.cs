using System;
using System.Collections.Generic;
using System.Linq;

// Day 05 — Methods & Parameters: Complete Example
// ServiceHub Customer Manager (Refactored from Day 04)
// Demonstrates: Clean methods, parameters, return values, DRY principle

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

// Call clean methods (no inline code)
DisplayAllCustomers(customerNames, customerEmails);
ShowCount(customerNames);
SearchAndDisplay(customerNames, "a");
SortAndDisplay(customerNames);
ShowStatistics(customerNames);
FilterByNameLength(customerNames, 10);
GroupByFirstLetter(customerNames);

Console.WriteLine("\n✅ Day 05 Complete!");

// ============ METHODS ============

/// <summary>
/// Display all customers with their emails
/// </summary>
void DisplayAllCustomers(List<string> names, Dictionary<string, string> emails)
{
    Console.WriteLine("--- All Customers ---");
    foreach (var name in names)
    {
        string email = emails[name];
        Console.WriteLine($"• {name,-20} ({email})");
    }
    Console.WriteLine();
}

/// <summary>
/// Show total customer count
/// </summary>
void ShowCount(List<string> names)
{
    Console.WriteLine($"Total customers: {names.Count}\n");
}

/// <summary>
/// Search customers by name substring (case-insensitive)
/// </summary>
void SearchAndDisplay(List<string> names, string searchTerm)
{
    var results = SearchCustomers(names, searchTerm);
    
    Console.WriteLine($"--- Customers containing '{searchTerm}' (LINQ) ---");
    foreach (var name in results)
    {
        Console.WriteLine($"• {name}");
    }
    Console.WriteLine();
}

/// <summary>
/// Search helper — returns matching customers
/// </summary>
List<string> SearchCustomers(List<string> names, string searchTerm)
{
    return names.Where(n => n.ToLower().Contains(searchTerm.ToLower())).ToList();
}

/// <summary>
/// Sort and display customers alphabetically
/// </summary>
void SortAndDisplay(List<string> names)
{
    var sorted = SortCustomers(names);
    
    Console.WriteLine("--- Customers sorted A-Z (LINQ) ---");
    foreach (var name in sorted)
    {
        Console.WriteLine($"• {name}");
    }
    Console.WriteLine();
}

/// <summary>
/// Sort helper — returns sorted names
/// </summary>
List<string> SortCustomers(List<string> names)
{
    return names.OrderBy(n => n).ToList();
}

/// <summary>
/// Calculate and display name statistics
/// </summary>
void ShowStatistics(List<string> names)
{
    int totalChars = GetTotalCharacters(names);
    double avgLength = GetAverageLength(names);
    int longestName = GetLongestNameLength(names);
    
    Console.WriteLine("--- Statistics ---");
    Console.WriteLine($"Total characters: {totalChars}");
    Console.WriteLine($"Average name length: {avgLength:F1} characters");
    Console.WriteLine($"Longest name: {longestName} characters");
    Console.WriteLine();
}

// Statistics helpers
int GetTotalCharacters(List<string> names) => names.Sum(n => n.Length);
double GetAverageLength(List<string> names) => names.Average(n => n.Length);
int GetLongestNameLength(List<string> names) => names.Max(n => n.Length);

/// <summary>
/// Filter and display customers by minimum name length
/// </summary>
void FilterByNameLength(List<string> names, int minLength)
{
    var filtered = FilterCustomersByLength(names, minLength);
    
    Console.WriteLine($"--- Customers with names > {minLength} characters (LINQ) ---");
    foreach (var name in filtered)
    {
        Console.WriteLine($"• {name} ({name.Length} chars)");
    }
    Console.WriteLine();
}

/// <summary>
/// Filter helper — returns names longer than minLength
/// </summary>
List<string> FilterCustomersByLength(List<string> names, int minLength)
{
    return names.Where(n => n.Length > minLength).ToList();
}

/// <summary>
/// Group customers by first letter
/// </summary>
void GroupByFirstLetter(List<string> names)
{
    var grouped = GetGroupedByFirstLetter(names);
    
    Console.WriteLine("--- Customers grouped by first letter ---");
    foreach (var group in grouped)
    {
        Console.WriteLine($"{group.Key}: {string.Join(", ", group)}");
    }
}

/// <summary>
/// Group helper — returns IEnumerable grouped by first letter
/// </summary>
IEnumerable<IGrouping<char, string>> GetGroupedByFirstLetter(List<string> names)
{
    return names.GroupBy(n => n[0]).OrderBy(g => g.Key);
}
