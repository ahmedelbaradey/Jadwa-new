# Clean DTOs Implementation Reference Template

## Overview

This document serves as a **comprehensive reference template** for implementing clean DTOs in the Jadwa API project following Clean Architecture principles. It provides standardized patterns based on the existing Categories implementation and established Jadwa API patterns for creating maintainable, testable, and flexible Data Transfer Objects.

### Key Principles (Based on Jadwa API Architecture)
- **Clean & Simple**: No data annotations or display attributes (following Categories pattern)
- **Inheritance-Based**: Use established base DTOs (LocalizedDto, UserBaseDto, DocumentBaseDto)
- **Audit-Free**: Exclude audit fields - handled by AuditableDbContext interceptor
- **Localization Ready**: Built-in Arabic/English support using existing patterns
- **Repository Pattern**: Compatible with IRepositoryManager facade pattern
- **CQRS Compatible**: Works seamlessly with MediatR and existing handlers

### Jadwa API DTO Architecture

Based on the existing implementation, the Jadwa API uses this inheritance hierarchy:

```
BaseDto (Id)
├── LocalizedDto (NameAr, NameEn, DescriptionAr, DescriptionEn, LocalizedName, LocalizedDescription)
│   ├── {EntityName}Dto (inherits localization for multilingual entities)
│   └── {EntityName}BaseDto (entity-specific properties + localization)
│       ├── Add{EntityName}Dto (+ creation-specific properties)
│       ├── Edit{EntityName}Dto (+ update-specific properties)
│       ├── Single{EntityName}Response (+ display properties)
│       └── {EntityName}DetailDto (+ navigation properties)
│           └── {EntityName}DetailsResponse (+ collections)
├── UserBaseDto (UserId, UserName, FullName, Email, IsActive, JoinedDate, etc.)
│   └── {EntityName}UserDto (+ role-specific properties)
└── DocumentBaseDto (FileName, FilePath, FileSize, UploadedDate, etc.)
    └── {EntityName}DocumentDto (+ category-specific properties)
```

## DTO Implementation Templates

### 1. Base Entity DTO Pattern (Following Categories/CategoryDto.cs)
**Template for main entity DTOs based on Categories implementation**

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
        // PROPERTY TEMPLATE - Add properties based on entity requirements from documentation
        // CRITICAL: Only add properties explicitly mentioned in Stories.md and TASK.md

        // Example property structure (replace with actual requirements from documentation):
        /// <summary>
        /// {PropertyDescription} based on Stories.md/TASK.md requirements
        /// {Required/Optional status based on documentation}
        /// </summary>
        public {PropertyType} {PropertyName} { get; set; } = {DefaultValue};

        // IMPLEMENTATION GUIDELINES:
        // 1. Follow Categories/CategoryDto.cs pattern exactly
        // 2. Only add properties that exist in the corresponding domain entity
        // 3. Do not include audit fields (CreatedAt, CreatedBy, UpdatedAt, UpdatedBy)
        // 4. Match property names and types exactly with the domain entity
        // 5. Use appropriate default values for non-nullable properties
        // 6. Add comprehensive XML documentation for each property
        // 7. Follow C# naming conventions: PascalCase for properties
        // 8. Verify all properties against Stories.md and TASK.md before adding
    }
}
```

### 2. Localized Entity DTO Pattern (Following Fund/FundStrategyDto.cs)
**Template for entities requiring Arabic/English localization**

```csharp
using Application.Common.Dtos;

namespace Application.Features.{DomainArea}.{EntityName}s.Dtos
{
    /// <summary>
    /// Data Transfer Object for {EntityName} entity with localization support
    /// Inherits from LocalizedDto to include localized name and description properties
    /// Based on Fund/FundStrategyDto.cs pattern
    /// Used for entities requiring Arabic/English multilingual support
    /// </summary>
    public record {EntityName}Dto : LocalizedDto
    {
        // ADDITIONAL PROPERTIES TEMPLATE (add only if specified in Stories.md/TASK.md)
        // All localization properties are inherited from LocalizedDto:
        // - Id (from BaseDto)
        // - NameAr, NameEn (localized names)
        // - DescriptionAr, DescriptionEn (localized descriptions)
        // - LocalizedName, LocalizedDescription (computed properties)

        // Add entity-specific properties only if documented in requirements:
        /// <summary>
        /// {PropertyDescription} based on Stories.md/TASK.md requirements
        /// {Required/Optional status based on documentation}
        /// </summary>
        public {PropertyType} {PropertyName} { get; set; } = {DefaultValue};

        // IMPLEMENTATION GUIDELINES:
        // 1. Inherit from LocalizedDto for automatic Arabic/English support
        // 2. LocalizedName and LocalizedDescription are computed automatically
        // 3. Only add additional properties if specified in documentation
        // 4. Follow existing Fund/FundStrategyDto.cs pattern
        // 5. No need to implement localization logic - inherited from base
    }
}
```

### 3. Complex Entity Base DTO Pattern (Following Fund/FundBaseDto.cs)
**Template for entities with many properties requiring a base DTO**

```csharp
using Application.Common.Dtos;

namespace Application.Features.{DomainArea}.{EntityName}s.Dtos
{
    /// <summary>
    /// Base Data Transfer Object for {EntityName}-related entities
    /// Contains common {EntityName} properties shared across different {EntityName} operations
    /// Inherits from LocalizedDto to include localized name and description properties
    /// IMPORTANT: Does not include audit fields - these are handled by the audit system
    /// Based on Fund/FundBaseDto.cs pattern
    /// </summary>
    public abstract record {EntityName}BaseDto : LocalizedDto
    {
        // ENTITY-SPECIFIC PROPERTIES TEMPLATE (add based on Stories.md/TASK.md)

        // Status/Enum Properties Template:
        /// <summary>
        /// {EntityName} status enumeration
        /// {Status description based on business requirements}
        /// Default value based on business rules from documentation
        /// </summary>
        public {EntityName}Status {StatusProperty} { get; set; } = {EntityName}Status.{DefaultStatus};

        // Foreign Key Properties Template:
        /// <summary>
        /// Foreign key reference to {RelatedEntity}
        /// {Relationship description based on Stories.md/TASK.md}
        /// </summary>
        public int {RelatedEntity}Id { get; set; }

        /// <summary>
        /// Optional foreign key reference to {OptionalRelatedEntity}
        /// {Relationship description - nullable based on business rules}
        /// </summary>
        public int? Optional{RelatedEntity}Id { get; set; }

