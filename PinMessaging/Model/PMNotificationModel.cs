using PinMessaging.Other;

namespace PinMessaging.Model
{
    public class PMNotificationModel
    {
        public NotificationCenter.NotificationType Type { get; set; }
        public long ContentId { get; set; }
        public string Content { get; set; }
    }
}
