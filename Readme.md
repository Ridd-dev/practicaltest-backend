# Department-Employee Management API (ADO.NET Implementation)

A robust, enterprise-ready backend API for managing departments and employees, built with **ASP.NET Core 8** and **pure ADO.NET** for high-performance database operations. This system provides complete CRUD functionality with clean architecture patterns, comprehensive validation, and seamless integration with React frontends.

## 🚀 Features

- **Complete CRUD Operations** for Departments and Employees
- **Pure ADO.NET Implementation** - No ORM overhead, maximum performance
- **Clean Architecture** - Repository pattern, Service layer, Dependency Injection
- **Data Validation** - Comprehensive input validation and business rules
- **Auto-Calculated Fields** - Employee age calculated from date of birth
- **Foreign Key Relationships** - Proper database normalization
- **Error Handling** - Comprehensive exception handling with user-friendly messages
- **API Documentation** - Swagger/OpenAPI integration
- **CORS Enabled** - Ready for cross-origin requests from React frontend

## 🏗️ Architecture

- **Controllers** - Handle HTTP requests/responses
- **Services** - Business logic layer
- **Repositories** - Data access layer with pure ADO.NET
- **Models** - Entity models and DTOs
- **AutoMapper** - Object-to-object mapping
- **Dependency Injection** - Loose coupling and testability

## 🛠️ Technology Stack

- **ASP.NET Core 8** - Web API framework
- **ADO.NET** - Pure database access (Microsoft.Data.SqlClient)
- **SQL Server** - Database engine (LocalDB/Express/Full)
- **AutoMapper** - Object mapping
- **FluentValidation** - Input validation
- **Swagger/OpenAPI** - API documentation
- **Dependency Injection** - Built-in .NET DI container

## 📋 Prerequisites

- **.NET 8 SDK** - [Download here](https://dotnet.microsoft.com/download)
- **SQL Server** (LocalDB, Express, or Full version)
- **SQL Server Management Studio (SSMS)** - For database management
- **Git** - Version control

## 🚀 Quick Start

### 1. Clone the Repository
git clone <repository-url>
cd DepartmentEmployeeSystem.API

### 2. Install Dependencies
dotnet restore

### 3. Create Database
Run the following SQL script in SQL Server Management Studio:

CREATE DATABASE DepartmentEmployeeDB;
GO

USE DepartmentEmployeeDB;
GO

CREATE TABLE Departments (
DepartmentId INT IDENTITY(1,1) PRIMARY KEY,
DepartmentCode NVARCHAR(10) NOT NULL UNIQUE,
DepartmentName NVARCHAR(100) NOT NULL,
Description NVARCHAR(500) NULL,
IsActive BIT NOT NULL DEFAULT 1,
CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
ModifiedDate DATETIME2 NULL
);

CREATE TABLE Employees (
EmployeeId INT IDENTITY(1,1) PRIMARY KEY,
FirstName NVARCHAR(50) NOT NULL,
LastName NVARCHAR(50) NOT NULL,
EmailAddress NVARCHAR(100) NOT NULL UNIQUE,
DateOfBirth DATETIME2 NOT NULL,
Salary DECIMAL(18,2) NOT NULL,
PhoneNumber NVARCHAR(15) NULL,
IsActive BIT NOT NULL DEFAULT 1,
CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
ModifiedDate DATETIME2 NULL,
DepartmentId INT NOT NULL,
FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId)
);

-- Sample data
INSERT INTO Departments (DepartmentCode, DepartmentName, Description, IsActive, CreatedDate)
VALUES
('IT', 'Information Technology', 'Technology and software development', 1, GETDATE()),
('HR', 'Human Resources', 'Employee management and policies', 1, GETDATE()),
('FIN', 'Finance', 'Financial operations and accounting', 1, GETDATE());

### 4. Configure Connection String
Update `appsettings.json`:
{
"ConnectionStrings": {
"DefaultConnection": "Server=(localdb)\MSSQLLocalDB;Database=DepartmentEmployeeDB;Trusted_Connection=true;MultipleActiveResultSets=true"
}
}

**Alternative connection strings:**
- **SQL Express:** `Server=.\\SQLEXPRESS;Database=DepartmentEmployeeDB;Trusted_Connection=true`
- **Full SQL Server:** `Server=localhost;Database=DepartmentEmployeeDB;User Id=sa;Password=YourPassword;TrustServerCertificate=true`

### 5. Build and Run
Build the project
dotnet build

Run the API
dotnet run


### 6. Access the API
- **API Base URL:** `http://localhost:5244`
- **Swagger Documentation:** `http://localhost:5244/swagger`

## 📡 API Endpoints

### Departments
- `GET /api/departments` - Get all departments
- `GET /api/departments/{id}` - Get department by ID
- `POST /api/departments` - Create new department
- `PUT /api/departments/{id}` - Update department
- `DELETE /api/departments/{id}` - Delete department

