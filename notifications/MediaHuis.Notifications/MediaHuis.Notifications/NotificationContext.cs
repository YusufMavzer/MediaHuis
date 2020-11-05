using Microsoft.EntityFrameworkCore;
using MediaHuis.Notifications.Models;

namespace MediaHuis.Notifications
{
    public class NotificationContext: DbContext
    {
        public NotificationContext(DbContextOptions<NotificationContext> options): base(options) { }
        
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationEvent> NotificationEvents { get; set; }
    }
}