# MinIO Localization Implementation

## Overview
This document outlines the comprehensive localization implementation for MinIO handlers and validators in the Jadwa project. All user-facing messages in MinIO operations are now localized to support both English and Arabic languages.

## Files Modified

### 1. Localization Resources

#### SharedResourcesKey.cs
Added 23 new localization keys for MinIO operations:
- `MinIORequestCannotBeBlank`
- `MinIOFileMissingOrEmpty`
- `MinIOStorageNotEnabled`
- `MinIOInvalidFileNameOrExtension`
- `MinIOFileUploadFailed`
- `MinIOFileUploadedSuccessfully`
- `MinIOFileNotFound`
- `MinIOFileNotFoundInStorage`
- `MinIOPreviewUrlGenerationFailed`
- `MinIOPreviewUrlGeneratedSuccessfully`
- `MinIOFileDeletedSuccessfully`
- `MinIOFileDeleteFailed`
- `MinIONoFilesProvided`
- `MinIOTooManyFiles`
- `MinIOFileNameCountMismatch`
- `MinIOFileNullOrEmpty`
- `MinIOFileSizeExceedsLimit`
- `MinIOInvalidBucketName`
- `MinIOFileIdRequired`
- `MinIOFileIdMustBeGreaterThanZero`
- `MinIOFileNameTooLong`
- `MinIOModuleIdMustBeGreaterThanZero`
- `MinIOMaxFilesExceeded`
- `MinIOExpiryTimeInvalid`

#### SharedResources.en-US.resx
Added English translations for all MinIO localization keys.

#### SharedResources.ar-EG.resx
Added Arabic translations for all MinIO localization keys.

### 2. MinIO Handlers Updated

#### MinIOUploadCommandHandler.cs
- Added `IStringLocalizer<SharedResources>` dependency injection
- Localized all error and success messages
- Updated constructor to accept localizer parameter

#### MinIOUploadMultipleCommandHandler.cs
- Added `IStringLocalizer<SharedResources>` dependency injection
- Localized all validation and error messages
- Updated constructor to accept localizer parameter

#### MinIOPreviewCommandHandler.cs
- Added `IStringLocalizer<SharedResources>` dependency injection
- Localized all error messages
- Updated constructor to accept localizer parameter

#### MinIODeleteCommandHandler.cs
- Added `IStringLocalizer<SharedResources>` dependency injection
- Localized all error and success messages
- Updated constructor to accept localizer parameter

#### MinIODownloadCommandHandler.cs
- Added `IStringLocalizer<SharedResources>` dependency injection
- Localized all error messages
- Updated constructor to accept localizer parameter

### 3. MinIO Validators Updated

#### MinIOUploadValidation.cs
- Updated all validation messages to use localized keys
- Replaced hardcoded strings with localization keys

#### MinIOUploadMultipleValidation.cs
- Updated all validation messages to use localized keys
- Replaced hardcoded strings with localization keys

#### MinIODeleteValidation.cs
- Updated all validation messages to use localized keys
- Replaced hardcoded strings with localization keys

#### MinIODownloadValidation.cs
- Updated all validation messages to use localized keys
- Replaced hardcoded strings with localization keys

#### MinIOPreviewValidation.cs
- Updated all validation messages to use localized keys
- Replaced hardcoded strings with localization keys
- Removed 24-hour expiry limit to support no-expiry configuration

## Localization Examples

### English Messages
```
"The request cannot be blank"
"File is missing or empty"
"MinIO storage is not enabled"
"Invalid file name or extension"
"Failed to upload file to MinIO"
"File uploaded successfully to MinIO"
"File not found"
"File not found in storage"
"Failed to generate preview URL"
"Preview URL generated successfully"
"File deleted successfully"
"Failed to delete file"
"No files provided for upload"
"Too many files. Maximum allowed: {0}"
"File size exceeds maximum allowed size of {0} bytes"
"Bucket name must be lowercase, contain only letters, numbers, and hyphens, and be 3-63 characters long"
"File ID is required"
"File ID must be greater than 0"
"File name cannot exceed 255 characters"
"Module ID must be greater than 0"
"Maximum {0} files allowed per upload"
"Expiry time must be greater than 0"
```

### Arabic Messages
```
"لا يمكن أن يكون الطلب فارغاً"
"الملف مفقود أو فارغ"
"تخزين MinIO غير مفعل"
"اسم الملف أو الامتداد غير صحيح"
"فشل في رفع الملف إلى MinIO"
"تم رفع الملف بنجاح إلى MinIO"
"الملف غير موجود"
"الملف غير موجود في التخزين"
"فشل في إنشاء رابط المعاينة"
"تم إنشاء رابط المعاينة بنجاح"
"تم حذف الملف بنجاح"
"فشل في حذف الملف"
"لم يتم توفير ملفات للرفع"
"عدد كبير من الملفات. الحد الأقصى المسموح: {0}"
"حجم الملف يتجاوز الحد الأقصى المسموح وهو {0} بايت"
"يجب أن يكون اسم الحاوية بأحرف صغيرة ويحتوي على أحرف وأرقام وشرطات فقط، وطوله بين 3-63 حرف"
"معرف الملف مطلوب"
"يجب أن يكون معرف الملف أكبر من صفر"
"لا يمكن أن يتجاوز اسم الملف 255 حرف"
"يجب أن يكون معرف الوحدة أكبر من صفر"
"الحد الأقصى {0} ملف مسموح لكل عملية رفع"
"يجب أن يكون وقت انتهاء الصلاحية أكبر من صفر"
```

## Benefits

1. **Consistent User Experience**: All MinIO operations now provide localized messages
2. **Better Error Handling**: Users receive clear, localized error messages
3. **Maintainability**: Centralized localization keys make updates easier
4. **Accessibility**: Arabic-speaking users get native language support
5. **Professional Quality**: Proper localization enhances application quality

## Usage

The localization is automatically applied based on the user's language preference. The system supports:
- **English (en-US)**: Default fallback language
- **Arabic (ar-EG)**: Primary language for the application

All MinIO operations will now return localized messages based on the current user's language setting.

## Testing

To test the localization:
1. Set the application culture to Arabic (`ar-EG`)
2. Perform MinIO operations (upload, download, preview, delete)
3. Verify that error and success messages appear in Arabic
4. Switch to English culture (`en-US`) and verify English messages

## Future Enhancements

- Additional language support can be easily added by creating new resource files
- Custom localization for specific business domains
- Dynamic language switching based on user preferences
