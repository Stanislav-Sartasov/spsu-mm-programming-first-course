using System;
using FirstTask.ImageDescription;

namespace FirstTask.FiltersDescription
{
	public class Sobel : Filter
	{
		public Sobel(int size, byte paramY) : base(size)
		{
			Bit = new double[Size * Size * sizeof(double)];

			if (paramY == 0) // SobelX
			{
				var mat = new int[9]
				{
					-1, 0, 1,
					-2, 0, 2,
					-1, 0, 1
				};

				for (int i = 0; i < Size * Size; i++)
				{
					Bit[i] = mat[i];
				}
			}
			else // SobelY
			{
				var mat = new int[9]
				{
					-1, -2, -1,
					0, 0, 0,
					1, 2, 1
				};

				for (int i = 0; i < Size * Size; i++)
				{
					Bit[i] = mat[i];
				}
			}
		}
		public override void FilterImplementation(BitMapFile image)
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

					int z = 0;
					if ((rgb[0] + rgb[1] + rgb[2]) > 384)
					{
						z = 255;
					}

					for (int k = 0; k < 3; k++)
					{
						newPixels[(i * image.Width + j) * 3 + k] = Convert.ToByte(z);
					}
				}
			}

			FilterAssignment(image, newPixels);
		}
	}
}
