# Session Timeout Testing Guide

## Overview

This document provides comprehensive testing strategies and examples for the 30-minute session timeout mechanism in the Jadwa Fund Management System. The testing covers unit tests, integration tests, API tests, and end-to-end scenarios.

## Testing Strategy

### 1. Unit Testing (95% Coverage Target)
- Session management service methods
- Session validation logic
- Timeout calculations
- Security features
- Audit logging
- Localization

### 2. Integration Testing
- API endpoint functionality
- Database interactions
- Redis cache operations
- Middleware integration
- Service dependencies

### 3. API Testing
- Postman collections
- Authentication flows
- Session management endpoints
- Error scenarios
- Rate limiting

### 4. End-to-End Testing
- Complete user journeys
- Cross-browser compatibility
- Mobile responsiveness
- Accessibility compliance

## Unit Test Examples

### SessionManagementService Tests

```csharp
// Tests/Infrastructure/Services/SessionManagementServiceTests.cs
using Xunit;
using Moq;
using Microsoft.Extensions.Caching.Distributed;
using Infrastructure.Service.Sessions;
using Domain.Entities.Sessions;
using Domain.Helpers;

namespace Tests.Infrastructure.Services
{
    public class SessionManagementServiceTests
    {
        private readonly Mock<IDistributedCache> _mockCache;
        private readonly Mock<ILoggerManager> _mockLogger;
        private readonly Mock<ISessionAuditService> _mockAuditService;
        private readonly Mock<ISessionSecurityService> _mockSecurityService;
        private readonly SessionSettings _sessionSettings;
        private readonly SessionManagementService _service;

        public SessionManagementServiceTests()
        {
            _mockCache = new Mock<IDistributedCache>();
            _mockLogger = new Mock<ILoggerManager>();
            _mockAuditService = new Mock<ISessionAuditService>();
            _mockSecurityService = new Mock<ISessionSecurityService>();
            
            _sessionSettings = new SessionSettings
            {
                TimeoutMinutes = 30,
                WarningMinutes = 5,
                EnableSlidingExpiration = true,
                EnableConcurrentSessions = true,
                MaxConcurrentSessions = 3,
                RoleBasedTimeouts = new Dictionary<string, int>
                {
                    { "FundManager", 60 },
                    { "BoardMember", 45 }
                }
            };

            _service = new SessionManagementService(
                _mockCache.Object,
                _sessionSettings,
                _mockLogger.Object,
                _mockAuditService.Object,
                _mockSecurityService.Object
            );
        }

        [Fact]
        public async Task CreateSessionAsync_WithValidData_ShouldCreateSession()
        {
            // Arrange
            var userId = 1;
            var jwtTokenId = "test-jwt-id";
            var userRole = "FundManager";

            // Act
            var result = await _service.CreateSessionAsync(userId, jwtTokenId, userRole);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(jwtTokenId, result.JwtTokenId);
            Assert.Equal(userRole, result.UserRole);
            Assert.True(result.IsActive);
            Assert.False(result.IsExpired);
            
            // Verify audit logging was called
            _mockAuditService.Verify(x => x.LogSessionCreatedAsync(
                It.IsAny<UserSession>(), 
                It.IsAny<string>()), 
                Times.Once);
        }

        [Theory]
        [InlineData("FundManager", 60)]
        [InlineData("BoardMember", 45)]
        [InlineData("LegalCouncil", 30)] // Default timeout
        [InlineData(null, 30)] // No role specified
        public async Task CreateSessionAsync_WithDifferentRoles_ShouldApplyCorrectTimeout(
            string userRole, int expectedTimeoutMinutes)
        {
            // Arrange
            var userId = 1;
            var jwtTokenId = "test-jwt-id";

            // Act
            var result = await _service.CreateSessionAsync(userId, jwtTokenId, userRole);

            // Assert
            var actualTimeout = (result.ExpiresAt - result.CreatedAt).TotalMinutes;
            Assert.Equal(expectedTimeoutMinutes, actualTimeout, precision: 1);
        }

        [Fact]
        public async Task ExtendSessionAsync_WithValidSession_ShouldExtendTimeout()
        {
            // Arrange
            var sessionId = "test-session-id";
            var userId = 1;
            var userRole = "FundManager";
            
            var existingSession = new UserSession
            {
                SessionId = sessionId,
                UserId = userId,
                UserRole = userRole,
                CreatedAt = DateTime.UtcNow.AddMinutes(-20),
                LastActivityAt = DateTime.UtcNow.AddMinutes(-5),
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                IsActive = true
            };

            _mockCache.Setup(x => x.GetStringAsync(It.IsAny<string>(), default))
                .ReturnsAsync(JsonSerializer.Serialize(existingSession));

            // Act
            var result = await _service.ExtendSessionAsync(sessionId, userRole);

            // Assert
            Assert.True(result);
            
            // Verify audit logging was called
            _mockAuditService.Verify(x => x.LogSessionExtendedAsync(
                sessionId, userId, It.IsAny<DateTime>(), It.IsAny<DateTime>(), 
                It.IsAny<string>(), userRole), 
                Times.Once);
        }

        [Fact]
        public async Task ValidateSessionAsync_WithExpiredSession_ShouldReturnInvalid()
        {
            // Arrange
            var sessionId = "expired-session-id";
            var expiredSession = new UserSession
            {
                SessionId = sessionId,
                UserId = 1,
                CreatedAt = DateTime.UtcNow.AddHours(-2),
                LastActivityAt = DateTime.UtcNow.AddHours(-1),
                ExpiresAt = DateTime.UtcNow.AddMinutes(-30), // Expired 30 minutes ago
                IsActive = true
            };

            _mockCache.Setup(x => x.GetStringAsync(It.IsAny<string>(), default))
                .ReturnsAsync(JsonSerializer.Serialize(expiredSession));

            // Act
            var result = await _service.ValidateSessionAsync(sessionId);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Session expired", result.FailureReason);
        }

        [Fact]
        public async Task GetSessionStatisticsAsync_WithMultipleSessions_ShouldReturnCorrectStats()
        {
            // Arrange
            var userId = 1;
            var sessions = new List<UserSession>
            {
                new UserSession 
                { 
                    SessionId = "session1", 
                    UserId = userId, 
                    DeviceType = "Desktop",
                    Location = "Riyadh",
                    CreatedAt = DateTime.UtcNow.AddHours(-2),
                    LastActivityAt = DateTime.UtcNow.AddMinutes(-10)
                },
                new UserSession 
                { 
                    SessionId = "session2", 
                    UserId = userId, 
                    DeviceType = "Mobile",
                    Location = "Jeddah",
                    CreatedAt = DateTime.UtcNow.AddHours(-1),
                    LastActivityAt = DateTime.UtcNow.AddMinutes(-5)
                }
            };

            // Mock the user sessions retrieval
            _mockCache.Setup(x => x.GetStringAsync($"user_sessions:{userId}", default))
                .ReturnsAsync(JsonSerializer.Serialize(sessions.Select(s => s.SessionId).ToList()));

            foreach (var session in sessions)
            {
                _mockCache.Setup(x => x.GetStringAsync($"session:{session.SessionId}", default))
                    .ReturnsAsync(JsonSerializer.Serialize(session));
            }

            // Act
            var result = await _service.GetSessionStatisticsAsync(userId);

            // Assert
            Assert.Equal(2, result.ActiveSessions);
            Assert.Contains("Desktop", result.DeviceTypes);
            Assert.Contains("Mobile", result.DeviceTypes);
            Assert.Contains("Riyadh", result.Locations);
            Assert.Contains("Jeddah", result.Locations);
            Assert.True(result.AverageSessionDurationMinutes > 0);
        }
    }
}
```

