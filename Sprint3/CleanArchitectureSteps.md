# Jadwa Fund Management System - Clean Architecture Implementation Guide

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

This document provides a comprehensive step-by-step template for implementing new features in the Jadwa Fund Management System using **Clean Architecture** principles with **CQRS pattern**. This template is based on the existing Categories implementation in `Application > Features > Catalog > Categories` and follows the established patterns in the codebase.

The Jadwa system follows **Clean Architecture** principles with clear separation of concerns across multiple layers, ensuring maintainability, testability, and scalability for fund management operations.

## Architecture Pattern

The Jadwa API uses Clean Architecture with:
- **CQRS Pattern** with MediatR for command/query separation
- **Repository pattern** with facade (IRepositoryManager) for data access
- **DTO pattern** for data transfer and API contracts
- **AutoMapper** for object mapping between layers
- **FluentValidation** with pipeline behaviors for comprehensive input validation
- **Entity Framework Core** with audit trail capabilities
- **Facade Pattern** with IRepositoryManager for repository coordination

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
│        (CQRS + DTOs + Validation + Mapping + Features)     │
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
- **Base Classes**: Common controller functionality and response patterns
- **API Documentation**: Swagger/OpenAPI specifications

### 2. Application Layer (`src/Core/Application`)
**Purpose**: Orchestrate business operations using CQRS pattern
- **Features**: Organized by domain areas (e.g., `Features/Catalog/Categories`)
- **Commands**: Write operations with command handlers
- **Queries**: Read operations with query handlers
- **DTOs**: API contracts and data structures
- **Validation**: FluentValidation with localized error messages
- **Mapping Profiles**: AutoMapper configurations for object transformations

### 3. Domain Layer (`src/Core/Domain`)
**Purpose**: Core business entities and domain logic
- **Entities**: Rich domain models with business behavior
- **Value Objects**: Immutable objects representing domain concepts
- **Business Rules**: Core domain validation and business constraints

### 4. Infrastructure Layer (`src/Infrastructure/Infrastructure`)
**Purpose**: External concerns and technical implementations
- **Data Access**: Entity Framework Core with DbContext and Repository pattern
- **External Services**: Third-party API integrations
- **Identity Services**: Authentication and authorization

## Prerequisites

Before starting implementation, ensure you understand:

### Technical Knowledge Required
- **Clean Architecture** principles and dependency inversion
- **CQRS pattern** with MediatR for command/query separation
- **Repository pattern** with facade (IRepositoryManager) implementation
- **FluentValidation** with pipeline behaviors for comprehensive input validation
- **AutoMapper** for object mapping between layers
- **Entity Framework Core** with audit trail and change tracking
- **C# Naming Conventions** following .NET standards

### Development Environment
- **.NET 8.0** or later
- **Entity Framework Core** 8.0+
- **SQL Server** for data persistence
- **Visual Studio** or **VS Code** with C# extensions
- **Postman** or similar for API testing

### Project Structure Understanding
- Understand the existing **Categories** implementation as reference in `Application > Features > Catalog > Categories`
- Familiarize with the **IRepositoryManager** facade pattern
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
dotnet new ca -fn {EntityName}s -rt {EntityName} -ft Repository -ce false
```

**Parameters Explanation**:
- `-fn`: Feature name (usually plural form of entity, e.g., "FundStrategies")
- `-rt`: Root type (entity name, e.g., "FundStrategy")
- `-ft`: The type of manager (Repository for Clean Architecture)
- `-ce`: Create entity (true/false)

**Expected Output**:
- Domain entity class generated in `src/Core/Domain/Entities`
- Application feature structure created in `src/Core/Application/Features`
- Repository interface and implementation scaffolded
- Basic CQRS commands and queries created
- DTO and validation classes created
- Basic controller structure generated

### Phase 1: Domain Layer Implementation

#### Step 1.1: Review and Enhance Generated Domain Entity
**Location**: `src/Core/Domain/Entities/{DomainArea}/{EntityName}.cs`

**Purpose**: Create or enhance the core domain entity with business properties based on documentation requirements

**IMPORTANT INSTRUCTIONS**:
- **Review Documentation First**: Always check Stories.md, TASK.md, and backend-development-plan.md for specific entity requirements
- **Property-Based Approach**: Add only properties explicitly mentioned in the documentation
- **No Static Names**: Do not use hardcoded property names like NameAr, NameEn, DescriptionAr, DescriptionEn unless specifically required
- **No Data Annotations**: Do not include data annotations in domain entities
- **Navigation Properties**: Add navigation properties directly to domain entities as specified in documentation

**Check if entity already exists**:
- If entity exists, review and enhance following the patterns below
- If entity doesn't exist, create following the comprehensive template

**Entity Template (Based on Documentation Requirements)**:
```csharp
using Domain.Entities.Base;

namespace Domain.Entities.{DomainArea}
{
    /// <summary>
    /// Represents a {EntityName} entity for {functionality} management
    /// Inherits from FullAuditedEntity to provide comprehensive audit trail functionality
    /// Properties are defined based on requirements in Stories.md, TASK.md, and backend-development-plan.md
    /// </summary>
    public class {EntityName} : FullAuditedEntity
    {
        // PROPERTY TEMPLATE - Add properties based on documentation requirements
        // Example property structure (replace with actual requirements):

        /// <summary>
        /// {Property description based on Stories.md/TASK.md requirements}
        /// {Required/Optional status based on documentation}
        /// </summary>
        public {PropertyType} {PropertyName} { get; set; } = {DefaultValue};

        // NAVIGATION PROPERTY TEMPLATE - Add navigation properties as specified
        // Example navigation property structure (replace with actual requirements):

        /// <summary>
        /// Navigation property to {RelatedEntity}
        /// {Relationship description based on documentation}
        /// </summary>
        public virtual {RelatedEntity}? {NavigationPropertyName} { get; set; }

        /// <summary>
        /// Collection navigation property to {RelatedEntity}
        /// {Relationship description based on documentation}
        /// </summary>
        public virtual ICollection<{RelatedEntity}> {CollectionPropertyName} { get; set; } = new List<{RelatedEntity}>();

