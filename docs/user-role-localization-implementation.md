# User Role Localization Implementation

## Overview

This document describes the implementation of localization for user roles in the `ListQueryHandler` class, enabling role names to be displayed in both Arabic and English based on the user's preferred language.

## Implementation Summary

### 1. LocalizationHelper Enhancement

**File**: `src/Core/Application/Common/Helpers/LocalizationHelper.cs`

Added a new method `GetUserRoleDisplay` that maps database role names to localized display names:

```csharp
/// <summary>
/// Gets localized display text for user role names
/// Maps database role names to localized display names based on user's preferred language
/// </summary>
/// <param name="roleName">Role name from database</param>
/// <param name="localizer">String localizer instance</param>
/// <returns>Localized role name display text</returns>
public static string GetUserRoleDisplay(string roleName, IStringLocalizer<SharedResources> localizer)
{
    if (string.IsNullOrEmpty(roleName))
        return roleName;

    // Convert to lowercase for consistent comparison
    var roleNameLower = roleName.ToLower();

    return roleNameLower switch
    {
        "fundmanager" => localizer[SharedResourcesKey.FundManager],
        "legalcouncil" => localizer[SharedResourcesKey.LegalCouncil],
        "boardsecretary" => localizer[SharedResourcesKey.BoardSecretary],
        "boardmember" => localizer[SharedResourcesKey.BoardMember],
        "superadmin" => localizer[SharedResourcesKey.SuperAdmin],
        "admin" => localizer[SharedResourcesKey.Admin],
        "basic" => localizer[SharedResourcesKey.Basic],
        "user" => localizer[SharedResourcesKey.User],
        "financecontroller" => localizer[SharedResourcesKey.FinanceController],
        "compliancelegalmanagingdirector" => localizer[SharedResourcesKey.ComplianceLegalManagingDirector],
        "headofrealestate" => localizer[SharedResourcesKey.HeadOfRealEstate],
        "associatefundmanager" => localizer[SharedResourcesKey.AssociateFundManager],
        _ => roleName // Return original name if no localization found
    };
}
```

### 2. ListQueryHandler Enhancement

**File**: `src/Core/Application/Features/Identity/Users/Queries/List/ListQueryHandler.cs`

Enhanced the ListQueryHandler with post-mapping localization:

```csharp
// In Handle method - apply localization after AutoMapper projection
var paginatedList = await _mapper.ProjectTo<GetUserListResponse>(users).ToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy);

// Apply localization to role names
ApplyRoleLocalization(paginatedList.Data);
```

Added a new private method for role localization:

```csharp
/// <summary>
/// Apply localization to role names in user response DTOs
/// Converts database role names to localized display names based on user's preferred language
/// </summary>
/// <param name="userResponses">List of user response DTOs to localize</param>
private void ApplyRoleLocalization(IEnumerable<GetUserListResponse> userResponses)
{
    foreach (var userResponse in userResponses)
    {
        // Localize all role names
        if (userResponse.Roles != null && userResponse.Roles.Count > 0)
        {
            userResponse.Roles = [.. userResponse.Roles
                .Select(roleName => LocalizationHelper.GetUserRoleDisplay(roleName, _localizer))];
        }

        // Localize primary role name
        if (!string.IsNullOrEmpty(userResponse.PrimaryRole))
        {
            userResponse.PrimaryRole = LocalizationHelper.GetUserRoleDisplay(userResponse.PrimaryRole, _localizer);
        }
    }
}
```

Key changes:
- Added using statement for `Application.Common.Helpers`
- Updated `HandleRoleBasedSorting` method to apply localization before sorting
- Added `ApplyRoleLocalization` method for consistent role name localization
- Applied localization in both regular listing and role-based sorting scenarios

## Localization Keys

All role localization keys are already defined in `SharedResourcesKey.cs`:

```csharp
// User Role Localization Keys
public const string FundManager = "FundManager";
public const string LegalCouncil = "LegalCouncil";
public const string BoardSecretary = "BoardSecretary";
public const string BoardMember = "BoardMember";
public const string SuperAdmin = "SuperAdmin";
public const string Admin = "Admin";
public const string Basic = "Basic";
public const string User = "User";
public const string FinanceController = "FinanceController";
public const string ComplianceLegalManagingDirector = "ComplianceLegalManagingDirector";
public const string HeadOfRealEstate = "HeadOfRealEstate";
public const string AssociateFundManager = "AssociateFundManager";
```

## Resource Files

### English Translations (`SharedResources.en-US.resx`)
- FundManager: "Fund Manager"
- LegalCouncil: "Legal Council"
- BoardSecretary: "Board Secretary"
- BoardMember: "Board Member"
- SuperAdmin: "Super Admin"
- Admin: "Admin"
- Basic: "Basic"
- User: "User"
- FinanceController: "Finance Controller"
- ComplianceLegalManagingDirector: "Compliance and Legal"
- HeadOfRealEstate: "Managing Director, Head Of Real Estate"
- AssociateFundManager: "Fund Manager Associate"

### Arabic Translations (`SharedResources.ar-EG.resx`)
- FundManager: "مدير الصندوق"
- LegalCouncil: "المستشار القانوني/ مدير الحوكمة"
- BoardSecretary: "أمين سر مجلس الإدارة"
- BoardMember: "عضو مجلس إدارة/ رئيس مجلس ادارة"
- SuperAdmin: "مشرف عام"
- Admin: "مشرف"
- Basic: "مستخدم أساسي"
- User: "مستخدم"
- FinanceController: "المراقب المالي"
- ComplianceLegalManagingDirector: "المطابقة والالتزام"
- HeadOfRealEstate: "رئيس إدارة الاستثمارات العقارية"
- AssociateFundManager: "مساعد مدير الصندوق"

## Benefits

1. **Automatic Localization**: Role names are automatically localized based on user's preferred language
2. **Consistent with System Pattern**: Follows the established localization pattern used throughout the Jadwa Fund Management System
3. **Maintainable**: Centralized localization logic in `LocalizationHelper`
4. **Performance Optimized**: Post-mapping localization with minimal overhead
5. **Fallback Support**: Returns original role name if localization fails
6. **Pagination Compatible**: Works seamlessly with the existing pagination system

## Usage

When users view the user list through the API, role names will automatically appear in their preferred language (Arabic/English) as configured in their user profile. No additional changes are required in the frontend or API controllers.

## Testing Considerations

When writing unit tests for the `ListQueryHandler`, mock the `IStringLocalizer<SharedResources>` to return appropriate localized strings based on the test scenario. The existing test patterns in the codebase can be followed for localization testing.

## Files Modified

1. **`src/Core/Application/Common/Helpers/LocalizationHelper.cs`** - Added `GetUserRoleDisplay` method for role localization
2. **`src/Core/Application/Features/Identity/Users/Queries/List/ListQueryHandler.cs`** - Enhanced with `ApplyRoleLocalization` method and post-mapping localization
3. **`docs/user-role-localization-implementation.md`** - Comprehensive implementation documentation

## Implementation Approach

The final implementation uses a **post-mapping localization approach** rather than custom AutoMapper resolvers. This approach:

- **Simplifies dependency injection** by avoiding complex resolver registration
- **Maintains separation of concerns** between mapping and localization
- **Provides better error handling** and debugging capabilities
- **Follows established patterns** in the existing codebase
- **Ensures consistent behavior** across all user list scenarios

The localization is applied after AutoMapper completes the entity-to-DTO mapping, ensuring that role names are properly localized before being returned to the client.
