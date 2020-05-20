using System;

namespace TaskFilters
{
    public class Filters
    {
        public static double GaussFunc(double a, double b)
        {
            double sigma = 0.8;
            double pi = 3.14159265358979323846264338;
            return (1 / Math.Sqrt(2 * pi * sigma) * Math.Exp(-(a * a + b * b) / (2 * sigma * sigma)));
        }

        public static void Convolution(byte [,,] image, uint height, uint width, string type)
        {
            if (String.Compare("Grayscale", type) == 0)
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        byte res = (byte)((image[i, j, 0] * 0.299 + image[i, j, 1] * 0.587 + image[i, j, 2] * 0.114));
                        image[i, j, 0] = res;
                        image[i, j, 1] = res;
                        image[i, j, 2] = res;

                    }
                }
                return;
            }
            
            int size;
            if (String.Compare("Gauss5", type) == 0) size = 2;
            else size = 1;

            double[,] kernel = new double[size * 2 + 1, size * 2 + 1];
            if (String.Compare("Averaging", type) == 0)
            {
                for (int i = 0; i < 2 * size + 1; i++)
                {
                    for (int j = 0; j < 2 * size + 1; j++)
                    {
                        kernel[i, j] = 1;
                    }
                }
            }
            else if ((String.Compare("Gauss3", type) == 0) || (String.Compare("Gauss5", type) == 0))
            {
                for (int i = -size; i <= size; i++)
                {
                    for (int j = -size; j <= size; j++)
                    {
                        kernel[i + size, j + size] = GaussFunc(i, j);
                    }
                }
            }
            else if ((String.Compare("SobelX", type) == 0))
            {
                double[] mask = { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
                for (int i = 0; i < 9; i++)
                {
                    kernel[i / 3, i % 3] = mask[i];
                }
            }
            else if ((String.Compare("SobelY", type) == 0))
            {
                double[] mask = { -1, -2, -1, 0, 0, 0, 1, 2, 1 };
                for (int i = 0; i < 9; i++)
                {
                    kernel[i / 3, i % 3] = mask[i];
                }
            }

            byte[,,] CopyImage = new byte[height, width, 3];
            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    double[] res = { 0, 0, 0 };
                    double sum = 0.0;
                    for (int i = 0; i < 2 * size + 1; i++)
                    {
                        for (int j = 0; j < 2 * size + 1; j++)
                        {
                            if ((x + i - 1) >= 0 && (x + i - 1) < height && (y + j - 1) >= 0 && (y + j - 1) < width)
                            {
                                res[0] += image[(x + i - 1),(y + j - 1), 0] * kernel[i, j];
                                res[1] += image[(x + i - 1), (y + j - 1), 1] * kernel[i, j];
                                res[2] += image[(x + i - 1), (y + j - 1), 2] * kernel[i, j];
                                sum += kernel[i, j];
                            }
                        }
                    }
                    if ((String.Compare("SobelX", type) == 0) || (String.Compare("SobelY", type) == 0))
                    {
                        byte result = (byte)((Math.Abs(res[0]) + Math.Abs(res[1]) + Math.Abs(res[2])) / 3);
                        for (int i = 0; i < 3; i++)
                        {
                            CopyImage[x, y, i] = result;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            CopyImage[x, y, i] = (byte)(Math.Abs(res[i]) / sum);
                        }
                    }
                }
            }
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    for (int k = 0; k < 3; k++)
                        image[i, j, k] = CopyImage[i, j, k];
                    
                }
            }


        }
    }
}
