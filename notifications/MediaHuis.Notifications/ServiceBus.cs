using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using RabbitMQ.Client;
using Google.Protobuf;

namespace MediaHuis.Notifications
{
    public class ServiceBus
    {
        private readonly ConnectionFactory _rabbitMqConnectionFactory;
        
        
        public ServiceBus(Config config)
        {
            _rabbitMqConnectionFactory = new ConnectionFactory()
            {
                HostName = config.RabbitMqHostName,
            };
        }


        public void Publish<T>(string queue, IEnumerable<T> messages) where T : IMessage, new ()
        {
            if (messages == null || !messages.Any()) return;
            using var connection = _rabbitMqConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue, false,false,false,null);
            foreach (var message in messages)
            {
                channel.BasicPublish("", queue, null, message.ToByteArray());
            }
            
        }
        
        public void Publish<T>(string queue, T message) where T : IMessage, new ()
        {
            this.Publish<T>(queue, new List<T>() {message});
        }
    }
}