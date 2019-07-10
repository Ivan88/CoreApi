using MessageBus;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WebApiProject.Models;

namespace WebApiProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TextSearchController : ControllerBase
	{
		private readonly MessageBrocker _messageBrocker;

		public TextSearchController(MessageBrocker messageBrocker)
		{
			_messageBrocker = messageBrocker;
		}

		[HttpPost("/document")]
		public async Task<ActionResult> UploadDocument(IFormFile file)
		{
			if (file.Length > 0)
			{
				using (MemoryStream stream = new MemoryStream())
				{
					await file.CopyToAsync(stream, new System.Threading.CancellationToken());

					_messageBrocker.Publish(stream.GetBuffer(), new[] { new Tuple<string, object>("filename", file.FileName) }, "documentsQueue");
					
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

			byte[] result = _messageBrocker.PublishAndWait(null, null, "getDocumentsQueue");

			return Ok(Encoding.UTF8.GetString(result));
		}

		[HttpGet("/ping")]
		public ActionResult<string> Ping()
		{
			return Ok("pong");
		}
	}
}