### Session Validation Middleware Tests

```csharp
// Tests/Infrastructure/Middleware/SessionValidationMiddlewareTests.cs
using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Middleware;

namespace Tests.Infrastructure.Middleware
{
    public class SessionValidationMiddlewareTests
    {
        private readonly Mock<RequestDelegate> _mockNext;
        private readonly Mock<ILoggerManager> _mockLogger;
        private readonly SessionSettings _sessionSettings;
        private readonly SessionValidationMiddleware _middleware;

        public SessionValidationMiddlewareTests()
        {
            _mockNext = new Mock<RequestDelegate>();
            _mockLogger = new Mock<ILoggerManager>();
            _sessionSettings = new SessionSettings
            {
                EnableSessionFingerprinting = true,
                TimeoutMinutes = 30
            };

            _middleware = new SessionValidationMiddleware(
                _mockNext.Object,
                _mockLogger.Object,
                _sessionSettings
            );
        }

        [Theory]
        [InlineData("/api/Users/Authentication/Sign-In")]
        [InlineData("/api/Users/Authentication/Sign-Out")]
        [InlineData("/swagger/index.html")]
        [InlineData("/health")]
        public async Task InvokeAsync_WithExcludedPaths_ShouldSkipValidation(string path)
        {
            // Arrange
            var context = CreateHttpContext(path);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _mockNext.Verify(x => x(context), Times.Once);
            Assert.DoesNotContain("SessionInfo", context.Items.Keys);
        }

        [Fact]
        public async Task InvokeAsync_WithUnauthenticatedUser_ShouldSkipValidation()
        {
            // Arrange
            var context = CreateHttpContext("/api/protected-endpoint");
            context.User = new ClaimsPrincipal(); // Not authenticated

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _mockNext.Verify(x => x(context), Times.Once);
            Assert.DoesNotContain("SessionInfo", context.Items.Keys);
        }

        [Fact]
        public async Task InvokeAsync_WithValidSession_ShouldAddSessionInfoToContext()
        {
            // Arrange
            var context = CreateAuthenticatedHttpContext("/api/protected-endpoint");
            var mockSessionService = new Mock<ISessionManagementService>();
            
            var sessionInfo = new SessionInfo
            {
                SessionId = "test-session",
                IsActive = true,
                IsExpired = false,
                RemainingSeconds = 1800
            };

            mockSessionService.Setup(x => x.ValidateSessionAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), true))
                .ReturnsAsync(new SessionValidationResponse
                {
                    IsValid = true,
                    SessionInfo = sessionInfo,
                    WasExtended = false
                });

            context.RequestServices = CreateServiceProvider(mockSessionService.Object);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _mockNext.Verify(x => x(context), Times.Once);
            Assert.Contains("SessionInfo", context.Items.Keys);
            Assert.Equal(sessionInfo, context.Items["SessionInfo"]);
        }

        private HttpContext CreateHttpContext(string path)
        {
            var context = new DefaultHttpContext();
            context.Request.Path = path;
            context.Request.Method = "GET";
            return context;
        }

        private HttpContext CreateAuthenticatedHttpContext(string path)
        {
            var context = CreateHttpContext(path);
            
            var claims = new[]
            {
                new Claim("Id", "1"),
                new Claim(ClaimTypes.Role, "FundManager"),
                new Claim(JwtRegisteredClaimNames.Jti, "test-jwt-id")
            };
            
            var identity = new ClaimsIdentity(claims, "Bearer");
            context.User = new ClaimsPrincipal(identity);
            
            context.Request.Headers["Authorization"] = "Bearer test-token";
            
            return context;
        }

        private IServiceProvider CreateServiceProvider(ISessionManagementService sessionService)
        {
            var services = new ServiceCollection();
            services.AddSingleton(sessionService);
            return services.BuildServiceProvider();
        }
    }
}
```

