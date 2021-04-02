using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTable
{
	public class Cell<TKey, TValue>
	{
		public bool Remote { get; set; } 
		public TValue Value { get; set; }
		public TKey Key { get; set; }
		public Cell (TKey key, TValue value)
		{
			Value = value;
			Key = key;
		}
	}
}
	