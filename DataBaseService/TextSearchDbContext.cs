using System;
using System.Collections.Generic;
using System.IO;
using Interfaces;

namespace DAL
{
	public class TextSearchDbContext : IDbContext
	{
		private readonly string _connectionString;
		public TextSearchDbContext(string connectionString)
		{
			this._connectionString = connectionString;
		}

		public void StoreDocument(Stream stream)
		{
			throw new NotImplementedException();
		}

		//public IEnumerable<TextSearchResult> FindText(string text)
		//{
		//	throw new NotImplementedException();
		//}
	}
}
