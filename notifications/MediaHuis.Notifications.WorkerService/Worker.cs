using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediaHuis.Contracts.Notifications;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Google.Protobuf;
using MediaHuis.Notifications.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MediaHuis.Notifications.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly AirshipClient _airship;
        private readonly ConnectionFactory _rabbitMqConnectionFactory;

        public Worker(ILogger<Worker> logger, AirshipClient airship, Config config)
        {
            _airship = airship;
            _logger = logger;
            _rabbitMqConnectionFactory = new ConnectionFactory()
            {
                HostName = config.RabbitMqHostName,
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                
                var queue = "push-notification";
                using (var connection = _rabbitMqConnectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue, false,false,false,null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += OnReceived;
                    channel.BasicConsume(queue, true, consumer);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }

        private void OnReceived(object? sender, BasicDeliverEventArgs e)
        {
            using (var scope = IoC.ServiceProvider.CreateScope())
            {
                var _ctx = scope.ServiceProvider.GetService<NotificationContext>();
                var notification = e.Body.ToObject<NotificationMessage>();
                if (notification.Status != NotificationMessageStatus.Pending) return;
                var response = _airship.PushNotification(notification.Title, notification.Body).Result;
                notification.Status = response.IsSuccessStatusCode
                    ? NotificationMessageStatus.Processed
                    : NotificationMessageStatus.Failed;
            
                var n =_ctx.Notifications.FirstOrDefault(i => i.Id == Guid.Parse(notification.Id));
                if(n == null)
                    throw new Exception("Cannot find notification in db");
            
                n.Version += 1;
                n.Status = (NotificationStatus) (int) notification.Status;

                var newEvent = n.MapToEvent();
                _ctx.NotificationEvents.Add(newEvent);

                _ctx.SaveChanges();
            }
        }
    }
}