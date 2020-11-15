using System;
using FirstTask.ImageDescription;

namespace FirstTask.FiltersDescription
{
	public abstract class Filter
	{
		protected double[] Bit { get; set; }
		protected int Size { get; set; }
		public Filter(int size)
		{
			Size = size;
		}
		public virtual void FilterImplementation(BitMapFile image)
		{
			var newPixels = new byte[3 * image.Height * image.Width * sizeof(byte)];

			for (int i = 0; i < image.Height; i++)
			{
				for (int j = 0; j < image.Width; j++)
				{
					double[] rgb = { 0, 0, 0 };
					double a = 0;

					for (int y = 0; y < Size; y++)
					{
						for (int x = 0; x < Size; x++)
						{
							if (((i + y - 1) >= 0) && ((i + y - 1) < image.Height) && ((j + x - 1) >= 0) && ((j + x - 1) < image.Width))
							{
								a += Bit[y * Size + x];
								for (int k = 0; k < 3; k++)
								{
									rgb[k] += image.PixelsBytes[((i + y - 1) * image.Width + j + x - 1) * 3 + k] * Bit[y * Size + x];
								}
							}
						}
					}

					for (int k = 0; k < 3; k++)
					{
						newPixels[(i * image.Width + j) * 3 + k] = Convert.ToByte(rgb[k] / a);
					}
				}
			}

			FilterAssignment(image, newPixels);
		}

		protected static void FilterAssignment(BitMapFile image, byte[] newPixels)
		{
			for (uint i = 0; i < image.Height * image.Width * 3; i++)
			{
				image.PixelsBytes[i] = newPixels[i];
			}
		}

	}
}