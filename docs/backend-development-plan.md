# Jadwa API Backend Development Plan

## Overview

This document outlines a comprehensive 4-phase backend development roadmap for the Jadwa API system, focusing exclusively on server-side implementation. The plan is designed to align with the existing Clean Architecture using .NET 9, CQRS with MediatR, Entity Framework Core, and the established technology stack.

## Architecture Alignment

The development plan leverages the existing Jadwa API architecture:
- **.NET 9** with Clean Architecture principles
- **CQRS Pattern** using MediatR for command/query separation
- **Generic Repository Pattern** with IRepositoryManager facade for data access abstraction
- **Entity Framework Core** Code First with SQL Server
- **JWT Authentication** with role-based authorization
- **Redis** for distributed caching and real-time notifications
- **Comprehensive Audit Trail** system with soft delete functionality
- **Multi-language Support** (Arabic/English) with localization
- **Fluent Validation Support** (Arabic/English) with localization

## Phase 1: Core Setup & Funds Management

### [JDWA-182] Navigate fund strategies ✅ COMPLETED

**Backend Development Focus:**
- ✅ Database schema design for `FundStrategies` entity with audit trail inheritance from `FullAuditedEntity`
- ✅ Implement Generic Repository Pattern using `IFundStrategyRepository` inheriting from `IGenericRepository`
- ✅ Create repository operations with sorting and filtering capabilities using CleanArchitectureSteps.md guide
- ✅ Develop CQRS handlers with IRepositoryManager facade pattern for data access
- ✅ API endpoint: `GET /api/FundStrategies` with pagination, sorting (Arabic/English names, update date)
- ✅ Localization support for Arabic/English strategy names with FluentValidation
- ✅ Error handling with standardized response format using `BaseResponse<T>`
- ✅ Performance optimization through Generic Repository built-in optimization
- **Implementation**: Generic Repository Pattern following Clean Architecture with IRepositoryManager facade

### [JDWA-184] Add fund strategy ✅ COMPLETED

**Backend Development Focus:**
- ✅ Database entity design with required fields validation and unique constraints
- ✅ Repository operation: `AddAsync<FundStrategy>` with comprehensive validation rules using Generic Repository
- ✅ CQRS Command Handler implementation with business logic for fund strategy creation using IRepositoryManager
- ✅ API endpoint: `POST /api/FundStrategies/Create` with request/response models
- ✅ Data validation using FluentValidation with custom validators for Arabic/English names
- ✅ Authorization requirements: System Admin role validation using claims-based authorization
- ✅ Audit logging: Automatic creation timestamp and user tracking via `FullAuditedEntity`
- ✅ Error handling for duplicate strategy names and validation failures
- ✅ Transaction management for data consistency through Generic Repository layer
- ✅ Integration with existing repository patterns using IRepositoryManager facade
- **Implementation**: Complete CRUD operations with localized validation and Generic Repository Pattern

### [JDWA-183] Edit fund strategy ✅ COMPLETED

**Backend Development Focus:**
- ✅ Repository operation: `UpdateAsync<FundStrategy>` with entity validation and concurrency handling
- ✅ CQRS Command Handler implementation with optimistic concurrency control using EF Core row versioning
- ✅ API endpoint: `PUT /api/FundStrategies/Update` with proper HTTP status codes
- ✅ Business logic for update validation and change tracking in Command Handler
- ✅ Authorization: System Admin role verification with claims-based security
- ✅ Audit trail: Automatic update timestamp and user tracking via `FullAuditedEntity`
- ✅ Data validation for required fields and business rules using FluentValidation
- ✅ Error handling for not found scenarios and validation failures
- ✅ Integration with Generic Repository Pattern and ValidationBehavior pipeline using IRepositoryManager
- **Implementation**: Complete update operations with comprehensive validation and Generic Repository Pattern

### [JDWA-187] & [JDWA-186] Create a fund (Unified Endpoint) 🔲 PENDING

