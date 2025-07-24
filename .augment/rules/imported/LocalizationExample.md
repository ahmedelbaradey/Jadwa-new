---
type: "always_apply"
---

# Localization Implementation Reference Template

## Overview

This document serves as a **comprehensive reference template** for implementing localization in the Jadwa API project following established patterns. It provides standardized patterns based on the existing Fund/FundStrategy localization implementation and Categories patterns for creating multilingual support that can be applied to any entity implementation.

### Key Localization Principles (Based on Jadwa API Implementation)
- **Culture-Based Selection**: Automatic Arabic/English selection using `CultureInfo.CurrentCulture.Name.StartsWith("ar")`
- **LocalizedDto Inheritance**: Use established LocalizedDto base class for consistent implementation
- **Resource-Based Messages**: Centralized message management through SharedResources and .resx files
- **Validation Localization**: Multilingual validation error messages using IStringLocalizer
- **Enum Localization**: Localized display names for enumerations following existing patterns
- **Performance Optimized**: Minimal overhead with computed properties in base DTOs

## Core Localization Pattern (Following Jadwa API Standard)

The fundamental localization pattern used throughout the Jadwa API system:

```csharp
public string Localized{PropertyName} =>
    System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("ar")
        ? {PropertyName}Ar
        : {PropertyName}En;
```

## Entity Localization Templates

### 1. Domain Entity Localization Template (Following Fund/FundStrategy Pattern)
**Template for implementing localization in domain entities based on existing patterns**

```csharp
using System.Globalization;
using Domain.Entities.Base;

namespace Domain.Entities.{DomainArea}
{
    /// <summary>
    /// {EntityName} entity with localization support
    /// Implements culture-based property selection for multilingual content
    /// Based on Fund/FundStrategy localization patterns in Jadwa API
    /// Properties should match requirements from Stories.md, TASK.md, and backend-development-plan.md
    /// </summary>
    public class {EntityName} : FullAuditedEntity
    {
        // MULTILINGUAL PROPERTIES TEMPLATE (add only if entity requires multilingual support per documentation)
        /// <summary>
        /// {EntityName} name in Arabic
        /// Required for entities with multilingual support based on Stories.md/TASK.md
        /// </summary>
        public string {PropertyName}Ar { get; set; } = string.Empty;

        /// <summary>
        /// {EntityName} name in English
        /// Required for entities with multilingual support based on Stories.md/TASK.md
        /// </summary>
        public string {PropertyName}En { get; set; } = string.Empty;

        /// <summary>
        /// {EntityName} description in Arabic (optional)
        /// Used for detailed multilingual descriptions if specified in documentation
        /// </summary>
        public string? {DescriptionProperty}Ar { get; set; }

        /// <summary>
        /// {EntityName} description in English (optional)
        /// Used for detailed multilingual descriptions if specified in documentation
        /// </summary>
        public string? {DescriptionProperty}En { get; set; }

        // COMPUTED LOCALIZATION PROPERTIES TEMPLATE (following Fund/FundStrategy pattern)
        /// <summary>
        /// Localized name based on current culture
        /// Returns Arabic text for Arabic cultures, English otherwise
        /// Follows established Jadwa API localization pattern
        /// </summary>
        public string Localized{PropertyName} =>
            CultureInfo.CurrentCulture.Name.StartsWith("ar")
                ? {PropertyName}Ar
                : {PropertyName}En;

        /// <summary>
        /// Localized description based on current culture
        /// Returns Arabic text for Arabic cultures, English otherwise
        /// Follows established Jadwa API localization pattern
        /// </summary>
        public string? Localized{DescriptionProperty} =>
            CultureInfo.CurrentCulture.Name.StartsWith("ar")
                ? {DescriptionProperty}Ar
                : {DescriptionProperty}En;

        // ENUM LOCALIZATION TEMPLATE (add for entities with status/category enums if specified in documentation)
        /// <summary>
        /// Localized display name for entity status
        /// Provides user-friendly status names in current culture
        /// Based on business requirements from Stories.md/TASK.md
        /// </summary>
        public string {StatusProperty}DisplayName => {StatusProperty} switch
        {
            {EntityName}Status.{Status1} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicStatus1}" : "{EnglishStatus1}",
            {EntityName}Status.{Status2} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicStatus2}" : "{EnglishStatus2}",
            {EntityName}Status.{Status3} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicStatus3}" : "{EnglishStatus3}",
            _ => {StatusProperty}.ToString()
        };

        // IMPLEMENTATION GUIDELINES:
        // 1. Follow Fund/FundStrategy localization patterns exactly
        // 2. Only add multilingual properties if explicitly required in Stories.md/TASK.md
        // 3. Use computed properties for culture-based selection
        // 4. Provide fallback to English when Arabic is not available
        // 5. Keep localization logic simple in domain entities
        // 6. Use consistent naming patterns for multilingual properties
        // 7. Document the purpose of each localized property
        // 8. Verify all properties against business requirements documentation
    }
}
```

### 2. Simple Entity Localization Template (Following Categories Pattern)
**Template for simple entities without complex localization requirements**

