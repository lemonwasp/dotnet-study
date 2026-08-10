# Dotnet Study

A learning repository for **C#**, **ASP.NET**, and backend development.

The goal of this repository is to learn backend development step by step, starting from **ASP.NET MVC 5 on .NET Framework 4.8** and gradually expanding toward modern .NET backend development.

---

## 📁 Projects

### Framework48Mvc

An ASP.NET MVC 5 application built on **.NET Framework 4.8**.

The project started with a basic MVC structure and is being incrementally extended with patterns and technologies commonly used in real-world backend applications.

---

## 🏗️ Current Architecture

```text
Vue.js
   │
   │ fetch / JSON
   ▼
Controller
   │
   ▼
Service
   │
   ▼
IHomeRepository
   │
   ▼
EntityFrameworkHomeRepository
   │
   ▼
Entity Framework 6
   │
   ▼
SQL Server LocalDB
```

The application separates responsibilities into multiple layers instead of placing business logic directly inside controllers.

---

## 🛠️ Tech Stack

### Backend

- C#
- .NET Framework 4.8
- ASP.NET MVC 5
- Entity Framework 6
- SQL Server LocalDB
- Unity
- AutoMapper
- log4net

### Frontend

- Razor View
- Vue.js
- Fetch API
- JSON-based client/server communication

### Testing

- MSTest
- Service layer unit tests
- Validation and exception tests

### Development

- Git
- GitHub
- GitHub Issues
- Feature Branch Workflow
- Visual Studio

---

## ✨ Implemented Features

### Message CRUD

The application currently supports basic message management.

- Create messages
- Read messages
- Update messages
- Delete messages

### Layered Architecture

Responsibilities are separated into:

- Controller
- Service
- Repository Interface
- Entity Framework Repository
- Database

### Dependency Injection

**Unity** is used as the dependency injection container.

Dependencies such as repositories, services, `ApplicationDbContext`, and AutoMapper are resolved through the DI container.

### Entity Framework

**Entity Framework 6** is used for database access.

The application currently uses:

- `DbContext`
- `DbSet`
- LINQ queries
- Code First
- Migrations
- LocalDB

### DTOs

Request and response models are separated from database entities.

Examples:

- `CreateMessageRequest`
- `UpdateMessageRequest`
- `MessageResponse`
- `Message`

### AutoMapper

**AutoMapper** is used to reduce manual mapping between DTOs and entities.

```text
CreateMessageRequest
        │
        ▼
      Message

UpdateMessageRequest
        │
        ▼
      Message

Message
   │
   ▼
MessageResponse
```

### Validation

Business validation is handled in the service layer.

For example, an empty message is rejected before reaching the repository.

```text
Empty Message
     │
     ▼
ArgumentException
     │
     ▼
400 Bad Request
```

### Global Exception Handling

Exceptions are handled centrally through an ASP.NET MVC exception filter.

| Exception | HTTP Status |
|---|---|
| `ArgumentException` | `400 Bad Request` |
| `KeyNotFoundException` | `404 Not Found` |
| Unexpected Exception | `500 Internal Server Error` |

This keeps exception handling logic out of individual controller actions.

### Logging

**log4net** is used for application logging.

The logging layer records application events and exceptions without coupling the business logic to a specific logging implementation.

---

## 🗄️ Database

The application currently uses **SQL Server LocalDB** for local development.

```text
ASP.NET MVC
     │
     ▼
Entity Framework 6
     │
     ▼
SQL Server LocalDB
     │
     ▼
Framework48MvcDb
```

Database schema changes are managed using **Entity Framework Code First Migrations**.

Typical migration commands:

```powershell
Enable-Migrations
Add-Migration InitialCreate
Update-Database
```

---

## 🌐 Request Flow

### Create Message

```text
Vue.js
   │
   │ POST /Home/AddMessage
   ▼
HomeController
   │
   ▼
HomeService
   │
   │ Validation
   ▼
IHomeRepository
   │
   ▼
EntityFrameworkHomeRepository
   │
   │ AutoMapper
   ▼
Message Entity
   │
   ▼
Entity Framework
   │
   ▼
SQL Server
```

### Read Messages

```text
SQL Server
   │
   ▼
Entity Framework
   │
   ▼
Message Entity
   │
   │ AutoMapper
   ▼
MessageResponse
   │
   ▼
Controller
   │
   │ JSON
   ▼
Vue.js
```

---

## 🧪 Testing

The project uses **MSTest** to verify service behavior and business rules.

Tests currently cover areas such as:

- Message creation
- Message updates
- Message deletion
- Empty message validation
- Invalid input
- Exception behavior

The service layer is tested independently from the actual database through repository abstraction.

---

## 🔄 Development Workflow

Features are developed incrementally using **GitHub Issues** and feature branches.

```text
GitHub Issue
     │
     ▼
Feature Branch
     │
     ▼
Implementation
     │
     ▼
Build
     │
     ▼
Tests
     │
     ▼
Pull Request
     │
     ▼
Merge
```

Each backend concept is introduced as a separate issue so that the development history also represents the learning process.

---

## 🗺️ Roadmap

### Next

- [ ] Server-side Pagination
- [ ] Search / Filtering
- [ ] Sorting
- [ ] Async Entity Framework Operations

### Backend Improvements

- [ ] Transaction Handling
- [ ] Database Indexing
- [ ] API Response Standardization
- [ ] Authentication
- [ ] Authorization

### Infrastructure

- [ ] Redis Caching
- [ ] Docker
- [ ] GitHub Actions CI/CD

### Future

- [ ] ASP.NET Core
- [ ] PostgreSQL
- [ ] Modern .NET Backend Architecture

---

## 🎯 Learning Goals

Through this repository, I am practicing:

- ASP.NET MVC application architecture
- Separation of concerns
- Dependency Injection
- Repository Pattern
- Service Layer Pattern
- Entity Framework
- ORM concepts
- Database migrations
- DTO design
- Object mapping
- Exception handling
- HTTP status codes
- Logging
- Unit testing
- HTTP / JSON communication
- Vue.js integration
- Git feature branch workflow
- Incremental backend architecture improvement

---

## 📌 Current Progress

```text
MVC
 │
 ▼
Service Layer
 │
 ▼
Repository Pattern
 │
 ▼
Dependency Injection
 │
 ▼
Entity Framework
 │
 ▼
Code First Migration
 │
 ▼
Validation
 │
 ▼
Global Exception Handling
 │
 ▼
Logging
 │
 ▼
AutoMapper
 │
 ▼
Pagination ← Next
```

This repository is continuously updated as new backend concepts are studied and implemented.
