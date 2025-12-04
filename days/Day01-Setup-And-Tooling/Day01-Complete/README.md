# Day 01 — Setup & Tooling: Complete Example

This is a **working, runnable example** of what you'll have at the end of Day 01 if you follow the Starter guide.

## How to Run

From the command line:

```bash
cd days/Day01-Setup-And-Tooling/Day01-Complete
dotnet run
```

You should see output like:

```
=== Welcome to AscendCSharp30 ===

Course: AscendCSharp30
Day 1: Setup & Tooling

Started on: Wednesday, December 04, 2025 at 2:45 PM

Total days in course: 30
Estimated time per day: 1.5 hours
Environment setup complete: True

Days remaining: 29
Total estimated course time: 45 hours

=== You're Ready! ===
Your development environment is set up and working.
Tomorrow you'll learn about Types and Variables in depth.

Good luck on your 30-day journey! 🚀
```

## What This Program Demonstrates

### 1. **Console Output**
```csharp
Console.WriteLine("Your text here");
```
Prints text to the console. The `WriteLine()` method adds a newline after the text.

### 2. **Variable Declaration**
```csharp
string courseName = "AscendCSharp30";
int dayNumber = 1;
double hoursPerDay = 1.5;
bool isSetupComplete = true;
```
Variables store data. Each has a **type** (`string`, `int`, `double`, `bool`) and a **name**.

### 3. **String Interpolation**
```csharp
Console.WriteLine($"Course: {courseName}");
```
The `$` prefix allows you to embed variables directly in strings using `{variable}` syntax.

### 4. **DateTime (Current Date/Time)**
```csharp
DateTime now = DateTime.Now;
Console.WriteLine($"Started on: {now.DayOfWeek}, {now:MMMM d, yyyy} at {now:h:mm tt}");
```
`DateTime.Now` gets the current date and time. Format strings like `:MMMM d, yyyy` control how it displays.

### 5. **Data Types Used**
- **`string`** — Text ("hello", "AscendCSharp30")
- **`int`** — Whole numbers (1, 30, -5)
- **`double`** — Decimal numbers (1.5, 3.14159)
- **`bool`** — True/False (true, false)

### 6. **Basic Arithmetic**
```csharp
int daysRemaining = totalDays - dayNumber;
double totalEstimatedHours = totalDays * hoursPerDay;
```
You can perform math operations on numbers.

## Key Takeaways

✅ **Your C# environment is working** — this program runs without errors.

✅ **Variables hold data** — each has a type and name.

✅ **Console output is your friend** — use `Console.WriteLine()` to display information.

✅ **String interpolation is powerful** — `$"text {variable}"` makes readable output.

✅ **Different data types for different needs** — string for text, int for whole numbers, double for decimals, bool for true/false.

## Next Steps

After you complete the Starter guide and create your own version, compare it to this Complete example. You should see similar output and understand each line of code.

**Your goal:** Follow the Starter README step-by-step, complete the mini challenge, and end up with a program that works like this one.

## Project Files

- **`Program.cs`** — The C# code you see above
- **`Day01.Complete.csproj`** — Project configuration (targets .NET 10)
- **`bin/`** — Compiled output (created after `dotnet run`)
- **`obj/`** — Temporary build files (created during compilation)

## Platform Notes

- **Windows:** Run commands in PowerShell, cmd, or Windows Terminal
- **macOS/Linux:** Run commands in Terminal or bash
- Output may vary slightly based on your system locale (date format, time format)

---

## Summary

This complete example demonstrates:
- How to structure a C# console application
- Variable declaration and usage (string, int, double, bool)
- String interpolation for clean, readable output
- DateTime formatting for displaying dates and times
- Basic arithmetic operations

**Use this as a reference:** After you complete the Starter guide on your local machine, compare your `Program.cs` to this example. You should understand each line and see similar output.

---

**Congratulations!** Day 01 is complete. Tomorrow: **Types & Variables**. 🚀

