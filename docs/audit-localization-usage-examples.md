# Resolution Audit Localization Usage Examples

This document provides examples of how to use the new localized audit logging functionality for ResolutionStatusHistory entries.

## Overview

The audit localization system follows the established notification pattern where:
- **Localization keys** are stored in the database (Notes field)
- **Translation occurs on retrieval** using `IAuditLocalizationService`
- **Multilingual support** for Arabic and English
- **User-specific language preferences** are respected

## Service Registration

The `IAuditLocalizationService` is automatically registered in the DI container:

```csharp
// Already registered in InfrastructureServicesRegisteration.cs
services.AddScoped<IAuditLocalizationService, AuditLocalizationService>();
```

## Basic Usage Examples

### 1. Simple Localized Action Description

```csharp
public class ResolutionDetailsQueryHandler
{
    private readonly IAuditLocalizationService _auditLocalizationService;

    public async Task<ResolutionDetailsDto> Handle(GetResolutionDetailsQuery request)
    {
        var resolution = await _repository.Resolutions.GetWithAuditHistoryAsync(request.Id);
        
        var auditEntries = new List<AuditEntryDto>();
        
        foreach (var historyEntry in resolution.ResolutionStatusHistories)
        {
            // Get localized action description
            var localizedDescription = historyEntry.GetLocalizedActionDescription(_auditLocalizationService);
            
            auditEntries.Add(new AuditEntryDto
            {
                Action = localizedDescription,
                Date = historyEntry.CreatedAt,
                UserRole = historyEntry.UserRole
            });
        }
        
        return new ResolutionDetailsDto { AuditHistory = auditEntries };
    }
}
```

### 2. User-Specific Language Preferences

```csharp
public class GetUserAuditHistoryQueryHandler
{
    private readonly IAuditLocalizationService _auditLocalizationService;
    private readonly ICurrentUserService _currentUserService;

    public async Task<List<AuditEntryDto>> Handle(GetUserAuditHistoryQuery request)
    {
        var auditEntries = await _repository.ResolutionStatusHistory.GetUserAuditHistoryAsync(request.UserId);
        var localizedEntries = new List<AuditEntryDto>();
        
        foreach (var entry in auditEntries)
        {
            // Get localized description based on user's preferred language
            var localizedDescription = await entry.GetLocalizedActionDescriptionAsync(
                _auditLocalizationService, 
                _currentUserService.UserId);
            
            localizedEntries.Add(new AuditEntryDto
            {
                Action = localizedDescription,
                Date = entry.CreatedAt
            });
        }
        
        return localizedEntries;
    }
}
```

### 3. Specific Culture/Language

```csharp
public class MultilingualAuditReportService
{
    private readonly IAuditLocalizationService _auditLocalizationService;

    public async Task<AuditReportDto> GenerateReport(int resolutionId, string culture)
    {
        var auditHistory = await _repository.ResolutionStatusHistory.GetByResolutionIdAsync(resolutionId);
        
        var localizedEntries = auditHistory.Select(entry => new AuditEntryDto
        {
            // Get localized description for specific culture
            Action = entry.GetLocalizedActionDescription(_auditLocalizationService, culture),
            
            // Get comprehensive description with full context
            ComprehensiveDescription = entry.GetLocalizedComprehensiveDescription(_auditLocalizationService, culture),
            
            // Get localized status transition
            StatusTransition = entry.GetLocalizedStatusTransition(_auditLocalizationService, culture),
            
            Date = entry.CreatedAt,
            UserRole = entry.UserRole
        }).ToList();
        
        return new AuditReportDto
        {
            ResolutionId = resolutionId,
            Culture = culture,
            AuditEntries = localizedEntries
        };
    }
}
```

### 4. Comprehensive Audit Display

