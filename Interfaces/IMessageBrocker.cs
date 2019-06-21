﻿using Models;
using System;
using System.Collections.Generic;

namespace Interfaces
{
	public interface IMessageBrocker
	{
		void Publish(byte[] message, IEnumerable<Tuple<string, object>> headers, string queueName);
		void Subscribe(string queueName, Action<MessageWrapper> action);
	}
}