        // Date Properties Template:
        /// <summary>
        /// {DateProperty} based on business requirements
        /// {Date description from documentation}
        /// </summary>
        public DateTime {DateProperty} { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Optional {DateProperty} based on business requirements
        /// {Date description - nullable based on business rules}
        /// </summary>
        public DateTime? Optional{DateProperty} { get; set; }

        // Numeric Properties Template:
        /// <summary>
        /// {NumericProperty} based on business requirements
        /// {Numeric description from documentation}
        /// </summary>
        public int {NumericProperty} { get; set; } = {DefaultValue};

        // IMPLEMENTATION GUIDELINES:
        // 1. Follow Fund/FundBaseDto.cs pattern exactly
        // 2. Inherit from LocalizedDto for automatic localization support
        // 3. Only add properties explicitly mentioned in Stories.md and TASK.md
        // 4. Use abstract record to prevent direct instantiation
        // 5. Group related properties with clear XML documentation
        // 6. Use appropriate default values based on business rules
    }
}
```

### 4. Simple Entity DTO Pattern (Following Categories/CategoryDto.cs)
**Template for simple entities without localization requirements**

```csharp
using Abstraction.Base.Dto;

namespace Application.Features.{DomainArea}.{EntityName}s.Dtos
{
    /// <summary>
    /// Data Transfer Object for {EntityName} entity
    /// Simple structure for entities without localization requirements
    /// Based on Categories/CategoryDto.cs pattern
    /// Properties based on domain entity requirements from Stories.md, TASK.md, and backend-development-plan.md
    /// </summary>
    public record {EntityName}Dto : BaseDto
    {
        // SIMPLE PROPERTIES TEMPLATE (add based on entity requirements from documentation)

        // String Properties Template:
        /// <summary>
        /// {PropertyDescription} based on Stories.md/TASK.md requirements
        /// {Required/Optional status based on documentation}
        /// </summary>
        public string {PropertyName} { get; set; } = string.Empty;

        /// <summary>
        /// Optional {PropertyDescription} based on Stories.md/TASK.md requirements
        /// {Description from documentation}
        /// </summary>
        public string? Optional{PropertyName} { get; set; }

        // Enum Properties Template:
        /// <summary>
        /// {EntityName} status/category enumeration
        /// {Enum description based on business requirements}
        /// </summary>
        public {EntityName}Status {StatusProperty} { get; set; } = {EntityName}Status.{DefaultStatus};

        // Boolean Properties Template:
        /// <summary>
        /// Indicates if the {EntityName} is active
        /// Used for filtering active/inactive items
        /// </summary>
        public bool IsActive { get; set; } = true;

        // Numeric Properties Template:
        /// <summary>
        /// {NumericProperty} based on business requirements
        /// {Numeric description from documentation}
        /// </summary>
        public int {NumericProperty} { get; set; } = {DefaultValue};

        // Date Properties Template:
        /// <summary>
        /// {DateProperty} based on business requirements
        /// {Date description from documentation}
        /// </summary>
        public DateTime {DateProperty} { get; set; } = DateTime.UtcNow;

        // IMPLEMENTATION GUIDELINES:
        // 1. Follow Categories/CategoryDto.cs pattern exactly
        // 2. Keep simple entities without localization complexity
        // 3. Only add properties explicitly mentioned in Stories.md and TASK.md
        // 4. Use appropriate default values based on business rules
        // 5. Add comprehensive XML documentation for each property
        // 6. Follow C# naming conventions: PascalCase for properties
    }
}
```

### 5. Command DTO Patterns (Following Categories Add/Edit Pattern)
**Templates for Create/Update operations based on Categories implementation**

#### Add DTO Pattern (Following Categories/AddCategoryDto.cs)
```csharp
using Microsoft.AspNetCore.Http;

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
        // FILE UPLOAD PROPERTIES TEMPLATE (add only if entity supports file uploads)
        /// <summary>
        /// Primary document file for upload
        /// Used for main entity document (e.g., Terms and Conditions)
        /// </summary>
        public IFormFile? {DocumentType}Document { get; set; }

        /// <summary>
        /// Additional supporting documents
        /// Collection of supplementary files
        /// </summary>
        public List<IFormFile>? AdditionalDocuments { get; set; } = [];

        // CREATION METADATA TEMPLATE (add only if specified in Stories.md/TASK.md)
        /// <summary>
        /// Optional notes for entity creation
        /// Additional context or comments from creator
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Indicates whether to send notifications upon creation
        /// Controls notification workflow
        /// </summary>
        public bool SendNotification { get; set; } = true;

        // IMPLEMENTATION GUIDELINES:
        // 1. Follow Categories/AddCategoryDto.cs pattern exactly
        // 2. Include only properties needed for entity creation
        // 3. Use IFormFile for file uploads if required
        // 4. Provide sensible defaults for optional properties
        // 5. Only add properties explicitly mentioned in Stories.md and TASK.md
    }
}
```

#### Edit DTO Pattern (Following Categories/EditCategoryDto.cs)
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
        // UPDATE METADATA TEMPLATE (add only if specified in Stories.md/TASK.md)
        /// <summary>
        /// Optional notes for the update
        /// Additional context or comments about changes
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Reason for the change
        /// Business justification for the update
        /// </summary>
        public string? ChangeReason { get; set; }

        /// <summary>
        /// Indicates if this update completes the entity
        /// Marks entity as complete/finalized
        /// </summary>
        public bool IsCompletion { get; set; } = false;

        // IMPLEMENTATION GUIDELINES:
        // 1. Follow Categories/EditCategoryDto.cs pattern exactly
        // 2. Include only properties needed for entity updates
        // 3. Provide workflow control properties if needed
        // 4. Only add properties explicitly mentioned in Stories.md and TASK.md
        // 5. Keep simple - complex update logic belongs in handlers
    }
}
```

### 6. Response DTO Patterns (Following Categories/SingleCategoryResponse.cs)
**Templates for API response structures based on Categories implementation**

