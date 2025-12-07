# Day 15 — EF Core Setup (ServiceHub Database)

## 🚀 Week 3: Full-Stack Sprint Begins

**Maya's Message:**
> "We have an API now. But it's still in memory. This week, we make ServiceHub real by adding a database. Every customer, every work order, lives forever."

Today, you'll set up **EF Core** and create ServiceHub's database schema.

---

## Learning Objectives
- Install and configure EF Core
- Create a DbContext for ServiceHub
- Define entities and relationships
- Run migrations to create the database
- Understand how ORM works

## Prerequisites
- Days 01-14 complete
- Comfortable with APIs and dependency injection
- ~60 minutes

---

## Starter Steps
1. Read the objectives above
2. Open this folder in your editor
3. Follow the TODOs to set up EF Core
4. Run migrations and verify the database is created

## Why This Matters for ServiceHub

Before today, ServiceHub's data lived in memory and disappeared on restart.

After today:
- Customers persist forever
- Work orders are stored reliably
- Relationships are enforced (a work order must have a customer)
- The database is your source of truth

This is the moment ServiceHub becomes real.

---

## Next
Day 16: Build CRUD operations for customers and work orders.

## Boilerplate
- This folder contains minimal code and instructions to get started.
- Do not commit build artifacts; keep only source and instructions.

