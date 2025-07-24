# Session Termination on Logout Integration

## Overview

This document describes the integration of session termination functionality with the user logout process in the Jadwa Fund Management System. The implementation ensures that when users log out, their sessions are properly terminated and cleaned up from Redis storage.

## Implementation Details

### 1. SignOutCommand Enhancement

The `SignOutCommand` has been enhanced to support flexible session termination:

```csharp
public record SignOutCommand : ICommand<BaseResponse<string>>
{
    /// <summary>
    /// Whether to terminate all user sessions or just the current session
    /// Default: true (terminate all sessions)
    /// </summary>
    public bool TerminateAllSessions { get; set; } = true;

    /// <summary>
    /// Specific session ID to terminate (if TerminateAllSessions is false)
    /// If not provided, will attempt to extract from current JWT token
    /// </summary>
    public string? SessionId { get; set; }
}
```

### 2. SignOutCommandHandler Integration

The handler now includes comprehensive session termination:

```csharp
// 1. Terminate user sessions based on request
await TerminateUserSessionsAsync(userId, request);

// 2. Remove FCM tokens (existing functionality)
// 3. Update user's security stamp to invalidate existing tokens
// 4. Audit logging
```

### 3. Session Termination Logic

#### Terminate All Sessions (Default)
```csharp
if (request.TerminateAllSessions)
{
    var terminatedCount = await _sessionManagementService.TerminateUserSessionsAsync(
        userId, SessionTerminationReason.UserLogout);
    _logger.LogInfo($"Terminated all {terminatedCount} sessions for user {userId}");
}
```

#### Terminate Specific Session
```csharp
else
{
    var sessionId = request.SessionId ?? GetCurrentSessionId();
    if (!string.IsNullOrEmpty(sessionId))
    {
        var terminated = await _sessionManagementService.TerminateSessionAsync(
            sessionId, SessionTerminationReason.UserLogout);
    }
}
```

## API Usage Examples

### 1. Standard Logout (Terminate All Sessions)

**Request:**
```http
POST /api/Users/Authentication/Sign-Out
Authorization: Bearer {jwt-token}
Content-Type: application/json

{
    "terminateAllSessions": true
}
```

**Response:**
```json
{
    "success": true,
    "message": "You have been logged out successfully.",
    "data": "Logout successful"
}
```

### 2. Single Session Logout

**Request:**
```http
POST /api/Users/Authentication/Sign-Out
Authorization: Bearer {jwt-token}
Content-Type: application/json

{
    "terminateAllSessions": false,
    "sessionId": "abc123def456"
}
```

**Response:**
```json
{
    "success": true,
    "message": "You have been logged out successfully.",
    "data": "Logout successful"
}
```

### 3. Current Session Only (Auto-detect)

**Request:**
```http
POST /api/Users/Authentication/Sign-Out
Authorization: Bearer {jwt-token}
Content-Type: application/json

{
    "terminateAllSessions": false
}
```

## Session Termination Process

### 1. Session Identification
- Extract session ID from JWT token claims (`jti` claim)
- Use provided session ID from request
- Fall back to current session context

### 2. Session Cleanup
- Mark session as terminated in Redis
- Set termination reason and timestamp
- Remove from user's active sessions list
- Clean up activity tracking data

### 3. Logging and Audit
- Log session termination events
- Record termination reason
- Track user logout activity
- Maintain audit trail

## Error Handling

### Session Termination Failures
```csharp
try
{
    // Terminate sessions
}
catch (Exception ex)
{
    _logger.LogError(ex, $"Error terminating sessions for user {userId}");
    // Don't throw - logout should succeed even if session termination fails
}
```

### Graceful Degradation
- Logout succeeds even if session termination fails
- Errors are logged but don't block the logout process
- Security stamp update still invalidates JWT tokens
- FCM tokens are still removed

## Testing Scenarios

### 1. Successful Session Termination
```csharp
[Fact]
public async Task SignOut_WithValidSession_ShouldTerminateSession()
{
    // Arrange
    var userId = 1;
    var sessionId = "test-session-id";
    var command = new SignOutCommand { TerminateAllSessions = false, SessionId = sessionId };

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.Success);
    _mockSessionService.Verify(x => x.TerminateSessionAsync(sessionId, SessionTerminationReason.UserLogout), Times.Once);
}
```

### 2. Multiple Sessions Termination
```csharp
[Fact]
public async Task SignOut_WithMultipleSessions_ShouldTerminateAllSessions()
{
    // Arrange
    var userId = 1;
    var command = new SignOutCommand { TerminateAllSessions = true };

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.Success);
    _mockSessionService.Verify(x => x.TerminateUserSessionsAsync(userId, SessionTerminationReason.UserLogout), Times.Once);
}
```

### 3. Session Termination Failure
```csharp
[Fact]
public async Task SignOut_WithSessionTerminationFailure_ShouldStillSucceed()
{
    // Arrange
    var command = new SignOutCommand();
    _mockSessionService.Setup(x => x.TerminateUserSessionsAsync(It.IsAny<int>(), It.IsAny<SessionTerminationReason>()))
                      .ThrowsAsync(new Exception("Redis connection failed"));

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.Success); // Logout should still succeed
}
```

## Security Considerations

### 1. Session Isolation
- Each user can only terminate their own sessions
- Session IDs are validated against the authenticated user
- Cross-user session termination is prevented

### 2. Token Invalidation
- JWT tokens are invalidated through security stamp update
- Session termination provides additional security layer
- Refresh tokens are handled separately

### 3. Audit Trail
- All session terminations are logged
- Termination reasons are recorded
- User actions are tracked for compliance

## Frontend Integration

### JavaScript Example
```javascript
// Standard logout (terminate all sessions)
const logoutAllSessions = async () => {
    try {
        const response = await fetch('/api/Users/Authentication/Sign-Out', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${accessToken}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                terminateAllSessions: true
            })
        });

        if (response.ok) {
            // Clear local storage and redirect to login
            localStorage.removeItem('accessToken');
            window.location.href = '/login';
        }
    } catch (error) {
        console.error('Logout failed:', error);
    }
};

// Single session logout
const logoutCurrentSession = async () => {
    try {
        const response = await fetch('/api/Users/Authentication/Sign-Out', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${accessToken}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                terminateAllSessions: false
            })
        });

        if (response.ok) {
            localStorage.removeItem('accessToken');
            window.location.href = '/login';
        }
    } catch (error) {
        console.error('Logout failed:', error);
    }
};
```

## Benefits

### 1. Security Enhancement
- Proper session cleanup prevents session hijacking
- Multiple device logout capability
- Comprehensive audit trail

### 2. User Experience
- Flexible logout options
- Clear session management
- Reliable logout process

### 3. System Reliability
- Graceful error handling
- Redis storage cleanup
- Consistent state management

This integration ensures that the session timeout mechanism works seamlessly with the user logout process, providing both security and usability benefits.