#### Single Response DTO Pattern (Following Categories/SingleCategoryResponse.cs)
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
        // RELATED ENTITY DISPLAY NAMES TEMPLATE (for UI convenience without joins)
        /// <summary>
        /// Display name of related {RelatedEntity}
        /// Populated for UI display without requiring additional queries
        /// </summary>
        public string? {RelatedEntity}Name { get; set; }

        /// <summary>
        /// Name of assigned user
        /// User responsible for or associated with this entity
        /// </summary>
        public string? AssignedUserName { get; set; }

        // COMPUTED STATISTICS TEMPLATE (populated by query handlers)
        /// <summary>
        /// Number of active users associated with this entity
        /// Computed statistic for quick overview
        /// </summary>
        public int ActiveUsersCount { get; set; }

        /// <summary>
        /// Total number of documents attached to this entity
        /// Computed statistic for document management
        /// </summary>
        public int DocumentsCount { get; set; }

        /// <summary>
        /// Number of pending tasks or approvals
        /// Computed statistic for workflow management
        /// </summary>
        public int PendingTasksCount { get; set; }

        // STATUS INFORMATION TEMPLATE
        /// <summary>
        /// Date of last status change
        /// Tracks when entity status was last modified
        /// </summary>
        public DateTime? LastStatusChangeDate { get; set; }

        /// <summary>
        /// Name of user who last changed the status
        /// Audit information for status changes
        /// </summary>
        public string? LastStatusChangedBy { get; set; }

        // IMPLEMENTATION GUIDELINES:
        // 1. Follow Categories/SingleCategoryResponse.cs pattern exactly
        // 2. Include only essential data for list views
        // 3. Populate statistics in query handlers, not computed properties
        // 4. Use display names for related entities to avoid N+1 queries
        // 5. Include audit information for transparency
        // 6. Keep response lightweight for performance
    }
}
```

#### List Response DTO Pattern (Following Categories List Pattern)
```csharp
using Abstraction.Common.Wappers;

namespace Application.Features.{DomainArea}.{EntityName}s.Dtos
{
    /// <summary>
    /// Paginated list response wrapper for {EntityName} entities
    /// Standard structure for all list endpoints following Categories pattern
    /// Used with PaginatedResult<T> for consistent pagination
    /// </summary>
    public record {EntityName}ListResponse
    {
        /// <summary>
        /// Collection of entity items for current page
        /// Main data payload for the response
        /// </summary>
        public List<Single{EntityName}Response> Items { get; set; } = [];

        /// <summary>
        /// Total number of items across all pages
        /// Used for pagination calculations
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Current page number (1-based)
        /// Indicates which page of results this represents
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Number of items per page
        /// Page size used for this request
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of pages available
        /// Computed based on total count and page size
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        /// <summary>
        /// Indicates if there are more pages after current
        /// Navigation helper for UI pagination
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;

        /// <summary>
        /// Indicates if there are pages before current
        /// Navigation helper for UI pagination
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        // IMPLEMENTATION GUIDELINES:
        // 1. Use PaginatedResult<Single{EntityName}Response> in handlers
        // 2. Follow Categories ListQueryHandler pattern
        // 3. Always include pagination metadata
        // 4. Provide navigation helpers for UI
        // 5. Use consistent naming across all list responses
    }
}
```

### 7. User Association DTO Pattern (Following UserBaseDto Pattern)
**Template for user-entity relationship DTOs based on existing UserBaseDto**

```csharp
using Application.Common.Dtos;

namespace Application.Features.{DomainArea}.{EntityName}s.Dtos
{
    /// <summary>
    /// Data Transfer Object for {EntityName} user associations
    /// Inherits from UserBaseDto to include common user properties
    /// Used for managing user relationships with {EntityName} entities
    /// Based on existing UserBaseDto pattern in the Jadwa API
    /// </summary>
    public record {EntityName}UserDto : UserBaseDto
    {
        // ASSOCIATION-SPECIFIC PROPERTIES TEMPLATE (add based on Stories.md/TASK.md)

        // Role Properties Template:
        /// <summary>
        /// Role of the user in relation to the {EntityName}
        /// Defines permissions and responsibilities
        /// Based on business requirements from documentation
        /// </summary>
        public {EntityName}UserRole Role { get; set; }

        // Assignment Properties Template:
        /// <summary>
        /// Date when the user was assigned to the {EntityName}
        /// Tracks when the relationship was established
        /// </summary>
        public DateTime AssignmentDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Optional assignment end date
        /// When the user assignment expires (if applicable)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Indicates if the assignment is currently active
        /// Used for enabling/disabling user access
        /// </summary>
        public bool IsActive { get; set; } = true;

        // METADATA TEMPLATE (add only if specified in Stories.md/TASK.md)
        /// <summary>
        /// Optional notes about the assignment
        /// Additional context or special instructions
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Priority level of the user assignment
        /// Used for ordering or importance ranking
        /// </summary>
        public int Priority { get; set; } = 0;

        // IMPLEMENTATION GUIDELINES:
        // 1. Inherit from UserBaseDto for automatic user properties
        // 2. Only add association-specific properties
        // 3. Follow existing user association patterns in the project
        // 4. Include role-based properties for permission management
        // 5. Support temporal assignments with start/end dates
        // 6. Only add properties explicitly mentioned in Stories.md and TASK.md
    }
}
```

### 8. Document DTO Pattern (Following DocumentBaseDto Pattern)
**Template for document management DTOs based on existing DocumentBaseDto**

```csharp
using Application.Common.Dtos;

namespace Application.Features.{DomainArea}.{EntityName}s.Dtos
{
    /// <summary>
    /// Data Transfer Object for {EntityName} documents
    /// Inherits from DocumentBaseDto to include common document properties
    /// Used for file upload, download, and document tracking
    /// Based on existing DocumentBaseDto pattern in the Jadwa API
    /// </summary>
    public record {EntityName}DocumentDto : DocumentBaseDto
    {
        // ENTITY REFERENCE TEMPLATE
        /// <summary>
        /// Reference to the parent {EntityName}
        /// Links document to the main entity
        /// </summary>
        public int {EntityName}Id { get; set; }

        // DOCUMENT-SPECIFIC PROPERTIES TEMPLATE (add based on Stories.md/TASK.md)

        // Category Properties Template:
        /// <summary>
        /// Document category or type
        /// Classifies documents for organization
        /// Based on business requirements from documentation
        /// </summary>
        public {EntityName}DocumentCategory Category { get; set; }

        /// <summary>
        /// Document version number
        /// Tracks document revisions
        /// </summary>
        public int Version { get; set; } = 1;

        /// <summary>
        /// Indicates if this is the current version
        /// Used for version management
        /// </summary>
        public bool IsCurrentVersion { get; set; } = true;

        // APPROVAL PROPERTIES TEMPLATE (add only if specified in Stories.md/TASK.md)
        /// <summary>
        /// Indicates if the document requires approval
        /// Used for workflow management
        /// </summary>
        public bool RequiresApproval { get; set; } = false;

