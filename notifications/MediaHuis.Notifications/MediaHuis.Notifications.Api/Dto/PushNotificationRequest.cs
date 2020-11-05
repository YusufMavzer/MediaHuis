using System.Collections;
using System.Collections.Generic;

namespace MediaHuis.Notifications.Api.Dto
{
    /*
     * Wrapping the messages inside data property allows us to add future properties that aren't tied to single message object
     * This prevents future backwards compatibility problems
     */
    public class PushNotificationRequest
    {
        public IEnumerable<PushNotificationRequestMessage> Data { get; set; }
        
        public class PushNotificationRequestMessage
        {
            public string Title { get; set; }
            public string Body { get; set; }
        }
    }
}