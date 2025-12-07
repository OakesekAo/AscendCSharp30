# Day 05 — Methods & Parameters

Welcome to Day 5! Today you'll learn about **methods** — reusable blocks of code that take inputs and produce outputs.

By the end of this day, you will have:
- ✅ Understood methods and why they matter
- ✅ Created methods with parameters
- ✅ Returned values from methods
- ✅ Understood scope and visibility
- ✅ Refactored Day 04 code into clean methods

---

## 🎯 Learning Objectives

1. **Understand methods** — Why we use them
2. **Create methods with parameters** — Accept input
3. **Return values** — Produce output
4. **Understand scope** — Variable visibility
5. **Call methods** — Use them in your code
6. **Refactor code** — Keep it clean and DRY

---

## 📋 Prerequisites

Before you start:
- Days 01-04 complete (comfortable with collections and control flow)
- Your editor open and ready
- ~60 minutes of uninterrupted time

---

## Setup: Create Your Day 05 Project

```bash
mkdir Day05-Methods
cd Day05-Methods
dotnet new console
```

---

## Step 1: Why Methods Matter

**Without methods (bad):**
```csharp
// Calculating discount repeatedly
double price1 = 100;
double discount1 = price1 * 0.1;
double finalPrice1 = price1 - discount1;

double price2 = 50;
double discount2 = price2 * 0.1;
double finalPrice2 = price2 - discount2;

double price3 = 200;
double discount3 = price3 * 0.1;
double finalPrice3 = price3 - discount3;
```

**With methods (good):**
```csharp
double ApplyDiscount(double price, double discountPercent)
{
    return price * (1 - discountPercent / 100);
}

double final1 = ApplyDiscount(100, 10);
double final2 = ApplyDiscount(50, 10);
double final3 = ApplyDiscount(200, 10);
```

**Benefits:**
- **DRY:** Don't Repeat Yourself
- **Readable:** Code is clearer
- **Testable:** Easy to verify
- **Maintainable:** Change once, works everywhere

---

## Step 2: Basic Method Syntax

```csharp
// Method signature
[return-type] [method-name]([parameters])
{
    // Method body
    // ...
    return [value];
}
```

**Example:**
```csharp
void SayHello(string name)
{
    Console.WriteLine($"Hello, {name}!");
}

int Add(int a, int b)
{
    return a + b;
}

string GetGreeting()
{
    return "Welcome to ServiceHub!";
}
```

---

## Step 3: Methods with Parameters

Parameters are **inputs** to a method:

```csharp
// Single parameter
void PrintCustomer(string name)
{
    Console.WriteLine($"Customer: {name}");
}
PrintCustomer("Alice"); // Output: Customer: Alice

// Multiple parameters
double CalculateSalaryWithRaise(double salary, double raisePercent)
{
    return salary * (1 + raisePercent / 100);
}
Console.WriteLine(CalculateSalaryWithRaise(50000, 10)); // 55000

// No parameters
string GetCurrentDate()
{
    return DateTime.Now.ToString();
}
Console.WriteLine(GetCurrentDate());
```

---

## Step 4: Return Values

Methods can **return** data:

```csharp
// Return a value
int GetLength(string text)
{
    return text.Length;
}

int len = GetLength("hello"); // 5

// Return nothing (void)
void PrintAlert(string message)
{
    Console.WriteLine($"ALERT: {message}");
}

PrintAlert("Job is overdue!"); // Just prints, returns nothing
```

---

## Step 5: Scope

Variables created inside a method only exist in that method:

```csharp
void ProcessOrder()
{
    string customerName = "Alice"; // Local variable
    int orderId = 123;
    
    Console.WriteLine(customerName); // ✅ Works
}

// Console.WriteLine(customerName); // ❌ Error — doesn't exist here!
```

**Scope rules:**
- Variables created in a **method** exist only in that method
- Variables created in **loops/if blocks** exist only there
- Global variables (at class level) exist everywhere

---

## Step 6: ServiceHub Methods

Example methods for ServiceHub:

```csharp
// Display a customer
void DisplayCustomer(string name, string email)
{
    Console.WriteLine($"Customer: {name}");
    Console.WriteLine($"Email: {email}");
}

// Check if work order is overdue
bool IsOverdue(DateTime scheduledDate)
{
    return DateTime.Now > scheduledDate;
}

// Calculate work order cost
double CalculateWorkOrderCost(int hours, double hourlyRate)
{
    return hours * hourlyRate;
}

// Find customer by name in list
string FindCustomer(List<string> customers, string name)
{
    foreach (string customer in customers)
    {
        if (customer.ToLower() == name.ToLower())
        {
            return customer;
        }
    }
    return "Not found";
}
```

---

## Step 7: Mini Challenge — ServiceHub Refactor

**Task:** Refactor Day 04's customer manager into clean methods.

**Create these methods:**

1. `void DisplayAllCustomers(List<string> names, Dictionary<string, string> emails)`
   - Prints all customers with their emails

2. `List<string> SearchCustomers(List<string> names, string searchTerm)`
   - Returns customers matching the search term

3. `void AddCustomer(List<string> names, Dictionary<string, string> emails, string name, string email)`
   - Adds a new customer

4. `double CalculateAverageNameLength(List<string> names)`
   - Returns average name length

5. `int CountCustomers(List<string> names)`
   - Returns total number of customers

**Example usage:**
```csharp
List<string> names = new List<string> { "Alice", "Bob" };
Dictionary<string, string> emails = new Dictionary<string, string>
{
    { "Alice", "alice@example.com" },
    { "Bob", "bob@example.com" }
};

DisplayAllCustomers(names, emails);
var search = SearchCustomers(names, "ice");
var avg = CalculateAverageNameLength(names);
```

---

## ✅ Checklist

- [ ] You understand what methods are and why they matter
- [ ] You can create methods with parameters
- [ ] You can return values from methods
- [ ] You understand variable scope
- [ ] You refactored Day 04 code into methods
- [ ] Your methods are clean, single-purpose, and testable

---

## 🔗 Next Steps

Day 06: **LINQ Fundamentals** — Advanced querying with collections.

---

## 📚 Resources

- <a href="https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/methods" target="_blank">Methods in C#</a>
- <a href="https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/basic-concepts/type-parameters" target="_blank">Scope and Parameters</a>

---

**You've got this.** See you on Day 06! 🚀
