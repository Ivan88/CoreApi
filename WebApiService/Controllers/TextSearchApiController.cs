using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using RabbitMQ.Client;
using WebApiProject.Models;

namespace WebApiProject.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class TextSearchController : ControllerBase
    {
		private readonly ConnectionFactory _connectionFactory;

		private const string _queueName = "documentQueue";

		public TextSearchController()
		{
			_connectionFactory = new ConnectionFactory { HostName = "localhost" };
		}

		[HttpPost("/document")]
		public async Task<ActionResult> UploadDocument(IFormFile file)
		{
			if (file.Length > 0)
			{
				using (MemoryStream stream = new MemoryStream())
				{
					await file.CopyToAsync(stream, new System.Threading.CancellationToken());

					using (var connection = _connectionFactory.CreateConnection())
					using (var channel = connection.CreateModel())
					{
						channel.QueueDeclare(_queueName, false, false, false);

						var properties = channel.CreateBasicProperties();
						properties.Headers.Add("fileName", file.Name);
						properties.Persistent = true;

						channel.BasicPublish("", _queueName, properties, stream.GetBuffer());
					}
					return Ok();
				}
			}

			return BadRequest();
		}

		[HttpGet("/documents")]
		public ActionResult<IEnumerable<TextSearchResult>> SearchText(string text)
		{
			if (String.IsNullOrEmpty(text))
				return BadRequest("Incorrect input parameter.");

			return Ok();
		}

		[HttpGet("/ping")]
        public ActionResult<string> Ping()
        {
            return Ok("pong");
        }
    }
}