using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace Task5
{
	class Program
	{
		static int Action()
		{
			Thread.Sleep(5000);
			return 42;
		}
		static void Main(string[] args)
		{
			int[] a = { 1, 2, 3, 42, -42, 5, 6, 7, 15, 84, -243, -169 };
			IVectorLengthComputer[] vectorLengthComputers = { new RegAdd(), new Cascade() };
			foreach (var computer in vectorLengthComputers)
				Console.WriteLine($"{computer.GetType()}: {computer.ComputeLength(a)}");
        }
	}
}
