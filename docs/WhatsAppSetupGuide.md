# WhatsApp Configuration Setup Guide

## 🚨 **Quick Fix for Authentication Error**

If you're encountering the `WhatsAppAuthenticationException: WhatsApp API token is not configured` error, follow these steps:

### **Step 1: Add WhatsApp Configuration**

The WhatsApp configuration section is now added to all environment files. You need to replace the placeholder values with actual WhatsApp Business API credentials.

### **Step 2: Get WhatsApp Business API Credentials**

1. **Create WhatsApp Business Account**: Go to [Facebook Business](https://business.facebook.com/)
2. **Set up WhatsApp Business API**: Follow [WhatsApp Business API Setup](https://developers.facebook.com/docs/whatsapp/getting-started)
3. **Get Required Values**:
   - **API Token**: Your WhatsApp Business API access token
   - **Phone Number ID**: The ID of your registered WhatsApp Business phone number
   - **API URL**: Usually `https://graph.facebook.com`

### **Step 3: Update Configuration Files**

#### **For Development (appsettings.Development.json)**
```json
{
  "WhatsApp": {
    "ApiUrl": "https://graph.facebook.com",
    "ApiToken": "YOUR_ACTUAL_DEV_TOKEN_HERE",
    "PhoneNumberId": "YOUR_ACTUAL_DEV_PHONE_ID_HERE",
    "Version": "v17.0",
    "TimeoutSeconds": 30,
    "MaxRetryAttempts": 3,
    "RetryDelayMs": 1000,
    "Enabled": true
  }
}
```

#### **For Production (appsettings.Production.json)**
```json
{
  "WhatsApp": {
    "ApiUrl": "https://graph.facebook.com",
    "ApiToken": "YOUR_ACTUAL_PRODUCTION_TOKEN_HERE",
    "PhoneNumberId": "YOUR_ACTUAL_PRODUCTION_PHONE_ID_HERE",
    "Version": "v17.0",
    "TimeoutSeconds": 60,
    "MaxRetryAttempts": 5,
    "RetryDelayMs": 2000,
    "Enabled": true
  }
}
```

### **Step 4: Temporary Workaround (Disable WhatsApp)**

If you don't have WhatsApp credentials yet, you can temporarily disable the service:

```json
{
  "WhatsApp": {
    "ApiUrl": "https://graph.facebook.com",
    "ApiToken": "PLACEHOLDER_TOKEN",
    "PhoneNumberId": "PLACEHOLDER_PHONE_ID",
    "Version": "v17.0",
    "Enabled": false
  }
}
```

When `Enabled: false`, the system will:
- ✅ Skip WhatsApp notifications gracefully
- ✅ Continue with fund member addition normally
- ✅ Log that WhatsApp service is disabled
- ✅ Not throw authentication errors

## 🔧 **Configuration Properties**

| Property | Description | Required | Default | Example |
|----------|-------------|----------|---------|---------|
| `ApiUrl` | WhatsApp Business API base URL | Yes | - | `https://graph.facebook.com` |
| `ApiToken` | WhatsApp Business API access token | Yes | - | `EAABsBCS...` |
| `PhoneNumberId` | WhatsApp Business phone number ID | Yes | - | `123456789012345` |
| `Version` | WhatsApp API version | No | `v17.0` | `v17.0` |
| `TimeoutSeconds` | Request timeout in seconds | No | `30` | `30` |
| `MaxRetryAttempts` | Maximum retry attempts | No | `3` | `3` |
| `RetryDelayMs` | Delay between retries (ms) | No | `1000` | `1000` |
| `Enabled` | Enable/disable WhatsApp service | No | `true` | `true` |

## 🧪 **Testing the Configuration**

### **Test 1: Check Configuration Loading**
1. Start the application
2. Check logs for: `"WhatsApp service configured successfully"`
3. If disabled: `"WhatsApp service is disabled in configuration"`

### **Test 2: Test Fund Member Addition**
1. Add a new board member to a fund
2. Check logs for WhatsApp notification attempts
3. Verify no authentication errors occur

### **Test 3: Test with Invalid Configuration**
1. Set `Enabled: true` with invalid credentials
2. Should see clear error message with missing configuration details
3. Should not break the fund member addition process

## 🔒 **Security Best Practices**

1. **Never commit real API tokens** to source control
2. **Use environment variables** for production:
   ```json
   {
     "WhatsApp": {
       "ApiToken": "${WHATSAPP_API_TOKEN}",
       "PhoneNumberId": "${WHATSAPP_PHONE_NUMBER_ID}"
     }
   }
   ```
3. **Rotate tokens regularly**
4. **Use different tokens** for different environments
5. **Monitor API usage** and set up alerts

## 🚀 **Environment-Specific Settings**

### **Development**
- `Enabled: false` (recommended until you have test credentials)
- Lower timeout and retry values
- Test credentials only

### **Staging**
- `Enabled: true`
- Test credentials
- Moderate timeout and retry values

### **Production**
- `Enabled: true`
- Production credentials
- Higher timeout and retry values for reliability

## 📞 **Support**

If you continue to experience issues:

1. **Check logs** for detailed error messages
2. **Verify credentials** with WhatsApp Business API
3. **Test with `Enabled: false`** to isolate the issue
4. **Review** the `WhatsAppConfiguration.md` file for detailed setup instructions

The system is designed to gracefully handle WhatsApp service unavailability without breaking core functionality.
