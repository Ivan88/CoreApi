using System;
using System.Collections.Generic;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApiProject.Models;

namespace WebApiProject.Controllers
{
	[Route("api/textsearch")]
    [ApiController]
    public class TextSearchApiController : ControllerBase
    {
		private readonly IDbContext dbContext;

		public TextSearchApiController(IDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		[HttpPost("/document")]
		public ActionResult UploadDocument()
		{
			return Ok();
		}

		[HttpGet("/documents")]
		public ActionResult<IEnumerable<TextSearchResult>> SearchText(string text)
		{
			if (String.IsNullOrEmpty(text))
				return BadRequest("Incorrect input parameter.");

			return Ok();
		}
    }
}