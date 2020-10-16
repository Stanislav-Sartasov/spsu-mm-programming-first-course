using System;
using System.Collections.Generic;
using System.Text;

namespace Filter.Filtering
{
    class Gray : IFilter
    {
        private int processedPixels = 0;
        public byte[] Process(byte[] inputImage, int height, int width)
        {
            byte[] result = new byte[height * width * 4];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    byte x = (byte)(0.299 * inputImage[i * width + j] + 0.587 * inputImage[i * width + j + 1] + 0.114 * inputImage[i * width + j + 2]);
                    result[i * width + j] = x;
                    result[i * width + j + 1] = x;
                    result[i * width + j + 2] = x;
                    result[i * width + j + 3] = x;
                    processedPixels++;
                    //byte x = (byte)((2126 * processedPicture[i, j, 0] + 7152 * processedPicture[i, j, 1] + 722 * processedPicture[i, j, 2]) / 10000);
                    //byte x = (byte)(0.212 * picture.processedPicture[i, j, 0] + 0.701 * picture.processedPicture[i, j, 1] + 0.087 * picture.processedPicture[i, j, 2]);
                    //byte x = (byte)(0.2989 * picture.processedPicture[i, j, 0] + 0.5870 * picture.processedPicture[i, j, 1] + 0.1140 * picture.processedPicture[i, j, 2]);
                    //byte x = (byte)(0.243 * picture.processedPicture[i, j, 0] + 0.41 * picture.processedPicture[i, j, 1] + 0.347 * picture.processedPicture[i, j, 2]);
                }
            return result;
        }

        public int Progress()
        {
            return processedPixels;
        }
    }
}
