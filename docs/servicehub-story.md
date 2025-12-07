# ServiceHub: The Complete Story Arc

This page tells the complete story of **ServiceHub** — why it exists, what it does, and how you'll build it across 30 days.

---

## 📖 The Setup

**Meet Maya Thompson.**

She runs a cleaning service in her city. Five years in, she has dozens of regular customers and a booked schedule. But her operation is a mess:

- Customers' contact info is scattered across text messages and old emails
- Work orders live in a spiral notebook
- She tracks job statuses with sticky notes on her wall
- Invoices are handwritten or created in Word
- She has no visibility into what's happening when

Every day, she spends 2-3 hours on admin work that has nothing to do with cleaning. She's losing money to inefficiency and missed jobs. She needs a system.

She needs **ServiceHub**.

---

## 🎯 The Brief

Maya can't afford a $200/month SaaS. She can't wait 6 months for a custom build. She needs something **fast, affordable, and simple**.

She hires you — a C# developer who's ready to level up — with a challenge:

> "Build me a web app where I can:
> 1. Store my customers
> 2. Schedule work orders
> 3. See my jobs in a list or calendar
> 4. Mark jobs as done
> 5. Generate simple invoices
> 
> I need it in 30 days. It needs to work. It needs to be reliable."

---

## 🏗️ The Architecture (What You'll Build)

**ServiceHub** is a full-stack web application:

```
┌─────────────────────────────────────────────────────┐
│                    Blazor WebAssembly UI            │
│            (React-like SPA in the browser)          │
└────────────────────┬────────────────────────────────┘
                     │ HTTP/JSON
                     │
┌────────────────────▼────────────────────────────────┐
│            ASP.NET Core Minimal API                 │
│   /api/customers, /api/workorders, /api/auth       │
└────────────────────┬────────────────────────────────┘
                     │ EF Core
                     │
┌────────────────────▼────────────────────────────────┐
│          PostgreSQL / SQLite Database               │
│    Customers | WorkOrders | Technicians | Config   │
└─────────────────────────────────────────────────────┘
```

Each layer uses modern C# patterns:
- **API:** Dependency Injection, validation, error handling
- **Database:** Entity Framework Core with migrations
- **UI:** Blazor components, routing, authentication
- **Auth:** JWT tokens, identity management

---

## 🗺️ The 4-Week Journey

### **Week 1: The Preparation Sprint**
**Goal:** Master C# fundamentals using ServiceHub examples.

What you're learning:
- Types, variables, control flow
- Collections and LINQ
- OOP basics (classes, interfaces)
- How to model a business domain in code

What's happening:
- You're learning the language
- You're not building yet
- But every example is *about* ServiceHub (customers, work orders, etc.)

By Day 7:
- You understand ServiceHub's domain model in code
- You've modeled customers, work orders, and technicians
- You can describe the problem in C# syntax

---

### **Week 2: The API Sprint**
**Goal:** Build ServiceHub API v0.1 — a working backend.

What you're building:
- `GET /api/customers` — list all customers
- `POST /api/customers` — create a new customer
- `GET /api/workorders` — list work orders (with filters)
- `POST /api/workorders` — create a new work order

Technologies:
- Minimal APIs (lightweight, modern)
- Dependency Injection (clean architecture)
- LINQ (powerful querying)
- Validation + error handling

By Day 14:
- ServiceHub API is live locally
- You can create and retrieve data via HTTP requests
- The foundation is solid
- Maya is impressed with the speed

---

### **Week 3: The Full-Stack Sprint**
**Goal:** Add a database and UI — ServiceHub becomes real.

What you're building:
- **EF Core setup:** Database schema, migrations
- **CRUD layer:** Data access for customers and work orders
- **Blazor pages:** Customer management, work order list, work order form
- **API integration:** Blazor components calling your API

