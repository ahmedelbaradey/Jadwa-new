# Role List Localization Implementation

## Overview

This document describes the implementation of localization for the `GetRoleListQuery` in the Jadwa Fund Management System. The implementation ensures that role names are returned in the user's preferred language (Arabic or English) while maintaining the original role names for internal system operations.

## Implementation Details

### 1. Enhanced GetRoleListQueryHandler

The handler now includes localization support:

```csharp
public class GetRoleListQueryHandler : BaseResponseHandler, IQueryHandler<GetRoleListQuery, BaseResponse<List<GetRoleListResponse>>>
{
    private readonly IIdentityServiceManager _service;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public async Task<BaseResponse<List<GetRoleListResponse>>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
    {
        // Get roles from database
        var roles = await _service.AuthorizationService.GetRoleListAsync();
        
        // Map to response DTOs
        var rolesMapper = _mapper.Map<List<GetRoleListResponse>>(roles);
        
        // Apply localization to role names
        foreach (var roleResponse in rolesMapper)
        {
            // Keep original role name for internal use
            var originalRoleName = roleResponse.roleName;
            
            // Set localized display name
            roleResponse.DisplayName = GetLocalizedRoleName(originalRoleName);
        }

        return Success(rolesMapper);
    }
}
```

### 2. Enhanced GetRoleListResponse

The response now includes both original and localized role names:

```csharp
public record GetRoleListResponse
{
    public int Id { get; set; }
    
    /// <summary>
    /// Original role name from database (for internal use)
    /// </summary>
    public string roleName { get; set; } = null!;
    
    /// <summary>
    /// Localized display name for the role (for UI display)
    /// </summary>
    public string DisplayName { get; set; } = null!;
}
```

### 3. Role Name Mapping

The system maps database role names to localized display names:

```csharp
private string GetLocalizedRoleName(string roleName)
{
    var roleNameLower = roleName.ToLower();

    return roleNameLower switch
    {
        "fundmanager" => _localizer[SharedResourcesKey.FundManager],
        "legalcouncil" => _localizer[SharedResourcesKey.LegalCouncil],
        "boardsecretary" => _localizer[SharedResourcesKey.BoardSecretary],
        "boardmember" => _localizer[SharedResourcesKey.BoardMember],
        "superadmin" => _localizer[SharedResourcesKey.SuperAdmin],
        "admin" => _localizer[SharedResourcesKey.Admin],
        "basic" => _localizer[SharedResourcesKey.Basic],
        "user" => _localizer[SharedResourcesKey.User],
        "financecontroller" => _localizer[SharedResourcesKey.FinanceController],
        "compliancelegalmanagingdirector" => _localizer[SharedResourcesKey.ComplianceLegalManagingDirector],
        "headofrealestate" => _localizer[SharedResourcesKey.HeadOfRealEstate],
        "associatefundmanager" => _localizer[SharedResourcesKey.AssociateFundManager],
        _ => roleName // Return original name if no localization found
    };
}
```

## Localization Resources

### English (SharedResources.en-US.resx)
```xml
<data name="FundManager" xml:space="preserve">
  <value>Fund Manager</value>
</data>
<data name="LegalCouncil" xml:space="preserve">
  <value>Legal Council</value>
</data>
<data name="BoardSecretary" xml:space="preserve">
  <value>Board Secretary</value>
</data>
<data name="BoardMember" xml:space="preserve">
  <value>Board Member</value>
</data>
<data name="SuperAdmin" xml:space="preserve">
  <value>Super Admin</value>
</data>
<data name="Admin" xml:space="preserve">
  <value>Admin</value>
</data>
<data name="Basic" xml:space="preserve">
  <value>Basic</value>
</data>
<data name="User" xml:space="preserve">
  <value>User</value>
</data>
<data name="FinanceController" xml:space="preserve">
  <value>Finance Controller</value>
</data>
<data name="ComplianceLegalManagingDirector" xml:space="preserve">
  <value>Compliance Legal Managing Director</value>
</data>
<data name="HeadOfRealEstate" xml:space="preserve">
  <value>Head of Real Estate</value>
</data>
<data name="AssociateFundManager" xml:space="preserve">
  <value>Associate Fund Manager</value>
</data>
```

