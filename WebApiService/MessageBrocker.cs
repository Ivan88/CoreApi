using Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBus
{
	public class MessageBrocker
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
					action(new MessageWrapper
					{
						Body = ea.Body,
						Headers = ea.BasicProperties.Headers
					});

					channel.BasicAck(ea.DeliveryTag, false);
				};

				channel.BasicConsume(queueName, false, consumer);
			}
		}

		public void SubscribeForReplying(string queueName, Func<MessageWrapper, byte[]> func)
		{
			using (var connection = _connectionFactory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queueName, false, false, false, null);
				channel.BasicQos(0, 1, false);
				var consumer = new EventingBasicConsumer(channel);
				channel.BasicConsume(queueName, false, consumer);

				consumer.Received += (model, ea) =>
				{
					var props = ea.BasicProperties;
					var replyProps = channel.CreateBasicProperties();
					replyProps.CorrelationId = props.CorrelationId;

					channel.BasicPublish("", props.ReplyTo, replyProps, func(new MessageWrapper { Body = ea.Body, Headers = ea.BasicProperties.Headers }));
					channel.BasicAck(ea.DeliveryTag, false);
				};
			}
		}

		public byte[] PublishAndWait(byte[] message, IEnumerable<Tuple<string, object>> headers, string queueName)
		{
			byte[] result = new byte[0];
			using (var connection = _connectionFactory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				var replyQueueName = channel.QueueDeclare().QueueName;
				var consumer = new EventingBasicConsumer(channel);

				var props = channel.CreateBasicProperties();
				var correlationId = Guid.NewGuid().ToString();
				props.CorrelationId = correlationId;
				props.ReplyTo = replyQueueName;

				consumer.Received += (model, ea) =>
				{
					var body = ea.Body;
					var response = Encoding.UTF8.GetString(body);
					if (ea.BasicProperties.CorrelationId == correlationId)
					{
						result = body;
					}
				};

				channel.BasicPublish("", queueName, props, message);
				channel.BasicConsume(consumer, replyQueueName, true);
			}

			return result;
		}
	}
}