Technologies:
- Entity Framework Core (ORM)
- Database migrations (schema versioning)
- Blazor WebAssembly (modern C# UI framework)
- Component lifecycle and state management

By Day 21:
- ServiceHub has a database (PostgreSQL or SQLite)
- You can manage customers through a web UI
- You can create and track work orders
- Data persists and is retrieved from the database
- Maya sees a real product taking shape

---

### **Week 4: The Launch Sprint**
**Goal:** Add security, polish, and ship.

What you're building:
- **Authentication:** Owner login (email + password)
- **Authorization:** Only logged-in users see data
- **Dashboard:** Today's jobs, completed this week, metrics
- **UI Polish:** Professional styling with MudBlazor
- **Testing:** Unit tests for core logic
- **Deployment:** API + Blazor live on the internet

Technologies:
- Identity & JWT tokens (authentication)
- Authorization policies (security)
- Advanced LINQ (dashboard queries)
- Unit testing (xUnit)
- Cloud deployment (Azure, AWS, Heroku, etc.)

By Day 30:
- **ServiceHub MVP v1.0 is live.**
- Maya logs in at `https://your-servicehub-app.com`
- She sees her dashboard, manages customers, schedules work orders
- It's production-ready, secure, and professional
- She's using it to run her business

---

## 🎯 Core Entities (The Domain Model)

ServiceHub revolves around four simple entities:

### **Customer**
```csharp
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

### **WorkOrder**
```csharp
public class WorkOrder
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int? TechnicianId { get; set; }
    public int? ServiceTypeId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string Description { get; set; }
    public string Status { get; set; } // "Scheduled", "InProgress", "Completed", "Canceled"
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public Customer Customer { get; set; }
    public Technician? Technician { get; set; }
    public ServiceType? ServiceType { get; set; }
}
```

### **Technician** (Optional)
```csharp
public class Technician
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
}
```

### **ServiceType** (Lookup Table)
```csharp
public class ServiceType
{
    public int Id { get; set; }
    public string Name { get; set; } // "Gutter Cleaning", "Mowing", "AC Service", etc.
    public int? DefaultEstimatedMinutes { get; set; }
    public bool IsActive { get; set; }
}
```

These four entities are the **entire domain** for ServiceHub MVP. Relationships:
- One customer has many work orders
- One work order is for one customer (required)
- One technician can have many work orders (optional)
- One service type can have many work orders (optional)

That's it. Simple. Powerful.

---

## 📋 User Stories (What Maya Can Do)

### **Customer Management**
- ✅ **View customers:** Maya opens the app and sees a list of all her customers
- ✅ **Add customer:** She fills out a form (name, phone, email, address, notes) and saves
- ✅ **Edit customer:** She updates a customer's contact info
- ✅ **Search customers:** She can filter by name

### **Work Order Management**
- ✅ **View work orders:** Maya sees a list of all scheduled jobs (grouped by date)
- ✅ **Create work order:** She picks a customer and schedules a job (date, description, service type)
- ✅ **Update status:** She marks jobs as "InProgress" or "Completed"
- ✅ **Filter jobs:** She sees "today's jobs" or "this week's completed jobs"
- ✅ **Assign technician:** She can assign a team member to a job (optional)

### **Dashboard**
- ✅ **Today's schedule:** "I have 3 jobs today"
- ✅ **Weekly summary:** "I completed 12 jobs this week"
- ✅ **By service type:** "Most jobs are gutter cleaning"
- ✅ **Overdue jobs:** "Alert: 1 job is overdue"

### **Security**
- ✅ **Login:** Only Maya (and future team members) can access the system
- ✅ **Protected data:** If someone visits without logging in, they see a login page
- ✅ **Session management:** Maya stays logged in for a day

---

## 🚀 Why This Project Is Perfect

1. **It's Real:** ServiceHub solves an actual problem for real small businesses
2. **It's Complete:** API, database, UI, auth, deployment — the full stack
3. **It's Achievable:** 30 days is realistic for an MVP with your learning path
4. **It's Scalable:** After MVP, ServiceHub can grow (invoicing, mobile app, multi-user, etc.)
5. **It's Portfolio-Grade:** You can show this to employers and get hired

---

## 🎓 What You'll Learn Building It

### **C# & .NET Skills**
- Object-oriented programming
- Design patterns (DI, repository, etc.)
- Async/await
- LINQ
- Testing

### **Web Development Skills**
- REST API design
- Authentication & authorization
- Relational databases
- ORM (Entity Framework)
- Modern frontend (Blazor)

### **Professional Skills**
- Working to a specification
- Shipping on deadline
- Deploying to production
- Debugging real systems
- Code organization and maintainability

### **Mindset Shift**
You move from "learning syntax" to "shipping features."

That's the difference between a student and a professional developer.

---

## 🏁 The Finish Line

On Day 30, you'll have:

1. **Shipped software** that real people can use
2. **Built a portfolio project** that proves your skills
3. **Worked full-stack** (backend, frontend, database, deployment)
4. **Shipped on a deadline** (30 days is tight!)
5. **Solved a real problem** (ServiceHub works because it's needed)

Maya will thank you. You'll have changed her business. And you'll have proven to yourself (and future employers) that you can ship.

**That's the power of AscendCSharp30.**

---

## 🔮 The Future

After Day 30, ServiceHub can grow:
- **Invoicing system:** Track revenue, send invoices
- **Customer portal:** Let customers see their own jobs
- **Mobile app:** iOS/Android versions
- **Multi-user:** Multiple techs, managers, etc.
- **Notifications:** Email/SMS alerts
- **Scheduling:** Calendar view, conflict detection
- **Analytics:** Trends, forecasting, revenue reports
- **Integrations:** Stripe for payments, Twilio for SMS, etc.

But first, **you ship the MVP.** That's the 30-day challenge.

---

**Ready to build ServiceHub?** [Start Day 1 →](../days/Day01-Setup-And-Tooling/Day01-Starter/README.md)