        /// <summary>
        /// Indicates if the document has been approved
        /// Used for approval workflow tracking
        /// </summary>
        public bool IsApproved { get; set; } = false;

        /// <summary>
        /// Date when the document was approved
        /// Tracks approval timestamp
        /// </summary>
        public DateTime? ApprovedDate { get; set; }

        /// <summary>
        /// ID of the user who approved the document
        /// Reference to the approver
        /// </summary>
        public int? ApprovedBy { get; set; }

        // IMPLEMENTATION GUIDELINES:
        // 1. Inherit from DocumentBaseDto for automatic document properties
        // 2. Only add document-specific properties
        // 3. Follow existing document management patterns in the project
        // 4. Include category and version management
        // 5. Support approval workflows if required
        // 6. Only add properties explicitly mentioned in Stories.md and TASK.md
    }
}
```

## AutoMapper Configuration Templates

### 1. Basic Entity Mapping (Following Categories/CategoryProfile.cs)
**Template for AutoMapper profiles based on Categories implementation**

```csharp
using AutoMapper;
using Application.Features.{DomainArea}.{EntityName}s.Dtos;
using Domain.Entities.{DomainArea};

namespace Application.Mapping.{EntityName}s
{
    /// <summary>
    /// AutoMapper profile for {EntityName} entity mappings
    /// Handles mapping between entities and DTOs following Categories pattern
    /// Based on Categories/CategoryProfile.cs implementation
    /// </summary>
    public partial class {EntityName}Profile : Profile
    {
        public void Get{EntityName}Mapping()
        {
            // BASIC ENTITY TO DTO MAPPING TEMPLATE
            CreateMap<{EntityName}, {EntityName}Dto>()
                // Localized properties are computed automatically if using LocalizedDto
                .ForMember(dest => dest.LocalizedName, opt => opt.Ignore())
                .ForMember(dest => dest.LocalizedDescription, opt => opt.Ignore());

            // ENTITY TO RESPONSE DTO MAPPING TEMPLATE
            CreateMap<{EntityName}, Single{EntityName}Response>()
                // Map related entity names for display
                .ForMember(dest => dest.{RelatedEntity}Name,
                    opt => opt.MapFrom(src => src.{RelatedEntity} != null ? src.{RelatedEntity}.LocalizedName : null))
                // Ignore computed properties
                .ForMember(dest => dest.LocalizedName, opt => opt.Ignore());

            // REVERSE MAPPING TEMPLATE (DTO to Entity)
            CreateMap<{EntityName}Dto, {EntityName}>()
                // Ignore computed properties and audit fields
                .ForMember(dest => dest.LocalizedName, opt => opt.Ignore())
                .ForMember(dest => dest.LocalizedDescription, opt => opt.Ignore())
                // Audit fields are handled by interceptor
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

            // COMMAND MAPPING TEMPLATE
            CreateMap<Add{EntityName}Dto, {EntityName}>()
                // Map only properties that exist in entity
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Auto-generated
                // Ignore audit fields - handled by interceptor
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

            CreateMap<Edit{EntityName}Dto, {EntityName}>()
                // Include Id for updates
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                // Ignore audit fields - handled by interceptor
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
        }

        // IMPLEMENTATION GUIDELINES:
        // 1. Follow Categories/CategoryProfile.cs pattern exactly
        // 2. Always ignore audit fields in DTO to Entity mappings
        // 3. Ignore computed properties (LocalizedName, etc.)
        // 4. Map related entity display names for UI convenience
        // 5. Use partial class to organize mappings by operation type
    }
}
```

## Implementation Guidelines

### 🏗️ DTO Structure Rules (Based on Jadwa API Architecture)

#### ✅ **DO Include:**
1. **Essential Properties**: Only properties that exist in the corresponding domain entity from Stories.md/TASK.md
2. **Proper Inheritance**: Use established base DTOs (LocalizedDto, UserBaseDto, DocumentBaseDto)
3. **XML Documentation**: Comprehensive documentation for all DTOs explaining purpose and usage
4. **Default Values**: Sensible defaults for required properties based on business rules
5. **Collection Initialization**: Use `[]` syntax for empty collections for better performance
6. **Placeholder Patterns**: Use `{EntityName}`, `{PropertyName}`, `{StatusProperty}` for template reusability
7. **Categories Pattern**: Follow Categories implementation as the reference standard
8. **Repository Compatibility**: Ensure DTOs work with IRepositoryManager facade pattern

#### ❌ **DON'T Include:**
1. **Data Annotations**: No `[Display]`, `[Required]`, or validation attributes (Clean Architecture principle)
2. **Audit Fields**: No `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy` (handled by AuditableDbContext interceptor)
3. **Navigation Properties**: Use separate DTOs for related entities to avoid circular references
4. **Business Logic**: Keep DTOs as pure data containers without business rules
5. **Complex Validation**: Handle validation in separate validators using FluentValidation
6. **Hardcoded Values**: Avoid hardcoded strings, use placeholders for template reusability
7. **Framework Coupling**: Avoid dependencies on specific UI or validation frameworks
8. **Undocumented Properties**: Never add properties not explicitly mentioned in Stories.md and TASK.md

### 📁 File Organization Template (Following Jadwa API Structure)

```
Application/
├── Common/
│   └── Dtos/                                   # Shared base DTOs
│       ├── LocalizedDto.cs                     # Base for multilingual entities
│       ├── UserBaseDto.cs                      # Base for user-related DTOs
│       ├── DocumentBaseDto.cs                  # Base for document DTOs
│       └── BaseDto.cs                          # Base DTO with Id property
├── Features/
│   └── {DomainArea}/                           # e.g., Catalog, Fund, etc.
│       └── {EntityName}s/                     # e.g., Categories, Funds, etc.
│           ├── Dtos/
│           │   ├── {EntityName}Dto.cs              # Base entity DTO
│           │   ├── Add{EntityName}Dto.cs           # Create command DTO
│           │   ├── Edit{EntityName}Dto.cs          # Update command DTO
│           │   ├── Single{EntityName}Response.cs   # List item response
│           │   ├── {EntityName}UserDto.cs          # User association DTO
│           │   └── {EntityName}DocumentDto.cs      # Document DTO
│           ├── Queries/
│           │   ├── List/
│           │   │   ├── List{EntityName}Query.cs
│           │   │   └── List{EntityName}QueryHandler.cs
│           │   └── Get/
│           │       ├── Get{EntityName}Query.cs
│           │       └── Get{EntityName}QueryHandler.cs
│           ├── Commands/
│           │   ├── Add/
│           │   │   ├── Add{EntityName}Command.cs
│           │   │   └── Add{EntityName}CommandHandler.cs
│           │   └── Edit/
│           │       ├── Edit{EntityName}Command.cs
│           │       └── Edit{EntityName}CommandHandler.cs
│           └── Validation/
│               ├── Base{EntityName}Validation.cs
│               ├── Add{EntityName}Validation.cs
│               └── Edit{EntityName}Validation.cs
└── Mapping/
    └── {EntityName}s/
        └── {EntityName}Profile.cs              # AutoMapper profile