```csharp
using System.Globalization;
using Domain.Entities.Base;

namespace Domain.Entities.{DomainArea}
{
    /// <summary>
    /// {EntityName} entity following Categories localization pattern
    /// Simple structure for entities that may or may not require localization
    /// Based on Categories implementation in Jadwa API
    /// </summary>
    public class {EntityName} : FullAuditedEntity
    {
        // SIMPLE PROPERTIES TEMPLATE (add based on Stories.md/TASK.md requirements)
        /// <summary>
        /// {PropertyName} based on business requirements
        /// {Required/Optional status based on documentation}
        /// </summary>
        public string {PropertyName} { get; set; } = string.Empty;

        /// <summary>
        /// Optional {PropertyDescription} based on business requirements
        /// {Description from documentation}
        /// </summary>
        public string? Optional{PropertyName} { get; set; }

        // ENUM PROPERTIES TEMPLATE (add only if specified in documentation)
        /// <summary>
        /// {EntityName} status/category enumeration
        /// {Enum description based on business requirements}
        /// </summary>
        public {EntityName}Status {StatusProperty} { get; set; } = {EntityName}Status.{DefaultStatus};

        // IMPLEMENTATION GUIDELINES:
        // 1. Follow Categories pattern for simple entities
        // 2. Only add localization if explicitly required in Stories.md/TASK.md
        // 3. Keep simple entities without unnecessary complexity
        // 4. Use appropriate default values based on business rules
        // 5. Add comprehensive XML documentation for each property
    }
}
```

### 3. Status/History Entity Localization Template (Following StatusHistory Pattern)
**Template for entities that track status changes with localization**

```csharp
using System.Globalization;
using Domain.Entities.Base;

namespace Domain.Entities.{DomainArea}
{
    /// <summary>
    /// {EntityName}StatusHistory entity with localization support
    /// Tracks status changes with multilingual status names
    /// Based on existing StatusHistory patterns in Jadwa API
    /// </summary>
    public class {EntityName}StatusHistory : FullAuditedEntity
    {
        // ENTITY REFERENCE TEMPLATE
        /// <summary>
        /// Reference to the parent {EntityName}
        /// Links status history to the main entity
        /// </summary>
        public int {EntityName}Id { get; set; }

        // STATUS TRACKING PROPERTIES TEMPLATE
        /// <summary>
        /// Previous status value
        /// Status before the change
        /// </summary>
        public {EntityName}Status FromStatus { get; set; }

        /// <summary>
        /// New status value
        /// Status after the change
        /// </summary>
        public {EntityName}Status ToStatus { get; set; }

        /// <summary>
        /// Date when status change occurred
        /// Timestamp of the status transition
        /// </summary>
        public DateTime ChangeDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Reason for status change
        /// Business context for the transition
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// Optional additional notes about the change
        /// Additional context or comments
        /// </summary>
        public string? Notes { get; set; }

        // AUDIT INFORMATION TEMPLATE
        /// <summary>
        /// ID of the user who initiated the status change
        /// Reference to the user responsible for the change
        /// </summary>
        public int ChangedBy { get; set; }

        /// <summary>
        /// Display name of the user who made the change
        /// User-friendly name for audit display
        /// </summary>
        public string ChangedByName { get; set; } = string.Empty;

        // COMPUTED LOCALIZATION PROPERTIES TEMPLATE
        /// <summary>
        /// Localized display name for the source status
        /// User-friendly status name in current culture
        /// </summary>
        public string FromStatusDisplayName => FromStatus switch
        {
            {EntityName}Status.{Status1} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicStatus1}" : "{EnglishStatus1}",
            {EntityName}Status.{Status2} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicStatus2}" : "{EnglishStatus2}",
            {EntityName}Status.{Status3} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicStatus3}" : "{EnglishStatus3}",
            _ => FromStatus.ToString()
        };

        /// <summary>
        /// Localized display name for the target status
        /// User-friendly status name in current culture
        /// </summary>
        public string ToStatusDisplayName => ToStatus switch
        {
            {EntityName}Status.{Status1} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicStatus1}" : "{EnglishStatus1}",
            {EntityName}Status.{Status2} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicStatus2}" : "{EnglishStatus2}",
            {EntityName}Status.{Status3} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicStatus3}" : "{EnglishStatus3}",
            _ => ToStatus.ToString()
        };

        /// <summary>
        /// Localized description of the status transition
        /// Complete sentence describing the change
        /// </summary>
        public string LocalizedTransitionDescription =>
            CultureInfo.CurrentCulture.Name.StartsWith("ar")
                ? $"تم تغيير الحالة من {FromStatusDisplayName} إلى {ToStatusDisplayName}"
                : $"Status changed from {FromStatusDisplayName} to {ToStatusDisplayName}";

        // IMPLEMENTATION GUIDELINES:
        // 1. Follow existing StatusHistory patterns in Jadwa API
        // 2. Include comprehensive audit information
        // 3. Provide localized display properties for status names
        // 4. Add computed properties for user-friendly displays
        // 5. Only add properties explicitly mentioned in Stories.md and TASK.md
    }
}
```

## DTO Localization Templates

### 1. LocalizedDto Base Template (Following Jadwa API LocalizedDto Pattern)
**Template for DTOs with localization support using established base classes**

