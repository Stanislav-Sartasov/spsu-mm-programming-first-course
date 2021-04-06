using NUnit.Framework;
using System;

namespace Task5Test
{
	public class Tests
	{
		Random random = new Random(42);
		int attemptsNumber = 100;
		int maxArrSize = 100000;
		int maxValue = 10000;
		Task5.IVectorLengthComputer[] vectorLengthComputers = { new Task5.Cascade(), new Task5.RegAdd() };
		int BruteForce(int[] a)
		{
			int sum = 0;
			foreach (int i in a)
				sum += i * i;
			return (int)Math.Sqrt(sum);
		}

		[Test]
		public void MainTest()
		{
			for (int i = 0; i < attemptsNumber; i++)
				foreach (var computer in vectorLengthComputers)
				{
					int[] a = new int[random.Next(0, maxArrSize)];
					for (int j = 0; j < a.Length; j++)
						a[j] = random.Next(-maxValue, maxValue);
					Assert.AreEqual(BruteForce(a), computer.ComputeLength(a));
				}
		}
	}
}