        // IMPLEMENTATION GUIDELINES:
        // 1. Only add properties explicitly mentioned in Stories.md, TASK.md, or backend-development-plan.md
        // 2. Do not add business methods, computed properties, or validation logic
        // 3. Keep the entity focused on core domain data only
        // 4. Navigation properties should be added directly to entities (not in EF configurations)
        // 5. Use appropriate data types based on documentation requirements
        // 6. Follow C# naming conventions: PascalCase for properties
        // 7. Do not include data annotations - handle in EF configurations instead
    }
}
```
**Additional Properties Guidelines**:
**IMPORTANT**: Only add properties that are explicitly specified in Stories.md and TASK.md documentation.

**Before adding any property, verify it exists in**:
1. **Stories.md** - Check the user story requirements and acceptance criteria
2. **TASK.md** - Review the technical details and acceptance criteria for the specific entity

**Common property patterns (only add if specified in requirements)**:
**Key Points**:
- Always inherit from `FullAuditedEntity` for audit trail
- Use Arabic/English naming convention for localized properties
- Add comprehensive XML documentation
- Follow naming conventions: PascalCase for properties
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
public virtual ICollection<{EntityName}> {EntityName}s { get; set; } = new List<{EntityName}>();

// One-to-One relationship (Parent side)
/// <summary>
/// Child {EntityName} navigation property
/// Represents child in the hierarchy
/// </summary>
[ForeignKey("{ChildEntityName}Id")]
public virtual {ChildEntityName} {ChildEntityName} { get; set; };

/// <summary>
/// Child Forign key reference
/// </summary>
public virtual int {ChildEntityName}Id { get; set; };

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

**Important DTO Guidelines**:
- **DO NOT** include audit fields (CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) in any DTO
- **ALWAYS** inherit response DTOs from the base {EntityName}Dto
- Audit fields are handled automatically by the AuditableDbContext interceptor
- DTOs should only contain business-relevant properties, not infrastructure concerns

#### Step 1.2: Create Domain Enums (if needed)
**Location**: `src/Core/Domain/Entities/{DomainArea}/{EntityName}Status.cs`

**Template**:
```csharp
using System.ComponentModel;

namespace Domain.Entities.{DomainArea}
{
    /// <summary>
    /// Enumeration representing the various statuses of {EntityName}
    /// </summary>
    public enum {EntityName}Status
    {
        /// <summary>
        /// Active status
        /// Arabic: نشط
        /// </summary>
        [Description("Active")]
        Active = 1,

        /// <summary>
        /// Inactive status
        /// Arabic: غير نشط
        /// </summary>
        [Description("Inactive")]
        Inactive = 2
    }
}
```

### Phase 2: Infrastructure Layer Implementation

#### Step 2.1: Create Repository Interface
**Location**: `src/Core/Abstraction/Contract/Repository/{DomainArea}/I{EntityName}Repository.cs`

**Template**:
```csharp
namespace Abstraction.Contracts.Repository.{DomainArea}
{
    /// <summary>
    /// Repository interface for {EntityName} entity operations
    /// Inherits from IGenericRepository to provide standard CRUD operations
    /// </summary>
    public interface I{EntityName}Repository : IGenericRepository
    {
        // Add specific repository methods if needed
        // Task<{EntityName}> GetByNameAsync(string name, bool trackChanges = false);
    }
}
```

#### Step 2.2: Create Repository Implementation
**Location**: `src/Infrastructure/Infrastructure/Repository/{DomainArea}/{EntityName}Repository.cs`

**Template**:
```csharp
using Abstraction.Contracts.Repository.{DomainArea};
using Infrastructure.Data;
using Infrastructure.Repository;

namespace Infrastructure.Repository.{DomainArea}
{
    /// <summary>
    /// Repository implementation for {EntityName} entity operations
    /// Provides data access functionality using Entity Framework Core
    /// </summary>
    public class {EntityName}Repository : GenericRepository, I{EntityName}Repository
    {
        public {EntityName}Repository(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }

        // Implement specific repository methods if needed
        // public async Task<{EntityName}> GetByNameAsync(string name, bool trackChanges = false)
        // {
        //     return await GetByCondition<{EntityName}>(x => x.NameEn == name || x.NameAr == name, trackChanges)
        //         .FirstOrDefaultAsync();
        // }
    }
}
```

#### Step 2.3: Update Repository Manager
**Location**: `src/Infrastructure/Infrastructure/Repository/RepositoryManager.cs`

**Add to existing ProductRepositoryManager class**:
```csharp
// Add field
private readonly Lazy<I{EntityName}Repository> _i{EntityName}Repository;

// Add to constructor
_i{EntityName}Repository = new Lazy<I{EntityName}Repository>(() => new {EntityName}Repository(repositoryContext));

// Add property
public I{EntityName}Repository {EntityName}s => _i{EntityName}Repository.Value;
```

#### Step 2.4: Update Repository Manager Interface
**Location**: `src/Core/Abstraction/Contract/Repository/IRepositoryManager.cs`

**Add property**:
```csharp
I{EntityName}Repository {EntityName}s { get; }
```

### Phase 3: Application Layer Implementation

#### Step 3.1: Create DTOs (Following Categories Pattern)
**Location**: `src/Core/Application/Features/{DomainArea}/{EntityName}s/Dtos/`

**CRITICAL DTO RULES** (Based on Categories Implementation):
- **NEVER** include audit fields (CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) in DTOs
- **ALWAYS** inherit response DTOs from the base {EntityName}Dto
- DTOs are for business data transfer, not infrastructure concerns
- Follow the exact pattern used in Categories implementation

**Base DTO** (`{EntityName}Dto.cs`) - Following Categories/CategoryDto.cs pattern:
```csharp
using Abstraction.Base.Dto;

namespace Application.Features.{DomainArea}.{EntityName}s.Dtos
{
    /// <summary>
    /// Base Data Transfer Object for {EntityName} entity
    /// Contains common properties shared across different {EntityName} operations
    /// IMPORTANT: Does not include audit fields - these are handled by the audit system
    /// Based on Categories/CategoryDto.cs pattern
    /// Properties should match the entity properties defined in Stories.md, TASK.md, and backend-development-plan.md
    /// </summary>
    public record {EntityName}Dto : BaseDto
    {
        // PROPERTY TEMPLATE - Add properties based on entity requirements
        // Example property structure (replace with actual requirements from documentation):

        /// <summary>
        /// {Property description based on Stories.md/TASK.md requirements}
        /// {Required/Optional status based on documentation}
        /// </summary>
        public {PropertyType} {PropertyName} { get; set; } = {DefaultValue};

