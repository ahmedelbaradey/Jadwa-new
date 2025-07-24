# WhatsApp Service Integration Examples

This document provides examples of how to integrate the WhatsApp notification service with existing system operations.

**Note**: The WhatsApp service uses DTOs (Data Transfer Objects) from the Application layer rather than domain entities, following proper separation of concerns.

## 1. User Registration Integration

### In User Registration Command Handler

```csharp
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, BaseResponse<int>>
{
    private readonly IWhatsAppNotificationService _whatsAppService;
    private readonly UserManager<User> _userManager;
    // ... other dependencies

    public async Task<BaseResponse<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Create user logic...
            var user = new User { /* user properties */ };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                // Send WhatsApp registration notification
                await _whatsAppService.SendUserRegistrationMessageAsync(
                    user.Id,
                    request.PhoneNumber,
                    request.Username,
                    "https://jadwa.com/login",
                    cancellationToken);

                return Success(user.Id);
            }

            return BadRequest<int>("User creation failed");
        }
        catch (Exception ex)
        {
            // Handle exceptions
            return BadRequest<int>(ex.Message);
        }
    }
}
```

## 2. Password Reset Integration

### In Password Reset Command Handler

```csharp
public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, BaseResponse<bool>>
{
    private readonly IWhatsAppNotificationService _whatsAppService;
    private readonly UserManager<User> _userManager;
    // ... other dependencies

    public async Task<BaseResponse<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return NotFound<bool>("User not found");

            // Generate temporary password
            var temporaryPassword = GenerateTemporaryPassword();
            
            // Reset user password
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, temporaryPassword);

            if (result.Succeeded)
            {
                // Send WhatsApp password reset notification
                await _whatsAppService.SendPasswordResetMessageAsync(
                    user.Id,
                    user.PhoneNumber,
                    temporaryPassword,
                    cancellationToken);

                return Success(true);
            }

            return BadRequest<bool>("Password reset failed");
        }
        catch (Exception ex)
        {
            return BadRequest<bool>(ex.Message);
        }
    }
}
```

## 3. Account Activation/Deactivation Integration

### In User Status Command Handler

```csharp
public class UpdateUserStatusCommandHandler : IRequestHandler<UpdateUserStatusCommand, BaseResponse<bool>>
{
    private readonly IWhatsAppNotificationService _whatsAppService;
    private readonly UserManager<User> _userManager;
    // ... other dependencies

    public async Task<BaseResponse<bool>> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                return NotFound<bool>("User not found");

            // Update user status
            user.IsActive = request.IsActive;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // Send appropriate WhatsApp notification
                if (request.IsActive)
                {
                    await _whatsAppService.SendAccountActivationMessageAsync(
                        user.Id,
                        user.PhoneNumber,
                        cancellationToken);
                }
                else
                {
                    await _whatsAppService.SendAccountDeactivationMessageAsync(
                        user.Id,
                        user.PhoneNumber,
                        cancellationToken);
                }

                return Success(true);
            }

            return BadRequest<bool>("Status update failed");
        }
        catch (Exception ex)
        {
            return BadRequest<bool>(ex.Message);
        }
    }
}
```

## 4. Integration with Existing Notification System

### Adding WhatsApp to Notification Observable

```csharp
public class FundNotificationJob : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IRepositoryManager>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var whatsAppService = scope.ServiceProvider.GetRequiredService<IWhatsAppNotificationService>();
                var logger = scope.ServiceProvider.GetRequiredService<ILoggerManager>();

                var observable = new FundNotificationObservable();
                
                // Add Firebase observer (existing)
                var firebaseObserver = new FundNotificationObserver(repository);
                observable.Subscribe(firebaseObserver);
                
                // Add WhatsApp observer (new)
                observable.AddWhatsAppNotifications(whatsAppService, userManager, logger);

                // Process notifications...
                var pendingNotifications = await repository.Notifications.GetPendingFundNotificationsAsync();
                
                foreach (var notification in pendingNotifications)
                {
                    // This will now send both Firebase and WhatsApp notifications
                    await observable.NotifyAsync(
                        notification.UserId,
                        fcmToken,
                        (NotificationType)notification.NotificationType,
                        parameters);
                }
            }
        }
    }
}
```

## 5. Direct WhatsApp Service Usage

### For Immediate Notifications

