# WhatsApp Notification Service Configuration

This document describes how to configure the WhatsApp notification service for the Jadwa system.

## Configuration Settings

Add the following configuration to your `appsettings.json` file:

```json
{
  "WhatsApp": {
    "ApiUrl": "https://graph.facebook.com",
    "ApiToken": "YOUR_WHATSAPP_BUSINESS_API_TOKEN",
    "PhoneNumberId": "YOUR_PHONE_NUMBER_ID",
    "Version": "v17.0",
    "TimeoutSeconds": 30,
    "MaxRetryAttempts": 3,
    "RetryDelayMs": 1000,
    "Enabled": true
  }
}
```

## Configuration Properties

| Property | Description | Required | Default |
|----------|-------------|----------|---------|
| `ApiUrl` | WhatsApp Business API base URL | Yes | - |
| `ApiToken` | WhatsApp Business API access token | Yes | - |
| `PhoneNumberId` | WhatsApp Business phone number ID | Yes | - |
| `Version` | WhatsApp API version | No | "v17.0" |
| `TimeoutSeconds` | Request timeout in seconds | No | 30 |
| `MaxRetryAttempts` | Maximum retry attempts | No | 3 |
| `RetryDelayMs` | Delay between retries in milliseconds | No | 1000 |
| `Enabled` | Enable/disable WhatsApp service | No | true |

## Setting Up WhatsApp Business API

### 1. Create a WhatsApp Business Account

1. Go to [Facebook Business Manager](https://business.facebook.com/)
2. Create a business account if you don't have one
3. Add WhatsApp Business API to your business account

### 2. Get API Credentials

1. **API Token**: Generate a permanent access token from the WhatsApp Business API dashboard
2. **Phone Number ID**: Get the phone number ID from your WhatsApp Business phone number
3. **API URL**: Use `https://graph.facebook.com` for production

### 3. Configure Phone Number

1. Verify your Saudi phone number (+966XXXXXXXXX format)
2. Complete the business verification process
3. Set up message templates if required

## Environment-Specific Configuration

### Development
```json
{
  "WhatsApp": {
    "Enabled": false
  }
}
```

### Testing
```json
{
  "WhatsApp": {
    "ApiUrl": "https://graph.facebook.com",
    "ApiToken": "TEST_TOKEN",
    "PhoneNumberId": "TEST_PHONE_ID",
    "Enabled": true
  }
}
```

### Production
```json
{
  "WhatsApp": {
    "ApiUrl": "https://graph.facebook.com",
    "ApiToken": "PRODUCTION_TOKEN",
    "PhoneNumberId": "PRODUCTION_PHONE_ID",
    "TimeoutSeconds": 60,
    "MaxRetryAttempts": 5,
    "Enabled": true
  }
}
```

## Security Considerations

1. **Never commit API tokens** to source control
2. Use **Azure Key Vault** or similar for production secrets
3. **Rotate tokens** regularly
4. **Monitor API usage** and set up alerts for unusual activity
5. **Validate phone numbers** before sending messages

## Usage Examples

### Basic Message Sending
```csharp
var response = await _whatsAppService.SendPasswordResetMessageAsync(
    userId: 123,
    phoneNumber: "+966501234567",
    temporaryPassword: "TempPass123"
);
```

### Localized Messages
```csharp
var response = await _whatsAppService.SendLocalizedMessageAsync(
    userId: 123,
    phoneNumber: "+966501234567",
    messageType: WhatsAppMessageType.UserRegistration,
    parameters: new object[] { "username", "https://jadwa.com/login" }
);
```

## Troubleshooting

### Common Issues

1. **Invalid Phone Number**: Ensure Saudi format (+966XXXXXXXXX)
2. **Authentication Failed**: Check API token validity
3. **Rate Limiting**: Implement proper retry logic
4. **Message Delivery Failed**: Check phone number status

### Error Codes

| Error Code | Description | Solution |
|------------|-------------|----------|
| `INVALID_PHONE` | Invalid phone number format | Use +966XXXXXXXXX format |
| `AUTH_FAILED` | Authentication failed | Check API token |
| `RATE_LIMIT_EXCEEDED` | Too many requests | Implement backoff strategy |
| `DELIVERY_FAILED` | Message delivery failed | Check recipient status |

## Monitoring and Logging

The service logs all operations with the following information:
- Message sending attempts
- Delivery status updates
- Error conditions
- API response details

Monitor these logs for:
- High failure rates
- Authentication issues
- Rate limiting
- Unusual usage patterns
