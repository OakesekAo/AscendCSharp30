# Day 17 — Logging with Serilog

## 🚀 Production-Grade Logging

**Maya's Message:**
> "Configuration is great, but how do you debug production issues? Logging is your window into what's happening. Serilog gives us structured logging that's powerful and flexible."

Today, you'll learn **structured logging with Serilog**: configuration, log levels, filtering, and production-ready logging patterns.

---

## 🎯 Learning Objectives

1. **Understand structured logging:** Logs as structured data, not strings
2. **Configure Serilog:** Rich configuration options
3. **Log levels and filtering:** Debug, Information, Warning, Error, Fatal
4. **Inject ILogger:** Use dependency injection for logging
5. **Sinks and enrichment:** Output logs to files, console, etc.
6. **Production patterns:** Security and performance

---

## 📋 Prerequisites

- Days 01-16 complete
- Comfortable with DI and configuration
- Understanding of appsettings.json
- ~90 minutes

---

## What Changed Since Day 16

Day 16 had basic .NET logging:
```csharp
// Basic logging
var logger = serviceProvider.GetService<ILogger<MyService>>();
logger.LogInformation("User logged in");
```

Day 17 adds Serilog:
```csharp
// Install Serilog
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

// Structured logging with Serilog
logger.LogInformation("User {UserId} logged in at {Timestamp}", userId, DateTime.UtcNow);

// Serilog configuration in appsettings.json
{
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [
      { "Name": "Console" },
      { "Name": "File", "Args": { "path": "logs/app.txt" } }
    ]
  }
}
```

---

## Step 1: Install Serilog

```bash
dotnet add package Serilog
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File
```

---

## Step 2: Configure Serilog in Program.cs

```csharp
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

// Rest of DI setup...

var app = builder.Build();
app.Run();
```

---

## Step 3: Configure in appsettings.json

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "theme": "Ansi"
        }
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/app-.txt",
          "rollingInterval": "Day",
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      }
    ],
    "Enrich": ["FromLogContext", "WithMachineName", "WithThreadId"]
  }
}
```

---

## Step 4: Use ILogger in Services

```csharp
public class CustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        ICustomerRepository repository,
        ILogger<CustomerService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Customer> CreateAsync(string name, string email)
    {
        _logger.LogInformation("Creating customer: {Name} ({Email})", name, email);
        
        try
        {
            var customer = new Customer { Name = name, Email = email };
            await _repository.AddAsync(customer);
            
            _logger.LogInformation("Customer created: {CustomerId}", customer.Id);
            return customer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create customer: {Name}", name);
            throw;
        }
    }
}
```

---

## Step 5: Mini Challenge

**Add Serilog logging to your Day 16 API:**

1. Install Serilog packages
2. Configure Serilog in Program.cs
3. Create appsettings.json with Serilog configuration
4. Inject ILogger into services
5. Add structured logging to key methods
6. Test that logs appear in console and file
7. Create appsettings.Production.json with different log level
8. Verify logs are structured and searchable

---

## ✅ Checklist

- [ ] Installed Serilog packages
- [ ] Configured Serilog in Program.cs
- [ ] Created Serilog config in appsettings.json
- [ ] Injected ILogger into services
- [ ] Added structured logging throughout
- [ ] Tested console and file output
- [ ] Created appsettings.Production.json
- [ ] Different log levels per environment
- [ ] Code compiles without errors
- [ ] Compared to Day 17 Complete

---

## 🔗 Next Steps

Day 18: **HttpClient & External APIs** — Call external services.

---

## 📚 Resources

- <a href="https://serilog.net" target="_blank">Serilog Official</a>
- <a href="https://github.com/serilog/serilog-aspnetcore" target="_blank">Serilog for ASP.NET Core</a>

---

**Structured logging is the key to production success.** See you on Day 18! 🚀
