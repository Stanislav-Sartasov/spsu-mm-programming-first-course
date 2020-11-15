using System;
using FirstTask.ImageDescription;

namespace FirstTask.FiltersDescription
{
	public class Median : Filter
	{
		public Median(int size) : base(size)
		{
			Bit = new double[Size * Size * sizeof(double)];

			for (int i = 0; i < Size * Size; i++)
			{
				Bit[i] = 1;
			}
		}
		public override void FilterImplementation(BitMapFile image)
		{
			base.FilterImplementation(image);
		}
	}
}
