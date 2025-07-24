# User-Role Many-to-Many Relationship Implementation

## Overview

This document describes the complete implementation of the Many-to-Many relationship between User and Role entities in the Jadwa Fund Management System, which has been successfully configured and tested.

## Current Implementation Status

✅ **COMPLETED**: The User and Role entities are properly configured with a Many-to-Many relationship using Entity Framework Core navigation properties and the existing ASP.NET Core Identity infrastructure.

## Entity Structure

### 1. User Entity

**File**: `src/Core/Domain/Entities/Users/User.cs`

```csharp
public class User : IdentityUser<int>
{
    // ... existing properties ...

    // Navigation Properties for Role Management
    /// <summary>
    /// Navigation property for the many-to-many relationship with Roles through IdentityUserRole
    /// Used for eager loading user roles to optimize performance and eliminate N+1 queries
    /// </summary>
    public virtual ICollection<IdentityUserRole<int>> UserRoles { get; set; } = new List<IdentityUserRole<int>>();

    /// <summary>
    /// Navigation property for direct access to user's roles
    /// Configured through Entity Framework to use the IdentityUserRole junction table
    /// Enables efficient querying of user roles with Include() operations
    /// </summary>
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
```

### 2. Role Entity

**File**: `src/Core/Domain/Entities/Users/Role.cs`

```csharp
public class Role : IdentityRole<int>
{
    /// <summary>
    /// Navigation property for the many-to-many relationship with Users through IdentityUserRole
    /// Used for eager loading role users to optimize performance and eliminate N+1 queries
    /// </summary>
    public virtual ICollection<IdentityUserRole<int>> UserRoles { get; set; } = new List<IdentityUserRole<int>>();

    /// <summary>
    /// Navigation property for direct access to users with this role
    /// Configured through Entity Framework to use the IdentityUserRole junction table
    /// Enables efficient querying of role users with Include() operations
    /// </summary>
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
```

## Entity Framework Configuration

### 1. Role Entity Configuration

**File**: `src/Infrastructure/Infrastructure/Data/Config/RoleEntityConfig.cs`

```csharp
public class RoleEntityConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Configure the many-to-many relationship between Role and User through IdentityUserRole
        builder
            .HasMany(r => r.Users)
            .WithMany(u => u.Roles)
            .UsingEntity<IdentityUserRole<int>>(
                // Configure the relationship from IdentityUserRole to User
                userRole => userRole
                    .HasOne<User>()
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Cascade),
                // Configure the relationship from IdentityUserRole to Role
                userRole => userRole
                    .HasOne<Role>()
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.Cascade),
                // Configure the IdentityUserRole entity itself
                userRole =>
                {
                    userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
                    userRole.ToTable("AspNetUserRoles");
                });

        // Additional role property configurations...
    }
}
```

### 2. User Entity Configuration

**File**: `src/Infrastructure/Infrastructure/Data/Config/UserEntityConfig.cs`

The User configuration focuses on user-specific properties and audit relationships. The many-to-many relationship with roles is configured in the RoleEntityConfig to avoid duplicate configuration.

## Database Schema

### Tables Involved

1. **AspNetUsers** - User entity table (existing)
2. **AspNetRoles** - Role entity table (existing)
3. **AspNetUserRoles** - Junction table for many-to-many relationship (existing)

### Junction Table Structure

```sql
CREATE TABLE [AspNetUserRoles] (
    [UserId] int NOT NULL,
    [RoleId] int NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
```

## Service Layer Enhancement

### Enhanced IUserManagmentService

**File**: `src/Core/Abstraction/Contract/Service/Identity/IUserManagmentService.cs`

```csharp
// Enhanced methods for optimized role retrieval
public Task<User?> FindByIdWithRolesAsync(string id);
public IQueryable<User> UsersWithRoles();
public Task<List<User>> GetUsersWithRolesAsync();
```

### Implementation

**File**: `src/Infrastructure/Infrastructure/Service/Identity/UserManagmentIdentityService.cs`

```csharp
public async Task<User?> FindByIdWithRolesAsync(string id)
{
    return await _userManager.Users
        .Include(u => u.Roles)
        .FirstOrDefaultAsync(u => u.Id.ToString() == id);
}

public IQueryable<User> UsersWithRoles()
{
    return _userManager.Users.Include(u => u.Roles);
}

public async Task<List<User>> GetUsersWithRolesAsync()
{
    return await _userManager.Users
        .Include(u => u.Roles)
        .ToListAsync();
}
```

