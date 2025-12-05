using System;

// Day 01 — Setup & Tooling: Complete Example
// Demonstrates: Console output, variables, string interpolation, and DateTime

// Step 1: Print a welcome message
Console.WriteLine("=== Welcome to AscendCSharp30 ===\n");

// Step 2: Declare and use variables
string name = "Developer";
Console.WriteLine($"Hello, I'm {name}!");

// Step 3: Display the current date and time
DateTime now = DateTime.Now;
Console.WriteLine($"Today is {now:MMMM d, yyyy} at {now:h:mm tt}\n");

// Step 4: Demonstrate different variable types
int totalDays = 30;
double hoursPerDay = 1.5;
bool isSetupComplete = true;

Console.WriteLine($"Course: 30 days of C#");
Console.WriteLine($"Hours per day: {hoursPerDay}");
Console.WriteLine($"Setup complete: {isSetupComplete}\n");

// Step 5: Simple calculation
int daysRemaining = totalDays - 1;
double totalHours = totalDays * hoursPerDay;

Console.WriteLine($"Days remaining: {daysRemaining}");
Console.WriteLine($"Total hours: {totalHours}\n");

// Step 6: Closing message
Console.WriteLine("Your environment is ready!");
Console.WriteLine("See you on Day 02! 🚀");


