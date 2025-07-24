# Edit Fund Exit Date Endpoint

## Overview
This endpoint provides dedicated functionality for editing fund exit dates as per user story JDWA-276 (Edit fund Exit date - legal council).

## Endpoint Details

**URL**: `PATCH /api/Fund/{id}/ExitDate`  
**Method**: PATCH  
**Authorization**: Required (Legal Council, Fund Manager, or Board Secretary)  
**Content-Type**: application/json

## Path Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| id | integer | Yes | The unique identifier of the fund |

## Request Body

```json
{
  "exitDate": "2025-12-31T00:00:00Z",
  "reason": "Board decision to extend fund duration",
  "notes": "Approved in board meeting on 2024-12-21"
}
```

### Request Body Schema

| Field | Type | Required | Max Length | Description |
|-------|------|----------|------------|-------------|
| exitDate | DateTime? | No | - | New exit date for the fund (null to clear) |
| reason | string? | No | 500 | Reason for the exit date change |
| notes | string? | No | 1000 | Additional notes for the change |

## Response Examples

### Success Response (200 OK)
```json
{
  "succeeded": true,
  "message": "Fund exit date updated successfully",
  "data": "Success",
  "errors": null
}
```

### Error Responses

#### 400 Bad Request - Invalid Data
```json
{
  "succeeded": false,
  "message": "Validation failed",
  "data": null,
  "errors": [
    {
      "message": "Please provide a valid ID",
      "source": "Id"
    }
  ]
}
```

#### 401 Unauthorized
```json
{
  "succeeded": false,
  "message": "User not authenticated",
  "data": null,
  "errors": null
}
```

#### 403 Forbidden
```json
{
  "succeeded": false,
  "message": "You do not have sufficient permissions to edit this fund",
  "data": null,
  "errors": null
}
```

#### 404 Not Found
```json
{
  "succeeded": false,
  "message": "Fund not found",
  "data": null,
  "errors": null
}
```

## Business Rules

1. **Authorization**: Only Legal Council, Fund Manager, or Board Secretary can edit exit dates
2. **Fund Status**: Cannot edit exit date for funds with "Under Construction" status
3. **Date Validation**: Exit date must be after the fund initiation date
4. **Future Limit**: Exit date cannot be more than 100 years in the future
5. **Null Values**: Setting exitDate to null clears the exit date

## Notifications

When an exit date is successfully updated, the system sends MSG004 notifications to:
- Fund Managers
- Board Secretaries  
- Board Members
- (Excludes the user who made the change)

## Example Usage

### Clear Exit Date
```bash
curl -X PATCH "https://api.jadwa.com/api/Fund/123/ExitDate" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "exitDate": null,
    "reason": "Exit date no longer applicable",
    "notes": "Cleared as per legal review"
  }'
```

### Set New Exit Date
```bash
curl -X PATCH "https://api.jadwa.com/api/Fund/123/ExitDate" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "exitDate": "2025-12-31T00:00:00Z",
    "reason": "Board decision to extend fund duration",
    "notes": "Approved in board meeting on 2024-12-21"
  }'
```

## Related User Stories

- **JDWA-276**: Edit fund Exit date - legal council
- **JDWA-452**: Send a Notification (MSG004 for exit date changes)

## Security Considerations

- Endpoint requires valid JWT authentication
- Role-based authorization enforced at handler level
- Fund-specific permission validation
- Audit trail automatically maintained
- Input validation prevents malicious data

## Localization

All error messages and responses support Arabic and English localization based on the `Accept-Language` header or current culture settings.
