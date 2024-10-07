using BeymenCase.Core.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Tasks;

namespace BeymenCase.Service_A
{
    public class RabbitMqConsumer: IMessageConsumer
    {
        private readonly string _hostName;
        private readonly string _queueName;

        public RabbitMqConsumer(string hostName, string queueName)
        {
            _hostName = hostName;
            _queueName = queueName;
        }

        public async Task<string> ReceiveMessageAsync()
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

                var consumer = new EventingBasicConsumer(channel);
                string receivedMessage = string.Empty;

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    receivedMessage = Encoding.UTF8.GetString(body);
                };

                channel.BasicConsume(queue: _queueName,
                                     autoAck: true,
                                     consumer: consumer);

                await Task.Delay(15000); 
                return receivedMessage;
            }
        }
    }
}
