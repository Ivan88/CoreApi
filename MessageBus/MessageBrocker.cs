using Interfaces;
using Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;

namespace MessageBus
{
	public class MessageBrocker : IMessageBrocker
	{

		private readonly ConnectionFactory _connectionFactory;

		public MessageBrocker(string hostName)
		{
			_connectionFactory = new ConnectionFactory { HostName = hostName };
		}

		public void Publish(byte[] message, IEnumerable<Tuple<string, object>> headers, string queueName)
		{
			using (var connection = _connectionFactory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queueName, false, false, false);

				var properties = channel.CreateBasicProperties();

				foreach (var header in headers)
					properties.Headers.Add(header.Item1, header.Item2);

				properties.Persistent = true;

				channel.BasicPublish("", queueName, properties, message);
			}
		}

		public void Subscribe(string queueName, Action<MessageWrapper> action)
		{
			using (var connection = _connectionFactory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queueName, false, false, false, null);

				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, ea) =>
				{
					//body = ea.Body;
					//var fileName = ea.BasicProperties.Headers["fileName"] ?? Guid.NewGuid().ToString();
					action(new MessageWrapper
					{
						Body = ea.Body,
						Headers = ea.BasicProperties.Headers
					});

					channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
				};

				channel.BasicConsume(queueName, autoAck: true, consumer: consumer);
			}
		}
	}
}