```

### 🎯 Inheritance Hierarchy Template (Based on Jadwa API Architecture)

#### Base DTO Inheritance Pattern Template:
```csharp
// INHERITANCE HIERARCHY TEMPLATE (Following Jadwa API patterns)

// 1. Base DTO (minimal properties with Id)
/// <summary>
/// Base DTO with Id property
/// Foundation for all DTOs in the system
/// </summary>
public record BaseDto
{
    public int Id { get; set; }
}

// 2. Localized Base DTO (for multilingual entities)
/// <summary>
/// Base DTO for entities requiring Arabic/English localization
/// Includes computed localization properties
/// </summary>
public record LocalizedDto : BaseDto
{
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? DescriptionAr { get; set; }
    public string? DescriptionEn { get; set; }

    // Computed properties
    public string LocalizedName =>
        CultureInfo.CurrentCulture.Name.StartsWith("ar") ? NameAr : NameEn;
    public string? LocalizedDescription =>
        CultureInfo.CurrentCulture.Name.StartsWith("ar") ? DescriptionAr : DescriptionEn;
}

// 3. Entity-Specific DTO (inherits from appropriate base)
/// <summary>
/// Entity-specific DTO following Categories pattern
/// Inherits from LocalizedDto for multilingual entities or BaseDto for simple entities
/// </summary>
public record {EntityName}Dto : LocalizedDto // or BaseDto for non-localized entities
{
    // Add only properties explicitly mentioned in Stories.md and TASK.md
    // Follow Categories/CategoryDto.cs pattern exactly
}

// 4. Command DTOs (extend entity DTO)
/// <summary>
/// Command DTOs for Create/Update operations
/// Inherit from entity DTO to include all base properties
/// </summary>
public record Add{EntityName}Dto : {EntityName}Dto { /* creation-specific properties */ }
public record Edit{EntityName}Dto : {EntityName}Dto { /* update-specific properties */ }

// 5. Response DTOs (extend entity DTO)
/// <summary>
/// Response DTOs for API operations
/// Inherit from entity DTO and add display-specific properties
/// </summary>
public record Single{EntityName}Response : {EntityName}Dto { /* display properties */ }

// 6. Specialized DTOs (inherit from appropriate base)
/// <summary>
/// Specialized DTOs for specific use cases
/// Inherit from established base DTOs
/// </summary>
public record {EntityName}UserDto : UserBaseDto { /* user association properties */ }
public record {EntityName}DocumentDto : DocumentBaseDto { /* document properties */ }
```

### 🌐 Localization Patterns Template (Based on Jadwa API Implementation)

#### Standard Localization Properties Template:
```csharp
// MULTILINGUAL PROPERTIES TEMPLATE (following LocalizedDto pattern)
/// <summary>
/// {PropertyName} in Arabic
/// Required for entities with multilingual support
/// </summary>
public string {PropertyName}Ar { get; set; } = string.Empty;

/// <summary>
/// {PropertyName} in English
/// Required for entities with multilingual support
/// </summary>
public string {PropertyName}En { get; set; } = string.Empty;

// COMPUTED LOCALIZATION PROPERTIES TEMPLATE
/// <summary>
/// Localized {PropertyName} based on current culture
/// Returns Arabic text for Arabic cultures, English otherwise
/// </summary>
public string Localized{PropertyName} =>
    CultureInfo.CurrentCulture.Name.StartsWith("ar") ? {PropertyName}Ar : {PropertyName}En;
```

#### Enum Localization Pattern Template:
```csharp
/// <summary>
/// Localized display name for {EnumProperty}
/// Provides user-friendly enum names in current culture
/// </summary>
public string {EnumProperty}DisplayName => {EnumProperty} switch
{
    {EntityName}{EnumType}.{EnumValue1} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicEnumValue1}" : "{EnglishEnumValue1}",
    {EntityName}{EnumType}.{EnumValue2} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicEnumValue2}" : "{EnglishEnumValue2}",
    {EntityName}{EnumType}.{EnumValue3} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicEnumValue3}" : "{EnglishEnumValue3}",
    _ => {EnumProperty}.ToString()
};
```

## Validation Strategy Template (Following Jadwa API FluentValidation Pattern)

### 1. Base Validation Template (Following Categories Validation Pattern)
```csharp
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.{DomainArea}.{EntityName}s.Validation
{
    /// <summary>
    /// Base validation for {EntityName} DTOs
    /// Provides common validation rules shared across Add/Edit operations
    /// Based on Categories validation pattern in Jadwa API
    /// </summary>
    public class Base{EntityName}Validation : AbstractValidator<{EntityName}Dto>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public Base{EntityName}Validation(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
        }

        public void ApplyValidationRules()
        {
            // REQUIRED FIELD VALIDATION TEMPLATE (for localized entities)
            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKey.EmptyValidation])
                .NotNull().WithMessage(_localizer[SharedResourcesKey.EmptyValidation])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKey.MaxLengthValidation]);

            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKey.EmptyValidation])
                .NotNull().WithMessage(_localizer[SharedResourcesKey.EmptyValidation])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKey.MaxLengthValidation]);

            // OPTIONAL FIELD VALIDATION TEMPLATE
            RuleFor(x => x.DescriptionAr)
                .MaximumLength(500).WithMessage(_localizer[SharedResourcesKey.MaxLengthValidation])
                .When(x => !string.IsNullOrEmpty(x.DescriptionAr));

            RuleFor(x => x.DescriptionEn)
                .MaximumLength(500).WithMessage(_localizer[SharedResourcesKey.MaxLengthValidation])
                .When(x => !string.IsNullOrEmpty(x.DescriptionEn));

            // ENUM VALIDATION TEMPLATE
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage(_localizer[SharedResourcesKey.InvalidEnumValidation]);

            // Add additional validation rules based on Stories.md and TASK.md requirements
        }
    }
}
```

### 2. Command Validation Templates
```csharp
/// <summary>
/// Validation for Add{EntityName} command
/// Inherits base validation and adds creation-specific rules
/// </summary>
public class Add{EntityName}Validation : AbstractValidator<Add{EntityName}Command>
{
    public Add{EntityName}Validation(IStringLocalizer<SharedResources> localizer)
    {
        Include(new Base{EntityName}Validation(localizer));
        // Add creation-specific validation rules here
    }
}

