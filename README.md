# 🏢 Customer Management System - Clean Architecture CRUD Application

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework%20Core-8.0-green.svg)](https://docs.microsoft.com/en-us/ef/core/)
[![xUnit](https://img.shields.io/badge/xUnit-2.4-orange.svg)](https://xunit.net/)
[![SpecFlow](https://img.shields.io/badge/SpecFlow-BDD-purple.svg)](https://specflow.org/)


## 📋 Project Overview

This is a **real-world Customer Management System** built using **Clean Architecture** principles with **.NET 8**. The application demonstrates modern software development practices including **Domain-Driven Design (DDD)**, **CQRS pattern**, **Test-Driven Development (TDD)**, and **Behavior-Driven Development (BDD)**.

The system provides a complete CRUD (Create, Read, Update, Delete) API for managing customer information with robust validation, domain events, and comprehensive testing coverage.

## ✨ Features

### Core Functionality
- 🆕 **Create Customer** - Add new customers with comprehensive validation
- 📖 **Read Customer** - Retrieve customers by ID, email, or with pagination
- ✏️ **Update Customer** - Modify existing customer information
- 🗑️ **Delete Customer** - Soft delete customers from the system
- 🔍 **Search & Pagination** - Advanced customer search with pagination support

### Business Rules & Validation
- 📧 **Email Uniqueness** - Prevents duplicate customers by email
- 👥 **Customer Uniqueness** - Unique constraint on FirstName + LastName + DateOfBirth
- 🔞 **Age Validation** - Customers must be at least 18 years old
- 📱 **Mobile Phone Validation** - International mobile number validation
- 🏦 **Bank Account Validation** - Bank account number format validation
- ✉️ **Email Format Validation** - RFC-compliant email validation

### Technical Features
- 🎯 **Domain Events** - CustomerCreated, CustomerUpdated, CustomerDeleted events
- 🔄 **CQRS Pattern** - Separate command and query models
- 🏗️ **Value Objects** - Email, PhoneNumber with built-in validation
- 📊 **Read Models** - Optimized data models for queries
- 🔒 **Data Integrity** - Database-level constraints and validation

## 🏗️ Architecture & Design Patterns

### Clean Architecture
The project follows **Clean Architecture** principles with clear separation of concerns:

- **Domain Layer** - Core business logic, entities, value objects, and domain events
- **Application Layer** - Use cases, commands, queries, and application services
- **Infrastructure Layer** - Data access, external services, and cross-cutting concerns
- **API Layer** - RESTful API controllers and presentation logic

### Design Patterns Implemented
- **🎯 Domain-Driven Design (DDD)** - Rich domain models with business logic encapsulation
- **⚡ CQRS (Command Query Responsibility Segregation)** - Separate models for read and write operations
- **🏭 Repository Pattern** - Abstraction layer for data access
- **📦 Value Objects** - Immutable objects representing domain concepts
- **📢 Domain Events** - Decoupled communication between domain entities
- **🔧 Dependency Injection** - Loose coupling and testability
- **🎭 Mediator Pattern** - Decoupled request/response handling

## 🛠️ Technologies & Frameworks

### Backend Technologies
- **[.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)** - Latest .NET framework with improved performance
- **[ASP.NET Core 8.0](https://docs.microsoft.com/en-us/aspnet/core/)** - Web API framework
- **[Entity Framework Core 8.0](https://docs.microsoft.com/en-us/ef/core/)** - Object-relational mapping (ORM)
- **[MediatR](https://github.com/jbogard/MediatR)** - Mediator pattern implementation for CQRS
- **[FluentValidation](https://fluentvalidation.net/)** - Fluent interface for building validation rules

### Database & Storage
- **[SQL Server](https://www.microsoft.com/en-us/sql-server)** - Production database
- **[InMemory Database](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/)** - Testing and development

### Validation & Utilities
- **[libphonenumber-csharp](https://github.com/twcclegg/libphonenumber-csharp)** - International phone number validation
- **[Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)** - Swagger/OpenAPI documentation

### Testing Frameworks
- **[xUnit](https://xunit.net/)** - Unit testing framework
- **[SpecFlow](https://specflow.org/)** - Behavior-Driven Development (BDD) framework
- **[FluentAssertions](https://fluentassertions.com/)** - Fluent assertion library
- **[Moq](https://github.com/moq/moq4)** - Mocking framework for unit tests
- **[Coverlet](https://github.com/coverlet-coverage/coverlet)** - Code coverage analysis

## 🧪 Testing Strategy

### Test-Driven Development (TDD)
The project was built using **TDD principles**:
- ✅ **Red-Green-Refactor** cycle implementation
- 🎯 **Unit Tests** for domain logic and value objects
- 🔧 **Test-first approach** for business rules

### Behavior-Driven Development (BDD)
**SpecFlow** is used for acceptance testing:
- 📝 **Gherkin syntax** for readable test scenarios
- 🎭 **Given-When-Then** structure
- 👥 **Stakeholder-friendly** test documentation

### Testing Levels
1. **🔬 Unit Tests** - Domain entities, value objects, and business logic
2. **🔗 Integration Tests** - Database operations and API endpoints
3. **🎯 Acceptance Tests** - End-to-end business scenarios
4. **📊 Code Coverage** - Comprehensive test coverage analysis

### Test Categories
- **Domain Logic Tests** - Customer entity, Email/PhoneNumber value objects
- **Validation Tests** - FluentValidation rules and business constraints
- **Repository Tests** - Data access layer functionality
- **API Tests** - Controller endpoints and HTTP responses
- **BDD Scenarios** - Customer management workflows

### Sample API Endpoints
```http
GET    /api/customers              # Get all customers with pagination
GET    /api/customers/{id}         # Get customer by ID
GET    /api/customers/email/{email} # Get customer by email
POST   /api/customers              # Create new customer
PUT    /api/customers/{id}         # Update existing customer
DELETE /api/customers/{id}         # Delete customer
```

### API Documentation
- 🔗 **Swagger UI**: Available at `http://localhost:5000/swagger`
- 📋 **Interactive Documentation**: Test all endpoints directly from the browser
- 🧪 **Built-in Testing**: No additional tools needed for API exploration

## 🚀 Getting Started

### Prerequisites
- **[.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)** or later
- **[Visual Studio 2022](https://visualstudio.microsoft.com/)** or **[VS Code](https://code.visualstudio.com/)**
- **[SQL Server](https://www.microsoft.com/en-us/sql-server)** (optional - uses InMemory for development)
- **[Git](https://git-scm.com/)** for version control

### Installation & Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/crud-test-dotnet.git
   cd crud-test-dotnet
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Run database migrations** (if using SQL Server)
   ```bash
   dotnet ef database update --project src/Mc2.CrudTest.Infrastructure --startup-project src/Mc2.CrudTest.API
   ```

5. **Run the application**
   ```bash
   dotnet run --project src/Mc2.CrudTest.API
   ```

6. **Access the API**
   - API: `http://localhost:5000`
   - Swagger UI: `http://localhost:5000/swagger`

### Running Tests

**Run all tests**
```bash
dotnet test
```

**Run unit tests only**
```bash
dotnet test test/Mc2.CrudTest.UnitTests
```

**Run acceptance tests (BDD)**
```bash
dotnet test test/Mc2.CrudTest.AcceptanceTests
```

**Generate code coverage report**
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## 💡 Usage Examples

### Create a New Customer
```http
POST /api/customers
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "dateOfBirth": "1990-01-15",
  "email": "john.doe@example.com",
  "phoneNumber": "+1234567890",
  "bankAccountNumber": "1234567890123456789"
}
```

### Get Customer by Email
```http
GET /api/customers/email/john.doe@example.com
```

### Update Customer
```http
PUT /api/customers/{id}
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Smith",
  "dateOfBirth": "1990-01-15",
  "email": "john.smith@example.com",
  "phoneNumber": "+1234567890",
  "bankAccountNumber": "1234567890123456789"
}
```

### Get Customers with Pagination
```http
GET /api/customers?pageNumber=1&pageSize=10
```

## 📁 Folder Structure

```
crud-test-dotnet/
├── src/
│   ├── Mc2.CrudTest.API/              # 🌐 Web API Layer
│   │   ├── Controllers/                # REST API controllers
│   │   ├── Program.cs                  # Application entry point
│   │   └── appsettings.json           # Configuration settings
│   ├── Mc2.CrudTest.Application/       # 🎯 Application Layer
│   │   ├── Customers/
│   │   │   ├── Commands/              # CQRS Commands
│   │   │   ├── Queries/               # CQRS Queries
│   │   │   └── DTOs/                  # Data Transfer Objects
│   │   └── DependencyInjection.cs    # Service registration
│   ├── Mc2.CrudTest.Domain/           # 🏛️ Domain Layer
│   │   ├── Customers/
│   │   │   ├── Entities/              # Domain entities
│   │   │   ├── ValueObjects/          # Value objects (Email, PhoneNumber)
│   │   │   ├── Events/                # Domain events
│   │   │   └── Repositories/          # Repository interfaces
│   │   └── Common/                    # Shared domain concepts
│   └── Mc2.CrudTest.Infrastructure/   # 🔧 Infrastructure Layer
│       ├── Data/                      # EF Core DbContext
│       ├── Repositories/              # Repository implementations
│       ├── Migrations/                # Database migrations
│       └── DependencyInjection.cs    # Infrastructure services
├── test/
│   ├── Mc2.CrudTest.UnitTests/        # 🔬 Unit Tests
│   │   ├── Domain/                    # Domain logic tests
│   │   └── Application/               # Application service tests
│   └── Mc2.CrudTest.AcceptanceTests/  # 🎭 BDD Acceptance Tests
│       ├── Features/                  # Gherkin feature files
│       └── StepDefinitions/           # SpecFlow step definitions
└── Mc2.CrudTest.sln                   # Solution file
```

## 🤝 Contributing

We welcome contributions! Please follow these guidelines:

### Development Workflow
1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/amazing-feature`)
3. **Write tests** for your changes (TDD approach)
4. **Implement** your feature
5. **Ensure all tests pass** (`dotnet test`)
6. **Commit** your changes (`git commit -m 'Add amazing feature'`)
7. **Push** to the branch (`git push origin feature/amazing-feature`)
8. **Open** a Pull Request

### Code Standards
- Follow **Clean Code** principles
- Maintain **high test coverage** (>80%)
- Use **meaningful commit messages**
- Follow **C# coding conventions**
- Write **comprehensive tests** for new features
- Update **documentation** for significant changes

### Pull Request Guidelines
- Provide clear description of changes
- Include relevant test cases
- Ensure CI/CD pipeline passes
- Request review from maintainers

## 🚧 Future Development

This project is under **active development**. Planned features include:

### 🔮 Upcoming Features
- 🤖 **AI Integration** - Smart customer insights and recommendations
- 📊 **Advanced Analytics** - Customer behavior analysis and reporting
- 🔐 **Authentication & Authorization** - JWT-based security with role management
- 📝 **Comprehensive Logging** - Structured logging with Serilog
- 📈 **Monitoring & Observability** - Application performance monitoring
- 🐳 **Docker Support** - Containerization for easy deployment
- ☁️ **Cloud Deployment** - Azure/AWS deployment configurations
- 🔄 **Event Sourcing** - Complete audit trail of customer changes
- 📧 **Email Notifications** - Customer lifecycle email automation
- 🌐 **GraphQL API** - Alternative API interface for flexible queries

### 🛠️ Technical Improvements
- **Performance Optimization** - Caching strategies and query optimization
- **API Versioning** - Backward-compatible API evolution
- **Rate Limiting** - API protection and throttling
- **Health Checks** - Application health monitoring endpoints
- **Integration Tests** - Comprehensive API testing suite

## 📞 Contact

**Project Maintainer**: Soheil Kaviani Mehr
- 📧 Email: Soheil79Km@hotmail.com
- 💼 LinkedIn: https://www.linkedin.com/in/soheilkm/
- 🐙 GitHub: [@SoheilKavianiMehr](https://github.com/SoheilKavianiMehr)

---

⭐ **If you find this project helpful, please give it a star!** ⭐

*Built with ❤️ using Clean Architecture and modern .NET practices*