```csharp
using Application.Common.Dtos;

namespace Application.Features.{DomainArea}.{EntityName}s.Dtos
{
    /// <summary>
    /// {EntityName} DTO with localization support
    /// Inherits from LocalizedDto to include localized name and description properties
    /// Based on existing LocalizedDto pattern in Jadwa API
    /// Clean DTO without data annotations, following Clean Architecture principles
    /// </summary>
    public record {EntityName}Dto : LocalizedDto
    {
        // ENTITY-SPECIFIC PROPERTIES TEMPLATE (add only if specified in Stories.md/TASK.md)
        // All localization properties are inherited from LocalizedDto:
        // - Id (from BaseDto)
        // - NameAr, NameEn (localized names)
        // - DescriptionAr, DescriptionEn (localized descriptions)
        // - LocalizedName, LocalizedDescription (computed properties)

        // ENUM PROPERTIES TEMPLATE (add based on entity requirements from documentation)
        /// <summary>
        /// {EntityName} status enumeration
        /// Default value based on business requirements from Stories.md/TASK.md
        /// </summary>
        public {EntityName}Status {StatusProperty} { get; set; } = {EntityName}Status.{DefaultStatus};

        // FOREIGN KEY PROPERTIES TEMPLATE (add based on entity relationships from documentation)
        /// <summary>
        /// Foreign key reference to {RelatedEntity}
        /// Required relationship based on domain model from Stories.md/TASK.md
        /// </summary>
        public int {RelatedEntity}Id { get; set; }

        /// <summary>
        /// Optional foreign key reference to {OptionalRelatedEntity}
        /// Nullable relationship based on domain model from Stories.md/TASK.md
        /// </summary>
        public int? Optional{RelatedEntity}Id { get; set; }

        // VALUE PROPERTIES TEMPLATE (add based on entity requirements from documentation)
        /// <summary>
        /// {PropertyDescription} based on business requirements from Stories.md/TASK.md
        /// Default value set according to domain rules
        /// </summary>
        public {PropertyType} {PropertyName} { get; set; } = {DefaultValue};

        /// <summary>
        /// Optional {PropertyDescription} based on business requirements from Stories.md/TASK.md
        /// Nullable for optional business data
        /// </summary>
        public {PropertyType}? Optional{PropertyName} { get; set; }

        // IMPLEMENTATION GUIDELINES:
        // 1. Inherit from LocalizedDto for automatic Arabic/English support
        // 2. LocalizedName and LocalizedDescription are computed automatically
        // 3. Only add additional properties if specified in Stories.md/TASK.md
        // 4. Follow existing Fund/FundStrategyDto.cs pattern
        // 5. Keep DTOs clean without data annotations
        // 6. Use appropriate default values based on business rules
        // 7. Verify all properties against business requirements documentation
    }
}
```

### 2. Complex DTO Localization Template
**Template for DTOs with multiple localized properties and enums**

```csharp
using System.Globalization;
using Abstraction.Base.Dto;

namespace Application.Features.{DomainArea}.{EntityName}s.Dtos
{
    /// <summary>
    /// Complex {EntityName} DTO with comprehensive localization support
    /// Includes multiple localized properties and enum display names
    /// </summary>
    public record {EntityName}Dto : BaseDto
    {
        // MULTILINGUAL PROPERTIES TEMPLATE
        /// <summary>
        /// {EntityName} name in Arabic
        /// Primary identifier in Arabic
        /// </summary>
        public string {PropertyName}Ar { get; set; } = string.Empty;

        /// <summary>
        /// {EntityName} name in English
        /// Primary identifier in English
        /// </summary>
        public string {PropertyName}En { get; set; } = string.Empty;

        /// <summary>
        /// {EntityName} description in Arabic (optional)
        /// Detailed description in Arabic
        /// </summary>
        public string? {DescriptionProperty}Ar { get; set; }

        /// <summary>
        /// {EntityName} description in English (optional)
        /// Detailed description in English
        /// </summary>
        public string? {DescriptionProperty}En { get; set; }

        // ENUM PROPERTIES TEMPLATE
        /// <summary>
        /// {EntityName} status enumeration
        /// Current status of the entity
        /// </summary>
        public {EntityName}Status Status { get; set; } = {EntityName}Status.{DefaultStatus};

        /// <summary>
        /// {EntityName} category enumeration
        /// Classification or type of the entity
        /// </summary>
        public {EntityName}Category Category { get; set; } = {EntityName}Category.{DefaultCategory};

        // COMPUTED LOCALIZATION PROPERTIES
        /// <summary>
        /// Localized name based on current culture
        /// Returns Arabic text for Arabic cultures, English otherwise
        /// </summary>
        public string Localized{PropertyName} =>
            CultureInfo.CurrentCulture.Name.StartsWith("ar")
                ? {PropertyName}Ar
                : {PropertyName}En;

        /// <summary>
        /// Localized description based on current culture
        /// Returns Arabic text for Arabic cultures, English otherwise
        /// </summary>
        public string? Localized{DescriptionProperty} =>
            CultureInfo.CurrentCulture.Name.StartsWith("ar")
                ? {DescriptionProperty}Ar
                : {DescriptionProperty}En;

        // ENUM LOCALIZATION TEMPLATES
        /// <summary>
        /// Localized display name for status
        /// Provides user-friendly status names in current culture
        /// </summary>
        public string StatusDisplayName => Status switch
        {
            {EntityName}Status.{Status1} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicStatus1}" : "{EnglishStatus1}",
            {EntityName}Status.{Status2} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicStatus2}" : "{EnglishStatus2}",
            {EntityName}Status.{Status3} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicStatus3}" : "{EnglishStatus3}",
            {EntityName}Status.{Status4} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicStatus4}" : "{EnglishStatus4}",
            {EntityName}Status.{Status5} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicStatus5}" : "{EnglishStatus5}",
            {EntityName}Status.{Status6} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicStatus6}" : "{EnglishStatus6}",
            _ => Status.ToString()
        };

        /// <summary>
        /// Localized display name for category
        /// Provides user-friendly category names in current culture
        /// </summary>
        public string CategoryDisplayName => Category switch
        {
            {EntityName}Category.{Category1} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicCategory1}" : "{EnglishCategory1}",
            {EntityName}Category.{Category2} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicCategory2}" : "{EnglishCategory2}",
            {EntityName}Category.{Category3} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicCategory3}" : "{EnglishCategory3}",
            {EntityName}Category.{Category4} => CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "{ArabicCategory4}" : "{EnglishCategory4}",
            _ => Category.ToString()
        };

        // IMPLEMENTATION GUIDELINES:
        // 1. Include all enum values in switch expressions
        // 2. Provide meaningful Arabic translations for business terms
        // 3. Use consistent naming patterns across all localized properties
        // 4. Include fallback to enum.ToString() for unknown values
        // 5. Keep computed properties simple and performant
        // 6. Document the business context for each localized property
    }
}
```