**Backend Development Focus:**
- 🔲 Database schema for `Funds` entity with complex relationships to users, strategies, and documents
- 🔲 Repository interface: `IFundRepository` inheriting from `IGenericRepository` with fund-specific operations
- 🔲 CQRS Command: `CreateFundCommand` with role-agnostic business logic and role detection
- 🔲 Command Handler implementation with unified fund creation workflow using IRepositoryManager facade:
  - **Fund Manager Role**: Set initial status to "under construction/تحت الإنشاء"
  - **Legal Council Role**: Set initial status to "waiting for adding members/لا يوجد أعضاء"
- 🔲 API endpoint: `POST /api/Funds/Create` with unified request model and role-based processing
- 🔲 Role detection logic: Automatically determine creator role from JWT claims and apply appropriate workflow
- 🔲 File upload handling for TNC documents with secure storage and validation
- 🔲 Status management: Dynamic status setting based on creator role using state machine pattern
- 🔲 User role assignment and validation for fund managers, legal council, board secretaries
- 🔲 Notification system integration with role-specific notification templates:
  - **Fund Manager Creation**: Notify other fund managers and legal council/board secretary
  - **Legal Council Creation**: Notify fund managers and board secretaries
- 🔲 Audit logging: Action tracking with user details, role information, and creation context
- 🔲 Document management: Save TNC files in categorized document storage with role-based permissions
- 🔲 Transaction coordination for multi-entity operations ensuring data consistency through Generic Repository
- 🔲 Authorization: Multi-role validation (Fund Manager OR Legal Council) with proper claims checking
- 🔲 Business logic branching: Conditional workflow execution based on authenticated user role in Command Handler
- 🔲 Error handling: Role-specific validation failures and unauthorized access scenarios
- **Implementation**: Generic Repository Pattern following CleanArchitectureSteps.md guide with IRepositoryManager facade
- **Note**: Fund Code Generation Service removed - using auto-increment ID column instead


### [JDWA-188] & [JDWA-185] Update fund data (Unified Endpoint) 🔲 PENDING

**Backend Development Focus:**
- 🔲 Database schema optimization for fund data updates with comprehensive change tracking
- 🔲 Repository operations: `UpdateAsync<Fund>` and fund-specific methods with unified validation and business rules
- 🔲 CQRS Commands: `UpdateFundCommand` and `CompleteFundCommand` with intelligent fund update workflow:
  - **Completion Scenario**: Fund status "under construction" → transition to "waiting for members"
  - **Edit Scenario**: Any other status → maintain current status with data updates
- 🔲 API endpoints: `PUT /api/Funds/Update` and `PUT /api/Funds/Complete` with scenario-based processing
- 🔲 Command Handlers with scenario detection logic: Automatically determine operation type based on current fund status
- 🔲 Status management: Dynamic status transitions using state machine pattern in Command Handlers:
  - **Status Transition**: "under construction" funds automatically move to "waiting for members"
  - **Status Preservation**: All other statuses remain unchanged during updates
- 🔲 File management: TNC document replacement with version control and rollback capabilities
- 🔲 User assignment change detection: Advanced change tracking for fund managers and board secretaries:
  - **Addition Detection**: Identify newly assigned users and trigger welcome notifications
  - **Removal Detection**: Identify removed users and trigger relief notifications
  - **Modification Detection**: Track role changes and permission updates
- 🔲 Notification system integration with intelligent notification logic:
  - **Completion Notifications**: Multi-scenario notification templates based on user changes
  - **Edit Notifications**: Conditional notifications only when significant changes occur
  - **Role-based Recipients**: Dynamic recipient lists based on current fund membership
- 🔲 Audit logging: Comprehensive change tracking with before/after state comparison:
  - **Action Classification**: Distinguish between completion and edit operations
  - **Change Details**: Log specific field changes, user assignments, and status transitions
  - **User Context**: Track user details, role information, and operation timestamp
