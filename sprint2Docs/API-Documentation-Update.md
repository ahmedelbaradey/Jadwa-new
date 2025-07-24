# API Documentation Update - Sprint 2
## New Resolution Management Endpoints

**Version:** 1.0  
**Date:** December 26, 2025  
**Base URL:** `/api/resolutions`  
**Authentication:** Required (JWT Bearer Token)  
**Authorization:** Legal Council or Board Secretary roles

---

## 📋 New Endpoints Overview

| Endpoint | Method | Description | User Story |
|----------|--------|-------------|------------|
| `/EditResolutionItems` | PUT | Edit resolution items and conflicts | JDWA-566 |
| `/EditResolutionAttachments` | PUT | Edit resolution attachments | JDWA-568 |
| `/CompleteResolutionData` | PUT | Complete basic resolution data | JDWA-506 |
| `/CompleteResolutionItems` | PUT | Complete resolution items | JDWA-507 |
| `/CompleteResolutionAttachments` | PUT | Complete resolution attachments | JDWA-505 |

---

## 🔧 Endpoint Details

### 1. Edit Resolution Items
**Endpoint:** `PUT /api/resolutions/EditResolutionItems`  
**Authorization:** `ResolutionPermission.Edit`  
**User Story:** JDWA-566

#### Request Body
```json
{
  "resolutionId": 123,
  "resolutionItems": [
    {
      "id": 0,
      "description": "Resolution item description (max 500 chars)",
      "hasConflict": true,
      "conflictMembers": [
        {
          "boardMemberId": 456
        }
      ]
    }
  ],
  "saveAsDraft": false
}
```

#### Response
```json
{
  "isSuccess": true,
  "message": "Resolution items updated successfully",
  "data": "Success message in user's preferred language"
}
```

#### Business Rules
- Maximum 500 characters for item description
- Conflict members must belong to the same fund
- Resolution status must be Draft, Pending, or CompletingData
- Auto-generates item titles (Item1, Item2, etc.)
- SaveAsDraft=true → Status: CompletingData
- SaveAsDraft=false → Status: WaitingForConfirmation + Notifications

---

### 2. Edit Resolution Attachments
**Endpoint:** `PUT /api/resolutions/EditResolutionAttachments`  
**Authorization:** `ResolutionPermission.Edit`  
**User Story:** JDWA-568

#### Request Body
```json
{
  "resolutionId": 123,
  "attachmentIds": [1, 2, 3, 4, 5],
  "saveAsDraft": true
}
```

#### Response
```json
{
  "isSuccess": true,
  "message": "Resolution attachments updated successfully",
  "data": "Success message in user's preferred language"
}
```

#### Business Rules
- Maximum 10 attachments per resolution
- All attachment IDs must exist and be valid
- Replaces existing attachments (not additive)
- Duplicate attachment IDs not allowed
- Resolution status must be editable

---

### 3. Complete Resolution Data
**Endpoint:** `PUT /api/resolutions/CompleteResolutionData`  
**Authorization:** `ResolutionPermission.Edit`  
**User Story:** JDWA-506

#### Request Body
```json
{
  "id": 123,
  "description": "Updated resolution description",
  "resolutionTypeId": 2,
  "votingType": 1,
  "memberVotingResult": 1,
  "fundId": 456,
  "attachmentId": 789,
  "saveAsDraft": false
}
```

#### Response
```json
{
  "isSuccess": true,
  "message": "Resolution data completed successfully",
  "data": "Success message in user's preferred language"
}
```

#### Business Rules
- Resolution must be in Pending or CompletingData status
- All required fields must be provided
- Description maximum 500 characters
- ResolutionType and Fund must exist
- Attachment (if provided) must exist

---

### 4. Complete Resolution Items
**Endpoint:** `PUT /api/resolutions/CompleteResolutionItems`  
**Authorization:** `ResolutionPermission.Edit`  
**User Story:** JDWA-507

#### Request Body
```json
{
  "resolutionId": 123,
  "resolutionItems": [
    {
      "description": "New resolution item",
      "hasConflict": false,
      "conflictMembers": []
    }
  ],
  "saveAsDraft": true
}
```

#### Response
```json
{
  "isSuccess": true,
  "message": "Resolution items completed successfully",
  "data": "Success message in user's preferred language"
}
```

#### Special Cases
- Empty items array returns MSG006 confirmation message
- Adds to existing items (not replacement)
- Auto-generates sequential item numbering

---

### 5. Complete Resolution Attachments
**Endpoint:** `PUT /api/resolutions/CompleteResolutionAttachments`  
**Authorization:** `ResolutionPermission.Edit`  
**User Story:** JDWA-505