## Resource Management Templates

### 1. SharedResourcesKey Template
**Template for managing localization resource keys**

```csharp
namespace Resources
{
    /// <summary>
    /// Centralized resource keys for localization
    /// Provides type-safe access to resource strings
    /// </summary>
    public static class SharedResourcesKey
    {
        // ENTITY PROPERTY LOCALIZATION KEYS TEMPLATE
        /// <summary>
        /// Resource key for localized {PropertyName}
        /// Used in UI labels and displays
        /// </summary>
        public const string Localized{PropertyName} = "Localized{PropertyName}";

        /// <summary>
        /// Resource key for localized {DescriptionProperty}
        /// Used in UI labels and displays
        /// </summary>
        public const string Localized{DescriptionProperty} = "Localized{DescriptionProperty}";

        // ENUM LOCALIZATION KEYS TEMPLATE
        /// <summary>
        /// Resource key for {EntityName}Status.{Status1}
        /// Localized display name for status
        /// </summary>
        public const string {EntityName}Status{Status1} = "{EntityName}Status{Status1}";

        /// <summary>
        /// Resource key for {EntityName}Status.{Status2}
        /// Localized display name for status
        /// </summary>
        public const string {EntityName}Status{Status2} = "{EntityName}Status{Status2}";

        /// <summary>
        /// Resource key for {EntityName}Category.{Category1}
        /// Localized display name for category
        /// </summary>
        public const string {EntityName}Category{Category1} = "{EntityName}Category{Category1}";

        // VALIDATION MESSAGE KEYS TEMPLATE
        /// <summary>
        /// Resource key for Arabic text validation message
        /// Used in FluentValidation rules
        /// </summary>
        public const string ArabicTextValidation = "ArabicTextValidation";

        /// <summary>
        /// Resource key for English text validation message
        /// Used in FluentValidation rules
        /// </summary>
        public const string EnglishTextValidation = "EnglishTextValidation";

        /// <summary>
        /// Resource key for duplicate name validation message
        /// Used in business rule validation
        /// </summary>
        public const string DuplicateNameValidation = "DuplicateNameValidation";

        // IMPLEMENTATION GUIDELINES:
        // 1. Use consistent naming patterns: {EntityName}{EnumValue}
        // 2. Group related keys together with comments
        // 3. Provide XML documentation for each key
        // 4. Use descriptive names that indicate the purpose
        // 5. Include validation message keys for multilingual error messages
    }
}
```

### 2. Resource File Templates
**Template for .resx file structure**

#### English Resource File (`SharedResources.en-US.resx`)
```xml
<!-- ENTITY PROPERTY LOCALIZATION TEMPLATE -->
<data name="Localized{PropertyName}" xml:space="preserve">
    <value>{English Property Display Name}</value>
</data>
<data name="Localized{DescriptionProperty}" xml:space="preserve">
    <value>{English Description Display Name}</value>
</data>

<!-- ENUM LOCALIZATION TEMPLATE -->
<data name="{EntityName}Status{Status1}" xml:space="preserve">
    <value>{English Status 1 Name}</value>
</data>
<data name="{EntityName}Status{Status2}" xml:space="preserve">
    <value>{English Status 2 Name}</value>
</data>
<data name="{EntityName}Category{Category1}" xml:space="preserve">
    <value>{English Category 1 Name}</value>
</data>

<!-- VALIDATION MESSAGE TEMPLATE -->
<data name="ArabicTextValidation" xml:space="preserve">
    <value>Please enter valid Arabic text</value>
</data>
<data name="EnglishTextValidation" xml:space="preserve">
    <value>Please enter valid English text</value>
</data>
<data name="DuplicateNameValidation" xml:space="preserve">
    <value>This name already exists</value>
</data>
```

