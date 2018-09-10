using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
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
		public async Task<ActionResult> UploadDocument(IFormFile file)
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