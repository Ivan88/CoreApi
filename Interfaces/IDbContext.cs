using System.Collections.Generic;
using System.IO;

namespace Interfaces
{
	public interface IDbContext
	{
		void StoreDocument(Stream stream);
		//IEnumerable<TextSearchResult> FindText(string text);
	}
}