```csharp
public class UserController : AppControllerBase
{
    private readonly IWhatsAppNotificationService _whatsAppService;

    [HttpPost("resend-registration")]
    public async Task<IActionResult> ResendRegistrationMessage([FromBody] ResendRegistrationRequest request)
    {
        try
        {
            var response = await _whatsAppService.SendLocalizedMessageAsync(
                request.UserId,
                request.PhoneNumber,
                WhatsAppMessageType.RegistrationMessageResend,
                new object[] { request.Username, request.LoginUrl });

            if (response.IsSuccess)
            {
                return Ok(new { message = "Registration message sent successfully", messageId = response.MessageId });
            }

            return BadRequest(new { error = response.ErrorMessage });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
```

## 6. Batch WhatsApp Notifications

### For Multiple Users

```csharp
public class BatchNotificationService
{
    private readonly IWhatsAppNotificationService _whatsAppService;

    public async Task<List<WhatsAppMessageResponse>> SendBatchNotificationsAsync(
        List<BatchNotificationRequest> requests,
        CancellationToken cancellationToken = default)
    {
        var responses = new List<WhatsAppMessageResponse>();
        var semaphore = new SemaphoreSlim(5); // Limit concurrent requests

        var tasks = requests.Select(async request =>
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                return await _whatsAppService.SendLocalizedMessageAsync(
                    request.UserId,
                    request.PhoneNumber,
                    request.MessageType,
                    request.Parameters,
                    cancellationToken);
            }
            finally
            {
                semaphore.Release();
            }
        });

        responses.AddRange(await Task.WhenAll(tasks));
        return responses;
    }
}
```

## 7. Error Handling and Retry Logic

### Robust Error Handling

```csharp
public class RobustWhatsAppService
{
    private readonly IWhatsAppNotificationService _whatsAppService;
    private readonly ILogger<RobustWhatsAppService> _logger;

    public async Task<bool> SendWithRetryAsync(
        int userId,
        string phoneNumber,
        WhatsAppMessageType messageType,
        object[] parameters,
        int maxRetries = 3)
    {
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                var response = await _whatsAppService.SendLocalizedMessageAsync(
                    userId, phoneNumber, messageType, parameters);

                if (response.IsSuccess)
                {
                    _logger.LogInformation($"WhatsApp message sent successfully on attempt {attempt}");
                    return true;
                }

                _logger.LogWarning($"WhatsApp message failed on attempt {attempt}: {response.ErrorMessage}");
            }
            catch (WhatsAppRateLimitException ex)
            {
                _logger.LogWarning($"Rate limit hit on attempt {attempt}, waiting until {ex.RetryAfter}");
                await Task.Delay(ex.RetryAfter - DateTime.UtcNow);
            }
            catch (WhatsAppNotificationException ex)
            {
                _logger.LogError(ex, $"WhatsApp error on attempt {attempt}: {ex.Message}");
                
                if (attempt == maxRetries)
                    return false;
                
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt))); // Exponential backoff
            }
        }

        return false;
    }
}
```

## 8. Testing Integration

### Unit Test Example

```csharp
[Test]
public async Task SendPasswordResetMessage_ShouldCallWhatsAppService()
{
    // Arrange
    var mockWhatsAppService = new Mock<IWhatsAppNotificationService>();
    var handler = new ResetPasswordCommandHandler(mockWhatsAppService.Object, /* other deps */);
    
    mockWhatsAppService
        .Setup(x => x.SendPasswordResetMessageAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new WhatsAppMessageResponse { IsSuccess = true, MessageId = "test-id" });

    var command = new ResetPasswordCommand { Email = "test@example.com" };

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.IsTrue(result.IsSuccess);
    mockWhatsAppService.Verify(x => x.SendPasswordResetMessageAsync(
        It.IsAny<int>(), 
        It.IsAny<string>(), 
        It.IsAny<string>(), 
        It.IsAny<CancellationToken>()), Times.Once);
}
```

## Configuration Notes

Remember to configure the WhatsApp service in your `appsettings.json`:

```json
{
  "WhatsApp": {
    "ApiUrl": "https://graph.facebook.com",
    "ApiToken": "YOUR_API_TOKEN",
    "PhoneNumberId": "YOUR_PHONE_NUMBER_ID",
    "Version": "v17.0",
    "Enabled": true
  }
}
```

And register the service in your DI container (already done in `InfrastructureServicesRegisteration.cs`).
