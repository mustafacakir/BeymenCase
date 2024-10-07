using BeymenCase.Core.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace BeymenCase.Infrastructure.Configuration
{
    public class RabbitMqService: IQueueService
    {
        private readonly string _hostName;
        private readonly string _queueName;

        public RabbitMqService(string hostName, string queueName)
        {
            _hostName = hostName;
            _queueName = queueName;
        }

        public void SendMessage(string message)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: _queueName,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
