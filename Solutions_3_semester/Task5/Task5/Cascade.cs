using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Task5
{
	public class Cascade : IVectorLengthComputer
	{
		public int ComputeLength(int[] a)
		{
			int count = FlpOfTwo(a.Length - 1);;
			Task<int>[] tasks = new Task<int>[count];

			for (int i = 0; i < count; i++)
			{
				int aa = i * 2 < a.Length ? a[i * 2] : 0;
				int bb = i * 2 + 1 < a.Length ? a[i * 2 + 1] : 0;
				tasks[i] = Task.Factory.StartNew(() => SquareSum(aa, bb));
			}

			for (int i = count / 2; i > 0; i /= 2)
				for (int j = 0; j < i; j++)
				{
					int aa = tasks[j * 2].Result;
					int bb = tasks[j * 2 + 1].Result;
					tasks[j] = Task.Factory.StartNew(() => aa + bb);
				}

			return (int)Math.Sqrt(tasks[0].Result);
		}
		static int FlpOfTwo(int x)
		{
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			return x - (x >> 1);
		}
		static int SquareSum(int a, int b)
		{
			return a * a + b * b;
		}
	}
}