/// <summary>
/// Validation for Edit{EntityName} command
/// Inherits base validation and adds update-specific rules
/// </summary>
public class Edit{EntityName}Validation : AbstractValidator<Edit{EntityName}Command>
{
    public Edit{EntityName}Validation(IStringLocalizer<SharedResources> localizer)
    {
        Include(new Base{EntityName}Validation(localizer));

        // ID validation for updates
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage(localizer[SharedResourcesKey.InvalidIdValidation]);
    }
}
```

## Usage Examples Template (Following Jadwa API Patterns)

### 1. Controller Template (Following Categories Controller Pattern)
```csharp
[ApiController]
[Route("api/[controller]")]
public class {EntityName}Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public {EntityName}Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] List{EntityName}Query query)
    {
        var result = await _mediator.Send(query);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new Get{EntityName}Query { Id = id };
        var result = await _mediator.Send(query);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Add{EntityName}Command command)
    {
        var result = await _mediator.Send(command);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Edit{EntityName}Command command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
}
```

### 2. Query Handler Template (Following Categories ListQueryHandler Pattern)
```csharp
public class List{EntityName}QueryHandler : BaseResponseHandler, IQueryHandler<List{EntityName}Query, BaseResponse<PaginatedResult<Single{EntityName}Response>>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public List{EntityName}QueryHandler(IRepositoryManager repository, IMapper mapper, IStringLocalizer<SharedResources> localizer)
        : base(localizer)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<PaginatedResult<Single{EntityName}Response>>> Handle(List{EntityName}Query request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await _repository.{EntityName}s.GetAllAsync();
            var mappedEntities = _mapper.Map<List<Single{EntityName}Response>>(entities);

            var paginatedResult = PaginatedResult<Single{EntityName}Response>.Create(
                mappedEntities, request.PageNumber, request.PageSize);

            return Success(paginatedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in List{EntityName}Query");
            return ServerError<PaginatedResult<Single{EntityName}Response>>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
        }
    }
}
```

### 3. Command Handler Template (Following Categories AddCommandHandler Pattern)
```csharp
public class Add{EntityName}CommandHandler : BaseResponseHandler, ICommandHandler<Add{EntityName}Command, BaseResponse<Single{EntityName}Response>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public Add{EntityName}CommandHandler(IRepositoryManager repository, IMapper mapper, IStringLocalizer<SharedResources> localizer)
        : base(localizer)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<Single{EntityName}Response>> Handle(Add{EntityName}Command request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<{EntityName}>(request);
            var addedEntity = await _repository.{EntityName}s.AddAsync(entity);
            await _repository.SaveAsync();

            var response = _mapper.Map<Single{EntityName}Response>(addedEntity);
            return Success(response, _localizer[SharedResourcesKey.AddedSuccessfully]);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Add{EntityName}Command");
            return ServerError<Single{EntityName}Response>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
        }
    }
}
```

### 🌐 Localization Patterns Template

#### Standard Localization Properties Template:
```csharp
// MULTILINGUAL PROPERTIES TEMPLATE (add only if entity requires multilingual support)
/// <summary>
/// {PropertyName} in Arabic
/// Required for entities with multilingual support
/// </summary>
public string {PropertyName}Ar { get; set; } = string.Empty;

/// <summary>
/// {PropertyName} in English
/// Required for entities with multilingual support
/// </summary>
public string {PropertyName}En { get; set; } = string.Empty;

/// <summary>
/// {DescriptionProperty} in Arabic (optional)
/// Used for detailed multilingual descriptions
/// </summary>
public string? {DescriptionProperty}Ar { get; set; }

/// <summary>
/// {DescriptionProperty} in English (optional)
/// Used for detailed multilingual descriptions
/// </summary>
public string? {DescriptionProperty}En { get; set; }

// COMPUTED LOCALIZATION PROPERTIES TEMPLATE
/// <summary>
/// Localized {PropertyName} based on current culture
/// Returns Arabic text for Arabic cultures, English otherwise
/// </summary>
public string Localized{PropertyName} =>
    CultureInfo.CurrentCulture.Name.StartsWith("ar") ? {PropertyName}Ar : {PropertyName}En;

/// <summary>
/// Localized {DescriptionProperty} based on current culture
/// Returns Arabic text for Arabic cultures, English otherwise
/// </summary>
public string? Localized{DescriptionProperty} =>
    CultureInfo.CurrentCulture.Name.StartsWith("ar") ? {DescriptionProperty}Ar : {DescriptionProperty}En;
```

#### Enum Localization Pattern Template:
```csharp
/// <summary>
/// Localized display name for {EnumProperty}
/// Provides user-friendly enum names in current culture
/// </summary>
public string {EnumProperty}DisplayName => {EnumProperty} switch
{
    {EntityName}{EnumType}.{EnumValue1} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicEnumValue1}" : "{EnglishEnumValue1}",
    {EntityName}{EnumType}.{EnumValue2} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicEnumValue2}" : "{EnglishEnumValue2}",
    {EntityName}{EnumType}.{EnumValue3} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicEnumValue3}" : "{EnglishEnumValue3}",
    _ => {EnumProperty}.ToString()
};
```

#### Resource-Based Localization Template (for Application Layer):
```csharp
// Use this pattern in handlers with IStringLocalizer
/// <summary>
/// Gets localized enum display name using resource files
/// Provides consistent localization with centralized resource management
/// </summary>
public static string GetLocalized{EnumType}Name({EntityName}{EnumType} enumValue, IStringLocalizer<SharedResources> localizer)
{
    return enumValue switch
    {
        {EntityName}{EnumType}.{EnumValue1} => localizer[SharedResourcesKey.{EntityName}{EnumType}{EnumValue1}],
        {EntityName}{EnumType}.{EnumValue2} => localizer[SharedResourcesKey.{EntityName}{EnumType}{EnumValue2}],
        {EntityName}{EnumType}.{EnumValue3} => localizer[SharedResourcesKey.{EntityName}{EnumType}{EnumValue3}],
        _ => enumValue.ToString()
    };
}
```

### 🎯 Inheritance Hierarchy Template

#### Base DTO Inheritance Pattern Template:
```csharp
// INHERITANCE HIERARCHY TEMPLATE
// 1. Base DTO (minimal properties from domain entity)
/// <summary>
/// Base DTO for {EntityName} with core properties
/// Contains essential properties from domain entity
/// </summary>
public record {EntityName}Dto : BaseDto
{
    // Core entity properties only
    // Inherits Id from BaseDto
    // Add properties based on domain entity requirements
}

