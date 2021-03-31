using System;

namespace CascadeLib
{
	public class Sequential : IVectorLengthComputer
	{
		public int ComputeLength(int[] a)
		{
			int sum = default;

			for (int i = 0; i < a.Length; i++)
			{
				sum += Func.Square(a[i]);
			}

			return (int)Math.Sqrt(sum);
		}
	}
}