        // IMPLEMENTATION GUIDELINES:
        // 1. Only add properties that exist in the corresponding domain entity
        // 2. Do not include navigation properties in DTOs
        // 3. Do not include audit fields (CreatedAt, CreatedBy, UpdatedAt, UpdatedBy)
        // 4. Match property names and types exactly with the domain entity
        // 5. Use appropriate default values for non-nullable properties
        // 6. Add comprehensive XML documentation for each property
    }
}
```

**Add DTO** (`Add{EntityName}Dto.cs`) - Following Categories/AddCategoryDto.cs pattern:
```csharp
namespace Application.Features.{DomainArea}.{EntityName}s.Dtos
{
    /// <summary>
    /// Data Transfer Object for adding new {EntityName}
    /// Inherits from {EntityName}Dto to include all base properties
    /// IMPORTANT: Does not include audit fields - these are set automatically
    /// Based on Categories/AddCategoryDto.cs pattern
    /// </summary>
    public record Add{EntityName}Dto : {EntityName}Dto
    {
        // Add specific properties for creation if needed
        // DO NOT ADD: CreatedAt, CreatedBy - these are set by the audit system
    }
}
```

**Edit DTO** (`Edit{EntityName}Dto.cs`) - Following Categories/EditCategoryDto.cs pattern:
```csharp
namespace Application.Features.{DomainArea}.{EntityName}s.Dtos
{
    /// <summary>
    /// Data Transfer Object for editing existing {EntityName}
    /// Inherits from {EntityName}Dto to include all base properties
    /// IMPORTANT: Does not include audit fields - these are set automatically
    /// Based on Categories/EditCategoryDto.cs pattern
    /// </summary>
    public record Edit{EntityName}Dto : {EntityName}Dto
    {
        // Add specific properties for editing if needed
        // DO NOT ADD: UpdatedAt, UpdatedBy - these are set by the audit system
    }
}
```

**Response DTO** (`Single{EntityName}Response.cs`) - Following Categories/SingleCategoryResponse.cs pattern:
```csharp
namespace Application.Features.{DomainArea}.{EntityName}s.Dtos
{
    /// <summary>
    /// Response Data Transfer Object for single {EntityName} operations
    /// Used for returning {EntityName} data in API responses
    /// IMPORTANT: Do not include audit fields (CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) in DTOs
    /// These fields are handled automatically by the audit system and should not be exposed in DTOs
    /// Based on Categories/SingleCategoryResponse.cs pattern
    /// </summary>
    public record Single{EntityName}Response : {EntityName}Dto
    {
        // Add additional response-specific properties if needed
        // DO NOT ADD: CreatedAt, CreatedBy, UpdatedAt, UpdatedBy - these are audit fields
    }
}
```

#### Step 3.2: Create Validation Classes (Following Categories Pattern)
**Location**: `src/Core/Application/Features/{DomainArea}/{EntityName}s/Validation/`

**Base Validation** (`BaseValidation.cs`) - Following Categories/BaseValidation.cs pattern:
```csharp
using Application.Features.{DomainArea}.{EntityName}s.Dtos;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.{DomainArea}.{EntityName}s.Validation
{
    /// <summary>
    /// Base validation rules for {EntityName} operations
    /// Contains common validation logic shared across different {EntityName} commands
    /// Based on Categories/BaseValidation.cs pattern
    /// Validation rules based on Stories.md, TASK.md, and backend-development-plan.md
    /// </summary>
    public class BaseValidation : AbstractValidator<{EntityName}Dto>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public BaseValidation(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
        }

        public void ApplyValidationsRules()
        {
            // ID validation (following Categories pattern)
            RuleFor(x => x.Id)
               .NotNull().WithMessage(_localizer[SharedResourcesKey.EmptyIdValidation]);

            // VALIDATION TEMPLATE - Add validation rules based on entity properties
            // Example validation structure (replace with actual requirements from documentation):

            // For required string properties:
            // RuleFor(x => x.{PropertyName})
            //     .NotEmpty().WithMessage(_localizer[SharedResourcesKey.EmptyValidation])
            //     .NotNull().WithMessage(_localizer[SharedResourcesKey.EmptyValidation])
            //     .MaximumLength({MaxLength}).WithMessage(_localizer[SharedResourcesKey.MaxLengthValidation]);

            // For optional string properties:
            // RuleFor(x => x.{PropertyName})
            //     .MaximumLength({MaxLength}).WithMessage(_localizer[SharedResourcesKey.MaxLengthValidation])
            //     .When(x => !string.IsNullOrEmpty(x.{PropertyName}));

            // For Arabic text properties (if applicable):
            // RuleFor(x => x.{PropertyName})
            //     .Matches(@"^[\u0600-\u06FF\u0750-\u077F\u08A0-\u08FF\uFB50-\uFDFF\uFE70-\uFEFF\s\d\-_().,!?]+$")
            //     .WithMessage(_localizer[SharedResourcesKey.ArabicTextValidation])
            //     .When(x => !string.IsNullOrEmpty(x.{PropertyName}));

            // For English text properties (if applicable):
            // RuleFor(x => x.{PropertyName})
            //     .Matches(@"^[a-zA-Z\s\d\-_().,!?]+$")
            //     .WithMessage(_localizer[SharedResourcesKey.EnglishTextValidation])
            //     .When(x => !string.IsNullOrEmpty(x.{PropertyName}));

            // For numeric properties:
            // RuleFor(x => x.{PropertyName})
            //     .GreaterThan(0).WithMessage(_localizer[SharedResourcesKey.GreaterThanValidation]);

            // For date properties:
            // RuleFor(x => x.{PropertyName})
            //     .NotEmpty().WithMessage(_localizer[SharedResourcesKey.EmptyValidation])
            //     .Must(BeValidDate).WithMessage(_localizer[SharedResourcesKey.InvalidDateValidation]);

            // For enum properties:
            // RuleFor(x => x.{PropertyName})
            //     .IsInEnum().WithMessage(_localizer[SharedResourcesKey.InvalidEnumValidation]);

            // IMPLEMENTATION GUIDELINES:
            // 1. Add validation rules based on entity properties defined in Stories.md, TASK.md, and backend-development-plan.md
            // 2. Use appropriate validation rules for each property type
            // 3. Include localized error messages using SharedResourcesKey
            // 4. Add conditional validation using When() for optional properties
            // 5. Use regex patterns for text format validation when specified
            // 6. Add business rule validation when required by documentation
        }
    }
}
```

**Add Validation** (`Add{EntityName}Validation.cs`) - Following Categories/AddCategoryValidation.cs pattern:
```csharp
using Application.Features.{DomainArea}.{EntityName}s.Commands.Add;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.{DomainArea}.{EntityName}s.Validation
{
    /// <summary>
    /// Validation rules for Add{EntityName}Command
    /// Inherits base validation rules and adds command-specific validations
    /// Based on Categories/AddCategoryValidation.cs pattern
    /// </summary>
    public class Add{EntityName}Validation : AbstractValidator<Add{EntityName}Command>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public Add{EntityName}Validation(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            Include(new BaseValidation(_localizer));
            // Add specific validation rules for Add command if needed
        }
    }
}
```

**Edit Validation** (`Edit{EntityName}Validation.cs`) - Following Categories/EditCategoryValidation.cs pattern:
```csharp
using Application.Features.{DomainArea}.{EntityName}s.Commands.Edit;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.{DomainArea}.{EntityName}s.Validation
{
    /// <summary>
    /// Validation rules for Edit{EntityName}Command
    /// Inherits base validation rules and adds command-specific validations
    /// Based on Categories/EditCategoryValidation.cs pattern
    /// </summary>
    public class Edit{EntityName}Validation : AbstractValidator<Edit{EntityName}Command>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public Edit{EntityName}Validation(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            Include(new BaseValidation(_localizer));
            // Add specific validation rules for Edit command if needed
        }
    }
}
```

#### Step 3.3: Create CQRS Commands (Following Categories Pattern)
**Location**: `src/Core/Application/Features/{DomainArea}/{EntityName}s/Commands/Add/`

**Add Command** (`Add{EntityName}Command.cs`) - Following Categories/Commands/Add/AddCategoryCommand.cs pattern:
```csharp
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.{DomainArea}.{EntityName}s.Dtos;

