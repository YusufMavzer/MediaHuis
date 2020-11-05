using System;

namespace MediaHuis.Notifications.Models
{
    public class NotificationEvent
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public byte[] Event { get; set; }
    }
}