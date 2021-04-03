using System;
using System.Collections.Generic;
using System.Text;

namespace Task3
{
	public interface IActor
	{
		public string Id { get; }
		public List<Product> LocalStorage { get; }
	}
}