namespace Application.Features.{DomainArea}.{EntityName}s.Commands.Add
{
    /// <summary>
    /// Command for adding a new {EntityName}
    /// Implements ICommand interface for CQRS pattern integration with MediatR
    /// Based on Categories/Commands/Add/AddCategoryCommand.cs pattern
    /// </summary>
    public record Add{EntityName}Command : Add{EntityName}Dto, ICommand<BaseResponse<string>>
    {
        // Command inherits all properties from Add{EntityName}Dto
        // No additional properties needed unless specific to command execution
    }
}
```

**Add Command Handler** (`Add{EntityName}CommandHandler.cs`) - Following Categories/Commands/Add/AddCategoryCommandHandler.cs pattern:
```csharp
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using AutoMapper;
using Domain.Entities.{DomainArea};
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.{DomainArea}.{EntityName}s.Commands.Add
{
    /// <summary>
    /// Handler for Add{EntityName}Command
    /// Implements business logic for creating new {EntityName} entities
    /// Based on Categories/Commands/Add/AddCategoryCommandHandler.cs pattern
    /// </summary>
    public class Add{EntityName}CommandHandler : BaseResponseHandler, ICommandHandler<Add{EntityName}Command, BaseResponse<string>>
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public Add{EntityName}CommandHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger, IStringLocalizer<SharedResources> localizer)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
        }
        #endregion

        #region Handle Functions
        public async Task<BaseResponse<string>> Handle(Add{EntityName}Command request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                    return BadRequest<string>(_localizer[SharedResourcesKey.EmptyRequestValidation]);

                var entityMapper = _mapper.Map<{EntityName}>(request);

                var result = await _repository.{EntityName}s.AddAsync(entityMapper);
                if (result is null)
                    return BadRequest<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);

                return Success(_localizer[SharedResourcesKey.RecordSavedSuccessfully]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Add{EntityName}Command");
                return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }
        #endregion
    }
}
```

**Edit Command** (`Edit{EntityName}Command.cs`) - Following Categories/Commands/Edit/EditCategoryCommand.cs pattern:
```csharp
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.{DomainArea}.{EntityName}s.Dtos;

namespace Application.Features.{DomainArea}.{EntityName}s.Commands.Edit
{
    /// <summary>
    /// Command for editing an existing {EntityName}
    /// Implements ICommand interface for CQRS pattern integration with MediatR
    /// Based on Categories/Commands/Edit/EditCategoryCommand.cs pattern
    /// </summary>
    public record Edit{EntityName}Command : Edit{EntityName}Dto, ICommand<BaseResponse<string>>
    {
        // Command inherits all properties from Edit{EntityName}Dto
        // No additional properties needed unless specific to command execution
    }
}
```

**Edit Command Handler** (`Edit{EntityName}CommandHandler.cs`) - Following Categories/Commands/Edit/EditCategoryCommandHandler.cs pattern:
```csharp
using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using AutoMapper;
using Domain.Entities.{DomainArea};
using Abstraction.Contracts.Repository;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.{DomainArea}.{EntityName}s.Commands.Edit
{
    /// <summary>
    /// Handler for Edit{EntityName}Command
    /// Implements business logic for updating existing {EntityName} entities
    /// Based on Categories/Commands/Edit/EditCategoryCommandHandler.cs pattern
    /// </summary>
    public class Edit{EntityName}CommandHandler : BaseResponseHandler, ICommandHandler<Edit{EntityName}Command, BaseResponse<string>>
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public Edit{EntityName}CommandHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger, IStringLocalizer<SharedResources> localizer)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
        }
        #endregion

        #region Handle Functions
        public async Task<BaseResponse<string>> Handle(Edit{EntityName}Command request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                    return BadRequest<string>(_localizer[SharedResourcesKey.EmptyRequestValidation]);

                var originalEntity = await _repository.{EntityName}s.GetByIdAsync<{EntityName}>(request.Id, true);
                _mapper.Map(request, originalEntity);
                var status = await _repository.{EntityName}s.UpdateAsync(originalEntity);
                if (!status)
                    return BadRequest<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
                return Success(_localizer[SharedResourcesKey.RecordSavedSuccessfully]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in Edit{EntityName}Command");
                return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }
        #endregion
    }
}
```

**Delete Command** (`Delete{EntityName}Command.cs`) - Following Categories/Commands/Delete/DeleteCategoryCommand.cs pattern:
```csharp
using Abstraction.Base.Dto;
using Application.Base.Abstracts;
using Abstraction.Base.Response;

