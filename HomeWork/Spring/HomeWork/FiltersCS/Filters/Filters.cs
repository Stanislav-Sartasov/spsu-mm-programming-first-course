using System;
using System.Collections.Generic;
using System.Text;

namespace Picture
{
	enum FiltersName : uint
	{
		Averaging = 1,
		Gauss = 2,
		SobelX = 3,
		SobelY = 4,
	};
	public class Filters
	{
		public void Grey(byte[,,] pixels, uint heigh, uint width)
		{
			for (int i = 0; i < heigh; i++)
			{
				for (int j = 0; j < width; j++)
				{
					byte mid = (byte)((pixels[i, j, 0] + pixels[i, j, 1] + pixels[i, j, 2]) / 3);
					pixels[i, j, 0] = mid;
					pixels[i, j, 1] = mid;
					pixels[i, j, 2] = mid;
				}
			}
		}
		private static void Averaging(int[,] matrix)
		{
			matrix[0, 0] = 1;
			matrix[0, 1] = 1;
			matrix[0, 2] = 1;
			matrix[1, 0] = 1;
			matrix[1, 1] = 1;
			matrix[1, 2] = 1;
			matrix[2, 0] = 1;
			matrix[2, 1] = 1;
			matrix[2, 2] = 1;
		}
		private static void Gauss (int[,] matrix)
		{
			matrix[0, 0] = 1;
			matrix[0, 1] = 2;
			matrix[0, 2] = 1;
			matrix[1, 0] = 2;
			matrix[1, 1] = 4;
			matrix[1, 2] = 2;
			matrix[2, 0] = 1;
			matrix[2, 1] = 2;
			matrix[2, 2] = 1;
		}
		private static void SobelX(int[,] matrix)
		{
			matrix[0, 0] = 1;
			matrix[0, 1] = 2;
			matrix[0, 2] = 1;
			matrix[1, 0] = 0;
			matrix[1, 1] = 0;
			matrix[1, 2] = 0;
			matrix[2, 0] = -1;
			matrix[2, 1] = -2;
			matrix[2, 2] = -1;
		}
		private static void SobelY(int[,] matrix)
		{
			matrix[0, 0] = -1;
			matrix[0, 1] = 0;
			matrix[0, 2] = 1;
			matrix[1, 0] = -2;
			matrix[1, 1] = 0;
			matrix[1, 2] = 2;
			matrix[2, 0] = -1;
			matrix[2, 1] = 0;
			matrix[2, 2] = 1;
		}
		public void Filter (byte [,,] pixels, uint height, uint width, uint choice)
		{
			int[,] matrix = new int[3, 3];

			if (choice == (uint)FiltersName.Averaging)
				Averaging(matrix);
			if (choice == (uint)FiltersName.Gauss)
				Gauss(matrix);
			if (choice == (uint)FiltersName.SobelX)
				SobelX(matrix);
			else if (choice == (uint)FiltersName.SobelY)
				SobelY(matrix);

			byte[,,] output = new byte[height, width, 3];
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					int[] result = new int[3] { 0, 0, 0 };
					int d = 0;
					for (int x = 0; x < 3; x++)
					{
						for (int y = 0; y < 3; y++)
						{
							if ((i + x - 1) >= 0 && (i + x - 1) <= (height - 1) && (j + y - 1) >= 0 && (j + y - 1) <= (width - 1))
							{
								d += matrix[x, y];
								for (int k = 0; k < 3; k++)
									result[k] += (pixels[i + x - 1, j + y - 1, k] * matrix[x, y]);
							
							}
						}
					}
					if (choice == (uint)FiltersName.Averaging || choice == (uint)FiltersName.Gauss) //усред гаус
					{
						for (int k = 0; k < 3; k++)
							output[i, j, k] = (byte)(result[k] / d);

					}
					else // sobelx sobely
					{
						for (int k = 0; k < 3; k++)
						{
							if (result [k] > 255)
								result[k] = 255;
							else if (result[k] < 0)
								result[k] = 0;
							output[i, j, k] = (byte)result[k];
						}
					}
				}
			}
			for (int i = 0; i < height; i++)
				for (int j = 0; j < width; j++)
					for (int k = 0; k < 3; k++)
						pixels[i, j, k] = output[i, j, k];
		}
	}
}
