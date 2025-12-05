# Day 03 — Control Flow: Complete Example

This is a **working, runnable example** of what you'll have at the end of Day 03 if you follow the Starter guide.

## How to Run

From the command line:

```bash
cd days/Day03-Control-Flow/Day03-Complete
dotnet run
```

You'll be prompted to enter 5 test scores (0-100):

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

## What This Program Demonstrates

### 1. **Arrays**
```csharp
int[] scores = new int[5];
```
Creates an array to hold 5 scores.

### 2. **For Loops with Validation**
```csharp
for (int i = 0; i < 5; i++)
{
    // Ask for each score
}
```
Loop through and collect 5 scores.

### 3. **While Loop for Input Validation**
```csharp
while (score < 0 || score > 100)
{
    // Keep asking until valid
}
```
Re-ask the user until they enter valid data (0-100).

### 4. **Logical Operators**
```csharp
if (score < 0 || score > 100)
```
Use `||` (OR) to check multiple invalid conditions.

### 5. **Conditional Assignment (if/else if/else)**
```csharp
if (average >= 90)
    grade = 'A';
else if (average >= 80)
    grade = 'B';
// ... etc
```
Assign grade based on average score.

### 6. **Switch Expression**
```csharp
string message = grade switch
{
    'A' => "Excellent work!",
    'B' => "Good job!",
    // ... etc
};
```
Modern C# way to handle multiple cases (cleaner than switch statement).

### 7. **Loop Through Array**
```csharp
for (int i = 0; i < scores.Length; i++)
{
    // Print each score
}
```
Iterate through the array to display results.

---

## Key Concepts Covered

✅ **If/Else Logic** — Make decisions based on conditions
✅ **Comparison Operators** — `>=`, `<=`, `==`, etc.
✅ **Logical Operators** — `&&`, `||`, `!`
✅ **For Loops** — Repeat a fixed number of times
✅ **While Loops** — Repeat until a condition is false
✅ **Input Validation** — Keep asking until data is valid
✅ **Arrays** — Store multiple values
✅ **Switch Expressions** — Modern alternative to switch statements

---

## Next Steps

After you complete the Starter guide and create your own version, compare it to this Complete example. You should:
- Understand each line of code
- See similar output when you run it
- Be ready to move on to Day 04 (Collections)

---

## Project Files

- **`Program.cs`** — The C# code with clear, step-by-step comments
- **`Day03.Complete.csproj`** — Project configuration (targets .NET 10)
- **`bin/`** — Compiled output (created after `dotnet run`)
- **`obj/`** — Temporary build files (created during compilation)

---

**Congratulations!** Day 03 is complete. Tomorrow: **Collections (arrays, lists, dictionaries)**. 🚀

