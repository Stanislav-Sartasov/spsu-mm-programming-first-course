using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Filter.Filtering
{
    class SobelY : IFilter
    {
        private int processedPixels = 0;
        public byte[] Process(byte[] inputImage, int height, int width, CancellationToken token)
        {
            processedPixels = 0;
            byte[] result = new byte[height * width * 4];
            int[,] directions = new int[3, 3] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    if (token.IsCancellationRequested)
                        return null;

                    int[] temp = new int[4] { 0, 0, 0, 0 };
                    for (int iterDirX = 0; iterDirX < 3; iterDirX++)
                        for (int iterDirY = 0; iterDirY < 3; iterDirY++)
                            if ((i + iterDirX - 1) >= 0 && (i + iterDirX - 1) <= (height - 1) && (j + iterDirY - 1) >= 0 && (j + iterDirY - 1) <= (width - 1))
                            {
                                for (int k = 0; k < 4; k++)
                                    temp[k] += (inputImage[((i + iterDirX - 1) * width + j + iterDirY - 1) * 4 + k] * directions[iterDirX, iterDirY]);
                            }
                    for (int k = 0; k < 4; k++)
                    {
                        temp[k] = temp[k] > 255 ? 255 : temp[k];
                        temp[k] = temp[k] < 0 ? 0 : temp[k];
                        result[(i * width + j) * 4 + k] = (byte)temp[k];
                    }
                    processedPixels++;
                }
            return result;
        }

        public int Progress()
        {
            return processedPixels;
        }
    }
}
