# Day 02 — Types & Variables

Welcome to Day 2! Today you'll learn about **data types** and **variables** — the fundamental building blocks for storing and working with data in C#.

By the end of this day, you will have:
- ✅ Understood C# data types (int, string, double, bool)
- ✅ Declared and used variables properly
- ✅ Retrieved user input from the console
- ✅ Converted between different types
- ✅ Performed calculations with different data types

---

## 🎯 Learning Objectives

1. **Understand data types** — What they are and when to use each one
2. **Declare variables** — Using `type name = value;` syntax
3. **Get user input** — Using `Console.ReadLine()`
4. **Convert types** — String to int, string to double, etc.
5. **Perform calculations** — With different numeric types
6. **Display results** — Using string interpolation with formatting

---

## 📋 Prerequisites

Before you start:
- Day 01 completed (comfortable with `Console.WriteLine()` and basic syntax)
- Your editor (VS Code or Visual Studio) open and ready
- ~60 minutes of uninterrupted time

---

## Setup: Create Your Day 02 Project

Remember yesterday? You created a new folder, moved into it, and ran `dotnet new console`. We're doing the same thing today!

### Step 0: Open a Terminal

Just like Day 01, open a terminal in your editor or operating system:

**If using VS Code:**
1. Open VS Code
2. At the top menu, click **Terminal** → **New Terminal**
   - Or press **Ctrl + ` (Ctrl + Backtick)**
3. A terminal panel will appear at the bottom

**If using Visual Studio:**
1. Open Visual Studio
2. At the top menu, click **Tools** → **Command Line** → **Developer PowerShell**
   - Or press **Ctrl + Alt + ` (Ctrl + Alt + Backtick)**
3. A PowerShell window will open

**If using Windows (without an editor):**
1. Press **Windows Key + R**
2. Type `powershell`
3. Press Enter

**If using macOS/Linux:**
1. Open Terminal

---

### Step 1: Create Your Day 02 Project Folder

In your terminal, run:

```bash
mkdir Day02-Profile
cd Day02-Profile
```

This creates a new folder called `Day02-Profile` and moves you into it.

---

### Step 2: Create a New Console Project

```bash
dotnet new console
```

You should see output like:
```
The template "Console App" was created successfully.

Processing post-creation actions...
Restoring C:\Users\YourName\Day02-Profile\Day02-Profile.csproj:
  Determining projects to restore...
  Restored C:\Users\YourName\Day02-Profile\Day02-Profile.csproj (in 2.34 sec)
```

Great! Your project is created. Now you're ready to learn about types and variables.

---

## Step 1: Understanding Data Types

C# has different **data types** for different kinds of data:

| Type | Purpose | Examples |
|------|---------|----------|
| `int` | Whole numbers | 25, -10, 1000 |
| `double` | Decimal numbers | 5.9, 3.14, -2.5 |
| `string` | Text | "Alex", "Hello World" |
| `bool` | True/False | true, false |
| `decimal` | Money/precise decimals | 19.99, 100.50 |

Each type is designed for a specific purpose. Using the right type matters!

---

## Step 2: Declaring Variables

A **variable** is a container that holds a value. You declare variables like this:

```csharp
type name = value;
```

**Examples:**
```csharp
int age = 25;
string name = "Alex";
double height = 5.9;
bool isStudent = true;
decimal salary = 50000.00m;
```

Key points:
- `type` — What kind of data (int, string, double, etc.)
- `name` — What you call the variable (must start with letter or `_`)
- `= value` — The initial value you store

---

## Step 3: Getting User Input

So far you've printed output. Now you'll **get input** from the user:

```csharp
Console.Write("What is your name? ");
string name = Console.ReadLine();
```

**Important notes:**
- `Console.Write()` — Prints text WITHOUT a newline (cursor stays on same line)
- `Console.ReadLine()` — Waits for user to type and press Enter
- The input is always returned as a **string**, even if it's numbers!

**Example:**
```
What is your name? 
> Alex
```

---

## Step 4: Converting Types (Parsing)

When the user enters "25", it's stored as the **string** `"25"`, not the **int** `25`. You need to convert it:

```csharp
Console.Write("How old are you? ");
string ageInput = Console.ReadLine();
int age = int.Parse(ageInput);  // Convert string to int
```

Or shorter:
```csharp
int age = int.Parse(Console.ReadLine());
```

**Common conversions:**
```csharp
int number = int.Parse("25");           // String to int
double decimal = double.Parse("5.9");   // String to double
bool boolean = bool.Parse("true");      // String to bool
```

---

## Step 5: Working With Input Data

