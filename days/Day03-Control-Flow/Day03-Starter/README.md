# Day 03 — Control Flow

Welcome to Day 3! Today you'll learn how to make your code make **decisions** and **repeat actions** — the building blocks of real programs.

By the end of this day, you will have:
- ✅ Used `if`, `else if`, `else` to make decisions
- ✅ Compared values with operators (`==`, `>`, `<`, etc.)
- ✅ Combined conditions with logical operators (`&&`, `||`, `!`)
- ✅ Used `switch` statements for cleaner multi-branch logic
- ✅ Repeated code with `while` and `for` loops
- ✅ Validated user input with loops

---

## 🎯 Learning Objectives

1. **Understand conditionals** — `if`, `else if`, `else` statements
2. **Use comparison operators** — `==`, `!=`, `>`, `<`, `>=`, `<=`
3. **Combine conditions** — `&&` (AND), `||` (OR), `!` (NOT)
4. **Use switch statements** — For cleaner multi-branch decisions
5. **Loop with `while`** — Repeat while a condition is true
6. **Loop with `for`** — Repeat a fixed number of times
7. **Control loops** — `break` to exit, `continue` to skip

---

## 📋 Prerequisites

Before you start:
- Day 01-02 complete (comfortable with variables, input, parsing)
- Your editor open and ready
- ~90 minutes of uninterrupted time

---

## Setup: Create Your Day 03 Project

Just like Days 01-02, create a new folder and console project:

### Step 0: Open a Terminal

Open a terminal in your editor or operating system (same as before).

---

### Step 1: Create Your Day 03 Project Folder

```bash
mkdir Day03-GradeCalculator
cd Day03-GradeCalculator
```

---

### Step 2: Create a New Console Project

```bash
dotnet new console
```

Great! Your project is ready.

---

## Step 1: If/Else Fundamentals

The simplest decision: do something **if** a condition is true:

```csharp
int age = 18;

if (age >= 18)
{
    Console.WriteLine("You are an adult!");
}
```

The condition `age >= 18` is evaluated as `true` or `false`.

**What if the condition is false?** Use `else`:

```csharp
if (age >= 18)
{
    Console.WriteLine("You are an adult!");
}
else
{
    Console.WriteLine("You are a minor.");
}
```

**What if you have multiple conditions?** Use `else if`:

```csharp
if (age >= 65)
{
    Console.WriteLine("You are a senior.");
}
else if (age >= 18)
{
    Console.WriteLine("You are an adult.");
}
else
{
    Console.WriteLine("You are a minor.");
}
```

The first **true** condition wins. Once one `if`/`else if` matches, the rest are skipped.

---

## Step 2: Comparison Operators

Here are the operators you use in conditions:

| Operator | Meaning | Example |
|----------|---------|---------|
| `==` | Equal to | `age == 18` |
| `!=` | Not equal to | `age != 18` |
| `>` | Greater than | `age > 18` |
| `<` | Less than | `age < 18` |
| `>=` | Greater than or equal | `age >= 18` |
| `<=` | Less than or equal | `age <= 18` |

**Example:**
```csharp
int score = 85;

if (score >= 90)
{
    Console.WriteLine("A");
}
else if (score >= 80)
{
    Console.WriteLine("B");
}
```

---

## Step 3: Logical Operators

Combine multiple conditions with `&&` (AND), `||` (OR), `!` (NOT):

### AND (`&&`) — Both must be true

```csharp
int age = 25;
bool hasLicense = true;

if (age >= 18 && hasLicense)
{
    Console.WriteLine("You can drive!");
}
```

Both `age >= 18` AND `hasLicense` must be true.

### OR (`||`) — At least one must be true

```csharp
int score = 75;

if (score >= 90 || score == 75)
{
    Console.WriteLine("Passed!");
}
```

Either `score >= 90` OR `score == 75` (or both) make this true.

### NOT (`!`) — Flip the result

