using PinMessaging.Other;
using System;

namespace PinMessaging.Model
{
    public class PMNotificationModel
    {
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public NotificationCenter.NotificationType Type { get; set; }
        public long ContentId { get; set; }
        public string Content { get; set; }
        public DateTime DateCreation { get; set; }
    }
}
