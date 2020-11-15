using System;
using FirstTask.ImageDescription;

namespace FirstTask.FiltersDescription
{
	public class Gauss : Filter
	{
		public Gauss(int size) : base(size) 
		{
			Bit = new double[Size * Size * sizeof(double)];
			double sig = 0.6, pi = Math.PI;

			for (int x = 0; x < Size; x++)
			{
				for (int y = 0; y < Size; y++)
				{
					Bit[x * Size + y] = 1.0 / Math.Sqrt(2 * pi * sig) * Math.Exp(-(x * x + y * y) / (2 * sig * sig));
				}
			}
		}
		public override void FilterImplementation(BitMapFile image)
		{
			base.FilterImplementation(image);
		}
	}
}