- 🔲 Authorization: Legal Council role validation with fund access permissions and ownership verification
- 🔲 Data validation: Complex business rules with scenario-specific validation using FluentValidation:
  - **Completion Validation**: Ensure all required fields for fund completion
  - **Edit Validation**: Validate data integrity and business rule compliance
  - **Change Validation**: Verify user assignment changes and role permissions
- 🔲 Transaction management: Atomic operations ensuring data consistency across multiple entities through Generic Repository
- 🔲 Concurrency handling: Optimistic concurrency control with conflict resolution using EF Core
- 🔲 Error handling: Scenario-specific error responses and validation failure management
- 🔲 Integration with existing fund management, notification, and user repositories through IRepositoryManager
- 🔲 Performance optimization: Efficient change detection and minimal database operations
- **Implementation**: Generic Repository Pattern following CleanArchitectureSteps.md guide with IRepositoryManager facade


### [JDWA-181] & [JDWA-278] Navigate and Search funds (Unified Endpoint) 🔲 PENDING

**Backend Development Focus:**
- 🔲 Database queries for user-specific fund retrieval with comprehensive filtering and search capabilities
- 🔲 Repository operations: Fund-specific query methods in `IFundRepository` for complex search and navigation
- 🔲 CQRS query: `GetUserFundsQuery` with unified search, filtering, and categorization logic
- 🔲 Query handler implementing intelligent fund retrieval with multiple operation modes using IRepositoryManager:
  - **Navigation Mode**: Default fund listing with strategy categorization when no search criteria provided
  - **Search Mode**: Full-text search and advanced filtering when search parameters are present
  - **Hybrid Mode**: Combined search with strategy grouping for enhanced user experience
- 🔲 API endpoint: `GET /api/Funds` with unified query parameters supporting both navigation and search:
  - **Navigation Parameters**: Strategy grouping, sorting, pagination
  - **Search Parameters**: Fund name search, advanced filters, relevance scoring
  - **Combined Parameters**: Search with strategy categorization and sorting options
- 🔲 Search logic implementation with multiple search strategies in Query Handler:
  - **Simple Search**: Fund name matching with fuzzy search and relevance scoring
  - **Advanced Search**: Multi-criteria filtering with AND logic implementation
  - **Full-text Search**: Comprehensive search across fund properties with indexing
- 🔲 Business logic for fund access control based on user roles (manager, legal council, board secretary, member)
- 🔲 Data aggregation with flexible presentation modes:
  - **Strategy Grouping**: Fund categorization by strategy with count statistics (navigation mode)
  - **Search Results**: Flat list with relevance scoring and highlighting (search mode)
  - **Hybrid Presentation**: Search results grouped by strategy with expanded/collapsed panels
- 🔲 Database indexing strategy for optimal performance:
  - **Navigation Indexes**: User role, strategy, and last action date indexes
  - **Search Indexes**: Full-text indexes on fund names and descriptions
  - **Composite Indexes**: Multi-column indexes for complex query optimization
- 🔲 Authorization: Multi-role access validation with proper claims checking and fund visibility control
- 🔲 Performance optimization with query intelligence:
  - **Efficient Navigation**: Optimized queries for strategy grouping and fund listing
  - **Fast Search**: Indexed search queries with pagination and result limiting
  - **Smart Caching**: Context-aware caching based on operation mode
- 🔲 Data validation and sanitization:
  - **Search Parameter Validation**: Input sanitization and query parameter validation
  - **Filter Validation**: Advanced filter criteria validation and security checks
  - **Pagination Validation**: Page size limits and boundary checking
- 🔲 Error handling with context-aware responses:
  - **No Funds Scenarios**: Graceful handling of empty fund lists with appropriate messages
  - **Search Timeout**: Search operation timeout handling with fallback strategies
  - **Invalid Filters**: Advanced filter validation errors with helpful error messages
