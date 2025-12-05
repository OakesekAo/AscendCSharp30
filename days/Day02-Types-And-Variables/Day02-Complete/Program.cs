using System;

// Day 02 — Types & Variables: Complete Example
// Demonstrates: data types, variables, user input, parsing, calculations

Console.WriteLine("=== Welcome to Your Profile ===\n");

// Step 1: Get name (string)
Console.Write("What is your name? ");
string name = Console.ReadLine();

// Step 2: Get age (convert string to int)
Console.Write("How old are you? ");
int age = int.Parse(Console.ReadLine());

// Step 3: Get salary (convert string to double)
Console.Write("What is your current salary? ");
double salary = double.Parse(Console.ReadLine());

// Step 4: Perform calculations
int ageIn10Years = age + 10;
double salaryWithRaise = salary * 1.10;  // 10% raise

// Step 5: Display results with formatting
Console.WriteLine("\n=== Your Information ===");
Console.WriteLine($"Name: {name}");
Console.WriteLine($"Current Age: {age}");
Console.WriteLine($"Age in 10 years: {ageIn10Years}");
Console.WriteLine($"Current Salary: ${salary:F2}");
Console.WriteLine($"Salary with 10% raise: ${salaryWithRaise:F2}");
Console.WriteLine();

// Bonus: Show what we learned
Console.WriteLine("=== Data Types Used ===");
Console.WriteLine($"name is a {name.GetType().Name}");
Console.WriteLine($"age is a {age.GetType().Name}");
Console.WriteLine($"salary is a {salary.GetType().Name}");