#### Arabic Resource File (`SharedResources.ar-EG.resx`)
```xml
<!-- ENTITY PROPERTY LOCALIZATION TEMPLATE -->
<data name="Localized{PropertyName}" xml:space="preserve">
    <value>{Arabic Property Display Name}</value>
</data>
<data name="Localized{DescriptionProperty}" xml:space="preserve">
    <value>{Arabic Description Display Name}</value>
</data>

<!-- ENUM LOCALIZATION TEMPLATE -->
<data name="{EntityName}Status{Status1}" xml:space="preserve">
    <value>{Arabic Status 1 Name}</value>
</data>
<data name="{EntityName}Status{Status2}" xml:space="preserve">
    <value>{Arabic Status 2 Name}</value>
</data>
<data name="{EntityName}Category{Category1}" xml:space="preserve">
    <value>{Arabic Category 1 Name}</value>
</data>

<!-- VALIDATION MESSAGE TEMPLATE -->
<data name="ArabicTextValidation" xml:space="preserve">
    <value>يرجى إدخال نص عربي صحيح</value>
</data>
<data name="EnglishTextValidation" xml:space="preserve">
    <value>يرجى إدخال نص إنجليزي صحيح</value>
</data>
<data name="DuplicateNameValidation" xml:space="preserve">
    <value>هذا الاسم موجود بالفعل</value>
</data>
```

## Validation Localization Templates

### 1. FluentValidation Localization Template
**Template for implementing localized validation messages**

```csharp
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;
using System.Globalization;

namespace Application.Features.{DomainArea}.{EntityName}s.Validation
{
    /// <summary>
    /// Base validation with localized error messages
    /// Provides multilingual validation feedback
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
            // REQUIRED FIELD VALIDATION TEMPLATE
            RuleFor(x => x.{PropertyName}Ar)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKey.EmptyValidation])
                .NotNull().WithMessage(_localizer[SharedResourcesKey.EmptyValidation])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKey.MaxLengthValidation]);

            RuleFor(x => x.{PropertyName}En)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKey.EmptyValidation])
                .NotNull().WithMessage(_localizer[SharedResourcesKey.EmptyValidation])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKey.MaxLengthValidation]);

            // ARABIC TEXT VALIDATION TEMPLATE
            RuleFor(x => x.{PropertyName}Ar)
                .Matches(@"^[\u0600-\u06FF\u0750-\u077F\u08A0-\u08FF\uFB50-\uFDFF\uFE70-\uFEFF\s\d\-_().,!?]+$")
                .WithMessage(_localizer[SharedResourcesKey.ArabicTextValidation])
                .When(x => !string.IsNullOrEmpty(x.{PropertyName}Ar));

            // ENGLISH TEXT VALIDATION TEMPLATE
            RuleFor(x => x.{PropertyName}En)
                .Matches(@"^[a-zA-Z\s\d\-_().,!?]+$")
                .WithMessage(_localizer[SharedResourcesKey.EnglishTextValidation])
                .When(x => !string.IsNullOrEmpty(x.{PropertyName}En));

            // OPTIONAL FIELD VALIDATION TEMPLATE
            RuleFor(x => x.{DescriptionProperty}Ar)
                .MaximumLength(500).WithMessage(_localizer[SharedResourcesKey.MaxLengthValidation])
                .When(x => !string.IsNullOrEmpty(x.{DescriptionProperty}Ar));

            RuleFor(x => x.{DescriptionProperty}En)
                .MaximumLength(500).WithMessage(_localizer[SharedResourcesKey.MaxLengthValidation])
                .When(x => !string.IsNullOrEmpty(x.{DescriptionProperty}En));

            // ENUM VALIDATION TEMPLATE
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage(_localizer[SharedResourcesKey.InvalidEnumValidation]);

            // BUSINESS RULE VALIDATION TEMPLATE
            RuleFor(x => x)
                .MustAsync(async (entity, cancellation) => await BeUniqueNameAsync(entity))
                .WithMessage(_localizer[SharedResourcesKey.DuplicateNameValidation]);
        }

        // CUSTOM VALIDATION METHODS TEMPLATE
        /// <summary>
        /// Validates that the entity name is unique
        /// Checks both Arabic and English names for duplicates
        /// </summary>
        private async Task<bool> BeUniqueNameAsync({EntityName}Dto entity)
        {
            // Implementation depends on repository access
            // This is a template for business rule validation
            return true; // Replace with actual validation logic
        }
    }
}
```

### 2. Localization Extension Methods Template
**Template for helper methods with resource-based localization**

