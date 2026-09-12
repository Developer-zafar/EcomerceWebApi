# 🛒 Enterprise E-Commerce Web API (.NET & C#)

A secure, scalable, and high-performance RESTful Web API built with **ASP.NET Core** and **Entity Framework Core**, featuring Stateless JWT Authentication, Role-Based Access Control (RBAC), Asynchronous Operations, and an Atomic Order Processing Engine.

---

## 🚀 Key Features

- **Decoupled Architecture:** Clean separation of concerns with isolated `TokenService` using Dependency Injection (Scoped Lifetime).
- **Cryptographic Security:** Passwords hashed with ASP.NET Core native `PasswordHasher<User>` utilizing PBKDF2 hashing algorithm.
- **Stateless JWT & RBAC:** Role-Based Access Control (`Admin` vs `Customer`) enforcing granular endpoint security using `[Authorize(Roles = "Admin")]`. Tested security triad: `401 Unauthorized`, `403 Forbidden`, and `201 Created`.
- **100% Asynchronous Non-Blocking Pipeline:** Every database interaction is non-blocking (`async/await`, `ToListAsync`, `SaveChangesAsync`) to eliminate thread starvation.
- **Atomic Order Checkout Engine:** 
  - Extracts authenticated `UserId` securely from JWT Bearer Claims (`ClaimsPrincipal`).
  - Validates stock availability and performs atomic stock deductions.
  - Freezes database product prices into `OrderItem.UnitPrice`.
  - Transaction rollback guarantee via EF Core.
- **Multi-Level Relational Queries:** "My Orders" endpoint leveraging `.Include()` followed by `.ThenInclude()` with safe anonymous DTO projections.
- **Global Exception Handling Middleware:** Centralized safety net that shields internal database stack traces in production environments while streaming standardized camelCase JSON error envelopes.

---

## 🛠️ Tech Stack

- **Framework:** ASP.NET Core Web API
- **Language:** C#
- **ORM:** Entity Framework Core
- **Database:** Microsoft SQL Server
- **Security:** JWT (JSON Web Tokens), PBKDF2 Hashing
- **Testing Tools:** Postman, Swagger / OpenAPI

## 👤 Author
**M. Zafar Iqbal**  
*Software Engineer | ASP.NET Core Developer*