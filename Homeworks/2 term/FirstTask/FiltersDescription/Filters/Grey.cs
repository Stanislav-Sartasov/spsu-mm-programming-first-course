using System;
using FirstTask.ImageDescription;

namespace FirstTask.FiltersDescription
{
	public class Grey : Filter
	{
		public Grey(int size) : base(size) { }
		public override void FilterImplementation(BitMapFile image)
		{
			for (int i = 0; i < image.Height * image.Width; i++)
			{
				var layout = Convert.ToByte((20 * image.PixelsBytes[i * 3] + 70 * image.PixelsBytes[i * 3 + 1] + 5 * image.PixelsBytes[i * 3 + 2]) / 100);
				for (int k = 0; k < 3; k++)
				{
					image.PixelsBytes[i * 3 + k] = layout;
				}
			}
		}
	}
}
