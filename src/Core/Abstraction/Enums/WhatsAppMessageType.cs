using System.ComponentModel;

namespace Abstraction.Enums
{
    /// <summary>
    /// Enumeration representing different types of WhatsApp messages
    /// Based on Sprint 3 requirements for WhatsApp notification scenarios
    /// </summary>
    public enum WhatsAppMessageType
    {
        /// <summary>
        /// Password reset message with temporary password
        /// Used in "Reset User Password" story (MSG-RESET-006)
        /// </summary>
        [Description("Password Reset")]
        PasswordReset = 1,

        /// <summary>
        /// User registration confirmation message
        /// Used in "Add New System User" story (MSG-ADD-008)
        /// </summary>
        [Description("User Registration")]
        UserRegistration = 2,

        /// <summary>
        /// Account activation notification
        /// Used in "Activate/Deactivate System User" story (MSG-ACTDEACT-009)
        /// </summary>
        [Description("Account Activation")]
        AccountActivation = 3,

        /// <summary>
        /// Account deactivation notification
        /// Used in "Activate/Deactivate System User" story (MSG-ACTDEACT-010)
        /// </summary>
        [Description("Account Deactivation")]
        AccountDeactivation = 4,

        /// <summary>
        /// Resend registration message
        /// Used in "Resend Account Registration Message" story (MSG-ADD-008)
        /// </summary>
        [Description("Registration Message Resend")]
        RegistrationMessageResend = 5,

        /// <summary>
        /// Fund member addition notification
        /// Used when a new member is added to a fund
        /// </summary>
        [Description("Fund Member Added")]
        FundMemberAdded = 6
    }

    /// <summary>
    /// Enumeration for message priority levels
    /// </summary>
    public enum MessagePriority
    {
        Low = 1,
        Normal = 2,
        High = 3,
        Critical = 4
    }

    /// <summary>
    /// Enumeration for WhatsApp message delivery status
    /// </summary>
    public enum WhatsAppDeliveryStatus
    {
        /// <summary>
        /// Message is queued for sending
        /// </summary>
        Queued = 1,

        /// <summary>
        /// Message has been sent to WhatsApp servers
        /// </summary>
        Sent = 2,

        /// <summary>
        /// Message has been delivered to recipient's device
        /// </summary>
        Delivered = 3,

        /// <summary>
        /// Message has been read by recipient
        /// </summary>
        Read = 4,

        /// <summary>
        /// Message sending failed
        /// </summary>
        Failed = 5,

        /// <summary>
        /// Message was rejected by WhatsApp
        /// </summary>
        Rejected = 6
    }
}
