using System;

namespace MediaHuis.Notifications.Models
{
    public class Notification
    {
       
            public Guid Id { get; set; }
            public int Version { get; set; }
            public DateTime Created { get; set; }
            public DateTime LastUpdated { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
            public NotificationStatus Status { get; set; }
        
    }

    public enum NotificationStatus
    {
        Pending,
        Failed,
        Processed
    }
}