- 🔲 Localization: Multi-language support for fund names, strategy names, and search results
- 🔲 Integration with existing repositories through IRepositoryManager:
  - **User Management**: Role-based fund access and permission validation
  - **Fund Repository**: Unified data access with search and navigation capabilities
- 🔲 Advanced features:
  - **Search Suggestions**: Auto-complete and search suggestion capabilities
  - **Recent Searches**: User search history tracking and quick access
  - **Saved Filters**: User-specific saved search criteria and quick filters
- **Implementation**: Generic Repository Pattern following CleanArchitectureSteps.md guide with IRepositoryManager facade

### [JDWA-274], [JDWA-275] & [JDWA-453] Navigate Fund details with Status History and Notifications (Comprehensive Endpoint) 🔲 PENDING

**Backend Development Focus:**
- 🔲 Database schema optimization for comprehensive fund details with integrated status history and notification tracking
- 🔲 Repository operations: Complex fund details queries in `IFundRepository` with related data loading
- 🔲 CQRS query: `GetFundDetailsQuery` with comprehensive fund information, status history, and notification retrieval
- 🔲 Query handler implementing unified fund details aggregation with integrated tabs functionality using IRepositoryManager:
  - **Core Fund Details**: Basic fund information, activities, members, documents, and current status
  - **Status History Integration**: Chronological status changes with user details and timestamps
  - **Notifications Integration**: Fund-specific notifications with LIFO ordering and activity filtering
  - **Tabbed Data Structure**: Complete support for side panel with notifications and status history tabs
- 🔲 API endpoint: `GET /api/Funds/{id}/Details` with comprehensive response model including all tab data
- 🔲 Enhanced response model structure supporting complete tabbed interface:
  - **Fund Details**: Core fund information and activity cards with member requirement validation
  - **Notifications Tab**: Fund-specific notifications displayed as cards, ordered DESC by date (LIFO)
  - **Status History Tab**: Complete status change timeline with user context and transition metadata
  - **Activity Counters**: Fund activity notification counters for filtering and display
- 🔲 Business logic for comprehensive fund access validation in Query Handler:
  - **Fund Access Control**: Role-based validation for fund membership and permissions
  - **Notification Access**: User-specific fund notification filtering and access control
  - **Status History Access**: Role-based access validation for fund status timeline
  - **Activity Filtering**: Fund activity notification counter filtering and categorization
- 🔲 Authorization: Multi-layered access control with fund membership validation for details, notifications, and status history
- 🔲 Data aggregation with optimized loading strategies through Generic Repository:
  - **Fund Core Data**: Activities, members, documents, and current status information
  - **Notification Data**: Fund-specific notifications with user targeting and activity categorization
  - **Status History Data**: Chronological status changes with user details, timestamps, and transition reasons
  - **Activity Enablement**: Business rules for activity availability based on member count and status
  - **Counter Management**: Real-time notification counters for fund and activity-specific notifications
- 🔲 Performance optimization with efficient data loading:
  - **Single Query Approach**: Minimize database calls by loading details, notifications, and history together
  - **Lazy Loading Options**: Optional tab data loading based on client requirements and user preferences
  - **Indexed Queries**: Comprehensive indexing on fund ID, notification dates, status changes, and user relationships
  - **Pagination Support**: Efficient pagination for notifications and status history within tabs
- 🔲 Database schema enhancements:
  - **FundStatusHistory Entity**: Comprehensive status tracking with user context and transition metadata
  - **FundNotifications Entity**: Fund-specific notification tracking with activity categorization and user targeting
  - **Audit Trail Integration**: Leverage existing audit system for status changes and notification tracking
  - **Relationship Optimization**: Efficient foreign key relationships for notifications, status history, and fund queries
- 🔲 Notification filtering and management:
  - **Activity-based Filtering**: Filter notifications by fund activity (Resolutions, Members, Meetings, Assessments, Documents)
  - **Counter Integration**: Fund activity notification counter filtering and real-time updates
  - **LIFO Ordering**: Notifications displayed in Last-In-First-Out order with proper date sorting
  - **User-specific Filtering**: Show only notifications sent to the logged-in user for the specific fund