namespace Application.Features.{DomainArea}.{EntityName}s.Commands.Delete
{
    /// <summary>
    /// Command for deleting an existing {EntityName}
    /// Implements ICommand interface for CQRS pattern integration with MediatR
    /// Based on Categories/Commands/Delete/DeleteCategoryCommand.cs pattern
    /// </summary>
    public record Delete{EntityName}Command : BaseDto, ICommand<BaseResponse<string>>
    {
        // Command inherits Id property from BaseDto
        // No additional properties needed for delete operation
    }
}
```

#### Step 3.4: Create CQRS Queries (Following Categories Pattern)
**Location**: `src/Core/Application/Features/{DomainArea}/{EntityName}s/Queries/`

**List Query** (`Queries/List/ListQuery.cs`) - Following Categories/Queries/List/ListQuery.cs pattern:
```csharp
using Abstraction.Base.Dto;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Application.Features.{DomainArea}.{EntityName}s.Dtos;

namespace Application.Features.{DomainArea}.{EntityName}s.Queries.List
{
    /// <summary>
    /// Query for retrieving a paginated list of {EntityName}s
    /// Supports filtering, sorting, and pagination
    /// Based on Categories/Queries/List/ListQuery.cs pattern
    /// </summary>
    public record ListQuery : BaseListDto, IQuery<BaseResponse<PaginatedResult<Single{EntityName}Response>>>
    {
        // Inherits PageNumber, PageSize, Search, OrderBy from BaseListDto
        // Add specific query parameters if needed for additional filtering
    }
}
```

**Get Query** (`Queries/Get/GetQuery.cs`) - Following Categories/Queries/Get/GetQuery.cs pattern:
```csharp
using Abstraction.Base.Dto;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.{DomainArea}.{EntityName}s.Dtos;

namespace Application.Features.{DomainArea}.{EntityName}s.Queries.Get
{
    /// <summary>
    /// Query for retrieving a single {EntityName} by ID
    /// Based on Categories/Queries/Get/GetQuery.cs pattern
    /// </summary>
    public record GetQuery : BaseDto, IQuery<BaseResponse<Single{EntityName}Response>>
    {
        // Inherits Id property from BaseDto
        // No additional properties needed for get by ID operation
    }
}
```

**List Query Handler** (`Queries/List/ListQueryHandler.cs`) - Following Categories/Queries/List/ListQueryHandler.cs pattern:
```csharp
using AutoMapper;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Application.Features.{DomainArea}.{EntityName}s.Dtos;
using Domain.Entities.{DomainArea};
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.{DomainArea}.{EntityName}s.Queries.List
{
    /// <summary>
    /// Handler for ListQuery
    /// Implements business logic for retrieving paginated {EntityName} lists
    /// Based on Categories/Queries/List/ListQueryHandler.cs pattern
    /// </summary>
    public class ListQueryHandler : BaseResponseHandler, IQueryHandler<ListQuery, BaseResponse<PaginatedResult<Single{EntityName}Response>>>
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor(s)
        public ListQueryHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger, IStringLocalizer<SharedResources> localizer)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<PaginatedResult<Single{EntityName}Response>>> Handle(ListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = _repository.{EntityName}s.GetAll<Single{EntityName}Response>(false);

                if (!result.Any())
                {
                    return EmptyCollection(PaginatedResult<Single{EntityName}Response>.Success(
                        new List<Single{EntityName}Response>(), 0, 0, 0));
                }

                var {entityName}List = await _mapper.ProjectTo<Single{EntityName}Response>(result)
                    .ToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy);

                return Success({entityName}List);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in List{EntityName}Query");
                return ServerError<PaginatedResult<Single{EntityName}Response>>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }
        #endregion
    }
}
```

**Get Query Handler** (`Queries/Get/GetQueryHandler.cs`) - Following Categories/Queries/Get/GetQueryHandler.cs pattern:
```csharp
using AutoMapper;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.{DomainArea}.{EntityName}s.Dtos;
using Domain.Entities.{DomainArea};
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.{DomainArea}.{EntityName}s.Queries.Get
{
    /// <summary>
    /// Handler for GetQuery
    /// Implements business logic for retrieving single {EntityName} by ID
    /// Based on Categories/Queries/Get/GetQueryHandler.cs pattern
    /// </summary>
    public class GetQueryHandler : BaseResponseHandler, IQueryHandler<GetQuery, BaseResponse<Single{EntityName}Response>>
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor(s)
        public GetQueryHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger, IStringLocalizer<SharedResources> localizer)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<Single{EntityName}Response>> Handle(GetQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _repository.{EntityName}s.GetByIdAsync<{EntityName}>(request.Id, false);
                if (entity == null)
                    return NotFound<Single{EntityName}Response>($"{_localizer[SharedResourcesKey.DoesntExist]}");

                var entityMapper = _mapper.Map<Single{EntityName}Response>(entity);
                return Success(entityMapper);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get{EntityName}Query");
                return ServerError<Single{EntityName}Response>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }
        #endregion
    }
}
```

**Get Query** (`Queries/Get/Get{EntityName}Query.cs`):
```csharp
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.{DomainArea}.{EntityName}s.Dtos;

namespace Application.Features.{DomainArea}.{EntityName}s.Queries.Get
{
    /// <summary>
    /// Query for retrieving a single {EntityName} by ID
    /// </summary>
    public record Get{EntityName}Query : IQuery<BaseResponse<Single{EntityName}Response>>
    {
        public int Id { get; set; }
    }
}
```

**Get Query Handler** (`Queries/Get/Get{EntityName}QueryHandler.cs`):
```csharp
using AutoMapper;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.{DomainArea}.{EntityName}s.Dtos;
using Domain.Entities.{DomainArea};
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;

namespace Application.Features.{DomainArea}.{EntityName}s.Queries.Get
{
    /// <summary>
    /// Handler for Get{EntityName}Query
    /// Implements business logic for retrieving a single {EntityName} by ID
    /// </summary>
    public class Get{EntityName}QueryHandler : BaseResponseHandler, IQueryHandler<Get{EntityName}Query, BaseResponse<Single{EntityName}Response>>
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor(s)
        public Get{EntityName}QueryHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<Single{EntityName}Response>> Handle(Get{EntityName}Query request, CancellationToken cancellationToken)
        {
            try
            {
                var {entityName} = await _repository.{EntityName}s.GetByIdAsync<{EntityName}>(request.Id, false);
                if ({entityName} is null)
                    return NotFound<Single{EntityName}Response>("{EntityName} with this ID not found!");

                var {entityName}Mapper = _mapper.Map<Single{EntityName}Response>({entityName});
                return Success({entityName}Mapper);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get{EntityName}ByIdQuery");
                return ServerError<Single{EntityName}Response>(ex.Message);
            }
        }
        #endregion
    }
}
```

#### Step 3.5: Create AutoMapper Profiles (Following Categories Pattern)
**Location**: `src/Core/Application/Mapping/{EntityName}s/{EntityName}Profile.cs`

**Main Profile Class** (`{EntityName}Profile.cs`) - Following Categories/CategoriesProfile.cs pattern:
```csharp
using AutoMapper;