## API Testing with Postman

### Session Management Collection

```json
{
  "info": {
    "name": "Session Management API Tests",
    "description": "Comprehensive tests for session timeout functionality"
  },
  "auth": {
    "type": "bearer",
    "bearer": [
      {
        "key": "token",
        "value": "{{accessToken}}",
        "type": "string"
      }
    ]
  },
  "variable": [
    {
      "key": "baseUrl",
      "value": "https://localhost:5001"
    },
    {
      "key": "accessToken",
      "value": ""
    },
    {
      "key": "sessionId",
      "value": ""
    }
  ],
  "item": [
    {
      "name": "Authentication",
      "item": [
        {
          "name": "Login - Fund Manager",
          "request": {
            "method": "POST",
            "header": [
              {
                "key": "Content-Type",
                "value": "application/json"
              }
            ],
            "body": {
              "mode": "raw",
              "raw": "{\n  \"email\": \"fundmanager@jadwa.com\",\n  \"password\": \"Test123!\",\n  \"rememberMe\": false\n}"
            },
            "url": {
              "raw": "{{baseUrl}}/api/Users/Authentication/Sign-In",
              "host": ["{{baseUrl}}"],
              "path": ["api", "Users", "Authentication", "Sign-In"]
            }
          },
          "event": [
            {
              "listen": "test",
              "script": {
                "exec": [
                  "pm.test('Login successful', function () {",
                  "    pm.response.to.have.status(200);",
                  "    const response = pm.response.json();",
                  "    pm.expect(response.success).to.be.true;",
                  "    pm.expect(response.data.accessToken).to.exist;",
                  "    pm.collectionVariables.set('accessToken', response.data.accessToken);",
                  "    if (response.data.sessionTimeout) {",
                  "        pm.expect(response.data.sessionTimeout.timeoutMinutes).to.equal(60); // Fund Manager timeout",
                  "    }",
                  "});"
                ]
              }
            }
          ]
        }
      ]
    },
    {
      "name": "Session Management",
      "item": [
        {
          "name": "Get Session Status",
          "request": {
            "method": "GET",
            "url": {
              "raw": "{{baseUrl}}/api/Users/Session/Session-Status",
              "host": ["{{baseUrl}}"],
              "path": ["api", "Users", "Session", "Session-Status"]
            }
          },
          "event": [
            {
              "listen": "test",
              "script": {
                "exec": [
                  "pm.test('Session status retrieved', function () {",
                  "    pm.response.to.have.status(200);",
                  "    const response = pm.response.json();",
                  "    pm.expect(response.success).to.be.true;",
                  "    pm.expect(response.data.sessionId).to.exist;",
                  "    pm.expect(response.data.isActive).to.be.true;",
                  "    pm.expect(response.data.remainingSeconds).to.be.above(0);",
                  "    pm.collectionVariables.set('sessionId', response.data.sessionId);",
                  "});"
                ]
              }
            }
          ]
        },
        {
          "name": "Extend Session",
          "request": {
            "method": "POST",
            "header": [
              {
                "key": "Content-Type",
                "value": "application/json"
              }
            ],
            "body": {
              "mode": "raw",
              "raw": "{\n  \"reason\": \"Manual extension for testing\",\n  \"isActivityBased\": false\n}"
            },
            "url": {
              "raw": "{{baseUrl}}/api/Users/Session/Extend-Session",
              "host": ["{{baseUrl}}"],
              "path": ["api", "Users", "Session", "Extend-Session"]
            }
          },
          "event": [
            {
              "listen": "test",
              "script": {
                "exec": [
                  "pm.test('Session extended successfully', function () {",
                  "    pm.response.to.have.status(200);",
                  "    const response = pm.response.json();",
                  "    pm.expect(response.success).to.be.true;",
                  "    pm.expect(response.data.success).to.be.true;",
                  "    pm.expect(response.data.newRemainingSeconds).to.be.above(3000); // Should be close to full timeout",
                  "});"
                ]
              }
            }
          ]
        },
        {
          "name": "Get Timeout Configuration",
          "request": {
            "method": "GET",
            "url": {
              "raw": "{{baseUrl}}/api/Users/Session/Timeout-Config?userRole=FundManager",
              "host": ["{{baseUrl}}"],
              "path": ["api", "Users", "Session", "Timeout-Config"],
              "query": [
                {
                  "key": "userRole",
                  "value": "FundManager"
                }
              ]
            }
          },
          "event": [
            {
              "listen": "test",
              "script": {
                "exec": [
                  "pm.test('Timeout config retrieved', function () {",
                  "    pm.response.to.have.status(200);",
                  "    const response = pm.response.json();",
                  "    pm.expect(response.success).to.be.true;",
                  "    pm.expect(response.data.timeoutMinutes).to.equal(60);",
                  "    pm.expect(response.data.warningMinutes).to.equal(5);",
                  "    pm.expect(response.data.slidingExpiration).to.be.true;",
                  "});"
                ]
              }
            }
          ]
        }
      ]
    },
    {
      "name": "Error Scenarios",
      "item": [
        {
          "name": "Access Protected Endpoint Without Token",
          "request": {
            "method": "GET",
            "header": [],
            "url": {
              "raw": "{{baseUrl}}/api/Users/Session/Session-Status",
              "host": ["{{baseUrl}}"],
              "path": ["api", "Users", "Session", "Session-Status"]
            }
          },
          "event": [
            {
              "listen": "test",
              "script": {
                "exec": [
                  "pm.test('Unauthorized access blocked', function () {",
                  "    pm.response.to.have.status(401);",
                  "});"
                ]
              }
            }
          ]
        },
        {
          "name": "Extend Session with Invalid Token",
          "request": {
            "method": "POST",
            "header": [
              {
                "key": "Authorization",
                "value": "Bearer invalid-token"
              },
              {
                "key": "Content-Type",
                "value": "application/json"
              }
            ],
            "body": {
              "mode": "raw",
              "raw": "{\n  \"reason\": \"Test with invalid token\"\n}"
            },
            "url": {
              "raw": "{{baseUrl}}/api/Users/Session/Extend-Session",
              "host": ["{{baseUrl}}"],
              "path": ["api", "Users", "Session", "Extend-Session"]
            }
          },
          "event": [
            {
              "listen": "test",
              "script": {
                "exec": [
                  "pm.test('Invalid token rejected', function () {",
                  "    pm.response.to.have.status(401);",
                  "});"
                ]
              }
            }
          ]
        }
      ]
    }
  ]
}
```

This comprehensive testing guide provides the foundation for ensuring the session timeout mechanism works correctly across all scenarios and maintains the required 95% code coverage target.
