using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer
{
	class Program
	{
		static void Main(string[] args)
		{
			Interaction inter = new Interaction();
			Console.Write("type int number - amount of Consumers: ");
			int amountCons = inter.GetInt();
			Console.Write("Type int number - amountCons of producers: ");
			int amountProd = inter.GetInt();

			List<Data<string>> lst = new List<Data<String>>();
			lst.Clear();

			Consumer[] consumers = new Consumer[amountCons];
			Producer[] producers = new Producer[amountProd];
			for (int i = 0; i < consumers.Length; ++i)
			{
				consumers[i] = new Consumer(lst, $"consumer {i}");
			}
			for (int i = 0; i < producers.Length; ++i)
			{
				producers[i] = new Producer(lst, $"produser {i}");
			}

			Console.ReadKey();

			foreach (var prods in producers)
			{
				//prods.Stop();
				prods.Join();
			}
			foreach (var cons in consumers)
			{
				cons.Join();
			}
			Console.WriteLine("rest tasks: ");
			foreach (var l in lst)
			{
				Console.WriteLine($"{l.DataElem}");
			}
			Console.WriteLine("/////////////////");
			Console.ReadKey();
		}
	}
}
