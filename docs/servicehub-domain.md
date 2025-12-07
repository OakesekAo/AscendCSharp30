# ServiceHub Domain & ERD

This page describes the domain model for **ServiceHub**, the capstone project for AscendCSharp30.  
It's your guide to how the core entities relate to each other and how they map into a relational database.

---

## Core Entities

ServiceHub keeps the core domain intentionally small and realistic.

### Customer

Represents a client of the business.

- `Id` (int, primary key)
- `Name` (string, required)
- `Email` (string, optional)
- `Phone` (string, optional)
- `Address` (string, optional)
- `Notes` (string, optional)
- `CreatedAt` (DateTime)
- `UpdatedAt` (DateTime)

### WorkOrder

Represents a job scheduled for a customer.

- `Id` (int, primary key)
- `CustomerId` (FK → Customer.Id)
- `ServiceTypeId` (FK → ServiceType.Id, optional)
- `TechnicianId` (FK → Technician.Id, optional)
- `ScheduledDate` (DateTime, required)
- `Description` (string, required)
- `Status` (enum or string: `Scheduled`, `InProgress`, `Completed`, `Canceled`)
- `CreatedAt` (DateTime)
- `UpdatedAt` (DateTime)

### Technician (optional for MVP, great for stretch goals)

Represents the person doing the work.

- `Id` (int, primary key)
- `Name` (string, required)
- `IsActive` (bool)
- `Notes` (string, optional)

### ServiceType

Represents the type of work being performed.

- `Id` (int, primary key)
- `Name` (string, required, e.g., "Gutter Cleaning", "Mowing", "AC Tune-up")
- `DefaultEstimatedDurationMinutes` (int, optional)
- `IsActive` (bool)

---

## Relationships

At a high level:

- A **Customer** can have many **WorkOrders**.
- A **Technician** can be assigned to many **WorkOrders**.
- A **ServiceType** can be associated with many **WorkOrders**.

Foreign keys:

- `WorkOrder.CustomerId` → `Customer.Id`
- `WorkOrder.TechnicianId` → `Technician.Id` (optional)
- `WorkOrder.ServiceTypeId` → `ServiceType.Id` (optional)

---

## ASCII ERD

Below is a simple text-based ERD to visualize the relationships:

```
+-------------+          +----------------+
|  Customer   |          |   Technician   |
+-------------+          +----------------+
| Id (PK)     |          | Id (PK)        |
| Name        |          | Name           |
| Email       |          | IsActive       |
| Phone       |          | Notes          |
| Address     |          +----------------+
| Notes       |
| CreatedAt   |                ^
| UpdatedAt   |                |
+-------------+                |
           ^                   |
           |                   |
           |                   |
     +-----------+      +-------------+
     | WorkOrder | ---> | ServiceType |
     +-----------+      +-------------+
     | Id (PK)   |      | Id (PK)     |
     | CustomerId|      | Name        |
     | TechnicianId     | DefaultEst. |
     | ServiceTypeId    | IsActive    |
     | ScheduledDate    +-------------+
     | Description |
     | Status      |
     | CreatedAt   |
     | UpdatedAt   |
     +-----------+
```

Where:

- `WorkOrder.CustomerId` → `Customer.Id`
- `WorkOrder.TechnicianId` → `Technician.Id` (optional)
- `WorkOrder.ServiceTypeId` → `ServiceType.Id` (optional)

---

## How This Maps to EF Core

In EF Core, you'll typically have:

- A `ServiceHubDbContext` (or similar) exposing `DbSet<Customer>`, `DbSet<WorkOrder>`, `DbSet<Technician>`, `DbSet<ServiceType>`.

Navigation properties such as:

- `Customer.WorkOrders`
- `WorkOrder.Customer`
- `WorkOrder.Technician`
- `WorkOrder.ServiceType`

**Example (simplified):**

```csharp
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
}

public class WorkOrder
{
    public int Id { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = default!;

    public int? TechnicianId { get; set; }
    public Technician? Technician { get; set; }

    public int? ServiceTypeId { get; set; }
    public ServiceType? ServiceType { get; set; }

    public DateTime ScheduledDate { get; set; }
    public string Description { get; set; } = default!;
    public string Status { get; set; } = "Scheduled";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

You don't need to fully implement this on Day 1, but this page gives you a north star for how the data model will evolve as you progress through the 30 days.

---

## Future Extensions

Once the core is working, ServiceHub can grow into:

- **Invoices / Payments** — Track what customers owe
- **Notes / Attachments** on WorkOrders — Store photos or job notes
- **Tags or labels** on Customers — Organize by service type or location
- **Technician scheduling** and availability — Assign work intelligently
- **Notifications** (email/SMS) — Alert customers and techs when jobs change

Those are great stretch goals once you've completed the base AscendCSharp30 path.