// 2. Localized Base DTO (for multilingual entities)
/// <summary>
/// Base DTO for {EntityName} with localization support
/// Extends BaseDto with multilingual properties
/// </summary>
public record {EntityName}Dto : LocalizedDto
{
    // Additional entity-specific properties
    // Inherits Id, NameAr, NameEn, DescriptionAr, DescriptionEn, LocalizedName, LocalizedDescription
}

// 3. Detail DTO (extends base with navigation properties)
/// <summary>
/// Detail DTO for {EntityName} with related entity information
/// Extends base DTO with navigation properties and statistics
/// </summary>
public record {EntityName}DetailDto : {EntityName}Dto
{
    // Navigation properties as separate DTOs
    // Computed statistics
    // Additional metadata
}

// 4. Response DTO (extends base with computed properties)
/// <summary>
/// Response DTO for {EntityName} optimized for list views
/// Extends base DTO with display-friendly computed properties
/// </summary>
public record Single{EntityName}Response : {EntityName}Dto
{
    // Related entity display names
    // Computed statistics
    // Status information
}

// 5. Details Response (comprehensive with all related data)
/// <summary>
/// Comprehensive details response for {EntityName}
/// Extends detail DTO with all related collections
/// </summary>
public record {EntityName}DetailsResponse : {EntityName}DetailDto
{
    // Related collections
    // Computed aggregates
    // Complete entity view
}

// 6. Command DTOs (extend base with operation-specific properties)
/// <summary>
/// Command DTO for creating new {EntityName}
/// Extends base DTO with creation-specific properties
/// </summary>
public record Add{EntityName}Dto : {EntityName}Dto
{
    // File upload properties
    // Creation metadata
    // Initial assignments
}

/// <summary>
/// Command DTO for updating existing {EntityName}
/// Extends base DTO with update-specific properties
/// </summary>
public record Edit{EntityName}Dto : {EntityName}Dto
{
    // File upload properties
    // Update metadata
    // Change tracking
}

// 7. Specialized DTOs (for specific use cases)
/// <summary>
/// Association DTO for {EntityName} relationships
/// Manages many-to-many relationships with metadata
/// </summary>
public record {EntityName}UserDto : UserBaseDto
{
    // Association-specific properties
    // Role information
    // Assignment metadata
}

/// <summary>
/// Document DTO for {EntityName} file management
/// Handles file metadata and document tracking
/// </summary>
public record {EntityName}DocumentDto : DocumentBaseDto
{
    // Document-specific properties
    // Category information
    // Version tracking
}
```

## Benefits of Clean DTOs

### 🚀 **Performance Benefits**
- **Zero Reflection Overhead**: No data annotations to process at runtime
- **Fast Serialization**: Simple properties serialize/deserialize quickly with JSON
- **Reduced Memory Footprint**: Minimal object overhead and efficient memory usage
- **Efficient AutoMapper**: Clean mapping without attribute interference or complex configurations
- **Computed Properties**: Culture-based localization with minimal performance impact
- **Collection Efficiency**: Modern `[]` syntax for better collection initialization

### 🔧 **Maintainability Benefits**
- **Clear Separation of Concerns**: DTOs focus solely on data transfer without UI coupling
- **Framework Independence**: No coupling to UI, validation, or specific frameworks
- **Easy Refactoring**: Simple structure makes changes straightforward and safe
- **Consistent Patterns**: Standardized templates across all entities and domains
- **Template Reusability**: Placeholder patterns enable easy adaptation to new entities
- **Inheritance Benefits**: Shared base DTOs reduce code duplication and ensure consistency

### 🧪 **Testability Benefits**
- **Simple Object Creation**: Easy to instantiate for unit tests without complex setup
- **No Complex Dependencies**: No attribute or framework dependencies to mock
- **Clear Data Contracts**: Explicit properties make testing predictable and reliable
- **Mock-Friendly**: Simple interfaces for mocking and test data creation
- **Localization Testing**: Easy to test culture-based behavior with different cultures
- **Computed Property Testing**: Straightforward testing of localized and computed properties

### 🌐 **Flexibility Benefits**
- **Multi-Framework Support**: Works with any UI framework (Blazor, React, Angular, Vue)
- **API Versioning**: Easy to create versioned DTOs without breaking changes
- **Client Generation**: Clean structure for OpenAPI/Swagger client generation
- **Extensibility**: Simple to add new properties, computed fields, or localization
- **Culture Support**: Built-in multilingual support with automatic culture detection
- **Template Adaptability**: Generic patterns work across different business domains

### 🎯 **Business Benefits**
- **Multilingual Ready**: Built-in support for Arabic/English and extensible to other languages
- **User Experience**: Localized content and user-friendly computed properties
- **Audit Compliance**: Clean separation from audit fields while maintaining data integrity
- **Workflow Support**: Status tracking and history management with localized displays
- **Document Management**: File handling with metadata and categorization support

## Usage Examples Template

### In Controllers Template:
```csharp
[HttpGet("{id}")]
public async Task<IActionResult> Get{EntityName}(int id)
{
    var query = new Get{EntityName}Query { Id = id };
    var result = await _mediator.Send(query);

    // DTO includes localized properties based on Accept-Language header
    return Ok(result.Data); // Returns Single{EntityName}Response with localization
}

[HttpPost]
public async Task<IActionResult> Create{EntityName}([FromBody] Add{EntityName}Dto dto)
{
    var command = _mapper.Map<Add{EntityName}Command>(dto);
    var result = await _mediator.Send(command);

    return result.Succeeded ? Ok(result) : BadRequest(result);
}
```

### In AutoMapper Template:
```csharp
public void Get{EntityName}Mapping()
{
    CreateMap<{EntityName}, {EntityName}Dto>()
        // Localized properties are computed automatically
        .ForMember(dest => dest.Localized{PropertyName}, opt => opt.Ignore())
        .ForMember(dest => dest.{StatusProperty}DisplayName, opt => opt.Ignore());

    CreateMap<{EntityName}, Single{EntityName}Response>()
        .ForMember(dest => dest.{RelatedEntity}Name,
            opt => opt.MapFrom(src => src.{RelatedEntity} != null ? src.{RelatedEntity}.Localized{PropertyName} : null))
        .ForMember(dest => dest.Localized{PropertyName}, opt => opt.Ignore());
}
```

### In Frontend Template:
```typescript
// TypeScript interface generated from DTO
interface {EntityName}Dto {
    id: number;
    {propertyName}Ar: string;
    {propertyName}En: string;
    localized{PropertyName}: string; // Computed property from backend
    {statusProperty}: {EntityName}Status;
    {statusProperty}DisplayName: string; // Localized enum display
    // ... other properties
}

