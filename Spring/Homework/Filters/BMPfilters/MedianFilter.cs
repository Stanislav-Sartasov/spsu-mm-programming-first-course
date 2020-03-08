using System;

namespace BMPfilters
{
    public class MedianFilter : Filter
    {
        public override BMPfile.Pixel[,] ApplyFilter(BMPfile.Pixel[,] pixels)
        {
            int imageHeight = pixels.GetLength(0);
            int imageWidth = pixels.GetLength(1);
            int numOfChannels = pixels[0, 0].Channel.Length;
            BMPfile.Pixel[,] newPixels = new BMPfile.Pixel[imageHeight, imageWidth];
            double[] blueValue = new double[9];
            double[] greenValue = new double[9];
            double[] redValue = new double[9];
            for (int height = 0; height < imageHeight; height++)
            {
                for (int width = 0; width < imageWidth; width++)
                {
                    newPixels[height, width].Channel = new byte[numOfChannels];
                    int index = 0;
                    if (height * width > 0 && height < (imageHeight - 1) && width < (imageWidth - 1))
                    {
                        for (int y = -1; y < 2; y++)
                        {
                            for (int x = -1; x < 2; x++)
                            {
                                blueValue[index] = pixels[height + y, width + x].Channel[0];
                                greenValue[index] = pixels[height + y, width + x].Channel[1];
                                redValue[index] = pixels[height + y, width + x].Channel[2];
                                index++;
                            }
                        }
                    }
                    Array.Sort(blueValue);
                    Array.Sort(greenValue);
                    Array.Sort(redValue);
                    newPixels[height, width].Channel[0] = ToByte(blueValue[4]);
                    newPixels[height, width].Channel[1] = ToByte(greenValue[4]);
                    newPixels[height, width].Channel[2] = ToByte(redValue[4]);

                    if (numOfChannels == 4)
                        newPixels[height, width].Channel[3] = pixels[height, width].Channel[3];

                }
            }
            return newPixels;
        }
    }
}