### Employees
- `GET /api/employees` - Get all employees
- `GET /api/employees/{id}` - Get employee by ID
- `GET /api/employees/department/{departmentId}` - Get employees by department
- `POST /api/employees` - Create new employee
- `PUT /api/employees/{id}` - Update employee
- `DELETE /api/employees/{id}` - Delete employee

## 📝 Request/Response Examples

### Create Department
POST /api/departments
{
"departmentCode": "ENG",
"departmentName": "Engineering",
"description": "Software engineering and development"
}

### Create Employee
POST /api/employees
{
"firstName": "John",
"lastName": "Doe",
"emailAddress": "john.doe@company.com",
"dateOfBirth": "1985-05-15",
"salary": 75000.00,
"phoneNumber": "+1234567890",
"departmentId": 1
}

## 🏢 Database Schema

### Departments Table
- `DepartmentId` (PK, Identity)
- `DepartmentCode` (Unique, Required)
- `DepartmentName` (Required)
- `Description` (Optional)
- `IsActive` (Boolean)
- `CreatedDate`, `ModifiedDate`

### Employees Table
- `EmployeeId` (PK, Identity)
- `FirstName`, `LastName` (Required)
- `EmailAddress` (Unique, Required)
- `DateOfBirth` (Required)
- `Salary` (Required)
- `PhoneNumber` (Optional)
- `IsActive` (Boolean)
- `CreatedDate`, `ModifiedDate`
- `DepartmentId` (FK to Departments)

## ✅ Validation Rules

### Department Validation
- Department Code: Required, max 10 characters, must be unique
- Department Name: Required, max 100 characters
- Description: Optional, max 500 characters

### Employee Validation
- First/Last Name: Required, max 50 characters each
- Email: Required, valid email format, must be unique
- Date of Birth: Required, must be a valid date
- Age: Auto-calculated from date of birth
- Salary: Required, must be positive number
- Department: Must exist in system

## 🔧 Configuration

### Environment Variables
Create `.env` or update `appsettings.json`:
{
"ConnectionStrings": {
"DefaultConnection": "your-connection-string"
},
"Logging": {
"LogLevel": {
"Default": "Information"
}
}
}

### CORS Configuration
Currently configured for React development:
"AllowedOrigins": ["http://localhost:5173", "https://localhost:5173"]

## 🚨 Troubleshooting

### Common Issues

1. **Database Connection Failed**
   - Verify SQL Server is running
   - Check connection string format
   - Ensure database exists

2. **Build Errors**
dotnet clean
dotnet restore
dotnet build

3. **Port Already in Use**
- Change port in `Properties/launchSettings.json`
- Or kill existing process using the port

## 📊 Performance Features

### ADO.NET Benefits
- **Zero ORM Overhead** - Direct SQL execution
- **Memory Efficient** - Manual object mapping
- **High Performance** - Optimized database queries
- **Full SQL Control** - Custom queries and stored procedures
- **Connection Pooling** - Automatic connection management

### Optimization Techniques
- Parameterized queries prevent SQL injection
- Proper connection disposal with `using` statements
- Ordinal column access for faster data reading
- Minimal object allocations

## 🏗️ Project Structure

DepartmentEmployeeSystem.API/
├── Controllers/
│ ├── DepartmentsController.cs
│ └── EmployeesController.cs
├── Data/
│ └── DatabaseConnection.cs
├── Interfaces/
│ ├── IDepartmentRepository.cs
│ ├── IEmployeeRepository.cs
│ ├── IDepartmentService.cs
│ └── IEmployeeService.cs
├── Mappings/
│ └── MappingProfile.cs
├── Models/
│ ├── Department.cs
│ └── Employee.cs
├── Repositories/
│ ├── DepartmentRepository.cs
│ └── EmployeeRepository.cs
├── Services/
│ ├── DepartmentService.cs
│ └── EmployeeService.cs
├── Program.cs
├── appsettings.json
└── DepartmentEmployeeSystem.API.csproj

## 🚀 Deployment

### Development
dotnet run --environment Development

### Production Build
dotnet publish -c Release -o ./publish

### Docker (Optional)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY ./publish /app
WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "DepartmentEmployeeSystem.API.dll"]

## 🧪 Testing

### Manual Testing
- Use Swagger UI: `http://localhost:5244/swagger`
- Use Postman or similar REST client
- Test all CRUD operations

### Integration with Frontend
- Configure CORS for your frontend URL
- Update API base URL in frontend configuration
- Test end-to-end functionality

## 📚 Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [ADO.NET Documentation](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/)
- [SQL Server Documentation](https://docs.microsoft.com/en-us/sql/sql-server/)

## 🤝 Contributing

1. Fork the repository
2. Create feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🎯 Interview Highlights

This project demonstrates:
- **Pure ADO.NET Implementation** as requested
- **Clean Architecture** with separation of concerns
- **Enterprise Patterns** (Repository, Service, DI)
- **Data Validation** and error handling
- **RESTful API Design** with proper HTTP status codes
- **Database Design** with normalization and relationships
- **Performance Optimization** with efficient data access
- **Production-Ready Code** with proper configuration and deployment support

Built with ❤️ using ASP.NET Core 8 and ADO.NET