namespace Application.Mapping
{
    /// <summary>
    /// AutoMapper profile for {EntityName} entity mappings
    /// Defines mapping configurations between entities, DTOs, and commands
    /// Based on Categories/CategoriesProfile.cs pattern
    /// </summary>
    public partial class {EntityName}Profile : Profile
    {
        public {EntityName}Profile()
        {
            Add{EntityName}Mapping();
            Edit{EntityName}Mapping();
            Get{EntityName}Mapping();
        }
    }
}
```

**Add Mapping** (`Add{EntityName}Mapping.cs`) - Following Categories/AddCategoryMapping.cs pattern:
```csharp
using Application.Features.{DomainArea}.{EntityName}s.Commands.Add;
using Domain.Entities.{DomainArea};

namespace Application.Mapping
{
    /// <summary>
    /// AutoMapper configuration for Add{EntityName}Command to {EntityName} entity mapping
    /// Based on Categories/AddCategoryMapping.cs pattern
    /// </summary>
    public partial class {EntityName}Profile
    {
        public void Add{EntityName}Mapping()
        {
            CreateMap<Add{EntityName}Command, {EntityName}>();
        }
    }
}
```

**Edit Mapping** (`Edit{EntityName}Mapping.cs`) - Following Categories/EditCategoryMapping.cs pattern:
```csharp
using Application.Features.{DomainArea}.{EntityName}s.Commands.Edit;
using Domain.Entities.{DomainArea};

namespace Application.Mapping
{
    /// <summary>
    /// AutoMapper configuration for Edit{EntityName}Command to {EntityName} entity mapping
    /// Based on Categories/EditCategoryMapping.cs pattern
    /// </summary>
    public partial class {EntityName}Profile
    {
        public void Edit{EntityName}Mapping()
        {
            CreateMap<Edit{EntityName}Command, {EntityName}>();
        }
    }
}
```

**Get Mapping** (`Get{EntityName}Mapping.cs`) - Following Categories/GetCategoryMapping.cs pattern:
```csharp
using Application.Features.{DomainArea}.{EntityName}s.Dtos;
using Domain.Entities.{DomainArea};

namespace Application.Mapping
{
    /// <summary>
    /// AutoMapper configuration for {EntityName} entity to response DTO mapping
    /// Based on Categories/GetCategoryMapping.cs pattern
    /// </summary>
    public partial class {EntityName}Profile
    {
        public void Get{EntityName}Mapping()
        {
            CreateMap<{EntityName}, Single{EntityName}Response>();
        }
    }
}
```

**Key Points About Categories AutoMapper Implementation**:
- **Partial Classes**: Uses `partial class` pattern to split mappings across multiple files
- **Simple Mappings**: No complex ForMember configurations - relies on AutoMapper conventions
- **Separate Files**: Each mapping type (Add, Edit, Get) has its own file
- **Namespace**: All mapping files are in `Application.Mapping` namespace (not in Features folder)
- **No Audit Field Handling**: AutoMapper relies on conventions; audit fields are handled by the audit interceptor
- **No ReverseMap**: Only maps in the direction needed (Command → Entity, Entity → Response)
- **Auto-Registration**: Profiles are automatically registered via `services.AddAutoMapper(Assembly.GetExecutingAssembly())` in ApplicationDependencies

**Note**: Validation registration is handled automatically by the ValidationBehavior pipeline configured in ApplicationDependencies:
```csharp
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
```
No manual validator registration is required as FluentValidation validators are auto-discovered via Assembly scanning.

### Phase 4: Presentation Layer Implementation

#### Step 4.1: Create API Controller (Following Categories Pattern)
**Location**: `src/Infrastructure/Presentation/Controllers/{EntityName}sController.cs`

**API Controller** - Following Categories controller pattern:
```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Features.{DomainArea}.{EntityName}s.Commands.Edit;
using Application.Features.{DomainArea}.{EntityName}s.Commands.Add;
using Application.Features.{DomainArea}.{EntityName}s.Queries.List;
using Application.Features.{DomainArea}.{EntityName}s.Queries.Get;
using Application.Features.{DomainArea}.{EntityName}s.Commands.Delete;
using Presentation.Bases;
using Abstraction.Constants.ModulePermissions;
using Abstraction.Base.Response;
using Microsoft.AspNetCore.Http;
using Abstraction.Common.Wappers;
using Application.Features.{DomainArea}.{EntityName}s.Dtos;