// Usage in React/Angular components
const displayName = entity.localized{PropertyName}; // Automatically localized
const statusText = entity.{statusProperty}DisplayName; // Localized status
```

### In Query Handlers Template:
```csharp
public async Task<BaseResponse<{EntityName}DetailsResponse>> Handle(Get{EntityName}DetailsQuery request, CancellationToken cancellationToken)
{
    try
    {
        var entity = await _repository.{EntityName}s.GetWithRelatedDataAsync(request.Id);
        var response = _mapper.Map<{EntityName}DetailsResponse>(entity);

        // Apply localization using extension methods
        foreach (var item in response.StatusHistory)
        {
            item.FromStatusDisplayName = LocalizationExtensions.GetLocalized{EntityName}StatusName(item.FromStatus, _localizer);
            item.ToStatusDisplayName = LocalizationExtensions.GetLocalized{EntityName}StatusName(item.ToStatus, _localizer);
        }

        return Success(response);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error in Get{EntityName}DetailsQuery");
        return ServerError<{EntityName}DetailsResponse>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
    }
}
```

## Validation Strategy Template

Since DTOs are clean and framework-independent, validation should be handled separately using proper Clean Architecture patterns:

### 1. **FluentValidation in Application Layer**
```csharp
public class Add{EntityName}Validation : AbstractValidator<Add{EntityName}Command>
{
    private readonly IStringLocalizer<SharedResources> _localizer;

    public Add{EntityName}Validation(IStringLocalizer<SharedResources> localizer)
    {
        _localizer = localizer;
        Include(new Base{EntityName}Validation(_localizer));
    }
}
```

### 2. **Domain Validation in Entities**
```csharp
public class {EntityName} : FullAuditedEntity
{
    // Domain validation through business rules and constraints
    // No validation attributes - keep entities clean
}
```

### 3. **API Validation in Controllers (if needed)**
```csharp
[HttpPost]
public async Task<IActionResult> Create{EntityName}([FromBody] Add{EntityName}Dto dto)
{
    // Validation handled by ValidationBehavior in MediatR pipeline
    var command = _mapper.Map<Add{EntityName}Command>(dto);
    var result = await _mediator.Send(command);

    return result.Succeeded ? Ok(result) : BadRequest(result);
}
```

### 4. **Client Validation for User Experience**
```typescript
// Frontend validation for immediate user feedback
const validate{EntityName} = (entity: {EntityName}Dto): ValidationResult => {
    const errors: string[] = [];

    if (!entity.{propertyName}Ar.trim()) {
        errors.push('Arabic name is required');
    }

    if (!entity.{propertyName}En.trim()) {
        errors.push('English name is required');
    }

    return { isValid: errors.length === 0, errors };
};
```

This layered validation approach maintains clean DTOs while ensuring comprehensive validation at appropriate architectural layers, supporting both immediate user feedback and robust server-side validation with localized error messages.

## Implementation Checklist

### ✅ **Before Creating DTOs:**
1. **Review Documentation**: Check Stories.md and TASK.md for required properties
2. **Choose Base DTO**: Select appropriate base (BaseDto, LocalizedDto, UserBaseDto, DocumentBaseDto)
3. **Follow Categories Pattern**: Use Categories implementation as reference
4. **Plan Inheritance**: Design DTO hierarchy to minimize duplication
5. **Consider Localization**: Determine if entity requires Arabic/English support

### ✅ **During Implementation:**
1. **Add Only Required Properties**: Include only properties from documentation
2. **Use Proper Inheritance**: Inherit from established base DTOs
3. **Add XML Documentation**: Comprehensive documentation for all properties
4. **Set Default Values**: Appropriate defaults based on business rules
5. **Follow Naming Conventions**: PascalCase for properties, consistent patterns

### ✅ **After Implementation:**
1. **Create AutoMapper Profile**: Map between entities and DTOs
2. **Implement Validation**: Use FluentValidation with localized messages
3. **Test Localization**: Verify culture-based behavior
4. **Update Handlers**: Ensure handlers use new DTOs correctly
5. **Document Changes**: Update relevant documentation

### ❌ **Common Pitfalls to Avoid:**
1. **Adding Undocumented Properties**: Only use properties from Stories.md/TASK.md
2. **Including Audit Fields**: Never add CreatedAt, CreatedBy, etc. to DTOs
3. **Using Data Annotations**: Keep DTOs clean without validation attributes
4. **Hardcoding Localization**: Use computed properties or resource-based localization
5. **Deep Object Graphs**: Use separate DTOs for related entities
6. **Ignoring Base DTOs**: Always use established inheritance patterns

## Troubleshooting Guide

### 🔧 **Common Issues and Solutions:**

#### Issue: "Localized properties not working"
**Solution**: Ensure entity inherits from LocalizedDto and culture is set correctly in request headers.

#### Issue: "AutoMapper mapping errors"
**Solution**: Always ignore computed properties in mapping profiles and ensure audit fields are excluded.

#### Issue: "Validation not working"
**Solution**: Verify ValidationBehavior is registered and validators inherit from base validation classes.

#### Issue: "Properties not appearing in API"
**Solution**: Check that properties exist in domain entity and are documented in Stories.md/TASK.md.

#### Issue: "Circular reference errors"
**Solution**: Use separate DTOs for related entities instead of including navigation properties.

### 📚 **Reference Links:**
- **Categories Implementation**: `Application/Features/Catalog/Categories/` (reference pattern)
- **Base DTOs**: `Application/Common/Dtos/` (LocalizedDto, UserBaseDto, DocumentBaseDto)
- **Validation**: `Application/Features/{DomainArea}/{EntityName}s/Validation/`
- **AutoMapper**: `Application/Mapping/{EntityName}s/{EntityName}Profile.cs`
- **Documentation**: `docs/Stories.md`, `docs/TASK.md`, `docs/architecture.md`

This comprehensive template ensures consistent, maintainable, and scalable DTO implementation following the established Jadwa API architecture and Clean Architecture principles.
