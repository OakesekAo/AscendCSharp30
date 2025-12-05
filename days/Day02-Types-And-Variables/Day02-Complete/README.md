# Day 02 — Types & Variables: Complete Example

This is a **working, runnable example** of what you'll have at the end of Day 02 if you follow the Starter guide.

## How to Run

From the command line:

```bash
cd days/Day02-Types-And-Variables/Day02-Complete
dotnet run
```

You'll be prompted to enter your information:

```
=== Welcome to Your Profile ===

What is your name?
> Alex

How old are you?
> 25

What is your current salary?
> 50000

=== Your Information ===
Name: Alex
Current Age: 25
Age in 10 years: 35
Current Salary: $50,000.00
Salary with 10% raise: $55,000.00

=== Data Types Used ===
name is a String
age is a Int32
salary is a Double
```

## What This Program Demonstrates

### 1. **User Input with Console.ReadLine()**
```csharp
Console.Write("What is your name? ");
string name = Console.ReadLine();
```
Gets text from the user and stores it in a variable.

### 2. **Data Types**
```csharp
string name = "Alex";      // Text
int age = 25;              // Whole numbers
double salary = 50000.00;  // Decimal numbers
```
Each type serves a different purpose.

### 3. **Type Conversion (Parsing)**
```csharp
int age = int.Parse(Console.ReadLine());
double salary = double.Parse(Console.ReadLine());
```
Converts string input into int and double for calculations.

### 4. **Calculations with Variables**
```csharp
int ageIn10Years = age + 10;
double salaryWithRaise = salary * 1.10;
```
Perform math with variables of different types.

### 5. **String Interpolation with Formatting**
```csharp
Console.WriteLine($"Current Salary: ${salary:F2}");
```
The `:F2` format specifier shows 2 decimal places (perfect for money).

### 6. **Bonus: Checking Types**
```csharp
Console.WriteLine($"name is a {name.GetType().Name}");
```
Shows what type each variable is (String, Int32, Double, etc.).

## Key Concepts Covered

✅ **Data Types** — int, string, double, and why they matter
✅ **Variable Declaration** — How to create and name variables
✅ **User Input** — Getting data from users
✅ **Type Conversion** — Turning strings into numbers
✅ **Calculations** — Math with different types
✅ **Formatting** — Displaying data nicely (especially money)

## Next Steps

After you complete the Starter guide and create your own version, compare it to this Complete example. You should:
- Understand each line of code
- See similar output when you run it
- Be ready to move on to Day 03 (Control Flow)

## Project Files

- **`Program.cs`** — The C# code with clear, step-by-step comments
- **`Day02.Complete.csproj`** — Project configuration (targets .NET 10)
- **`bin/`** — Compiled output (created after `dotnet run`)
- **`obj/`** — Temporary build files (created during compilation)

---

**Congratulations!** Day 02 is complete. Tomorrow: **Control Flow (if/else, loops)**. 🚀

