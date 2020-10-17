using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Filter.Filtering
{
    class SobelX : IFilter // исправить фильтр
    {
        private int processedPixels = 0;
        public byte[] Process(byte[] inputImage, int height, int width)
        {
            byte[] result = new byte[height * width * 4];
            int[,] directions = new int[3, 3] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    int[] temp = new int[3] { 0, 0, 0 };
                    for (int iterDirX = 0; iterDirX < 3; iterDirX++)
                        for (int iterDirY = 0; iterDirY < 3; iterDirY++)
                            if ((i + iterDirX - 1) >= 0 && (i + iterDirX - 1) <= (height - 1) && (j + iterDirY - 1) >= 0 && (j + iterDirY - 1) <= (width - 1))
                            {
                                for (int k = 0; k < 3; k++)
                                    temp[k] += (inputImage[((i + iterDirX - 1) * width + j + iterDirY - 1) * 3 + k] * directions[iterDirX, iterDirY]);
                            }
                    for (int k = 0; k < 3; k++)
                    {
                        temp[k] = temp[k] > 255 ? 255 : temp[k];
                        temp[k] = temp[k] < 0 ? 0 : temp[k];
                        result[(i * width + j) * 3 + k] = (byte)temp[k];
                    }
                }
            return result;
        }

        public int Progress()
        {
            return processedPixels;
        }
    }
}