namespace Presentation.Controllers
{
    /// <summary>
    /// API Controller for {EntityName} management operations
    /// Provides RESTful endpoints for CRUD operations
    /// Based on Categories controller pattern
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class {EntityName}Controller : AppControllerBase
    {
        /// <summary>
        /// Get paginated list of {EntityName}s with optional search and filtering
        /// </summary>
        /// <param name="query">List query parameters including pagination and search</param>
        /// <returns>Paginated list of {EntityName}s</returns>
        [Authorize(Policy = {EntityName}Permission.List)]
        [HttpGet("List")]
        [ProducesResponseType(typeof(PaginatedResult<Single{EntityName}Response>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get{EntityName}sPaginatedList([FromQuery] ListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        /// <summary>
        /// Get a single {EntityName} by ID
        /// </summary>
        /// <param name="id">{EntityName} ID</param>
        /// <returns>Single {EntityName} details</returns>
        [Authorize(Policy = {EntityName}Permission.View)]
        [HttpGet("GetById")]
        [ProducesResponseType(typeof(BaseResponse<Single{EntityName}Response>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get{EntityName}sById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetQuery() { Id = id });
            return NewResult(response);
        }

        /// <summary>
        /// Create a new {EntityName}
        /// </summary>
        /// <param name="command">Add {EntityName} command</param>
        /// <returns>Creation result</returns>
        [Authorize(Policy = {EntityName}Permission.Create)]
        [HttpPost("Add")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add{EntityName}([FromBody] Add{EntityName}Command command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        /// <summary>
        /// Update an existing {EntityName}
        /// </summary>
        /// <param name="command">Edit {EntityName} command</param>
        /// <returns>Update result</returns>
        [Authorize(Policy = {EntityName}Permission.Edit)]
        [HttpPut("Edit")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit{EntityName}([FromBody] Edit{EntityName}Command command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        /// <summary>
        /// Delete a {EntityName} by ID
        /// </summary>
        /// <param name="id">{EntityName} ID to delete</param>
        /// <returns>Deletion result</returns>
        [Authorize(Policy = {EntityName}Permission.Delete)]
        [HttpDelete("Delete")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete{EntityName}([FromRoute] int id)
        {
            var response = await Mediator.Send(new Delete{EntityName}Command() { Id = id });
            return NewResult(response);
        }
    }
}
```

### Phase 5: Database Configuration

#### Step 5.1: Update DbContext
**Location**: `src/Infrastructure/Infrastructure/Data/AppDbContext.cs`

**Add DbSet property**:
```csharp
public DbSet<{EntityName}> {EntityName}s { get; set; }
```

**Note**: Entity Framework configurations are not needed in this project as it uses Entity Framework conventions. The project relies on automatic table generation based on entity properties and navigation properties defined directly in the domain entities.


### Phase 6: Database Migration

#### Step 6.1: Create Migration
**Command**: Run in Package Manager Console or Terminal
```bash
# Navigate to the Infrastructure project directory
cd src/Infrastructure/Infrastructure

# Add migration
dotnet ef migrations add Add{EntityName}Entity --startup-project ../../Infrastructure/Presentation

# Update database
dotnet ef database update --startup-project ../../Infrastructure/Presentation
```

#### Step 6.2: Verify Migration
- Check that the migration file is created in `Migrations` folder
- Verify the migration includes table creation with proper columns
- Ensure indexes and constraints are properly configured
- Test the migration on development database

### Phase 7: Documentation and Deployment

#### Step 7.1: Update API Documentation
- Update Swagger documentation with new endpoints
- Add XML comments for all public methods
- Document request/response models
- Include example requests and responses

#### Step 7.2: Update Project Documentation
- Add feature to README.md
- Update architecture documentation if needed
- Document any new patterns or conventions used
- Update deployment scripts if necessary

## Implementation Checklist (Based on Categories Pattern)

### Phase 1: Domain Layer ✅
- [ ] **Step 1.1**: Review and enhance generated domain entity with properties based on Stories.md, TASK.md, and backend-development-plan.md
- [ ] **Step 1.2**: Add navigation properties for entity relationships as specified in documentation (directly in entities, not EF configurations)
- [ ] **Step 1.3**: Create domain enums if needed (e.g., {EntityName}Status)

### Phase 2: Infrastructure Layer ✅
- [ ] **Step 2.1**: Create repository interface extending IGenericRepository
- [ ] **Step 2.2**: Create repository implementation extending GenericRepository
- [ ] **Step 2.3**: Update RepositoryManager with new repository (lazy initialization pattern)
- [ ] **Step 2.4**: Update IRepositoryManager interface with new repository property

### Phase 3: Application Layer ✅
- [ ] **Step 3.1**: Create DTOs following Categories pattern:
  - [ ] Base{EntityName}Dto inheriting from BaseDto (only Id property)
  - [ ] Add{EntityName}Dto inheriting from {EntityName}Dto
  - [ ] Edit{EntityName}Dto inheriting from {EntityName}Dto
  - [ ] Single{EntityName}Response inheriting from {EntityName}Dto
  - [ ] **CRITICAL**: No audit fields in DTOs (handled by audit system)
- [ ] **Step 3.2**: Create validation classes following Categories pattern:
  - [ ] BaseValidation with common validation rules
  - [ ] Add{EntityName}Validation including BaseValidation
  - [ ] Edit{EntityName}Validation including BaseValidation
  - [ ] Use localized messages with SharedResourcesKey
- [ ] **Step 3.3**: Create CQRS commands following Categories pattern:
  - [ ] Add{EntityName}Command inheriting from Add{EntityName}Dto, implementing ICommand<BaseResponse<string>>
  - [ ] Edit{EntityName}Command inheriting from Edit{EntityName}Dto, implementing ICommand<BaseResponse<string>>
  - [ ] Delete{EntityName}Command inheriting from BaseDto, implementing ICommand<BaseResponse<string>>
  - [ ] Command handlers inheriting from BaseResponseHandler with proper error handling
- [ ] **Step 3.4**: Create CQRS queries following Categories pattern:
  - [ ] ListQuery inheriting from BaseListDto, implementing IQuery<BaseResponse<PaginatedResult<Single{EntityName}Response>>>
  - [ ] GetQuery inheriting from BaseDto, implementing IQuery<BaseResponse<Single{EntityName}Response>>
  - [ ] Query handlers inheriting from BaseResponseHandler with pagination support
- [ ] **Step 3.5**: Create AutoMapper profiles following Categories pattern:
  - [ ] Entity to DTO mappings with ReverseMap
  - [ ] Command to Entity mappings with audit field exclusions
  - [ ] Proper handling of navigation properties

### Phase 4: Presentation Layer ✅
- [ ] **Step 4.1**: Create API controller following Categories pattern:
  - [ ] Inherit from AppControllerBase (not BaseController)
  - [ ] Use MediatR pattern with Mediator.Send()
  - [ ] RESTful endpoints: GetList, GetById, Create, Edit, Delete
  - [ ] Proper route attributes and HTTP verbs
- [ ] **Step 4.2**: Add proper response handling:
  - [ ] Return Ok(response) for all operations
  - [ ] Use BaseResponse<T> pattern for consistent responses
  - [ ] Proper HTTP status codes handled by BaseResponseHandler
- [ ] **Step 4.3**: Add comprehensive Swagger documentation attributes

### Phase 5: Integration ✅
- [ ] **Step 5.1**: Verify automatic service registration:
  - [ ] MediatR handlers auto-registered via Assembly scanning
  - [ ] AutoMapper profiles auto-registered via Assembly scanning
  - [ ] FluentValidation validators auto-registered via Assembly scanning
- [ ] **Step 5.2**: Test ValidationBehavior pipeline integration
- [ ] **Step 5.3**: Test audit system integration (CreatedAt, UpdatedAt fields)
- [ ] **Step 5.4**: Test all endpoints with proper error handling and localization

### Testing (Following Categories Pattern)
- [ ] Unit tests for command handlers with BaseResponseHandler pattern
- [ ] Unit tests for query handlers with pagination testing
- [ ] Integration tests for API endpoints using AppControllerBase
- [ ] Validation testing with localized messages
- [ ] AutoMapper configuration testing

### Documentation
- [ ] XML comments added to all public methods
- [ ] API documentation updated with Categories pattern examples
- [ ] Project documentation updated with new entity relationships
- [ ] Migration documentation updated

## Best Practices and Guidelines

### Clean Architecture Principles
1. **Dependency Inversion**: Always depend on abstractions, not concretions
2. **Single Responsibility**: Each class should have one reason to change
3. **Open/Closed Principle**: Open for extension, closed for modification
4. **Interface Segregation**: Clients should not depend on interfaces they don't use
5. **Dependency Injection**: Use DI container for managing dependencies

### CQRS Best Practices
1. **Command/Query Separation**: Commands modify state, queries return data
2. **Immutable Commands/Queries**: Use records for immutable data structures
3. **Validation Pipeline**: Use FluentValidation with MediatR pipeline behaviors
4. **Error Handling**: Use Result pattern for consistent error handling
5. **Localization**: Always use localized error messages

### Repository Pattern Guidelines
1. **Facade Pattern**: Use IRepositoryManager for coordinating multiple repositories
2. **Generic Repository**: Leverage generic repository for common operations
3. **Specific Repositories**: Create specific repositories for complex queries
4. **Unit of Work**: Use SaveAsync() for transaction management
5. **Async Operations**: Always use async/await for database operations

### Validation Best Practices
1. **Localized Messages**: Use SharedResources for all validation messages
2. **Business Rules**: Implement business validation in domain layer
3. **Input Validation**: Use FluentValidation for input validation
4. **Regex Patterns**: Use appropriate regex for Arabic/English text validation
5. **Conditional Validation**: Use When() for conditional validation rules

### Mapping Guidelines
1. **AutoMapper Profiles**: Create separate profiles for each entity
2. **Explicit Mapping**: Define explicit mappings for clarity
3. **DTO Mapping**: Map between entities and DTOs, never expose entities directly
4. **Audit Fields**: Never include audit fields in DTOs
5. **Performance**: Use ProjectTo for query projections

## Troubleshooting

### Common Issues and Solutions

#### 1. Template Generation Issues
**Problem**: `dotnet new ca` command fails
**Solution**:
- Ensure ArabDt.Template is properly installed
- Check template parameters are correct
- Verify you're in the correct directory (`src`)

#### 2. Compilation Errors
**Problem**: Missing references or namespace issues
**Solution**:
- Check all using statements are correct
- Verify project references are added
- Ensure NuGet packages are restored

#### 3. Repository Manager Issues
**Problem**: Repository not found in IRepositoryManager
**Solution**:
- Add repository interface to IRepositoryManager
- Implement repository in RepositoryManager
- Register repository in DI container

#### 4. Validation Not Working
**Problem**: FluentValidation rules not applied
**Solution**:
- Ensure ValidationBehavior is registered in pipeline
- Check validator is registered in DI container
- Verify command inherits from correct base class

#### 5. AutoMapper Issues
**Problem**: Mapping configuration errors
**Solution**:
- Ensure mapping profiles are registered
- Check property names match between source and destination
- Use explicit mapping for complex scenarios

#### 6. Database Migration Issues
**Problem**: Migration fails or creates incorrect schema
**Solution**:
- Check entity configuration is applied in DbContext
- Verify entity properties are correctly configured
- Ensure migration is created from correct project

### Performance Considerations
1. **Query Optimization**: Use appropriate indexes for search fields
2. **Lazy Loading**: Configure lazy loading appropriately
3. **Projection**: Use AutoMapper ProjectTo for query projections
4. **Pagination**: Always implement pagination for list queries
5. **Caching**: Consider caching for frequently accessed data

### Security Guidelines
1. **Authorization**: Always apply appropriate authorization attributes
2. **Input Validation**: Validate all inputs at API boundary
3. **SQL Injection**: Use parameterized queries (EF Core handles this)
4. **XSS Protection**: Validate and sanitize user inputs
5. **CORS**: Configure CORS appropriately for production

### Testing Recommendations
1. **Unit Tests**: Test command/query handlers in isolation
2. **Integration Tests**: Test API endpoints end-to-end
3. **Repository Tests**: Test repository implementations with in-memory database
4. **Validation Tests**: Test validation rules thoroughly
5. **Mapping Tests**: Test AutoMapper configurations

### Naming Conventions
- Use PascalCase for classes, methods, and properties
- Use camelCase for parameters and local variables
- Use underscore prefix for private fields (_field)
- Follow established naming patterns in the project

### Error Handling
- Always use try-catch blocks in handlers
- Log errors with appropriate context
- Return meaningful error messages
- Use localized error messages where applicable

### Performance Considerations
- Use AsNoTracking() for read-only queries
- Implement proper indexing in entity configurations
- Use pagination for large data sets
- Consider caching for frequently accessed data

### Security
- Apply proper authorization attributes
- Validate all input data
- Use parameterized queries (handled by EF Core)
- Implement proper audit logging

### Localization
- Support both Arabic and English content
- Use resource files for validation messages
- Implement proper RTL/LTR text direction support
- Consider cultural formatting for dates and numbers

## Troubleshooting Common Issues

### Migration Issues
- Ensure Entity Framework tools are installed
- Check connection string configuration
- Verify entity configurations are properly applied
- Check for naming conflicts with existing tables

### Validation Issues
- Ensure FluentValidation is properly configured
- Check that validation pipeline behavior is registered
- Verify localization resources are available
- Test validation rules with various input scenarios

### Mapping Issues
- Ensure AutoMapper profiles are registered
- Check property name matching between entities and DTOs
- Verify complex mapping configurations
- Test mapping with null values and edge cases

### Repository Issues
- Ensure repository is properly registered in DI container
- Check that repository manager includes new repository
- Verify generic repository methods work correctly
- Test repository methods with various query scenarios

This comprehensive template provides a complete guide for implementing new features in the Jadwa API using Clean Architecture principles. Follow each step carefully and refer to the existing Categories implementation for additional guidance.

