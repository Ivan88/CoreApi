using Interfaces;
using Models;
using System.Collections.Generic;
using System.Text;

namespace AppWorker
{
	public class Program
	{
		private static readonly IMessageBrocker _messageBrocker;

		private static readonly List<string> docs = new List<string>();

		static void Main(string[] args)
		{
			_messageBrocker.Subscribe("documentsQueue", HandleReceiving);
		}

		private static void HandleReceiving(MessageWrapper messageWrapper)
		{
			docs.Add(Encoding.UTF8.GetString(messageWrapper.Body));
			//send to elastic search
		}
	}
}
