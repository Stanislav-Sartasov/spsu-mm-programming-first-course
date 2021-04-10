using System;
using System.Collections.Generic;
using System.Text;

namespace Task6.LazySet
{
	class Elem<T>
	{
		public Locker Locker { get; } = new Locker();
		public int Key { get; set; } = int.MinValue;
		public T Value { get; set; } = default;
		public Elem<T> Next { get; set; } = null;
		public bool Marked { get; set; } = false;
	}
}
