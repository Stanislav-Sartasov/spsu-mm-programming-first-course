using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FilterLibrary
{
	static class Masking
	{
        static byte[,] CreateMask(int size, Func<int, int, int> Func)
        {
            if (size <= 0)
                return null;
            if (size % 2 == 0)
                size++;

            byte[,] mask = new byte[size, size];
            int m = size / 2;

            for (int j = -m; j <= m; j++)
                for (int i = -m; i <= m; i++)
                    if (Func(i, j) > 0)
                        mask[i + m, j + m] = 1;
                    else
                        mask[i + m, j + m] = 0;

            return mask;
        }
        public static byte[,] CreateSquareMask(int size)
        {
            int Func(int i, int j)
            {
                return 1;
            }

            return CreateMask(size, Func);
        }
        public static byte[,] CreateCircleMask(int size)
        {
            int Func(int i, int j)
            {
                if (Math.Pow(i, 2) + Math.Pow(j, 2) <= Math.Pow(size / 2, 2))
                    return 1;
                else
                    return 0;
            }

            return CreateMask(size, Func);
        }
        public static byte[,] CreateCrossMask(int size)
        {
            int Func(int i, int j)
            {
                if (i == 0 || j == 0)
                    return 1;
                else
                    return 0;
            }

            return CreateMask(size, Func);
        }
        public static byte[,] CreateDiagonalCrossMask(int size)
        {
            int Func(int i, int j)
            {
                if (Math.Abs(i) == Math.Abs(j))
                    return 1;
                else
                    return 0;
            }

            return CreateMask(size, Func);
        }
        public static byte[,] CreateEmptySquareMask(int size)
        {
            int Func(int i, int j)
            {
                if (Math.Abs(i) == size / 2 || Math.Abs(j) == size / 2)
                    return 1;
                else
                    return 0;
            }

            return CreateMask(size, Func);
        }
        public static double UseMask(Bitmap image, int x, int y, int c, double[,] mask, double maxSum)
        {
            int maxX = mask.GetLength(0) / 2;
            int maxY = mask.GetLength(1) / 2;
            double sum = 0;
            for (int j = -maxY; j <= maxY; j++)
                for (int i = -maxX; i <= maxX; i++)
                    if ((y + j) >= 0 && (y + j) < image.Height && (x + i) >= 0 && (x + i) < image.Width)
                        sum += BitmapGetter.GetRGBPixel(image, x + i, y + j)[c] * mask[i + maxX, j + maxY];
                    else
                        maxSum -= mask[i + maxX, j + maxY];
            if (maxSum == 0)
                return -1;
            return sum / maxSum;
        }
    }
}
