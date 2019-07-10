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
			_messageBrocker.SubscribeForReplying("documentsQueue", HandleSending);
		}

		private static void HandleReceiving(MessageWrapper messageWrapper)
		{
			docs.Add(Encoding.UTF8.GetString(messageWrapper.Body));
			//send to elastic search
		}

		private static byte[] HandleSending(MessageWrapper messageWrapper)
		{
			//get from elastic search
			return Encoding.UTF8.GetBytes("asdfasdf");
		}
	}
}