```csharp
public class ResolutionAuditDisplayService
{
    private readonly IAuditLocalizationService _auditLocalizationService;

    public async Task<List<ComprehensiveAuditEntryDto>> GetComprehensiveAuditHistory(int resolutionId)
    {
        var auditHistory = await _repository.ResolutionStatusHistory.GetByResolutionIdAsync(resolutionId);
        
        return auditHistory.Select(entry => new ComprehensiveAuditEntryDto
        {
            Id = entry.Id,
            
            // Basic localized action
            LocalizedAction = entry.GetLocalizedActionDescription(_auditLocalizationService),
            
            // Comprehensive description with full context
            ComprehensiveDescription = entry.GetLocalizedComprehensiveDescription(_auditLocalizationService),
            
            // Status transition (if applicable)
            StatusTransition = entry.IsStatusChange() 
                ? entry.GetLocalizedStatusTransition(_auditLocalizationService)
                : null,
            
            // Original data
            ActionDetails = entry.ActionDetails,
            UserRole = entry.UserRole,
            CreatedAt = entry.CreatedAt,
            CreatedBy = entry.CreatedBy,
            
            // Localization metadata
            LocalizationKey = entry.GetLocalizationKey(),
            HasLocalizationKey = entry.HasLocalizationKey()
            
        }).ToList();
    }
}
```

## API Controller Example

```csharp
[ApiController]
[Route("api/[controller]")]
public class ResolutionAuditController : ControllerBase
{
    private readonly IAuditLocalizationService _auditLocalizationService;
    private readonly IRepositoryManager _repository;

    [HttpGet("{resolutionId}/audit-history")]
    public async Task<IActionResult> GetLocalizedAuditHistory(
        int resolutionId, 
        [FromQuery] string? culture = null)
    {
        var auditHistory = await _repository.ResolutionStatusHistory.GetByResolutionIdAsync(resolutionId);
        
        var localizedHistory = auditHistory.Select(entry => new
        {
            Id = entry.Id,
            Action = entry.GetLocalizedActionDescription(_auditLocalizationService, culture),
            ComprehensiveDescription = entry.GetLocalizedComprehensiveDescription(_auditLocalizationService, culture),
            StatusTransition = entry.GetLocalizedStatusTransition(_auditLocalizationService, culture),
            Date = entry.CreatedAt,
            UserRole = entry.UserRole,
            IsStatusChange = entry.IsStatusChange()
        }).ToList();
        
        return Ok(localizedHistory);
    }
}
```

## Available Methods

### ResolutionStatusHistory Entity Methods

1. **GetLocalizedActionDescription(service, culture?)** - Basic localized action name
2. **GetLocalizedActionDescriptionAsync(service, userId?)** - User-specific language preference
3. **GetLocalizedComprehensiveDescription(service, culture?)** - Full context description
4. **GetLocalizedStatusTransition(service, culture?)** - Status change description
5. **GetLocalizationKey()** - Returns the stored localization key
6. **HasLocalizationKey()** - Checks if localization key exists

### IAuditLocalizationService Methods

1. **GetLocalizedActionDescription(entry, culture?)** - Direct service call
2. **GetLocalizedActionDescriptionAsync(entry, userId?)** - Async with user preference
3. **GetLocalizedComprehensiveDescription(entry, culture?)** - Comprehensive description
4. **GetLocalizedStatusTransition(previousStatus, newStatus, culture?)** - Status transitions

## Localization Keys

The following localization keys are stored in the Notes field:

- `AuditActionResolutionCreation`
- `AuditActionResolutionDataUpdate`
- `AuditActionResolutionVoteSuspend`
- `AuditActionResolutionConfirmation`
- `AuditActionResolutionRejection`
- `AuditActionResolutionSentToVote`
- `AuditActionResolutionCancellation`
- `AuditActionResolutionDeletion`

## Best Practices

1. **Always use the service** - Don't access Notes field directly for display
2. **Respect user preferences** - Use async methods when possible for user-specific languages
3. **Handle fallbacks** - Service provides fallback descriptions if localization fails
4. **Cache when appropriate** - Consider caching localized descriptions for performance
5. **Maintain backward compatibility** - Original GetActionDescription() method still works

This localization system ensures that audit trails are properly displayed in the user's preferred language while maintaining the established notification pattern for multilingual support.
