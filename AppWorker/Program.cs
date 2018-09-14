using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AppWorker
{
	class Program
	{
		private static ConnectionFactory _connectionFactory;
		private const string _queueName = "documentQueue";

		static void Main(string[] args)
		{
			_connectionFactory = new ConnectionFactory { HostName = "localhost" };

			using (var connection = _connectionFactory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(_queueName, false, false, false, null);

				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, ea) =>
				{
					var body = ea.Body;
					var fileName = ea.BasicProperties.Headers["fileName"] ?? Guid.NewGuid().ToString();

					//save file

					channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
				};

				channel.BasicConsume(_queueName, autoAck: true, consumer : consumer);
			}
		}
	}
}