#### Request Body
```json
{
  "resolutionId": 123,
  "attachmentIds": [10, 11, 12],
  "saveAsDraft": false
}
```

#### Response
```json
{
  "isSuccess": true,
  "message": "Resolution attachments completed successfully",
  "data": "Success message in user's preferred language"
}
```

#### Business Rules
- Adds to existing attachments (not replacement)
- Prevents duplicate attachments
- Maximum 10 total attachments enforced

---

## 🔒 Security & Authorization

### Required Permissions
- **Authentication:** Valid JWT token required
- **Authorization:** User must have Legal Council or Board Secretary role
- **Fund Access:** User must have access to the specific fund
- **Operation Level:** ResolutionPermission.Edit policy enforced

### RBAC Implementation
```csharp
[Authorize(Policy = ResolutionPermission.Edit)]
```

### Cross-Fund Security
- Users can only edit resolutions for funds they have access to
- Board member conflicts validated within fund scope
- Attachment access validated per fund permissions

---

## 📨 Notification System

### MSG003 - Resolution Data Completion
**Triggered When:** SaveAsDraft = false
**Recipients:**
- Fund Managers (always)
- Legal Council (if editor is Board Secretary)
- Board Secretaries (if editor is Legal Council)

**Notification Content:**
```json
{
  "title": "Resolution Data Completed",
  "body": "Resolution {code} data has been completed for fund {fundName} by {userName}",
  "notificationType": "ResolutionUpdated",
  "isLocalized": true
}
```

---

## 🌐 Localization Support

### Supported Languages
- **Arabic (ar)** - Right-to-left text direction
- **English (en)** - Left-to-right text direction

### Localized Elements
- Success/error messages
- Validation error messages
- Notification content
- Enum value displays
- Field labels and descriptions

### Resource Keys
All messages use SharedResourcesKey constants:
- `ResolutionDataCompletedSuccessfully`
- `RecordSavedSuccessfully`
- `MaxAttachmentsReached`
- `NoItemsAddedConfirmation`

---

## ⚠️ Error Handling

### Standard Error Codes
- **MSG001** - No data found
- **MSG002** - System error occurred
- **400 Bad Request** - Validation errors
- **401 Unauthorized** - Authentication required
- **403 Forbidden** - Insufficient permissions
- **404 Not Found** - Resource not found
- **500 Internal Server Error** - System error

### Error Response Format
```json
{
  "isSuccess": false,
  "message": "Localized error message",
  "errors": [
    {
      "field": "ResolutionId",
      "message": "Invalid resolution ID"
    }
  ]
}
```

---

## 🧪 Testing Guidelines

### Unit Testing
- Test all validation rules
- Test authorization scenarios
- Test localization functionality
- Test business logic edge cases

### Integration Testing
- End-to-end workflow testing
- Database transaction testing
- Notification system testing
- Cross-service integration

### Manual Testing
- Role-based access testing
- Multi-language testing
- File upload/attachment testing
- Workflow state transitions

---

## 📋 Validation Rules Summary

### Common Validations
- **ResolutionId:** Required, must exist, must be accessible to user
- **SaveAsDraft:** Boolean flag for workflow control
- **Status Validation:** Resolution must be in editable status

### Item-Specific Validations
- **Description:** Maximum 500 characters
- **Conflict Members:** Must belong to same fund as resolution
- **Board Member IDs:** Must exist and be valid

### Attachment-Specific Validations
- **Maximum Count:** 10 attachments per resolution
- **Attachment IDs:** Must exist and be accessible
- **Duplicates:** Not allowed in single request

---

## 🔄 Workflow States

### Status Transitions
```
Draft/Pending/CompletingData → [Edit/Complete Operations] → CompletingData (SaveAsDraft=true)
Draft/Pending/CompletingData → [Edit/Complete Operations] → WaitingForConfirmation (SaveAsDraft=false)
```

### State Validation
- **Editable States:** Draft, Pending, CompletingData
- **Non-Editable States:** WaitingForConfirmation, Confirmed, Rejected, etc.
- **Completion States:** Pending, CompletingData (for Complete operations)

---

## 📚 Additional Resources

### Related Documentation
- [Sprint 2 Implementation Summary](./Sprint2-Implementation-Summary.md)
- [Clean Architecture Guidelines](./CleanArchitectureSteps.md)
- [Localization Implementation](./CleanDTOsImplementation.md)
- [State Pattern Documentation](./StatePatternImplementationSummary.md)

### Code Examples
- Command Handler implementations in `/src/Core/Application/Features/Resolutions/Commands/`
- Validation classes in `/src/Core/Application/Features/Resolutions/Validation/`
- Controller endpoints in `/src/Infrastructure/Presentation/Controllers/Resolution/`
