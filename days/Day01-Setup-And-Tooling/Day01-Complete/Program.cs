using System;

// ============================================================================
// Day 01 — Setup & Tooling: Complete Example
// ============================================================================
// This program demonstrates:
// - Console output using Console.WriteLine()
// - Variable declaration and assignment
// - String interpolation
// - Working with DateTime
// - Basic comments and code structure
// ============================================================================

// Step 1: Print a welcome message
Console.WriteLine("=== Welcome to AscendCSharp30 ===\n");

// Step 2: Declare and use string variables
string courseName = "AscendCSharp30";
string dayTitle = "Setup & Tooling";
int dayNumber = 1;

// Print course info using string interpolation (the $ prefix allows {variable} syntax)
Console.WriteLine($"Course: {courseName}");
Console.WriteLine($"Day {dayNumber}: {dayTitle}\n");

// Step 3: Get and display the current date and time
DateTime now = DateTime.Now;
Console.WriteLine($"Started on: {now.DayOfWeek}, {now:MMMM d, yyyy} at {now:h:mm tt}");
Console.WriteLine();

// Step 4: Demonstrate basic variable types
// Integer: whole numbers
int totalDays = 30;
Console.WriteLine($"Total days in course: {totalDays}");

// Double: decimal numbers
double hoursPerDay = 1.5;
Console.WriteLine($"Estimated time per day: {hoursPerDay} hours");

// Boolean: true/false
bool isSetupComplete = true;
Console.WriteLine($"Environment setup complete: {isSetupComplete}");
Console.WriteLine();

// Step 5: Simple calculation
int daysRemaining = totalDays - dayNumber;
double totalEstimatedHours = totalDays * hoursPerDay;
Console.WriteLine($"Days remaining: {daysRemaining}");
Console.WriteLine($"Total estimated course time: {totalEstimatedHours} hours\n");

// Step 6: Print a closing message
Console.WriteLine("=== You're Ready! ===");
Console.WriteLine("Your development environment is set up and working.");
Console.WriteLine("Tomorrow you'll learn about Types and Variables in depth.");
Console.WriteLine("\nGood luck on your 30-day journey! 🚀");

