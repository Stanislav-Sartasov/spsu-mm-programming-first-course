using System;
using CascadeLib;

namespace FifthTask
{
	class Program
	{
		static void Main(string[] args)
		{
			int capacity = 200000;
			int maxValue = 100;

			var arr = ArrayGenerator.Generate(capacity, maxValue);

			var sequential = new Sequential();
			Console.WriteLine(sequential.ComputeLength(arr));

			var cascadeSimple = new CascadeSimple();
			Console.WriteLine(cascadeSimple.ComputeLength(arr));

			var cascadeMod = new CascadeMod();
			Console.WriteLine(cascadeMod.ComputeLength(arr));
		}
	}
}