Once you have variables, you can work with them:

```csharp
// Get input
Console.Write("Current salary: ");
double salary = double.Parse(Console.ReadLine());

// Calculate
double salaryWithRaise = salary * 1.10;  // 10% raise

// Display
Console.WriteLine($"New salary: ${salaryWithRaise:F2}");
```

The `:N2` format means "show 2 decimal places with thousand separators (commas)" — perfect for money and large numbers.

---

## Step 6: Mini Challenge

**Build a simple calculator/profile that:**
1. Asks for the user's name
2. Asks for their age
3. Asks for their current salary
4. Calculates:
   - What their age will be in 10 years
   - What their salary would be with a 10% raise
5. Displays all the results nicely formatted

**Example output:**
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
```

**Steps to build it:**
1. Open the `Program.cs` file in your `Day02-Profile` folder
2. Clear out the default code
3. Use `Console.Write()` to ask for name, age, salary
4. Use `Console.ReadLine()` to get each input
5. Convert the strings to `int` and `double` with `int.Parse()` and `double.Parse()`
6. Calculate the new age and new salary
7. Display everything with string interpolation

**Hints:**
```csharp
// Convert and get in one line
int age = int.Parse(Console.ReadLine());

// Format money with 2 decimals and commas
Console.WriteLine($"Salary: ${salary:N2}");

// Do math
int futureAge = currentAge + 10;
double raisedSalary = currentSalary * 1.10;
```

---

## Step 7: Test Your Program

Once you've written your code in `Program.cs`, test it:

```bash
dotnet run
```

Your program should prompt you for name, age, and salary, then display the results. Try it a few times with different inputs!

If you get errors, check:
- Did you use `Console.Write()` for prompts and `Console.ReadLine()` for input?
- Did you convert strings to int/double with `int.Parse()` and `double.Parse()`?
- Did you use the right format specifier `:F2` for money?


---

## ✅ Checklist: You've Completed Day 02 If...

- [ ] You created the `Day02-Profile` folder and ran `dotnet new console`
- [ ] You understand what int, string, double, and bool are
- [ ] You can declare variables with proper syntax
- [ ] You can get user input with `Console.ReadLine()`
- [ ] You can convert strings to int and double with `Parse()`
- [ ] You can do calculations with different types
- [ ] You can format numbers (especially money with `:N2` for commas or `:C` for currency)
- [ ] You completed the mini challenge (profile calculator)
- [ ] Your program runs with `dotnet run` and shows all outputs correctly

---

## 🔗 Next Steps

Tomorrow (Day 03) you'll learn about **Control Flow** — how to make decisions in your code with `if`, `else`, and `switch` statements. You'll be able to say things like:
- "If the user is over 18, show one message, otherwise show another"
- "Keep asking until the user enters valid data"

---

## 📚 Optional: Deeper Exploration

If you finish early:

1. **Try different formats:**
   ```csharp
   double price = 19.99;
   Console.WriteLine($"{price:C}");     // Currency: $19.99
   Console.WriteLine($"{price:N2}");    // Number with commas
   ```

2. **Try `decimal` for money:**
   ```csharp
   decimal price = 19.99m;  // Note the 'm' at the end
   Console.WriteLine($"Price: {price:C}");
   ```

3. **Learn about `var` (type inference):**
   ```csharp
   var name = "Alex";   // C# figures out it's a string
   var age = 25;        // C# figures out it's an int
   ```

4. **Read the official docs:**
   - <a href="https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/" target="_blank">C# Data Types</a>
   - <a href="https://learn.microsoft.com/en-us/dotnet/standard/base-types/parsing-strings" target="_blank">Parsing Strings</a>

---

## 💬 Troubleshooting

**Problem:** "Input string was not in a correct format"
- **Cause:** You tried to parse invalid data (e.g., parsing "abc" as an int)
- **Solution:** For now, make sure users enter the right type. Day 03+ we'll handle this better with error checking.

**Problem:** Numbers don't show decimal places
- **Cause:** You used `int` instead of `double` for decimal values
- **Solution:** Use `double` for any value that might have decimals

**Problem:** Math is giving strange results
- **Cause:** Integer division (5 / 2 = 2, not 2.5)
- **Solution:** Use `double` for division: `5.0 / 2` or `(double)5 / 2`

---

## 🎬 Summary

Today you learned the fundamental building blocks: **types** and **variables**. You also learned how to get **input from users** and **convert data** between types. These are the core skills you'll use in every program you write.

With input, conversion, and calculations, you're moving from "Hello, World" to real interaction!

**You've got this.** See you on Day 03! 🚀

