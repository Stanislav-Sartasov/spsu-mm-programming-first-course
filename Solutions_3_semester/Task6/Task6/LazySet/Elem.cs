using System;
using System.Collections.Generic;
using System.Text;

namespace Task6.LazySet
{
	class Elem<T>
	{
		volatile Locker locker = new Locker();
		volatile int key = int.MinValue;
		volatile Elem<T> next = null;
		volatile bool marked = false;
		public Locker Locker {
			get
			{
				return locker;
			}
		}
		public int Key
		{
			get
			{
				return key;
			}
			set
			{
				key = value;
			}
		}
		public T Value { get; set; } = default;
		public Elem<T> Next
		{
			get
			{
				return next;
			}
			set
			{
				next = value;
			}
		}
		public bool Marked
		{
			get
			{
				return marked;
			}
			set
			{
				marked = value;
			}
		}
	}
}