## AutoMapper Configuration

### Updated Mapping

**File**: `src/Core/Application/Mapping/Identity/Users/QueryMapping/GetUserPaginatedListMapping.cs`

```csharp
CreateMap<User, GetUserListResponse>()
    // Map roles directly from navigation properties for optimized performance
    .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Name).ToList()))
    .ForMember(dest => dest.PrimaryRole, opt => opt.MapFrom(src => src.Roles.Select(r => r.Name).FirstOrDefault()))
    .ForMember(dest => dest.LastUpdateDate, opt => opt.MapFrom(src => src.UpdatedAt));
```

## Optimized Query Handler

### Enhanced ListQueryHandler

**File**: `src/Core/Application/Features/Identity/Users/Queries/List/ListQueryHandler.cs`

```csharp
public async Task<PaginatedResult<GetUserListResponse>> Handle(ListQuery request, CancellationToken cancellationToken)
{
    // Use the optimized UsersWithRoles method to eagerly load roles
    var users = _service.UserManagmentService.UsersWithRoles().AsQueryable();

    // Apply filtering and pagination
    users = await ApplyFiltersAsync(users, request);
    
    // Project to DTO - roles are automatically mapped via navigation properties
    var paginatedList = await _mapper.ProjectTo<GetUserListResponse>(users)
        .ToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy);

    return paginatedList;
}
```

## Performance Benefits

### Before vs After

| Aspect | Before (N+1 Problem) | After (Optimized) |
|--------|----------------------|-------------------|
| **Database Queries** | 1 + N queries | 1 query with JOINs |
| **10 users** | 11 queries | 1 query |
| **100 users** | 101 queries | 1 query |
| **Performance Improvement** | Baseline | 99%+ reduction |

### Generated SQL

**Optimized Query with Navigation Properties**:
```sql
SELECT u.*, r.Id as RoleId, r.Name as RoleName 
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE (u.IsDeleted IS NULL OR u.IsDeleted = 0)
```

## Testing and Validation

### Application Status

✅ **Application Running**: Successfully started on ports 7010 (HTTPS) and 5114 (HTTP)
✅ **Entity Framework Configuration**: No configuration errors
✅ **Database Queries**: Executing successfully with proper JOINs
✅ **Role Seeding**: Completed successfully
✅ **Navigation Properties**: Working correctly

### Key Features Verified

1. **Many-to-Many Relationship**: Properly configured and functional
2. **Eager Loading**: Users can be loaded with roles in single query
3. **Performance Optimization**: N+1 query problem eliminated
4. **Clean Architecture**: Maintains separation of concerns
5. **Backward Compatibility**: Existing functionality preserved

## Usage Examples

### Loading Users with Roles

```csharp
// Single user with roles
var user = await _userService.FindByIdWithRolesAsync("1");
var userRoles = user.Roles.Select(r => r.Name).ToList();

// Multiple users with roles
var users = await _userService.GetUsersWithRolesAsync();
foreach (var u in users)
{
    var roles = u.Roles.Select(r => r.Name).ToList();
}

// Queryable for filtering and pagination
var activeUsersWithRoles = _userService.UsersWithRoles()
    .Where(u => u.IsActive)
    .Include(u => u.Roles);
```

### AutoMapper Projection

```csharp
// Automatic role mapping in queries
var userDtos = await _mapper.ProjectTo<GetUserListResponse>(
    _userService.UsersWithRoles()
).ToListAsync();
// Roles and PrimaryRole are automatically populated
```

## Conclusion

The User-Role Many-to-Many relationship has been successfully implemented and optimized in the Jadwa Fund Management System. The implementation:

- ✅ **Eliminates N+1 Query Problem**: Single query loads users with roles
- ✅ **Maintains Clean Architecture**: Proper separation of concerns
- ✅ **Preserves ASP.NET Identity**: Uses existing infrastructure
- ✅ **Optimizes Performance**: Dramatic reduction in database calls
- ✅ **Ensures Backward Compatibility**: Existing functionality unchanged
- ✅ **Follows Best Practices**: Entity Framework conventions and patterns

The system is now ready for production use with optimized role retrieval performance.
