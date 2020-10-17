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
            processedPixels = 0;
            for (int i = 0; i < height * width; i++)
                {
                    byte x = (byte)(0.299 * inputImage[i * 4] + 0.587 * inputImage[i * 4 + 1] + 0.114 * inputImage[i * 4 + 2]);
                    result[i * 4] = x;
                    result[i * 4 + 1] = x;
                    result[i * 4 + 2] = x;
                    result[i * 4 + 3] = inputImage[i * 4 + 3];
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
