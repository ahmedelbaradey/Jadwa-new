# Firebase Cloud Messaging (FCM) Implementation Guide for Jadwa API

## Overview
This guide provides comprehensive instructions for implementing Firebase Cloud Messaging (FCM) push web notifications in the Jadwa API fund notification system. The implementation integrates seamlessly with the existing notification infrastructure while adding real-time push notification capabilities.

## Architecture Overview

### Backend Components
1. **Firebase Admin SDK Integration** - Server-side Firebase integration
2. **Device Token Management** - Database storage and management of FCM tokens
3. **Firebase Notification Service** - Service for sending push notifications
4. **Enhanced Fund Notification Service** - Integration with existing notification system
5. **Background Cleanup Service** - Token validation and cleanup
6. **Device Token API Endpoints** - Token registration and management

### Frontend Components
1. **Firebase SDK Integration** - Client-side Firebase setup
2. **Service Worker** - Background notification handling
3. **Token Registration** - Device token management
4. **Notification Handling** - Click actions and user interactions

## Backend Implementation

### 1. Firebase Configuration

#### appsettings.json
```json
{
  "Firebase": {
    "ProjectId": "jadwa-api-project",
    "ServiceAccountPath": "firebase-service-account.json",
    "DatabaseUrl": "https://jadwa-api-project-default-rtdb.firebaseio.com/",
    "CleanupIntervalHours": 6
  }
}
```

#### Service Registration
The following services are automatically registered in the DI container:
- `IFirebaseNotificationService` → `FirebaseNotificationService`
- `IDeviceTokenRepository` → `DeviceTokenRepository`
- `FirebaseTokenCleanupService` (Background Service)

### 2. Database Schema

#### DeviceToken Entity
```sql
CREATE TABLE DeviceTokens (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Token NVARCHAR(500) NOT NULL,
    DeviceType INT NOT NULL DEFAULT 1, -- 1=Web, 2=Android, 3=iOS
    UserAgent NVARCHAR(1000) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    LastVerifiedAt DATETIME2 NULL,
    ExpiresAt DATETIME2 NULL,
    Metadata NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 NOT NULL,
    CreatedBy INT NOT NULL,
    UpdatedAt DATETIME2 NULL,
    UpdatedBy INT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE INDEX IX_DeviceTokens_UserId ON DeviceTokens(UserId);
CREATE INDEX IX_DeviceTokens_Token ON DeviceTokens(Token);
CREATE INDEX IX_DeviceTokens_IsActive ON DeviceTokens(IsActive);
```

### 3. API Endpoints

#### Device Token Registration
```http
POST /api/DeviceToken/Register
Authorization: Bearer {jwt-token}
Content-Type: application/json

{
  "token": "fcm-device-token",
  "deviceType": 1,
  "userAgent": "Mozilla/5.0...",
  "metadata": "{\"browser\":\"Chrome\"}"
}
```

#### Token Deactivation
```http
POST /api/DeviceToken/Deactivate
Authorization: Bearer {jwt-token}
Content-Type: application/json

"fcm-device-token"
```

### 4. Notification Integration

The Firebase service integrates with the existing `FundNotificationService` to send both database notifications and push notifications simultaneously:

```csharp
// Database notification
await _mediator.Send(command);

// Push notification
await SendFirebasePushNotification("MSG002", fund, userName, recipientUserId, additionalData);
```

### 5. Message Types and Localization

All existing MSG notification types are supported with full Arabic/English localization:

- **MSG002**: Fund creation by Legal Council
- **MSG003**: User removal from fund
- **MSG004**: Exit date change
- **MSG005**: User assignment to fund
- **MSG006**: Fund data modification

## Frontend Implementation

### 1. Firebase Configuration

#### Install Firebase SDK
```bash
npm install firebase
```

#### Initialize Firebase
```javascript
import { initializeFirebaseMessaging } from './firebase-config.js';

// Initialize when user logs in
const success = await initializeFirebaseMessaging(
  'https://api.jadwa.com',
  userAuthToken,
  handleNotificationMessage
);
```

