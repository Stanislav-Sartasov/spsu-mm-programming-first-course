using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Task5
{
	public class RegAdd : IVectorLengthComputer
	{
		public int ComputeLength(int[] a)
		{
			int coreCount = Environment.ProcessorCount;
			int blockSize = a.Length / coreCount + (a.Length % coreCount == 0 ? 0 : 1);

			Task<int>[] tasks = new Task<int>[coreCount];

			for (int i = 0; i < coreCount; i++)
			{
				List<int> arr = new List<int>();
				for (int j = i; j < a.Length; j += coreCount)
					arr.Add(a[j]);
				tasks[i] = Task.Factory.StartNew(() => SquareSum(arr));
			}

			int sum = 0;
			foreach (var task in tasks)
				sum += task.Result;

			return (int)Math.Sqrt(sum);
		}

		static int SquareSum(List<int> arr)
		{
			int sum = 0;
			foreach (int i in arr)
				sum += i * i;
			return sum;
		}
	}
}