- 🔲 Error handling with comprehensive scenarios:
  - **Fund Not Found**: Graceful handling of invalid fund IDs with appropriate error messages
  - **Access Denied**: Role-based access control for fund details, notifications, and status history
  - **Data Loading Errors**: Fallback strategies for partial data loading failures with tab-specific error handling
  - **Empty Data Scenarios**: Proper handling of funds with no notifications or status history
- 🔲 Audit trail enhancement:
  - **Access Logging**: Track fund details viewing with user context and tab access patterns
  - **Notification Access**: Log notification viewing for compliance and user activity tracking
  - **Status History Access**: Log status history viewing for audit and compliance requirements
  - **Combined Audit**: Unified audit trail for complete fund details and tab interaction tracking
- 🔲 Integration with existing repositories through IRepositoryManager:
  - **Notification Repository**: Real-time notification integration with SignalR for live updates
  - **Member Management**: Member information and activity enablement logic with role validation
  - **Document Repository**: Document listing and access control with fund-specific permissions
  - **User Management**: User details for notifications, status history, and audit information
  - **Counter Management**: Real-time notification counter management and synchronization
- 🔲 Real-time updates and synchronization:
  - **Live Notification Updates**: Real-time notification list updates using SignalR for fund details screen
  - **Counter Synchronization**: Real-time notification counter updates when new notifications arrive
  - **Status Change Updates**: Live status history updates when fund status changes occur
  - **Activity Updates**: Real-time activity enablement updates based on member changes
- 🔲 Localization support with comprehensive notification localization:
  - **Multi-language Fund Data**: Arabic/English support for fund information and descriptions
  - **Notification Content Localization**:
    - Localized notification messages with parameter substitution for fund names, user names, and dates
    - Activity-specific notification templates in both Arabic and English
    - Dynamic message generation based on user's preferred language from JWT claims
    - Localized notification subjects and content with proper RTL/LTR text direction support
  - **Status Descriptions**: Localized status change descriptions and transition reasons with notification integration
  - **User Interface Text**: Localized tab labels, buttons, and interface elements
  - **Notification Template System**: Multi-language notification templates with resource file integration
  - **Cultural Formatting**: Date, time, and number formatting based on user's cultural preferences
  - **Message Interpolation**: Dynamic parameter replacement in notification messages with localized formatting
- 🔲 Advanced features:
  - **Notification Filtering**: Advanced filtering options for notifications by activity type and date range
  - **Status Transition Visualization**: Timeline view of status changes with visual indicators and user context
  - **Activity Counter Display**: Visual representation of fund activity notification counters
  - **Export Capabilities**: Comprehensive export functionality for notifications, status history, and fund details
  - **Search within Tabs**: Search functionality within notifications and status history for large datasets
- **Implementation**: Generic Repository Pattern following CleanArchitectureSteps.md guide with IRepositoryManager facade

## Phase 2: Notifications Framework

### [JDWA-452] Send a Notification 🔲 PENDING

**Backend Development Focus:**
- 🔲 Database schema for `Notifications` entity with activity categorization and user targeting
- 🔲 Repository interface: `INotificationRepository` inheriting from `IGenericRepository` for notification operations
- 🔲 CQRS command: `SendNotificationCommand` with multi-recipient support
- 🔲 Command handler implementing notification creation and distribution logic using IRepositoryManager
- 🔲 API endpoint: `POST /api/Notifications/Send` with batch notification support
- 🔲 Business logic for notification activity determination (General, Fund, Resolutions, Members, Meetings, Assessments, Documents)
- 🔲 Counter management: Update user notification counters (general, fund-specific, activity-specific)
- 🔲 Real-time notification delivery using SignalR integration with Redis backplane
- 🔲 Notification templates with comprehensive localization:
  - **Multi-language Template System**: Separate templates for Arabic and English with resource file management
  - **Parameter Substitution**: Dynamic parameter replacement with localized formatting (dates, names, amounts)
  - **Template Versioning**: Version control for notification templates with backward compatibility
  - **Cultural Formatting**: Locale-specific formatting for dates, times, numbers, and currency
  - **RTL/LTR Support**: Proper text direction handling for Arabic and English content
  - **Message Interpolation**: Smart parameter injection with gender-aware and context-sensitive translations