### Arabic (SharedResources.ar-EG.resx)
```xml
<data name="FundManager" xml:space="preserve">
  <value>مدير صندوق</value>
</data>
<data name="LegalCouncil" xml:space="preserve">
  <value>المستشار القانوني</value>
</data>
<data name="BoardSecretary" xml:space="preserve">
  <value>سكرتير المجلس</value>
</data>
<data name="BoardMember" xml:space="preserve">
  <value>عضو مجلس الإدارة</value>
</data>
<data name="SuperAdmin" xml:space="preserve">
  <value>مشرف عام</value>
</data>
<data name="Admin" xml:space="preserve">
  <value>مشرف</value>
</data>
<data name="Basic" xml:space="preserve">
  <value>مستخدم أساسي</value>
</data>
<data name="User" xml:space="preserve">
  <value>مستخدم</value>
</data>
<data name="FinanceController" xml:space="preserve">
  <value>مدير المالية</value>
</data>
<data name="ComplianceLegalManagingDirector" xml:space="preserve">
  <value>مدير الادارة</value>
</data>
<data name="HeadOfRealEstate" xml:space="preserve">
  <value>مدير العقارات</value>
</data>
<data name="AssociateFundManager" xml:space="preserve">
  <value>مدير صندوق مساعد</value>
</data>
```

## API Response Examples

### English Response
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "roleName": "fundmanager",
      "displayName": "Fund Manager"
    },
    {
      "id": 2,
      "roleName": "legalcouncil",
      "displayName": "Legal Council"
    },
    {
      "id": 3,
      "roleName": "boardsecretary",
      "displayName": "Board Secretary"
    },
    {
      "id": 4,
      "roleName": "boardmember",
      "displayName": "Board Member"
    }
  ]
}
```

### Arabic Response
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "roleName": "fundmanager",
      "displayName": "مدير صندوق"
    },
    {
      "id": 2,
      "roleName": "legalcouncil",
      "displayName": "المستشار القانوني"
    },
    {
      "id": 3,
      "roleName": "boardsecretary",
      "displayName": "سكرتير المجلس"
    },
    {
      "id": 4,
      "roleName": "boardmember",
      "displayName": "عضو مجلس الإدارة"
    }
  ]
}
```

## Frontend Integration

### JavaScript Example
```javascript
// Fetch localized role list
const fetchRoles = async () => {
    try {
        const response = await fetch('/api/Users/Authorization/RoleList', {
            headers: {
                'Accept-Language': 'ar-EG', // or 'en-US'
                'Authorization': `Bearer ${accessToken}`
            }
        });
        
        const result = await response.json();
        
        if (result.success) {
            // Use displayName for UI, roleName for form values
            result.data.forEach(role => {
                console.log(`Display: ${role.displayName}, Value: ${role.roleName}`);
            });
        }
    } catch (error) {
        console.error('Error fetching roles:', error);
    }
};
```

### React Component Example
```jsx
const RoleSelector = () => {
    const [roles, setRoles] = useState([]);
    
    useEffect(() => {
        fetchRoles().then(setRoles);
    }, []);
    
    return (
        <select>
            {roles.map(role => (
                <option key={role.id} value={role.roleName}>
                    {role.displayName}
                </option>
            ))}
        </select>
    );
};
```

## Benefits

### 1. User Experience
- ✅ Role names displayed in user's preferred language
- ✅ Consistent localization across the application
- ✅ Maintains backward compatibility with existing code

### 2. System Architecture
- ✅ Separation of internal role names and display names
- ✅ Centralized localization management
- ✅ Easy to add new roles and languages

### 3. Maintenance
- ✅ Single source of truth for role localization
- ✅ Easy to update translations
- ✅ Consistent with existing localization patterns

This implementation ensures that the role list is properly localized while maintaining system integrity and providing a better user experience for both Arabic and English users.
