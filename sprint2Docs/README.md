# Jadwa API Documentation

Welcome to the Jadwa API documentation. This folder contains comprehensive documentation for the Jadwa API project, including architecture, database design, and implementation details.

## Documentation Structure

### 📋 [Architecture Documentation](./architecture.md)
Comprehensive architecture documentation covering:
- **High-Level Architecture**: System overview and component relationships
- **Low-Level Architecture**: Detailed layer dependencies and CQRS implementation
- **Database Design**: Entity Relationship Diagrams and audit trail design
- **Technology Stack**: Complete list of technologies and frameworks used
- **Security Architecture**: Authentication flows and authorization models
- **API Design**: RESTful API structure and response formats
- **Deployment Architecture**: Production environment setup and configuration
- **Performance Considerations**: Caching strategies and rate limiting
- **Monitoring and Observability**: Logging architecture and monitoring setup

### 🗄️ [Database Schema Documentation](./database-schema.md)
Detailed database documentation including:
- **Database Schema**: Complete SQL table definitions
- **Entity Relationships**: ERD diagrams and relationship mappings
- **Audit Trail Implementation**: Comprehensive audit system design
- **Indexes and Performance**: Recommended indexes for optimal performance
- **Data Seeding**: Default data and configuration
- **Migration Strategy**: Entity Framework migration management

## Quick Start

### Prerequisites
- .NET 9 SDK
- SQL Server (LocalDB or full instance)
- Redis (for caching)
- Visual Studio 2022 or VS Code

### Architecture Overview

The Jadwa API follows Clean Architecture principles with the following layers:

```
┌─────────────────────────────────────────┐
│              Presentation               │
│         (Controllers, Middleware)       │
├─────────────────────────────────────────┤
│              Application                │
│     (Commands, Queries, Handlers)       │
├─────────────────────────────────────────┤
│               Domain                    │
│        (Entities, Aggregates)           │
├─────────────────────────────────────────┤
│            Infrastructure               │
│    (Data Access, External Services)     │
└─────────────────────────────────────────┘
```

### Key Features

- ✅ **Clean Architecture** with clear separation of concerns
- ✅ **CQRS Pattern** using MediatR for command/query separation
- ✅ **JWT Authentication** with refresh token support
- ✅ **Role-based Authorization** with claims-based permissions
- ✅ **Comprehensive Audit Trail** for all entities
- ✅ **Soft Delete** functionality
- ✅ **Rate Limiting** for API protection
- ✅ **Response Caching** for performance optimization
- ✅ **Redis Integration** for distributed caching
- ✅ **Swagger/OpenAPI** documentation
- ✅ **Structured Logging** with NLog
- ✅ **FluentValidation** for input validation
- ✅ **AutoMapper** for object mapping

### Technology Stack

#### Backend
- **.NET 9** - Latest .NET framework
- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - ORM for data access
- **MediatR** - Mediator pattern for CQRS
- **AutoMapper** - Object-to-object mapping
- **FluentValidation** - Input validation
- **NLog** - Structured logging

#### Database & Caching
- **SQL Server** - Primary database
- **Redis** - Distributed caching

#### Security
- **JWT Bearer** - Token-based authentication
- **ASP.NET Core Identity** - User management
- **Role-based Authorization** - Access control

#### API & Documentation
- **Swagger/OpenAPI** - API documentation
- **Versioning** - API versioning support
- **CORS** - Cross-origin resource sharing

## Project Structure

```
src/
├── apis/
│   └── Main/                    # API entry point
├── Core/
│   ├── Abstraction/            # Contracts and interfaces
│   ├── Application/            # Application layer (CQRS)
│   ├── Domain/                 # Domain entities and logic
│   └── Resources/              # Localization resources
└── Infrastructure/
    ├── Infrastructure/         # Data access and services
    └── Presentation/           # Controllers and middleware
```

## API Endpoints Overview

### Authentication
- `POST /api/Users/Authentication/Sign-In` - User login
- `POST /api/Users/Authentication/Refresh-Token` - Token refresh
- `POST /api/Users/Authentication/Validate-Token` - Token validation

### User Management
- `GET /api/Users/UserManagement/List` - List users
- `GET /api/Users/UserManagement/GetUserById` - Get user details
- `POST /api/Users/UserManagement/AddUser` - Create user
- `PUT /api/Users/UserManagement/UpdateUser` - Update user
- `POST /api/Users/UserManagement/ChangePassword` - Change password

### Authorization
- `GET /api/Users/Authorization/List` - List roles
- `GET /api/Users/Authorization/{id}` - Get role details
- `POST /api/Users/Authorization/AddRole` - Create role
- `PUT /api/Users/Authorization/EditRole` - Update role
- `GET /api/Users/Authorization/ClaimList` - List claims

### Catalog Management
- `GET /api/Catalog/Product/List` - List products
- `GET /api/Catalog/Product/{id}` - Get product details
- `POST /api/Catalog/Product` - Create product
- `PUT /api/Catalog/Product` - Update product
- `GET /api/Catalog/Category/List` - List categories
- `GET /api/Catalog/Category/{id}` - Get category details
- `POST /api/Catalog/Category` - Create category

## Security Model

### Roles
- **superadmin**: Full system access
- **admin**: Administrative access to catalog management
- **basic**: Basic user access with read permissions

### Claims-Based Authorization
The system uses claims-based authorization for fine-grained access control:
- Product permissions: List, View, Create, Update, Delete
- Category permissions: List, View, Create, Update, Delete
- User management permissions: List, Create, Update, Delete

## Development Guidelines

### Adding New Features
1. Define domain entities in the Domain layer
2. Create commands/queries in the Application layer
3. Implement handlers with proper validation
4. Add controllers in the Presentation layer
5. Configure dependencies and routing

### Database Changes
1. Update domain entities
2. Add Entity Framework migration
3. Update database schema documentation
4. Test migration in development environment

### API Versioning
The API supports versioning through URL path versioning:
- Current version: v2
- Swagger endpoint: `/swagger/v2/swagger.json`

## Monitoring and Logging

### Logging Levels
- **Trace**: Detailed diagnostic information
- **Debug**: Development debugging information
- **Info**: General application flow
- **Warn**: Potentially harmful situations
- **Error**: Error events that allow application to continue
- **Fatal**: Critical errors that may cause application termination

### Log Categories
- Authentication and authorization events
- API request/response logging
- Database operations
- Error tracking and exception handling
- Performance metrics

## Contributing

When contributing to this project:
1. Follow Clean Architecture principles
2. Implement proper error handling
3. Add comprehensive logging
4. Include unit tests for new features
5. Update documentation as needed
6. Follow the established coding standards

## Support

For questions or issues related to the architecture or implementation:
1. Review the architecture documentation
2. Check the database schema documentation
3. Examine existing code patterns
4. Follow the established conventions

---

*This documentation is maintained alongside the codebase and should be updated when architectural changes are made.*