- 🔲 User targeting: Role-based notification recipient determination with language preference detection
- 🔲 Authorization: System-level notification sending with proper service authentication
- 🔲 Audit logging: Notification sending tracking with recipient details and language information
- 🔲 Error handling: Failed delivery scenarios and retry mechanisms with localized error messages
- 🔲 Integration with Redis for real-time push notifications and counter management
- 🔲 Performance optimization: Batch notification processing for multiple recipients with language grouping
- **Implementation**: Generic Repository Pattern following CleanArchitectureSteps.md guide with IRepositoryManager facade

### [JDWA-454] Receive a Notification 🔲 PENDING

**Backend Development Focus:**
- 🔲 Repository operations: User-specific notification queries in `INotificationRepository` with filtering capabilities
- 🔲 CQRS query: `GetUserNotificationsQuery` with real-time notification retrieval
- 🔲 Query handler implementing user-specific notification fetching with filtering using IRepositoryManager
- 🔲 API endpoints: `GET /api/Notifications/User` and `GET /api/Notifications/Unread`
- 🔲 Real-time notification delivery using SignalR hubs with user-specific connections
- 🔲 Counter update logic: Automatic counter increments for general, fund, and activity notifications
- 🔲 Notification state management: Read/unread status tracking with timestamps
- 🔲 Authorization: User-specific notification access with JWT token validation
- 🔲 Caching strategy: User notification caching with real-time cache updates
- 🔲 Push notification integration: Browser push notifications for real-time delivery with localized content
- 🔲 Business logic for notification display rules based on user context (screen location) and language preferences
- 🔲 Localization integration for real-time notifications:
  - **Dynamic Language Detection**: Automatic language selection based on user JWT claims and browser settings
  - **Real-time Template Rendering**: On-the-fly notification content generation in user's preferred language
  - **Localized Push Notifications**: Browser push notifications with proper Arabic/English content and RTL support
  - **Cultural Date/Time Display**: Localized timestamp formatting for notification display
- 🔲 Error handling: Connection failures and notification delivery errors with localized error messages
- 🔲 Integration with Redis for real-time notification distribution and counter management
- 🔲 Performance optimization: Efficient notification queries with user-based indexing and language caching
- **Implementation**: Generic Repository Pattern following CleanArchitectureSteps.md guide with IRepositoryManager facade



## Implementation Guidelines

### Naming Conventions

#### Entity and Model Naming
- **Entities**: Use singular PascalCase names (e.g., `Fund`, `FundStrategy`, `User`)
- **Entity Properties**: PascalCase for all properties (e.g., `NameAr`, `NameEn`, `CreatedDate`)
- **Navigation Properties**: Use descriptive names matching the relationship (e.g., `FundStrategy`, `FundUsers`)
- **Collection Properties**: Use plural names (e.g., `ICollection<FundUser> FundUsers`)
- **Enum Values**: PascalCase with descriptive names (e.g., `FundStatus.UnderConstruction`, `FundStatus.WaitingForMembers`)

#### Database Naming
- **Table Names**: Singular PascalCase matching entity names (e.g., `Fund`, `FundStrategy`)
- **Column Names**: PascalCase matching property names (e.g., `NameAr`, `NameEn`, `FundStrategyId`)
- **Foreign Keys**: EntityName + "Id" format (e.g., `FundStrategyId`, `UserId`)
- **Indexes**: `IX_TableName_ColumnName` format (e.g., `IX_Fund_FundStrategyId`)
- **Constraints**: `CK_TableName_ColumnName` for check constraints, `UK_TableName_ColumnName` for unique

