using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApiProject.Models
{
	public class TextSearchDbContext : DbContext
	{
		public TextSearchDbContext(DbContextOptions<TextSearchDbContext> options)
			:base(options)
		{

		}

		public DbSet<TextSearchModel> TextSearchItems { get; set; }
	}
}