```csharp
using Microsoft.Extensions.Localization;
using Resources;
using System.Globalization;

namespace Application.Common.Extensions
{
    /// <summary>
    /// Extension methods for consistent localization patterns
    /// Provides centralized localization logic with resource-based messages
    /// </summary>
    public static class LocalizationExtensions
    {
        // CULTURE DETECTION TEMPLATE
        /// <summary>
        /// Gets localized text based on current culture
        /// Returns Arabic text for Arabic cultures, English otherwise
        /// </summary>
        public static string GetLocalizedText(string arabicText, string englishText)
        {
            return CultureInfo.CurrentCulture.Name.StartsWith("ar")
                ? arabicText
                : englishText;
        }

        /// <summary>
        /// Determines if the current culture is Arabic
        /// Used for conditional localization logic
        /// </summary>
        public static bool IsArabicCulture()
        {
            return CultureInfo.CurrentCulture.Name.StartsWith("ar");
        }

        // ENUM LOCALIZATION TEMPLATES
        /// <summary>
        /// Gets localized status name using resource files
        /// Provides consistent status name localization
        /// </summary>
        public static string GetLocalized{EntityName}StatusName({EntityName}Status status, IStringLocalizer<SharedResources> localizer)
        {
            return status switch
            {
                {EntityName}Status.{Status1} => localizer[SharedResourcesKey.{EntityName}Status{Status1}],
                {EntityName}Status.{Status2} => localizer[SharedResourcesKey.{EntityName}Status{Status2}],
                {EntityName}Status.{Status3} => localizer[SharedResourcesKey.{EntityName}Status{Status3}],
                _ => status.ToString()
            };
        }

        /// <summary>
        /// Gets localized category name using resource files
        /// Provides consistent category name localization
        /// </summary>
        public static string GetLocalized{EntityName}CategoryName({EntityName}Category category, IStringLocalizer<SharedResources> localizer)
        {
            return category switch
            {
                {EntityName}Category.{Category1} => localizer[SharedResourcesKey.{EntityName}Category{Category1}],
                {EntityName}Category.{Category2} => localizer[SharedResourcesKey.{EntityName}Category{Category2}],
                {EntityName}Category.{Category3} => localizer[SharedResourcesKey.{EntityName}Category{Category3}],
                _ => category.ToString()
            };
        }

        /// <summary>
        /// Gets localized role name using resource files
        /// Provides consistent role name localization
        /// </summary>
        public static string GetLocalized{EntityName}RoleName({EntityName}Role role, IStringLocalizer<SharedResources> localizer)
        {
            return role switch
            {
                {EntityName}Role.{Role1} => localizer[SharedResourcesKey.{EntityName}Role{Role1}],
                {EntityName}Role.{Role2} => localizer[SharedResourcesKey.{EntityName}Role{Role2}],
                {EntityName}Role.{Role3} => localizer[SharedResourcesKey.{EntityName}Role{Role3}],
                _ => role.ToString()
            };
        }

        // IMPLEMENTATION GUIDELINES:
        // 1. Always use IStringLocalizer for resource-based localization
        // 2. Provide fallback to enum.ToString() for unknown values
        // 3. Include all enum values in switch expressions
        // 4. Use consistent naming patterns for extension methods
        // 5. Document the purpose and usage of each method
        // 6. Keep methods simple and focused on single responsibility
    }
}
```

## AutoMapper Integration Templates

### 1. Entity to DTO Mapping with Localization
**Template for AutoMapper profiles with localized property mapping**

```csharp
using AutoMapper;
using Application.Features.{DomainArea}.{EntityName}s.Dtos;
using Domain.Entities.{DomainArea};

namespace Application.Mapping.{EntityName}s
{
    /// <summary>
    /// AutoMapper profile for {EntityName} entity mappings with localization support
    /// Handles mapping between entities and DTOs with localized properties
    /// </summary>
    public partial class {EntityName}Profile : Profile
    {
        public void Get{EntityName}Mapping()
        {
            // BASIC ENTITY TO DTO MAPPING TEMPLATE
            CreateMap<{EntityName}, {EntityName}Dto>()
                // Localized properties are computed automatically
                .ForMember(dest => dest.Localized{PropertyName}, opt => opt.Ignore())
                .ForMember(dest => dest.Localized{DescriptionProperty}, opt => opt.Ignore())
                .ForMember(dest => dest.StatusDisplayName, opt => opt.Ignore());

            // ENTITY TO RESPONSE DTO MAPPING TEMPLATE
            CreateMap<{EntityName}, Single{EntityName}Response>()
                // Map related entity localized names
                .ForMember(dest => dest.{RelatedEntity}Name,
                    opt => opt.MapFrom(src => src.{RelatedEntity} != null ? src.{RelatedEntity}.Localized{PropertyName} : null))
                .ForMember(dest => dest.{AnotherRelatedEntity}Name,
                    opt => opt.MapFrom(src => src.{AnotherRelatedEntity} != null ? src.{AnotherRelatedEntity}.Localized{PropertyName} : null))
                // Ignore computed localized properties
                .ForMember(dest => dest.Localized{PropertyName}, opt => opt.Ignore())
                .ForMember(dest => dest.StatusDisplayName, opt => opt.Ignore());

            // ENTITY TO DETAILS RESPONSE MAPPING TEMPLATE
            CreateMap<{EntityName}, {EntityName}DetailsResponse>()
                // Map complex related entities
                .ForMember(dest => dest.{RelatedEntity}, opt => opt.MapFrom(src => src.{RelatedEntity}))
                // Ignore computed collections and properties
                .ForMember(dest => dest.StatusHistory, opt => opt.Ignore())
                .ForMember(dest => dest.Notifications, opt => opt.Ignore())
                .ForMember(dest => dest.Localized{PropertyName}, opt => opt.Ignore());
        }
    }
}
```