#### CQRS Pattern Naming
- **Commands**: Verb + Entity + "Command" (e.g., `CreateFundCommand`, `UpdateFundDataCommand`)
- **Queries**: "Get" + Entity/Description + "Query" (e.g., `GetFundDetailsQuery`, `GetUserFundsQuery`)
- **Handlers**: Command/Query name + "Handler" (e.g., `CreateFundCommandHandler`, `GetFundDetailsQueryHandler`)
- **Validators**: Command/Query name + "Validator" (e.g., `CreateFundCommandValidator`)

#### API and Controller Naming
- **Controllers**: Entity + "Controller" (e.g., `FundsController`, `FundStrategiesController`)
- **Action Methods**: HTTP verb style (e.g., `GetFunds`, `CreateFund`, `UpdateFund`, `DeleteFund`)
- **Route Templates**: Kebab-case for URLs (e.g., `/api/funds`, `/api/fund-strategies`)
- **Parameter Names**: camelCase in URLs (e.g., `{fundId}`, `{strategyId}`)

#### Repository and Manager Naming
- **Repository Interfaces**: "I" + EntityName + "Repository" (e.g., `IFundRepository`, `INotificationRepository`, `IFundStrategyRepository`)
- **Repository Implementations**: EntityName + "Repository" (e.g., `FundRepository`, `NotificationRepository`, `FundStrategyRepository`)
- **Repository Manager Interface**: `IRepositoryManager` with entity-specific properties
- **Repository Manager Implementation**: `ProductRepositoryManager` implementing `IRepositoryManager`
- **Repository Methods**: Generic methods from `IGenericRepository` (e.g., `GetByIdAsync<T>`, `AddAsync<T>`, `UpdateAsync<T>`)
- **Custom Repository Methods**: Entity-specific methods when needed (e.g., `GetFundsByUserRoleAsync`, `GetNotificationsByActivityAsync`)

#### File and Folder Structure
- **Entities**: `Core/Domain/Entities/` (e.g., `Fund.cs`, `FundStrategy.cs`)
- **DTOs**: `Application/Features/EntityName/Dtos/` with entity grouping (e.g., `Features/Funds/Dtos/CreateFundDto.cs`)
- **Commands/Queries**: `Application/Features/EntityName/Commands|Queries/` (e.g., `Features/Funds/Commands/CreateFundCommand.cs`)
- **Controllers**: `Infrastructure/Presentation/Controllers/` (e.g., `FundsController.cs`)
- **Repository Interfaces**: `Core/Abstraction/Contract/Repository/DomainArea/` (e.g., `Contract/Repository/Funds/IFundRepository.cs`)
- **Repository Implementations**: `Infrastructure/Infrastructure/Repository/DomainArea/` (e.g., `Repository/Funds/FundRepository.cs`)
- **Repository Manager**: `Infrastructure/Infrastructure/Repository/RepositoryManager.cs` (ProductRepositoryManager class)

#### Localization Naming
- **Resource Keys**: Entity_Property_Language format (e.g., `Fund_Name_Ar`, `Fund_Status_En`)
- **Template Keys**: Action_Entity_MessageType (e.g., `Create_Fund_Success`, `Update_Fund_Error`)
- **Notification Templates**: NotificationType_Entity_Action (e.g., `Email_Fund_Created`, `Push_Fund_StatusChanged`)

#### Constants and Configuration
- **Constants**: UPPER_SNAKE_CASE (e.g., `MAX_FUND_NAME_LENGTH`, `DEFAULT_PAGE_SIZE`)
- **Configuration Sections**: PascalCase (e.g., `JwtSettings`, `RedisConfiguration`)
- **Environment Variables**: UPPER_SNAKE_CASE (e.g., `DATABASE_CONNECTION_STRING`, `REDIS_HOST`)

