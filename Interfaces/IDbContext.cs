using System.Collections.Generic;
using System.IO;
using Models;

namespace Interfaces
{
	public interface IDbContext
	{
		void StoreDocument(Stream stream);
		IEnumerable<TextSearchResult> FindText(string text);
	}
}