```csharp
bool isRaining = false;

if (!isRaining)
{
    Console.WriteLine("Go outside!");
}
```

`!isRaining` means "if NOT raining" → if it's false, this is true.

---

## Step 4: Switch Statements

When you have **many branches**, `switch` is cleaner than `if`/`else if`:

```csharp
char grade = 'A';

switch (grade)
{
    case 'A':
        Console.WriteLine("Excellent!");
        break;
    case 'B':
        Console.WriteLine("Good!");
        break;
    case 'C':
        Console.WriteLine("Average.");
        break;
    default:
        Console.WriteLine("Unknown grade.");
        break;
}
```

Each `case` is like an `if`. The `break` exits the switch. The `default` is like `else`.

**Important:** Always add `break` after each `case` (unless you want fall-through behavior).

---

## Step 5: While Loops

**Repeat code while a condition is true:**

```csharp
int count = 1;

while (count <= 5)
{
    Console.WriteLine(count);
    count++;
}
```

Output:
```
1
2
3
4
5
```

The loop runs **while** `count <= 5`. When count becomes 6, the condition is false and the loop stops.

### Loop Until Valid Input

A common pattern: **keep asking until the user enters valid data**:

```csharp
int score = -1;

while (score < 0 || score > 100)
{
    Console.Write("Enter a score (0-100): ");
    score = int.Parse(Console.ReadLine());
    
    if (score < 0 || score > 100)
    {
        Console.WriteLine("Invalid! Try again.");
    }
}

Console.WriteLine($"Score accepted: {score}");
```

The loop keeps running until `score` is valid (0-100).

---

## Step 6: For Loops

**Repeat a fixed number of times:**

```csharp
for (int i = 1; i <= 5; i++)
{
    Console.WriteLine(i);
}
```

The `for` loop has three parts:
1. **Initialization:** `int i = 1` — Start at 1
2. **Condition:** `i <= 5` — Keep going while true
3. **Increment:** `i++` — Add 1 each time

This is equivalent to the `while` loop above, but more compact.

**Example: Print a table**
```csharp
for (int i = 1; i <= 10; i++)
{
    Console.WriteLine($"{i} × 2 = {i * 2}");
}
```

---

## Step 7: Loop Control — Break and Continue

### `break` — Exit the loop early

```csharp
for (int i = 1; i <= 10; i++)
{
    if (i == 5)
    {
        break;  // Stop at 5
    }
    Console.WriteLine(i);
}
```

Output: `1 2 3 4`

### `continue` — Skip to the next iteration

```csharp
for (int i = 1; i <= 5; i++)
{
    if (i == 3)
    {
        continue;  // Skip 3
    }
    Console.WriteLine(i);
}
```

Output: `1 2 4 5`

---

## Step 8: Mini Challenge — Grade Calculator

**Build a program that:**
1. Asks user for 5 test scores (0-100)
2. **Validates each input** — Use a `while` loop to re-ask if invalid
3. Stores the 5 scores
4. Calculates the average
5. Assigns a letter grade using `if`/`else if` or `switch`:
   - 90-100 → A (Excellent)
   - 80-89 → B (Good)
   - 70-79 → C (Average)
   - 60-69 → D (Below Average)
   - <60 → F (Fail)
6. Displays all scores, average, and letter grade

**Example output:**
```
=== Grade Calculator ===

Enter test score 1 (0-100): 95
Enter test score 2 (0-100): 87
Enter test score 3 (0-100): 92
Enter test score 4 (0-100): 88
Enter test score 5 (0-100): 90

=== Results ===
Scores: 95, 87, 92, 88, 90
Average: 90.4
Grade: A
Excellent work!
```

**Steps to build it:**
1. Create an array to store 5 scores: `int[] scores = new int[5];`
2. Use a `for` loop to ask for each score (1-5)
3. For each score, use a `while` loop to validate (0-100)
4. After collecting all scores, calculate the average
5. Use `if`/`else if` (or `switch`) to assign a grade
6. Display everything