### 2. Handler-Level Localization Template
**Template for applying localization in query handlers**

```csharp
using Microsoft.Extensions.Localization;
using Resources;
using Application.Common.Extensions;

namespace Application.Features.{DomainArea}.{EntityName}s.Queries.GetDetails
{
    /// <summary>
    /// Query handler with comprehensive localization support
    /// Demonstrates proper localization implementation in handlers
    /// Based on existing Jadwa API patterns
    /// </summary>
    public class Get{EntityName}DetailsQueryHandler : BaseResponseHandler, IQueryHandler<Get{EntityName}DetailsQuery, BaseResponse<{EntityName}DetailsResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public Get{EntityName}DetailsQueryHandler(
            IRepositoryManager repository,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<BaseResponse<{EntityName}DetailsResponse>> Handle(Get{EntityName}DetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _repository.{EntityName}s.GetWithRelatedDataAsync(request.Id);
                if (entity == null)
                    return NotFound<{EntityName}DetailsResponse>(_localizer[SharedResourcesKey.NotFound]);

                var response = _mapper.Map<{EntityName}DetailsResponse>(entity);

                // APPLY LOCALIZATION TO COLLECTIONS TEMPLATE
                foreach (var statusHistory in response.StatusHistory)
                {
                    statusHistory.FromStatusDisplayName = LocalizationExtensions.GetLocalized{EntityName}StatusName(statusHistory.FromStatus, _localizer);
                    statusHistory.ToStatusDisplayName = LocalizationExtensions.GetLocalized{EntityName}StatusName(statusHistory.ToStatus, _localizer);
                }

                foreach (var user in response.UserGroups)
                {
                    user.RoleDisplayName = LocalizationExtensions.GetLocalized{EntityName}RoleName(user.Role, _localizer);
                }

                foreach (var document in response.DocumentGroups.SelectMany(g => g.Documents))
                {
                    document.CategoryDisplayName = LocalizationExtensions.GetLocalized{EntityName}CategoryName(document.Category, _localizer);
                }

                return Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get{EntityName}DetailsQuery");
                return ServerError<{EntityName}DetailsResponse>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }
    }
}

## Implementation Checklist

### ✅ **Before Implementation:**
1. **Review Documentation**: Check Stories.md and TASK.md for localization requirements
2. **Choose Pattern**: Select appropriate template (LocalizedDto vs Simple DTO)
3. **Plan Resources**: Identify all text that needs localization
4. **Design Enums**: Plan enum values and their localized names

### ✅ **During Implementation:**
1. **Follow Patterns**: Use established Jadwa API localization patterns
2. **Use Base Classes**: Inherit from LocalizedDto when appropriate
3. **Add Resource Keys**: Create entries in SharedResourcesKey.cs
4. **Update Resource Files**: Add translations to .resx files
5. **Implement Validation**: Use localized validation messages

### ✅ **After Implementation:**
1. **Test Cultures**: Verify behavior with Arabic and English cultures
2. **Test Fallbacks**: Ensure English fallback works when Arabic is missing
3. **Update Handlers**: Apply localization in query/command handlers
4. **Document Changes**: Update relevant documentation

### ❌ **Common Pitfalls to Avoid:**
1. **Hardcoded Strings**: Always use resource-based localization in handlers
2. **Missing Fallbacks**: Always provide English fallback for Arabic text
3. **Inconsistent Patterns**: Follow established culture detection patterns
4. **Performance Issues**: Use computed properties efficiently
5. **Missing Translations**: Ensure all resource keys have both Arabic and English values

This comprehensive template ensures consistent, maintainable, and scalable localization implementation following the established Jadwa API architecture and Clean Architecture principles.

namespace Application.Features.{DomainArea}.{EntityName}s.Queries.Get
{
    /// <summary>
    /// Query handler with localization support
    /// Applies localized values during DTO mapping
    /// </summary>
    public class Get{EntityName}DetailsQueryHandler : BaseResponseHandler, IQueryHandler<Get{EntityName}DetailsQuery, BaseResponse<{EntityName}DetailsResponse>>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        // ... other dependencies

        public async Task<BaseResponse<{EntityName}DetailsResponse>> Handle(Get{EntityName}DetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get entity with related data
                var entity = await _repository.{EntityName}s.GetWithRelatedDataAsync(request.Id);

                // Map to DTO
                var response = _mapper.Map<{EntityName}DetailsResponse>(entity);

                // APPLY LOCALIZATION TEMPLATE
                // Apply localized status names to status history
                foreach (var historyItem in response.StatusHistory)
                {
                    historyItem.FromStatusDisplayName = LocalizationExtensions.GetLocalized{EntityName}StatusName(historyItem.FromStatus, _localizer);
                    historyItem.ToStatusDisplayName = LocalizationExtensions.GetLocalized{EntityName}StatusName(historyItem.ToStatus, _localizer);
                }

                // Apply localized category names to documents
                foreach (var document in response.DocumentGroups.SelectMany(g => g.Documents))
                {
                    document.CategoryDisplayName = LocalizationExtensions.GetLocalized{EntityName}CategoryName(document.Category, _localizer);
                }

                // Apply localized role names to users
                foreach (var user in response.UserGroups)
                {
                    user.RoleDisplayName = LocalizationExtensions.GetLocalized{EntityName}RoleName(user.Role, _localizer);
                }

                return Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get{EntityName}DetailsQuery");
                return ServerError<{EntityName}DetailsResponse>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }
    }
}
```

