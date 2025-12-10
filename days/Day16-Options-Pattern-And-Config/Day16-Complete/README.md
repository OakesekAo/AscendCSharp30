# Day 16 — Options Pattern & Configuration (Complete)

## 🎯 Building on Day 15

This is the **complete, working implementation** of Day 16: **Options pattern and configuration management**.

**Key difference from Day 15:**
- Day 15: Hardcoded configuration in Program.cs
- Day 16: Configuration from appsettings.json using Options pattern

---

## 🏗️ Configuration Architecture

```
Day 16 Complete/
├── appsettings.json           Configuration file (Dev)
├── appsettings.Production.json Configuration file (Prod)
│
├── Options/
│   ├── EmailSettings.cs       Strongly-typed email config
│   ├── DatabaseSettings.cs    Strongly-typed database config
│   └── AppSettings.cs         Strongly-typed app config
│
├── Services/ (Inject IOptions<T>)
│   ├── ConsoleEmailService    Uses EmailSettings
│   └── SmtpEmailService       Uses EmailSettings
│
└── Program.cs (Register & configure options)
```

---

## 🚀 Run This Code

```bash
dotnet run
```

Open: **https://localhost:5001/swagger**

---

## 📝 Key Concepts

### 1. Options Pattern

```csharp
// Define strongly-typed settings
public class EmailSettings
{
    public string Provider { get; set; }
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
}

// Register in DI
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// Inject IOptions<T>
public class EmailService
{
    public EmailService(IOptions<EmailSettings> options)
    {
        var settings = options.Value; // Access configuration
    }
}
```

**Benefits:**
- ✅ Strongly-typed configuration
- ✅ Compile-time safety
- ✅ IntelliSense support
- ✅ Easy to validate
- ✅ Environment-specific settings

### 2. appsettings.json

```json
{
  "EmailSettings": {
    "Provider": "Console",
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "ApiKey": "your-api-key"
  }
}
```

**Hierarchy:**
- appsettings.json (base)
- appsettings.{Environment}.json (overrides)
- Environment variables (highest priority)

### 3. Environment-Specific Settings

```csharp
// Development
{
  "EmailSettings": {
    "Provider": "Console"  // Use console, no real email
  }
}

// Production
{
  "EmailSettings": {
    "Provider": "Smtp",
    "SmtpServer": "smtp.office365.com"  // Real server
  }
}
```

---

## ✅ Endpoints Available

```
GET    /health               Health check
GET    /customers            List all customers
GET    /customers/{id}       Get one customer
POST   /customers            Create customer

GET    /workorders           List all work orders
POST   /workorders           Create with email notification

GET    /config-info          Show configuration (new!)
```

---

## 📊 Configuration Features Demonstrated

| Feature | Purpose |
|---------|---------|
| **appsettings.json** | Base configuration |
| **IOptions<T>** | Strongly-typed settings |
| **GetSection()** | Read nested config |
| **Environment-based** | Dev vs Prod settings |
| **Dependency injection** | Services use IOptions<T> |
| **Validation** | Ensure valid config |

---

## 🎯 What to Notice

1. **No hardcoded settings** — All from appsettings.json
2. **Strong typing** — EmailSettings, DatabaseSettings classes
3. **IOptions<T> injection** — Services receive options
4. **Environment awareness** — Different configs per environment
5. **Centralized configuration** — One source of truth

---

## 🔄 Data Flow

**Starting the app:**
```
1. Read appsettings.json
2. Register Services.Configure<EmailSettings>()
3. Services injected with IOptions<EmailSettings>
4. Services call options.Value to get settings
5. Different settings per environment
```

---

## 📊 Seed Data

Configured in appsettings.json:
- Email provider: Console (dev) or SMTP (prod)
- SMTP server: smtp.gmail.com
- Database: InMemory (dev) or SQL Server (prod)

---

## 🚀 Next: Day 17

Day 17 will **build on this code** by:
- Keeping options pattern from Day 16
- Adding structured logging with Serilog
- Logging configuration from appsettings.json
- Professional production logging

**Configuration + Logging = Production-ready!**

---

**Ready to run?** `dotnet run` then visit `https://localhost:5001/config-info` 🚀

**Configuration is the backbone of production applications!**


---

## 🟦 ServiceHub Context  
Implement the storage and retrieval of ServiceHub's core entities.  
You'll build the persistence layer that the API and Blazor UI will rely on for the rest of the project.