**Hints:**
```csharp
// Array to store 5 scores
int[] scores = new int[5];

// Loop through and collect scores
for (int i = 0; i < 5; i++)
{
    int score = -1;
    
    // Keep asking until valid
    while (score < 0 || score > 100)
    {
        Console.Write($"Enter test score {i + 1} (0-100): ");
        score = int.Parse(Console.ReadLine());
        
        if (score < 0 || score > 100)
        {
            Console.WriteLine("Invalid! Enter 0-100.");
        }
    }
    
    scores[i] = score;
}

// Calculate average
double average = 0;
for (int i = 0; i < scores.Length; i++)
{
    average += scores[i];
}
average /= scores.Length;

// Assign grade
char grade;
if (average >= 90)
    grade = 'A';
else if (average >= 80)
    grade = 'B';
// ... etc
```

---

## Step 9: Test Your Program

Once you've written your code, test it:

```bash
dotnet run
```

Your program should:
- Ask for 5 scores
- Reject invalid input (negative, >100, non-numeric)
- Calculate and display average
- Show the correct letter grade

Try different inputs:
- All high scores (90+)
- All low scores (<60)
- Mix of valid and invalid inputs

---

## ✅ Checklist: You've Completed Day 03 If...

- [ ] You created the `Day03-GradeCalculator` folder and ran `dotnet new console`
- [ ] You understand `if`, `else if`, `else` syntax
- [ ] You can compare values with `==`, `!=`, `>`, `<`, `>=`, `<=`
- [ ] You can combine conditions with `&&`, `||`, `!`
- [ ] You understand `switch` statements
- [ ] You can write a `while` loop
- [ ] You can write a `for` loop
- [ ] You understand `break` and `continue`
- [ ] You completed the mini challenge (grade calculator)
- [ ] Your program validates input and displays results correctly

---

## 🔗 Next Steps

Tomorrow (Day 04) you'll learn about **Collections** — how to store and work with **groups of data** (arrays, lists, dictionaries). Instead of one `score`, you'll work with many!

---

## 📚 Optional: Deeper Exploration

If you finish early:

1. **Try nested loops:**
   ```csharp
   for (int i = 1; i <= 3; i++)
   {
       for (int j = 1; j <= 3; j++)
       {
           Console.WriteLine($"{i}, {j}");
       }
   }
   ```

2. **Try do-while loops:**
   ```csharp
   int choice;
   do
   {
       Console.WriteLine("Menu: 1=Run, 2=Exit");
       choice = int.Parse(Console.ReadLine());
   } while (choice != 2);
   ```

3. **Try ternary operator (one-line if):**
   ```csharp
   string status = age >= 18 ? "Adult" : "Minor";
   ```

4. **Read the official docs:**
   - <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/if-elseif-else" target="_blank">if, else if, else Statements</a>
   - <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/iteration-statements" target="_blank">Loops (for, while, do-while)</a>

---

## 💬 Troubleshooting

**Problem:** "Infinite loop" — program doesn't stop
- **Cause:** Loop condition never becomes false
- **Solution:** Make sure you're updating the loop variable. Example: `count++` in a `while` loop.

**Problem:** "The condition is always true/false"
- **Cause:** Logic error in your condition
- **Solution:** Test small parts. Print the value: `Console.WriteLine($"Score: {score}");`

**Problem:** Arrays give errors
- **Cause:** Index out of bounds (accessing an index that doesn't exist)
- **Solution:** Arrays start at 0. An array of 5 has indices 0-4.

---

## 🎬 Summary

Today you learned the **logic and flow** of programs. With `if`/`else`, `switch`, and loops, you can:
- Make decisions based on data
- Repeat actions without writing the same code twice
- Validate user input and keep asking until it's valid
- Process groups of data

These are fundamental skills you'll use in **every program** and every **web application**.

**You've got this.** See you on Day 04! 🚀

