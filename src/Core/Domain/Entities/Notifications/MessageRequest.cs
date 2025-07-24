namespace Domain.Entities.Notifications
{
    public class MessageRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string DeviceToken { get; set; }
        public NotificationType Type { get; set; }
        public int UserId { get; set; }
        public string? Culture { get; set; } // Optional: if not provided, will use user's preferred language
        public object[]? Parameters { get; set; } // Parameters for message formatting
    }
}
