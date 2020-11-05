using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.WellKnownTypes;
using MediaHuis.Notifications.Api.Dto;
using MediaHuis.Contracts.Notifications;
using MediaHuis.Notifications.Models;

namespace MediaHuis.Notifications.Api.Extensions
{
    public static class MapToExtension
    {
        public static IEnumerable<Notification> ToNotifications(this PushNotificationRequest pushNotificationRequest)
        {
            return pushNotificationRequest.Data
                .Where(i =>  
                  !string.IsNullOrWhiteSpace(i.Title) && 
                  !string.IsNullOrWhiteSpace(i.Body))
                .Select(n => new Notification()
                {
                    Version = 1,
                    Title = n.Title,
                    Body = n.Body,
                    Created = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow,
                    Status = NotificationStatus.Pending,
                });
        }
    }
}