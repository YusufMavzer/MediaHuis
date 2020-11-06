using System.Linq;
using System.Threading.Tasks;
using MediaHuis.Contracts.Notifications;
using MediaHuis.Notifications.Api.Dto;
using MediaHuis.Notifications.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace MediaHuis.Notifications.Api.Controllers
{
    [Route("/api/notifications")]
    public class NotificationController: Controller
    {
    
        [HttpGet]
        public ActionResult Test()
        {
            return Ok("Hello World");
        }
        
        [HttpPost]
        public async Task<ActionResult> PushNotification([FromBody] PushNotificationRequest payload, [FromServices] NotificationContext ctx, [FromServices] ServiceBus bus)
        {
            var notifications =  payload.ToNotifications().ToList();
            await ctx.Notifications.AddRangeAsync(notifications);
            await ctx.NotificationEvents.AddRangeAsync(notifications.MapToEvents());
            await ctx.SaveChangesAsync();
            bus.Publish("push-notification", notifications.MapToMessages());
            return NoContent();
        }
    }
}
