# Day 01 — Setup & Tooling

Welcome to Day 1! Today is all about **getting your development environment ready** so you can start coding tomorrow.

By the end of this day, you will have:
- ✅ .NET 10 SDK installed and verified
- ✅ A code editor or IDE ready to use
- ✅ Created and run your first C# console application
- ✅ Understood the basic project structure

---

## 🎯 Learning Objectives

1. **Install the .NET 10 SDK** and verify the installation
2. **Choose and install a code editor** (VS Code, Visual Studio, or JetBrains Rider)
3. **Create a new console project** using `dotnet new`
4. **Run a simple program** and see it work
5. **Understand the project structure** (`.csproj`, `Program.cs`, `bin/`, `obj/`)
6. **Learn essential `dotnet` CLI commands**

---

## 📋 Prerequisites

Before you start:
- A computer with Windows, macOS, or Linux
- Administrator access (to install software)
- ~90 minutes of uninterrupted time
- A terminal/command prompt (PowerShell, bash, cmd, etc.)

---

## Step 1: Install .NET 10 SDK

The .NET SDK includes everything you need to build and run C# applications.

### Option A: Official Installer (Recommended for Beginners)

1. Go to <a href="https://dotnet.microsoft.com/download" target="_blank">https://dotnet.microsoft.com/download</a>
2. Select **.NET 10** (the latest LTS or current release)
3. Download the **SDK** (not just the Runtime)
4. Run the installer and follow the prompts
5. Restart your terminal/command prompt

### Option B: Using a Package Manager (Linux/macOS)

**macOS (Homebrew):**
```bash
brew install dotnet@10
```

**Ubuntu/Debian:**
```bash
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 10
```

---

## Step 2: Verify Installation

Open a terminal and run:

```bash
dotnet --version
```

You should see something like:
```
10.0.0
```

If you see a version number, you're good! If not, restart your terminal or reinstall.

---

## Step 3: Choose Your Code Editor

You have three main options:

### **Option A: Visual Studio Code (Lightweight, Free)**
- Download: <a href="https://code.visualstudio.com" target="_blank">https://code.visualstudio.com</a>
- Install the **C# DevKit** extension from the VS Code Marketplace
- Fast, customizable, great for learning

### **Option B: Visual Studio Community (Full IDE, Free)**
- Download: <a href="https://visualstudio.microsoft.com/vs/community/" target="_blank">https://visualstudio.microsoft.com/vs/community/</a>
- During installation, select **.NET Desktop Development** workload
- More features, larger download, great if you already use it

### **Option C: JetBrains Rider (Professional, Paid)**
- Download: <a href="https://www.jetbrains.com/rider/" target="_blank">https://www.jetbrains.com/rider/</a>
- Free 30-day trial
- Excellent IDE, but not required for this course

**For this course, we recommend VS Code or Visual Studio Community.**

---

## Step 4: Create Your First Project

Open a terminal and create a new folder for your Day 01 work:

```bash
mkdir Day01-FirstApp
cd Day01-FirstApp
```

Now create a new console project:

```bash
dotnet new console
```

This creates:
- `Program.cs` — your C# code
- `Day01-FirstApp.csproj` — project configuration
- `bin/` — compiled output (created later)
- `obj/` — temporary build files (created later)

---

## Step 5: Run Your First Program

In the same folder, run:

```bash
dotnet run
```

You should see:
```
Hello, World!
```

**Congratulations!** You just ran your first C# program! 🎉

---

## Step 6: Explore the Project Structure

Open the `Program.cs` file in your editor. It looks like:

```csharp
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
```

This is your **entry point** — the code that runs when you execute `dotnet run`.

### Key Files & Folders:

| File/Folder | Purpose |
|---|---|
| `Program.cs` | Your C# source code |
| `Day01-FirstApp.csproj` | Project metadata (name, target framework, dependencies) |
| `bin/` | Compiled executable and dependencies |
| `obj/` | Temporary build artifacts |
| `.gitignore` | Files to ignore in Git |

---

## Step 7: Essential `dotnet` CLI Commands

Save these for future reference:

```bash
# Create a new console project
dotnet new console

# Run the project
dotnet run

# Build the project (compiles without running)
dotnet build

# Clean build artifacts
dotnet clean

# Restore dependencies
dotnet restore

# List installed SDKs and runtimes
dotnet --info
```

---

## 🎯 Mini Challenge

**Goal:** Modify `Program.cs` so it prints your name and the current date.

**Hint:** Use `Console.WriteLine()` to print text, and `DateTime.Now` to get the current date.

**Example output:**
```
Hello, I'm Alex!
Today is 12/4/2025 2:45:13 PM
```

**Steps:**
1. Open `Program.cs` in your editor
2. Replace the `Hello, World!` line with your own code
3. Run `dotnet run` to test
4. Experiment! Try adding more lines, different text, etc.

---

## ✅ Checklist: You've Completed Day 01 If...

- [ ] .NET 10 SDK is installed and `dotnet --version` works
- [ ] You have a code editor installed (VS Code, Visual Studio, or Rider)
- [ ] You created a new project with `dotnet new console`
- [ ] You ran `dotnet run` and saw output
- [ ] You understand the basic project structure
- [ ] You completed the mini challenge (modified Program.cs)

---

## 🔗 Next Steps

Tomorrow (Day 02) you'll learn about **Types & Variables** — the building blocks of any C# program. You'll understand:
- What data types are (int, string, bool, etc.)
- How to declare and use variables
- Type casting and conversion

**Come back tomorrow ready to dive deeper!**

---

## 📚 Optional: Deeper Exploration

If you finish early and want to explore more:

1. **Run in Release mode:**
   ```bash
   dotnet run --configuration Release
   ```
   Compare the output and binary size with Debug mode.

2. **Explore the `.csproj` file:**
   Open `Day01-FirstApp.csproj` in your editor. It's XML that defines your project configuration.

3. **Create another project:**
   ```bash
   cd ..
   mkdir MySecondApp
   cd MySecondApp
   dotnet new console
   dotnet run
   ```

4. **Read the official docs:**
- <a href="https://learn.microsoft.com/en-us/dotnet/core/get-started" target="_blank">Getting Started with .NET</a>
- <a href="https://learn.microsoft.com/en-us/dotnet/csharp/" target="_blank">C# Basics</a>

---

## 💬 Troubleshooting

**Problem:** `dotnet` command not found
- **Solution:** Restart your terminal or reinstall the SDK. Check `dotnet --version` again.

**Problem:** `dotnet new console` fails with permission error
- **Solution:** Make sure you have write permissions in your directory, or run with `sudo` (Linux/macOS).

**Problem:** Editor doesn't show syntax highlighting for C#
- **Solution:** Install the C# extension/plugin for your editor (C# DevKit for VS Code, etc.).

**Problem:** `dotnet run` takes a long time
- **Solution:** This is normal on first run. Subsequent runs are faster because dependencies are cached.

---

## 🎬 Summary

You now have a complete, working C# development environment. You've created and run your first program. **That's huge!**

The hard part (setup) is done. From tomorrow onward, you'll focus purely on learning C# — one concept, one day, at a time.

**You've got this.** See you on Day 02! 🚀