### Database Design Principles
- Follow existing audit trail patterns using `FullAuditedEntity` base class
- Implement proper foreign key relationships with cascade rules
- Use appropriate indexing strategies for query performance
- Maintain data consistency with proper transaction boundaries

### API Design Standards
- Follow RESTful conventions with proper HTTP status codes
- Implement consistent request/response models using DTOs
- Use standardized error responses with `BaseResponse<T>` pattern
- Apply proper versioning strategy for API evolution

### Security Implementation
- Implement JWT-based authentication with refresh token support
- Use claims-based authorization for role and permission validation
- Apply input validation and sanitization for all endpoints
- Implement rate limiting and request throttling for API protection

### Performance Optimization
- Implement efficient database queries with proper indexing
- Use pagination for large data sets
- Apply lazy loading strategies for related entities
- **Note**: Caching tasks removed from current implementation scope

### Generic Repository Pattern Implementation

#### Repository Architecture
- **IGenericRepository**: Base interface providing standard CRUD operations (`GetAll<T>`, `GetByIdAsync<T>`, `AddAsync<T>`, `UpdateAsync<T>`, `DeleteAsync<T>`)
- **GenericRepository**: Base implementation with Entity Framework Core integration and transaction support
- **Entity-Specific Repositories**: Inherit from `IGenericRepository` for specialized operations (e.g., `IFundRepository`, `INotificationRepository`)
- **IRepositoryManager**: Facade interface providing access to all entity repositories
- **ProductRepositoryManager**: Implementation of `IRepositoryManager` using lazy loading pattern

#### Handler Implementation Pattern
```csharp
public class CreateFundCommandHandler : BaseResponseHandler, ICommandHandler<CreateFundCommand, BaseResponse<string>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public CreateFundCommandHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<string>> Handle(CreateFundCommand request, CancellationToken cancellationToken)
    {
        var fund = _mapper.Map<Fund>(request);
        var result = await _repository.Funds.AddAsync(fund);
        return Success("Fund created successfully");
    }
}
```

#### Repository Registration
- Register `IGenericRepository` and `GenericRepository` in DI container
- Register `IRepositoryManager` and `ProductRepositoryManager` in DI container
- Entity-specific repositories are created lazily through `ProductRepositoryManager`

### Integration Patterns
- Follow CQRS pattern with MediatR for command/query separation
- Implement pipeline behaviors for cross-cutting concerns (validation, logging, caching)
- Use dependency injection for repository layer integration through IRepositoryManager facade
- Apply Generic Repository Pattern with IRepositoryManager for data access abstraction
- Implement Command/Query Handlers using IRepositoryManager instead of direct repository injection
- Follow facade pattern with ProductRepositoryManager for centralized repository access

### Notification Localization Framework
- Implement comprehensive multi-language notification system with Arabic/English support
- **Template Management**: Resource-based notification templates with version control and fallback mechanisms
- **Dynamic Content Generation**: Real-time notification content generation based on user language preferences
- **Parameter Localization**: Smart parameter substitution with cultural formatting for dates, numbers, and names
- **RTL/LTR Support**: Proper text direction handling for Arabic content in notifications and UI elements
- **Cultural Formatting**: Locale-specific formatting for timestamps, currency, and numeric values in notifications
- **Language Detection**: Automatic language preference detection from JWT claims, user profile, and browser settings
- **Template Caching**: Efficient caching of localized notification templates with language-based cache keys
- **Fallback Strategy**: Graceful fallback to default language when translations are missing
- **Notification Subjects**: Localized notification subjects with proper encoding for email and push notifications

### Monitoring and Logging
- Implement comprehensive audit logging for all operations with localization context
- Use structured logging with NLog for better observability including language and cultural information
- Track performance metrics for critical operations including notification localization performance
- Monitor notification delivery success rates and failures with language-specific metrics
- Log localization errors and template rendering failures for debugging and improvement
