using System.Collections.Generic;

namespace Models
{
	public class MessageWrapper
	{
		public byte[] Body { get; set; }
		public IDictionary<string, object> Headers { get; set; }
	}
}
