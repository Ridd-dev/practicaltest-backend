# Department-Employee Management Backend API

This repository contains the full backend for an enterprise-ready Department-Employee Management system, built with clean architecture principles using ASP.NET Core 8, Entity Framework Core (EF Core), and Microsoft SQL Server. The API is designed for robust scalability, separation of concerns, and professional maintainability.

## 🚀 Featuress

- RESTful API for Department & Employee CRUD operations
- Repository & Service Layer architecture
- Clean, layered organization for testability and maintainability
- Entity Framework Core Code-First and Migrations
- Strong-typed models with validation and DTOs
- AutoMapper for efficient object mapping (with project-based profiles)
- Dependency Injection everywhere (repositories, services, mapping)
- Role-based authorization & environment-specific CORS
- OpenAPI/Swagger UI for fast API inspection and testing
- Comprehensive error handling

## 🏆 What’s Technical & Professional

- **Clean Architecture**: Service and Repository patterns, DTO separation, mapping, and dependency injection
- **Code-First Migrations**: Database evolves from C# models, never manually created SQL
- **Automated Mapping**: Profiles, projections, and flattening with AutoMapper
- **Strong API Contracts**: Input/Output all via DTOs/validation
- **Scalable for Real-World**: Can be expanded to multi-tenancy, auth, audit logs, cloud deployments
- **Configuration Variants**: Easily switch database or connection in `appsettings.json`
- **Swagger & CORS**: Ready for dev, QA, and production environments

## 🛠️ Stack

- **ASP.NET Core 8** (WebAPI)
- **Entity Framework Core 8**
- **Microsoft SQL Server / LocalDB**
- **AutoMapper**
- **Swashbuckle (Swagger)**
- **FluentValidation**
- **Dependency Injection**

## 📂 Project Structure

DepartmentEmployeeSystem.API/
├── Controllers/
├── Data/
├── Interfaces/
├── Mappings/
├── Models/
├── Repositories/
├── Services/
├── Program.cs
├── appsettings.json
└── ...


## ⚡ Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB, Express, or Full)
- Git

### Setup

1. **Clone** this repository
   `git clone <repo-url>`
2. **Restore** dependencies
   `dotnet restore`
3. **Configure** the database in `appsettings.json`

"DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=DepartmentEmployeeDB;Trusted_Connection=true;MultipleActiveResultSets=true"

4. **Run Migrations**
- Create DB schema:
  `dotnet ef database update`
5. **Run API**
`dotnet run`
API runs on `http://localhost:5244` and `https://localhost:7071`

### Database Notes

- Uses SQL Server with EF Core Code-First: all tables/relations generated from code
- Fully typed, normalized schema (Departments, Employees, Foreign Keys)
- Edit `appsettings.json` to use SQL Server Express/Full

## 🧩 API Documentation

- Auto-generated at `/swagger` after running (`http://localhost:5244/swagger`)

## 📦 Deployment & Test

- Ready for local (dev) and production deployment (`dotnet publish`)
- Unit-testable architecture (repository/services easily mockable)

## 💡 Design Practices

- **Domain-Driven Design:** Follows DDD-style separation (domain, data, service, controller)
- **SOLID Principles** throughout (DI, interfaces, single responsibility)
- **Modern .NET conventions**
- **AutoMapper and DTOs** to decouple entity and API schema

---