## Implementation Guidelines

### 🌐 **Localization Architecture**

#### Domain Layer Approach
- **Pattern**: Use computed properties with `CultureInfo.CurrentCulture.Name.StartsWith("ar")`
- **Rationale**: No dependencies on Application layer services
- **Usage**: Simple localization for basic entity display

#### Application Layer Approach
- **Pattern**: Use `IStringLocalizer` service with resource keys
- **Rationale**: Proper dependency injection and centralized resource management
- **Usage**: Complex localization with validation messages and business terms

#### Presentation Layer Approach
- **Pattern**: Receive localized data from Application layer
- **Rationale**: Clean separation of concerns
- **Usage**: Display pre-localized data without additional processing

### 📁 **File Organization Template**

```
Resources/
├── SharedResources.cs                    # Resource class
├── SharedResourcesKey.cs                 # Resource keys constants
├── SharedResources.en-US.resx           # English resources
├── SharedResources.ar-EG.resx           # Arabic resources
└── SharedResources.Designer.cs          # Generated resource accessor

Application/
├── Common/
│   └── Extensions/
│       └── LocalizationExtensions.cs    # Helper methods
└── Features/
    └── {DomainArea}/
        └── {EntityName}s/
            ├── Dtos/                     # DTOs with localization
            ├── Validation/               # Localized validation
            └── Queries/                  # Handlers with localization
```

### 🎯 **Best Practices**

#### ✅ **DO:**
1. **Use Resource Files**: Centralize all translations in .resx files
2. **Consistent Naming**: Follow `{EntityName}{EnumValue}` pattern for resource keys
3. **Fallback Strategy**: Always provide English fallback for missing translations
4. **Computed Properties**: Use computed properties for simple localization in entities
5. **Service-Based**: Use `IStringLocalizer` in Application layer for complex scenarios
6. **Documentation**: Document the business context for each localized property

#### ❌ **DON'T:**
1. **Hardcode Translations**: Avoid hardcoded strings in business logic
2. **Mix Approaches**: Don't mix computed properties and service-based localization inconsistently
3. **Deep Dependencies**: Don't inject localization services into Domain layer
4. **Performance Issues**: Avoid complex localization logic in computed properties
5. **Missing Fallbacks**: Don't forget fallback values for unknown enum values

## Usage Examples

### In Controllers
```csharp
[HttpGet("{id}")]
public async Task<IActionResult> GetDetails(int id)
{
    var query = new Get{EntityName}DetailsQuery { Id = id };
    var result = await _mediator.Send(query);

    // Localized properties are automatically populated based on Accept-Language header
    return Ok(result.Data);
}
```

### In Frontend (TypeScript)
```typescript
interface {EntityName}Dto {
    nameAr: string;
    nameEn: string;
    localizedName: string; // Computed property from backend
    status: {EntityName}Status;
    statusDisplayName: string; // Localized enum display
}

// Usage in React/Angular components
const displayName = entity.localizedName; // Automatically localized
const statusText = entity.statusDisplayName; // Localized status
```

### Culture Configuration
```csharp
// In Program.cs or Startup.cs
services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en-US", "ar-EG" };
    options.SetDefaultCulture("en-US")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
});

// Culture can be set via:
// 1. Request Headers: Accept-Language: ar-EG
// 2. Query Parameters: ?culture=ar-EG
// 3. Cookies: .AspNetCore.Culture=c=ar-EG|uic=ar-EG
// 4. User Profile: Database-stored preference
```

## Benefits

### 🚀 **Performance Benefits**
- **Computed Properties**: No database overhead for simple localization
- **Cached Resources**: Framework caches resource strings automatically
- **Minimal Processing**: Localization only when needed

### 🔧 **Maintainability Benefits**
- **Centralized Management**: All translations in resource files
- **Type Safety**: Compile-time checking of resource keys
- **Consistent Patterns**: Standardized localization across entities

### 🌐 **User Experience Benefits**
- **Automatic Detection**: Culture-based language selection
- **Seamless Switching**: Dynamic language changes without restart
- **Cultural Appropriateness**: Proper business term translations

### 🧪 **Testing Benefits**
- **Isolated Testing**: Easy to test localization logic separately
- **Mock Resources**: Simple mocking of `IStringLocalizer`
- **Culture Testing**: Test behavior with different cultures
