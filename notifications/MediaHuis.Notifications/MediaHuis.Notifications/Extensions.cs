using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using MediaHuis.Contracts.Notifications;
using MediaHuis.Notifications.Models;

namespace MediaHuis.Notifications
{
    public static class Extensions
    {
        /// <summary>
        /// Deserialize byte array to IMessage
        /// </summary>
        /// <param name="buf"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToObject<T>(this byte[] buf) where T : IMessage<T>, new()
        {
            if (buf == null)
                return default(T);

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(buf, 0, buf.Length);
                ms.Seek(0, SeekOrigin.Begin);

                MessageParser<T> parser = new MessageParser<T>(() => new T());
                return parser.ParseFrom(ms);
            }
        }
        public static T ToObject<T>(this ReadOnlyMemory<byte> buf) where T : IMessage<T>, new()
        {
            return buf.ToArray().ToObject<T>();
        }

        
        public static NotificationMessage MapToMessage(this Notification notification)
        {
            return new NotificationMessage()
            {
                Id = notification.Id.ToString(),
                Version = notification.Version,
                Created = notification.Created.ToUniversalTime().ToTimestamp(),
                LastUpdated = notification.LastUpdated.ToUniversalTime().ToTimestamp(),
                Title = notification.Title,
                Body = notification.Body,
                Status = (NotificationMessageStatus)(int)notification.Status
            };
        }
        
        public static NotificationEvent MapToEvent(this Notification notification)
        {
            var e = notification.MapToMessage().ToByteArray();
            var stamp = DateTime.UtcNow;
            return new NotificationEvent()
            {
                Event = e,
                Timestamp = stamp
            };
        }

        public static IEnumerable<NotificationMessage> MapToMessages(this IEnumerable<Notification> notification)
        {
            foreach (var i in notification.ToList()) 
                yield return i.MapToMessage();
        }

        public static IEnumerable<NotificationEvent> MapToEvents(this IEnumerable<Notification> notifications)
        {
            foreach (var i in notifications.ToList()) 
                yield return i.MapToEvent();
        }
        
    }
}