### 2. Service Worker Setup

Place `firebase-messaging-sw.js` in your public folder and ensure it's accessible at `/firebase-messaging-sw.js`.

### 3. Notification Handling

#### Foreground Messages
```javascript
function handleNotificationMessage(payload) {
  // Handle notification received while app is in foreground
  console.log('Notification received:', payload);
  
  // Update UI, show toast, etc.
  showInAppNotification(payload);
}
```

#### Background Messages
Background messages are automatically handled by the service worker and will show browser notifications.

### 4. Click Actions

Notifications include click actions that navigate users to relevant fund details:
- Fund notifications → `/funds/{fundId}`
- General notifications → `/funds`

## Security Considerations

### 1. Firebase Service Account
- Store the Firebase service account JSON file securely
- Use environment variables for sensitive configuration
- Restrict file permissions (600 or 400)

### 2. Token Validation
- All tokens are validated with Firebase before storage
- Invalid tokens are automatically deactivated
- Regular cleanup removes expired tokens

### 3. Authentication
- Device token registration requires valid JWT authentication
- Users can only manage their own device tokens
- API endpoints are protected with authorization

## Testing and Validation

### 1. Browser Testing
Test push notifications across different browsers:
- Chrome (Desktop & Mobile)
- Firefox (Desktop & Mobile)
- Safari (Desktop & Mobile)
- Edge (Desktop)

### 2. Notification States
Test notifications in different app states:
- Foreground (app is active)
- Background (app is open but not focused)
- Closed (app is completely closed)

### 3. Permission Handling
Test permission scenarios:
- First-time permission request
- Permission denied
- Permission revoked
- Permission re-granted

## Monitoring and Analytics

### 1. Logging
The implementation includes comprehensive logging:
- Token registration/deactivation
- Notification delivery success/failure
- Firebase API errors
- Background service activities

### 2. Metrics to Monitor
- Token registration rate
- Notification delivery rate
- Token validation success rate
- Background service performance

## Troubleshooting

### Common Issues

#### 1. Service Worker Not Registering
- Ensure `firebase-messaging-sw.js` is in the public folder
- Check browser console for registration errors
- Verify HTTPS is enabled (required for service workers)

#### 2. Notifications Not Appearing
- Check browser notification permissions
- Verify Firebase configuration
- Check service worker console logs
- Ensure token is registered with backend

#### 3. Token Registration Failing
- Verify Firebase project configuration
- Check API authentication
- Validate Firebase service account permissions

### Debug Tools
- Browser DevTools → Application → Service Workers
- Browser DevTools → Application → Storage → IndexedDB → Firebase
- Firebase Console → Cloud Messaging → Send test message

## Performance Considerations

### 1. Token Cleanup
- Background service runs every 6 hours (configurable)
- Processes tokens in batches of 100
- Removes expired tokens older than 30 days

### 2. Batch Operations
- Multiple token validation in single Firebase call
- Batch notification sending for multiple recipients
- Efficient database queries with proper indexing

### 3. Error Handling
- Graceful degradation when Firebase is unavailable
- Retry logic for failed notifications
- Non-blocking error handling to prevent service disruption

## Deployment Checklist

### Backend
- [ ] Firebase service account JSON file deployed securely
- [ ] Firebase configuration in appsettings.json
- [ ] Database migration for DeviceTokens table
- [ ] Background service registered and running
- [ ] API endpoints tested and documented

### Frontend
- [ ] Firebase SDK integrated
- [ ] Service worker deployed to public folder
- [ ] Firebase configuration updated with production values
- [ ] VAPID key configured
- [ ] Notification permissions implemented
- [ ] Click actions tested

### Firebase Console
- [ ] Project created and configured
- [ ] Web app registered
- [ ] VAPID key generated
- [ ] Service account created with appropriate permissions
- [ ] Cloud Messaging enabled

## Conclusion

This implementation provides a robust, scalable, and secure push notification system that integrates seamlessly with the existing Jadwa API notification infrastructure. The system supports full localization, comprehensive error handling, and automatic token management while maintaining high performance and reliability.
