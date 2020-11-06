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
    public class Worker : BackgroundService, IDisposable
    {
        private bool _disposed = false;
        private readonly ILogger<Worker> _logger;
        private readonly AirshipClient _airship;
        private readonly ConnectionFactory _rabbitMqConnectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly EventingBasicConsumer _consumer;
        public Worker(ILogger<Worker> logger, AirshipClient airship, Config config)
        {
            _airship = airship;
            _logger = logger;
            _rabbitMqConnectionFactory = new ConnectionFactory()
            {
                HostName = config.RabbitMqHostName,
            };
            var queue = "push-notification";
            _connection = _rabbitMqConnectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue, false, false, false, null);
            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += OnReceived;
            _channel.BasicConsume(queue: queue,
                autoAck: false, //this is on purpose
                consumer: _consumer);
        }

        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Needs a better HostService Implementation
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            Console.Read();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _channel?.Dispose();
                    _connection?.Dispose();
                }
                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
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