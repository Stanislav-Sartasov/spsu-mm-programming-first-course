using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer
{
	public class Data<T>
	{
		public Data(T t)
		{
			DataElem = t;
		}
		public T DataElem
		{
			get;
			set;
		}
	}
}
