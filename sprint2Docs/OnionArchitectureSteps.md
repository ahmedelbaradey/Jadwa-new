# Jadwa Fund Management System - Onion Architecture Implementation Guide

## Table of Contents
1. [Overview](#overview)
2. [Architecture Pattern](#architecture-pattern)
3. [High-Level Architecture](#high-level-architecture)
4. [Layer Responsibilities](#layer-responsibilities)
5. [Prerequisites](#prerequisites)
6. [Implementation Steps](#implementation-steps)
7. [Best Practices](#best-practices)
8. [Troubleshooting](#troubleshooting)

## Overview

This document provides a comprehensive step-by-step template for implementing new features in the Jadwa Fund Management System using **Onion Architecture** principles with a service-based approach. This template is based on the existing DemoEntity implementation and follows the established patterns in the codebase.

The Jadwa system follows **Clean Architecture** principles with clear separation of concerns across multiple layers, ensuring maintainability, testability, and scalability for fund management operations.

## Architecture Pattern

The Jadwa API uses a hybrid approach combining:
- **Onion Architecture** principles for dependency inversion and layer separation
- **Service-based pattern** for business logic (not CQRS)
- **Repository pattern** with generic implementation for data access
- **DTO pattern** for data transfer and API contracts
- **AutoMapper** for object mapping between layers
- **FluentValidation** for comprehensive input validation
- **Entity Framework Core** with audit trail capabilities
- **State Pattern** for fund lifecycle management
- **Facade Pattern** with IRepositoryManager and ServiceManager

## High-Level Architecture

The system follows a layered architecture with clear dependency inversion:

```
┌─────────────────────────────────────────────────────────────┐
│                    Presentation Layer                       │
│              (Web API Controllers + Middleware)             │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                    Application Layer                        │
│           (Services + DTOs + Validation + Mapping)          │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                      Domain Layer                           │
│        (Entities + Value Objects + Business Rules)          │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                  Infrastructure Layer                       │
│     (Data Access + External Services + Identity + Cache)    │
└─────────────────────────────────────────────────────────────┘
```

## Layer Responsibilities

### 1. Presentation Layer (`src/Infrastructure/Presentation`)
**Purpose**: Handle HTTP requests, responses, and API contracts
- **Controllers**: RESTful API endpoints organized by domain
- **Middleware Pipeline**:
  - Error handling and exception management
  - Authentication and authorization
  - Request/response validation
  - Rate limiting and throttling
  - Logging and monitoring
- **Base Classes**: Common controller functionality and response patterns
- **API Documentation**: Swagger/OpenAPI specifications

### 2. Application Layer (`src/Core/Application`)
**Purpose**: Orchestrate business operations and coordinate between layers
- **Service Implementations**: Business logic and workflow orchestration
- **DTOs (Data Transfer Objects)**: API contracts and data structures
- **Validation**: FluentValidation with localized error messages
- **Mapping Profiles**: AutoMapper configurations for object transformations
- **Service Manager**: Facade pattern for service coordination
- **Business Logic**: Application-specific business rules and validations

### 3. Domain Layer (`src/Core/Domain`)
**Purpose**: Core business entities and domain logic
- **Entities**: Rich domain models with business behavior
  - Fund entities with comprehensive lifecycle management
  - User and role entities for access control
  - Audit entities for tracking changes
- **Value Objects**: Immutable objects representing domain concepts
- **State Patterns**: Fund lifecycle and status management
- **Business Rules**: Core domain validation and business constraints
- **Domain Services**: Complex business logic that doesn't belong to entities

### 4. Infrastructure Layer (`src/Infrastructure/Infrastructure`)
**Purpose**: External concerns and technical implementations
- **Data Access**:
  - Entity Framework Core with DbContext
  - Generic repository pattern implementation
  - Database migrations and configurations
- **External Services**:
  - Firebase FCM for push notifications
  - Third-party API integrations
  - Email and SMS services
- **Identity Services**:
  - Authentication and authorization
  - JWT token management
  - Role-based access control
- **Caching**: Redis implementation for performance optimization

## Prerequisites

Before starting implementation, ensure you understand:

### Technical Knowledge Required
- **Onion Architecture** principles and dependency inversion
- **Service-based architecture** patterns vs CQRS
- **Repository pattern** with generic implementation and facade pattern
- **FluentValidation** for comprehensive input validation with localization
- **AutoMapper** for object mapping between layers
- **Entity Framework Core** with audit trail and change tracking
- **State Pattern** for managing entity lifecycles
- **C# Naming Conventions** following .NET standards

### Development Environment
- **.NET 8.0** or later
- **Entity Framework Core** 8.0+
- **SQL Server** for data persistence
- **Redis** for caching (optional but recommended)
- **Visual Studio** or **VS Code** with C# extensions
- **Postman** or similar for API testing

### Project Structure Understanding
- Understand the existing **DemoEntity** implementation as reference
- Familiarize with the **ServiceManager** and **IRepositoryManager** patterns
- Review existing **validation** and **localization** implementations
- Understand the **audit trail** system with FullAuditedEntity

### Business Domain Knowledge
- **Fund Management** concepts and workflows
- **User roles** and permissions (Fund Manager, Legal Council, Board Secretary, Board Member)
- **Notification system** requirements
- **Localization** requirements (Arabic/English support)

## Implementation Steps

### Phase 0: Project Setup and Template Execution

#### Step 0.1: Execute ArabDt.Template Commands
**Purpose**: Generate boilerplate code using the project's custom templates

**Prerequisites**:
- Ensure you're in the project root directory
- Verify ArabDt.Template is installed and configured
- Have entity name and domain area identified

**Commands**:
1. Open Terminal
2. Navigate to the `src` directory
3. Execute the template command:
```bash
dotnet new oa -fn {EntityName}s -rt {EntityName} -ce false
```

**Parameters Explanation**:
- `-fn`: Feature name (usually plural form of entity)
- `-rt`: Root type (entity name)
- `-ce`: Create entity (true/false)

**Expected Output**:
- Domain entity class generated
- Service interface and implementation scaffolded
- DTO and validation classes created
- Basic controller structure generated

### Phase 1: Domain Layer Implementation

#### Step 1.1: Review and Enhance Generated Domain Entity
**Location**: `src/Core/Domain/Entities/{DomainArea}/{EntityName}.cs`

**Purpose**: Create or enhance the core domain entity with rich business behavior

**Check if entity already exists**:
- If entity exists, review and enhance following the patterns below
- If entity doesn't exist, create following the comprehensive template

**Entity Template (Based on Stories.md and TASK.md Requirements)**:
```csharp
using Domain.Entities.Base;

namespace Domain.Entities
{
    /// <summary>
    /// Represents a {EntityName} entity for {functionality} management
    /// Inherits from FullAuditedEntity to provide comprehensive audit trail functionality
    /// Properties are defined based on requirements in Stories.md and TASK.md
    /// </summary>
    public class {EntityName} : FullAuditedEntity
    {
        /// <summary>
        /// Arabic name of the {EntityName}
        /// Required field for entity identification
        /// </summary>
        public string NameAr { get; set; } = string.Empty;

        /// <summary>
        /// English name of the {EntityName}
        /// Required field for entity identification
        /// </summary>
        public string NameEn { get; set; } = string.Empty;

        /// <summary>
        /// Arabic description providing detailed information
        /// Optional field for additional context
        /// </summary>
        public string? DescriptionAr { get; set; }

        /// <summary>
        /// English description providing detailed information
        /// Optional field for additional context
        /// </summary>
        public string? DescriptionEn { get; set; }

        // Add additional properties only if specified in Stories.md or TASK.md
        // Do not add navigation properties, business methods, or computed properties
        // Keep the entity focused on core domain data only
    }
}
```

**Additional Properties Guidelines**:
**IMPORTANT**: Only add properties that are explicitly specified in Stories.md and TASK.md documentation.

**Before adding any property, verify it exists in**:
1. **Stories.md** - Check the user story requirements and acceptance criteria
2. **TASK.md** - Review the technical details and acceptance criteria for the specific entity

**Common property patterns (only add if specified in requirements)**:

```csharp
// Only add if specified in Stories.md/TASK.md
// For entities that need unique codes or identifiers
public string Code { get; set; } = string.Empty;

// Only add if specified in Stories.md/TASK.md
// For entities with status management
public {EntityName}Status Status { get; set; } = {EntityName}Status.Draft;

// Only add if specified in Stories.md/TASK.md
// For entities with parent-child relationships
public int? Parent{EntityName}Id { get; set; }

// Only add if specified in Stories.md/TASK.md
// For entities with user ownership
public string UserId { get; set; } = string.Empty;

// Only add if specified in Stories.md/TASK.md
// For entities with date ranges
public DateTime StartDate { get; set; }
public DateTime? EndDate { get; set; }

// Only add if specified in Stories.md/TASK.md
// For entities with ordering/display
public bool IsActive { get; set; } = true;
public int DisplayOrder { get; set; } = 0;
```

**What NOT to add to entities**:
- Business methods (these belong in services)
- Computed properties (these belong in DTOs)
- Localization methods (these belong in services or DTOs)
- State management methods (these belong in services)

**Navigation Properties Guidelines**:
**IMPORTANT**: Navigation properties SHOULD be added to domain entities when relationships exist.

**Why navigation properties should be included in entities**:
- Enables Entity Framework to understand and manage relationships
- Allows for proper lazy loading and eager loading of related data
- Simplifies querying related entities through LINQ
- Follows Entity Framework conventions and best practices
- Enables proper foreign key constraint generation

**Navigation Properties Template**:
Add these navigation properties based on your entity relationships specified in Stories.md and TASK.md:

```csharp
// One-to-Many relationship (Parent side)
/// <summary>
/// Collection of child {RelatedEntity} entities
/// Represents all related entities belonging to this {EntityName}
/// </summary>
public virtual ICollection<{RelatedEntity}> {RelatedEntity}s { get; set; } = new List<{RelatedEntity}>();

// Many-to-One relationship (Child side)
/// <summary>
/// Parent {ParentEntity} entity identifier
/// Foreign key reference to the parent entity
/// </summary>
public int {ParentEntity}Id { get; set; }

/// <summary>
/// Parent {ParentEntity} entity navigation property
/// Provides access to the parent entity data
/// </summary>
public virtual {ParentEntity} {ParentEntity} { get; set; } = null!;

// Many-to-Many relationship
/// <summary>
/// Collection of associated {RelatedEntity} entities
/// Represents many-to-many relationship through junction table
/// </summary>
public virtual ICollection<{RelatedEntity}> {RelatedEntity}s { get; set; } = new List<{RelatedEntity}>();

// Self-referencing relationship (Hierarchical)
/// <summary>
/// Parent {EntityName} navigation property
/// Used for hierarchical structures (categories, organizational units, etc.)
/// </summary>
public virtual {EntityName}? Parent{EntityName} { get; set; }

/// <summary>
/// Collection of child {EntityName} entities
/// Represents all direct children in the hierarchy
/// </summary>
public virtual ICollection<{EntityName}> Child{EntityName}s { get; set; } = new List<{EntityName}>();

// User relationship (for entities with ownership)
/// <summary>
/// User navigation property
/// Provides access to the user who owns or manages this entity
/// </summary>
public virtual ApplicationUser User { get; set; } = null!;
```

**Repository Method Example** (for handling relationships):
```csharp
// In repository methods - use Include for eager loading
public async Task<{EntityName}> GetWithRelatedDataAsync(int id)
{
    return await _context.{EntityName}s
        .Include(e => e.RelatedEntities)
        .Include(e => e.ParentEntity)
        .FirstOrDefaultAsync(e => e.Id == id);
}
```
```

**Key Points**:
- **Inheritance**: Always inherit from `FullAuditedEntity` for comprehensive audit trail
- **Requirements-Driven**: Only add properties explicitly specified in Stories.md and TASK.md
- **Localization**: Use Arabic/English naming convention (NameAr/NameEn, DescriptionAr/DescriptionEn)
- **Documentation**: Add comprehensive XML documentation for all properties
- **No Data Annotations**: Avoid data annotations in domain entities; Entity Framework uses conventions
- **Naming**: Follow PascalCase for properties, use meaningful descriptive names
- **Navigation Properties**: Add navigation properties for relationships specified in requirements
- **Virtual Properties**: Use virtual keyword for navigation properties to enable lazy loading
- **Entity Framework Conventions**: Rely on EF conventions, no custom configurations needed
- **No Business Methods**: Keep business logic in services, not in entities
- **No Computed Properties**: Put computed logic in DTOs or services
- **Simplicity**: Entities should contain core domain data and navigation properties only
- **Verification**: Always verify requirements in documentation before adding properties

#### Step 1.2: Add Navigation Properties (If Required)
**Purpose**: Add navigation properties for entity relationships as specified in Stories.md and TASK.md

**Important**: Only add navigation properties if relationships are explicitly mentioned in the requirements documentation.

**Example**: For FundStrategy entity, if Stories.md mentions that funds are associated with strategies, add:
```csharp
// Add to FundStrategy entity only if specified in requirements
/// <summary>
/// Collection of funds that use this strategy
/// Navigation property for Fund entities
/// </summary>
public virtual ICollection<Fund> Funds { get; set; } = new List<Fund>();
```

**Note**: Entity Framework will automatically handle:
- Database table creation
- Foreign key constraints
- Index generation
- Relationship mapping

**No Entity Framework Configuration Required**: The project uses Entity Framework conventions and does not implement custom configurations.

### Phase 2: Infrastructure Layer - Data Transfer Objects

#### Step 2.1: Create Comprehensive DTO
**Location**: `src/Infrastructure/Infrastructure/Dto/{EntityName}/{EntityName}Dto.cs`

**Purpose**: Define API contracts and data structures for client communication

**Enhanced DTO Template**:
```csharp
using Abstraction.Base.Dto;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Dto.{EntityName}
{
    /// <summary>
    /// Data Transfer Object for {EntityName} entity
    /// Contains properties for {EntityName} management operations
    /// Follows the established DTO pattern in the project
    /// Excludes audit fields as they are handled automatically by the system
    /// </summary>
    public record {EntityName}Dto : BaseDto
    {
        /// <summary>
        /// Arabic name of the {EntityName}
        /// Required field for API operations
        /// </summary>
        [Display(Name = "Arabic Name")]
        public string NameAr { get; set; } = string.Empty;

        /// <summary>
        /// English name of the {EntityName}
        /// Required field for API operations
        /// </summary>
        [Display(Name = "English Name")]
        public string NameEn { get; set; } = string.Empty;

        /// <summary>
        /// Arabic description providing detailed information
        /// Optional field for additional context
        /// </summary>
        [Display(Name = "Arabic Description")]
        public string? DescriptionAr { get; set; }

        /// <summary>
        /// English description providing detailed information
        /// Optional field for additional context
        /// </summary>
        [Display(Name = "English Description")]
        public string? DescriptionEn { get; set; }

        /// <summary>
        /// Indicates whether the {EntityName} is currently active
        /// Used for enabling/disabling functionality
        /// </summary>
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Display order for sorting and presentation
        /// Lower values appear first in ordered lists
        /// </summary>
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; } = 0;

        // Additional computed properties for API responses
        /// <summary>
        /// Localized name based on current culture
        /// Computed property for display purposes
        /// </summary>
        public string LocalizedName =>
            System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("ar")
                ? NameAr
                : NameEn;

        /// <summary>
        /// Localized description based on current culture
        /// Computed property for display purposes
        /// </summary>
        public string? LocalizedDescription =>
            System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("ar")
                ? DescriptionAr
                : DescriptionEn;
    }
}
```

#### Step 2.2: Create AutoMapper Profile
**Location**: `src/Infrastructure/Infrastructure/Dto/{EntityName}/{EntityName}Profile.cs`

**Purpose**: Configure object mapping between Entity and DTO

**Template**:
```csharp
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Dto.{EntityName}
{
    /// <summary>
    /// AutoMapper profile for {EntityName} entity and DTO mapping
    /// Configures bidirectional mapping between domain entity and data transfer object
    /// Automatically registered by the AutoMapper configuration
    /// </summary>
    public class {EntityName}Profile : Profile
    {
        /// <summary>
        /// Initializes the mapping configuration for {EntityName}
        /// </summary>
        public {EntityName}Profile()
        {
            // Entity to DTO mapping
            CreateMap<{EntityName}, {EntityName}Dto>();

            // DTO to Entity mapping
            CreateMap<{EntityName}Dto, {EntityName}>()
                // Audit fields are handled automatically by AuditableDbContext
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedByUser, opt => opt.Ignore());

            // Additional mappings for related DTOs if needed
            // CreateMap<{EntityName}, {EntityName}SummaryDto>();
            // CreateMap<{EntityName}CreateDto, {EntityName}>();
        }
    }
}
```

#### Step 2.3: Create Validation Rules
**Location**: `src/Infrastructure/Infrastructure/Dto/{EntityName}/{EntityName}Validation.cs`

**Purpose**: Define validation rules based on Stories.md, TASK.md, and backend-development-plan.md requirements

**Enhanced Template with Requirements-Based Validation**:
```csharp
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Infrastructure.Dto.{EntityName}
{
    /// <summary>
    /// Validation rules for {EntityName} DTO
    /// Implements FluentValidation for comprehensive input validation
    /// Supports localization for Arabic and English error messages
    /// Validation rules based on Stories.md, TASK.md, and backend-development-plan.md
    /// </summary>
    public class {EntityName}Validation : AbstractValidator<{EntityName}Dto>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        /// <summary>
        /// Initializes a new instance of the {EntityName}Validation
        /// </summary>
        /// <param name="localizer">String localizer for error messages</param>
        public {EntityName}Validation(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
        }

        /// <summary>
        /// Applies validation rules for {EntityName} properties
        /// Rules are based on requirements specified in project documentation
        /// </summary>
        public void ApplyValidationsRules()
        {
            // Example validation rules for FundStrategy (based on TASK.md requirements)
            // Arabic name validation - Required field as per TASK.md
            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKey.EmptyNameValidation])
                .NotNull().WithMessage(_localizer[SharedResourcesKey.EmptyNameValidation])
                .MaximumLength(200).WithMessage(_localizer[SharedResourcesKey.MaxLengthValidation])
                .Matches(@"^[\u0600-\u06FF\u0750-\u077F\u08A0-\u08FF\uFB50-\uFDFF\uFE70-\uFEFF\s\d\-_()]+$")
                .WithMessage(_localizer[SharedResourcesKey.ArabicTextValidation]);

            // English name validation - Required field as per TASK.md
            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKey.EmptyNameValidation])
                .NotNull().WithMessage(_localizer[SharedResourcesKey.EmptyNameValidation])
                .MaximumLength(200).WithMessage(_localizer[SharedResourcesKey.MaxLengthValidation])
                .Matches(@"^[a-zA-Z\s\d\-_()]+$")
                .WithMessage(_localizer[SharedResourcesKey.EnglishTextValidation]);

            // Arabic description validation - Optional field as per TASK.md
            RuleFor(x => x.DescriptionAr)
                .MaximumLength(1000).WithMessage(_localizer[SharedResourcesKey.MaxLengthValidation])
                .Matches(@"^[\u0600-\u06FF\u0750-\u077F\u08A0-\u08FF\uFB50-\uFDFF\uFE70-\uFEFF\s\d\-_().,!?]+$")
                .WithMessage(_localizer[SharedResourcesKey.ArabicTextValidation])
                .When(x => !string.IsNullOrEmpty(x.DescriptionAr));

            // English description validation - Optional field as per TASK.md
            RuleFor(x => x.DescriptionEn)
                .MaximumLength(1000).WithMessage(_localizer[SharedResourcesKey.MaxLengthValidation])
                .Matches(@"^[a-zA-Z\s\d\-_().,!?]+$")
                .WithMessage(_localizer[SharedResourcesKey.EnglishTextValidation])
                .When(x => !string.IsNullOrEmpty(x.DescriptionEn));

            // Custom business rule validation (based on backend-development-plan.md)
            // Duplicate name validation - as mentioned in validation requirements
            RuleFor(x => x)
                .MustAsync(async (dto, cancellation) => await ValidateUniqueNamesAsync(dto))
                .WithMessage(_localizer[SharedResourcesKey.DuplicateNameValidation])
                .WithName("Names");

            // Add additional validation rules based on specific entity requirements
            // Check Stories.md, TASK.md, and backend-development-plan.md for entity-specific rules
        }

        /// <summary>
        /// Custom validation method for unique name validation
        /// Implements business rule from backend-development-plan.md
        /// </summary>
        /// <param name="dto">The DTO to validate</param>
        /// <returns>True if names are unique, false otherwise</returns>
        private async Task<bool> ValidateUniqueNamesAsync({EntityName}Dto dto)
        {
            // Implement unique name validation logic here
            // This should check against existing entities in the database
            // Return true for now - implement actual logic in service layer
            return await Task.FromResult(true);
        }
    }
}
```

**Key Validation Guidelines**:
- **Requirements-Based**: Only add validation rules specified in Stories.md, TASK.md, or backend-development-plan.md
- **Localization**: All error messages must use localized resource keys
- **Arabic/English Support**: Separate validation patterns for Arabic and English text
- **Business Rules**: Implement custom validators for business-specific rules
- **Conditional Validation**: Use `.When()` for optional field validation
- **Async Validation**: Use `MustAsync()` for database-dependent validation rules

### Phase 3: Application Layer - Service Implementation


#### Step 3.1: Create Service Implementation
**Location**: `src/Infrastructure/Infrastructure/Service/{DomainArea}/{EntityName}Service.cs`

**Template**:
```csharp
using Domain.Entities.{DomainArea};
using AutoMapper;
using Abstraction.Contracts.Service.{DomainArea};
using Abstraction.Contracts.Repository;
using Infrastructure.Service;

namespace Infrastructure.Onion.Service.{DomainArea}
{
    /// <summary>
    /// Service implementation for {EntityName} operations
    /// Inherits from BaseService to provide standard CRUD operations
    /// Follows the established service pattern in the project
    /// </summary>
    public class {EntityName}Service : BaseService<{EntityName}>, I{EntityName}Service
    {
        /// <summary>
        /// Initializes a new instance of the {EntityName}Service
        /// </summary>
        /// <param name="repository">Generic repository for data access</param>
        /// <param name="mapper">AutoMapper for object mapping</param>
        public {EntityName}Service(IGenericRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
        }

        // Additional {EntityName}-specific methods can be implemented here if needed
        // For example:

        // public async Task<BaseResponse<bool>> IsNameExistsAsync(string nameAr, string nameEn, int? excludeId = null)
        // {
        //     try
        //     {
        //         var query = _repository.GetByCondition<{EntityName}>(entity =>
        //             (entity.NameAr == nameAr || entity.NameEn == nameEn) &&
        //             (!entity.IsDeleted.HasValue || !entity.IsDeleted.Value), false);
        //
        //         if (excludeId.HasValue)
        //         {
        //             query = query.Where(entity => entity.Id != excludeId.Value);
        //         }
        //
        //         var exists = await query.AnyAsync();
        //         return Success(exists);
        //     }
        //     catch (Exception ex)
        //     {
        //         return ServerError<bool>(ex.Message);
        //     }
        // }
    }
}
```

**Key Points**:
- Service must be in `Infrastructure.Onion.Service` namespace for automatic registration
- Inherit from `BaseService<TEntity>` to get standard CRUD operations
- Implement the service interface
- Use dependency injection for repository and mapper

### Phase 4: Service Manager Registration

#### Step 4.1: Update Service Manager Interface
**Location**: `src/Core/Abstraction/Contract/Service/IServiceManager.cs`

**Add to interface**:
```csharp
I{EntityName}Service {EntityName}Service { get; }
```

#### Step 4.2: Update Service Manager Implementation
**Location**: `src/Infrastructure/Infrastructure/Service/ServiceManager.cs`

**Add using statement**:
```csharp
using Abstraction.Contracts.Service.{DomainArea};
using Infrastructure.Onion.Service.{DomainArea};
```

**Add field**:
```csharp
private readonly Lazy<I{EntityName}Service> _{entityName}Service;
```

**Add to constructor**:
```csharp
_{entityName}Service = new Lazy<I{EntityName}Service>(() => new {EntityName}Service(_repository, _mapper));
```

**Add property**:
```csharp
public I{EntityName}Service {EntityName}Service => _{entityName}Service.Value;
```

## Implementation Checklist

### Phase 0: Template Execution
- [ ] ArabDt.Template command executed successfully (`dotnet new oa -fn {EntityName}s -rt {EntityName} -ce true`)
- [ ] Generated files reviewed and verified

### Phase 1: Domain Layer
- [ ] Entity created with proper inheritance from FullAuditedEntity
- [ ] Properties added based on Stories.md and TASK.md requirements only
- [ ] Arabic/English naming convention followed (NameAr/NameEn, DescriptionAr/DescriptionEn)
- [ ] Navigation properties added only if specified in requirements
- [ ] Virtual keyword used for navigation properties
- [ ] No data annotations added to entity
- [ ] No business methods added to entity
- [ ] No computed properties added to entity
- [ ] Entity placed in correct namespace (Domain.Entities)

### Phase 2: Infrastructure Layer - DTOs
- [ ] DTO created inheriting from BaseDto
- [ ] DTO properties match entity properties (excluding audit fields)
- [ ] AutoMapper profile created with bidirectional mapping
- [ ] Audit fields properly ignored in DTO to Entity mapping
- [ ] Validation class created with FluentValidation
- [ ] Validation rules based on Stories.md, TASK.md, and backend-development-plan.md
- [ ] Arabic text validation patterns implemented
- [ ] English text validation patterns implemented
- [ ] Localized validation messages using resource keys
- [ ] Custom business rule validators implemented (if required)
- [ ] Conditional validation for optional fields
- [ ] Async validation for database-dependent rules (if required)

### Phase 3: Application Layer - Services
- [ ] Service interface created inheriting from IBaseService
- [ ] Service implementation created in Infrastructure.Onion.Service namespace
- [ ] Service inherits from BaseService<TEntity>
- [ ] Service constructor properly configured with repository and mapper injection
- [ ] Additional business methods implemented only if specified in requirements

### Phase 4: Service Manager Registration
- [ ] Service interface added to IServiceManager
- [ ] Service field added to ServiceManager implementation
- [ ] Service lazy initialization added to ServiceManager constructor
- [ ] Service property added to ServiceManager implementation
- [ ] Using statements updated in ServiceManager

### Phase 5: Presentation Layer (Generated by Template)
- [ ] Controller created inheriting from BaseController
- [ ] All CRUD endpoints implemented
- [ ] Proper HTTP status codes and response types
- [ ] Authorization attributes applied
- [ ] Swagger documentation attributes added

### Verification and Testing
- [ ] All generated files compile without errors
- [ ] Entity Framework can create/update database schema
- [ ] AutoMapper mappings work correctly
- [ ] Validation rules work as expected
- [ ] Service registration works correctly
- [ ] API endpoints respond correctly
- [ ] Localization works for validation messages

## Best Practices and Guidelines

### Naming Conventions
- Use PascalCase for classes, methods, and properties
- Use camelCase for parameters and local variables
- Use underscore prefix for private fields (_field)
- Follow established naming patterns in the project

### Service Implementation
- Always place services in `Infrastructure.Onion.Service` namespace for automatic registration
- Inherit from `BaseService<TEntity>` to get standard CRUD operations
- Implement the corresponding service interface
- Use dependency injection for repository and mapper

### Error Handling
- Services inherit error handling from BaseService
- Controllers use BaseController.NewResult() for consistent responses
- Use localized error messages where applicable

This comprehensive template provides a complete guide for implementing new features in the Jadwa API using Onion Architecture with service-based approach. Follow each step carefully and refer to the existing DemoEntity implementation for additional guidance.
