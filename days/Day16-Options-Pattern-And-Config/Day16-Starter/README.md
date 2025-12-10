# Day 16 — Options Pattern & Configuration

## 🚀 Professional Configuration Management

**Maya's Message:**
> "Hard-coded settings are a nightmare. Real applications need flexible configuration that changes per environment. Today you'll master the Options pattern — the .NET way to manage settings professionally."

Today, you'll learn **configuration management**: reading from appsettings.json, environment-specific settings, Options pattern, and secure credentials.

---

## 🎯 Learning Objectives

1. **Understand the Options pattern:** Strongly-typed configuration
2. **Read from appsettings.json:** JSON configuration files
3. **Environment-based configuration:** Dev vs Prod settings
4. **Dependency injection of options:** Inject IOptions<T> into services
5. **Secure credentials:** API keys and secrets management
6. **Configuration hierarchies:** Nested configuration structures

---

## 📋 Prerequisites

- Days 01-15 complete
- Comfortable with DI and services
- Understanding of JSON
- ~90 minutes

---

## What Changed Since Day 15

Day 15 had hardcoded settings:
```csharp
// Hardcoded - not flexible!
if (builder.Environment.IsProduction())
    builder.Services.AddScoped<IEmailService, SmtpEmailService>();
else
    builder.Services.AddScoped<IEmailService, ConsoleEmailService>();
```

Day 16 uses configuration:
```csharp
// In appsettings.json
{
  "EmailSettings": {
    "Provider": "Console",
    "ApiKey": "your-api-key"
  },
  "DatabaseSettings": {
    "ConnectionString": "Server=localhost;..."
  }
}

// In Program.cs
var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// In service
public EmailService(IOptions<EmailSettings> options)
{
    var settings = options.Value;
    // Use settings
}
```

---

## Step 1: Create appsettings.json

```json
{
  "EmailSettings": {
    "Provider": "Console",
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "ApiKey": "your-api-key-here"
  },
  "DatabaseSettings": {
    "ConnectionString": "Server=localhost;Database=ServiceHub;..."
  },
  "AppSettings": {
    "ApiVersion": "1.0.0",
    "Environment": "Development"
  }
}
```

---

## Step 2: Create Options Classes

```csharp
public class EmailSettings
{
    public string Provider { get; set; } = "";
    public string SmtpServer { get; set; } = "";
    public int SmtpPort { get; set; }
    public string ApiKey { get; set; } = "";
}

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = "";
}
```

---

## Step 3: Register Options in DI

```csharp
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));
```

---

## Step 4: Inject IOptions<T> into Services

```csharp
public class EmailService
{
    private readonly EmailSettings _settings;
    
    public EmailService(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }
    
    public Task SendAsync(string to, string subject, string message)
    {
        // Use _settings.Provider, _settings.SmtpServer, etc.
        return Task.CompletedTask;
    }
}
```

---

## Step 5: Mini Challenge

**Add configuration to your Day 15 API:**

1. Create `appsettings.json` with email settings
2. Create `EmailSettings` and `DatabaseSettings` classes
3. Register them with `services.Configure<T>()`
4. Inject `IOptions<EmailSettings>` into `EmailService`
5. Create `appsettings.Production.json` with different settings
6. Read and use settings in services
7. Verify it works

---

## ✅ Checklist

- [ ] Created appsettings.json
- [ ] Created Options classes (EmailSettings, DatabaseSettings)
- [ ] Registered options in DI container
- [ ] Injected IOptions<T> into services
- [ ] Created appsettings.Production.json
- [ ] Settings properly loaded from configuration
- [ ] Different settings per environment
- [ ] Code compiles without errors
- [ ] Compared to Day 16 Complete example

---

## 🔗 Next Steps

Day 17: **Logging with Serilog** — Structured logging for production.

---

## 📚 Resources

- <a href="https://docs.microsoft.com/dotnet/core/extensions/configuration" target="_blank">Configuration in .NET</a>
- <a href="https://docs.microsoft.com/dotnet/core/extensions/options" target="_blank">Options Pattern</a>

---

**Configuration is power.** See you on Day 17! 🚀
