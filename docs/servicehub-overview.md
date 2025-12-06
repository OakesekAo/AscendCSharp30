# ServiceHub – Your Capstone Project

ServiceHub is the capstone project for AscendCSharp30: a lightweight operations platform for small local service businesses (cleaners, landscapers, HVAC techs, handypeople, etc.).

The idea is simple:

> Give a solo or small business owner one place to track **customers**, **work orders**, and **daily schedules** – without needing a giant enterprise system.

Over 30 days, you'll gradually build ServiceHub from scratch using modern C# and .NET:

- A **C# domain model** for customers, technicians, and work orders
- An **ASP.NET Core Web API** exposing that domain
- **EF Core** for relational data and migrations
- A **Blazor WebAssembly** frontend that calls your API
- **Authentication** so only the owner can manage their data
- A simple **dashboard** with real-time stats

By Day 30, you'll have a full-stack mini SaaS deployed and ready to demo.

---

## Core Domain Concepts

ServiceHub keeps the domain intentionally small and realistic:

### Customer

Represents a client of the business.

- `Id`
- `Name`
- `Email`
- `Phone`
- `Address`
- `Notes`

### WorkOrder

Represents a job to be done for a customer.

- `Id`
- `CustomerId`
- `ServiceTypeId` (optional)
- `TechnicianId` (optional)
- `ScheduledDate`
- `Description`
- `Status` (`Scheduled`, `InProgress`, `Completed`, `Canceled`)
- `CreatedAt`, `UpdatedAt`

### Technician (optional for MVP, great for stretch goals)

Represents the person doing the work.

- `Id`
- `Name`
- `IsActive`
- `Notes`

### ServiceType (lookup table)

Represents the type of work:

- `Id`
- `Name` (e.g., "Gutter Cleaning", "Mowing", "AC Tune-up")
- `DefaultEstimatedDurationMinutes`

These entities are perfect for practicing:

- C# classes + records
- EF Core relationships
- LINQ queries
- Blazor forms and dropdowns

---

## User Stories for the MVP

These stories give meaning to the features you'll build:

### Customer Management

- As an **owner**, I can create a new customer with name, contact info, and notes.
- As an **owner**, I can view and search a list of customers.
- As an **owner**, I can edit a customer's details.

### Work Order Management

- As an **owner**, I can create a work order for a customer with a scheduled date and description.
- As an **owner**, I can set and update the status of each work order.
- As an **owner**, I can filter work orders by status or date (e.g., "today's jobs").
- As an **owner**, I can see upcoming work orders in a single list.

### Dashboard

- As an **owner**, I can see:
  - Total jobs scheduled for today
  - Jobs completed this week
  - Jobs by service type
  - Overdue jobs

### Authentication

- As an **owner**, I must log in to see or change any ServiceHub data.
- A logged-out visitor should not be able to view or modify customers or work orders.

---

## How ServiceHub Maps to the 30 Days

You won't build ServiceHub all at once. Instead, each week adds a layer:

- **Week 1 – Language:** C# fundamentals using examples from the ServiceHub domain (customers, work orders, statuses).
- **Week 2 – API:** Minimal API that exposes customer and work-order endpoints.
- **Week 3 – Data + UI:** EF Core for persistence + first Blazor pages that call your API.
- **Week 4 – Real-World:** Authentication, dashboard, cleanup, deployment.

The goal is that by Day 30 you don't just "know C#" – you've shipped a real full-stack app.
