namespace BMPfilters
{
    public class BWfilter : Filter
    {
        public override BMPfile.Pixel[,] ApplyFilter(BMPfile.Pixel[,] pixels)
        {
            byte middleValue;
            int imageHeight = pixels.GetLength(0);
            int imageWidth = pixels.GetLength(1);
            int numOfChannels = pixels[0, 0].Channel.Length;
            BMPfile.Pixel[,] newPixels = new BMPfile.Pixel[imageHeight, imageWidth];
            for (int height = 0; height < imageHeight; height++)
            {
                for (int width = 0; width < imageWidth; width++)
                {
                    newPixels[height, width].Channel = new byte[numOfChannels];
                    middleValue = ToByte(pixels[height, width].Channel[0] / 3 + pixels[height, width].Channel[1] / 3 + pixels[height, width].Channel[2] / 3);

                    for (int i = 0; i < 3; i++)
                        newPixels[height, width].Channel[i] = middleValue;
                    if (numOfChannels == 4)
                        newPixels[height, width].Channel[3] = pixels[height, width].Channel[3];
                }
            }
            return newPixels;
        }
    }
}
