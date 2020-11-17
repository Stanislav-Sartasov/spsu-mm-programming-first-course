using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
			string a = "Мзуги";
			string b = "Илиас";
			int c = a.Length + b.Length;
			Console.Write(1 + c % 4); // = 3
			*/
			Console.WriteLine("This prog. creates a Dynamic Array with next functions:");
			Console.WriteLine("1 - Find index of a value in the Array from the start");
			Console.WriteLine("2 - Find index of a value in the Array from the end");
			Console.WriteLine("3 - Add value with an index in the Array");
			Console.WriteLine("4 - Add value at the end of the Array");
			Console.WriteLine("5 - Add value to the start of the Array");
			Console.WriteLine("6 - Reverse the Array");
			Console.WriteLine("7 - Remove value by the index from the Array");
			Console.WriteLine("8 - Clear teh Array");
			Console.WriteLine("9 - Exit the prog.");
			DynArray<string> A = new DynArray<string>(20);

			byte choice = Convert.ToByte(Console.ReadLine());
			while (choice != 9)
			{
				
				switch (choice)
				{
					
					case 1:
						{
							Console.WriteLine("Enter please Value");
							string value = Console.ReadLine();
							A.FindIndexFromStart(value);
							A.Print();
							break;
						}
					case 2:
						{
							string value = Console.ReadLine();
							A.FindIndexFromEnd(value);
							A.Print();
							break;
						}
					case 3:
						{
							Console.WriteLine("Enter please Value");
							string value = Console.ReadLine();
							Console.WriteLine("Enter please Index of a Value");
							int index = Convert.ToInt32(Console.ReadLine());
							A.Insert(value, index);
							A.Print();
							break;
						}
					case 4:
						{
							Console.WriteLine("Enter please Value");
							string value = Console.ReadLine();
							A.PushBack(value);
							A.Print();
							break;
						}
					case 5:
						{
							Console.WriteLine("Enter please Value");
							string value = Console.ReadLine();
							A.PushFront(value);
							A.Print();
							break;
						}
					case 6:
						{
							A.Reverse();
							A.Print();
							break;
						}
					case 7:
						{
							Console.WriteLine("Enter please the Idex of the Value");
							int index = Convert.ToInt32(Console.ReadLine());
							A.Remove(index);
							A.Print();
							break;
						}
					case 8:
						{
							A.Clear();
							A.Print();
							break;
						}
					default:
						{
							Console.WriteLine("Error, wrong dicision");
							break;
						}
				}
				Console.WriteLine("Choose your action");
				choice = Convert.ToByte(Console.ReadLine());
			}
		}
	}
}
