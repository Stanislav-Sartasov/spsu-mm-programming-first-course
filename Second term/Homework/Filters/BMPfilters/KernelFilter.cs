
namespace BMPfilters
{
    public abstract class KernelFilter : Filter
    {
        public double[,] Matrix { get; set; } //only square matrix
        public override BMPfile.Pixel[,] ApplyFilter(BMPfile.Pixel[,] pixels)
        {

            double blueValue;
            double greenValue;
            double redValue;
            int imageHeight = pixels.GetLength(0);
            int imageWidth = pixels.GetLength(1);
            int numOfChannels = pixels[0, 0].Channel.Length;
            int frameWidth = (Matrix.GetLength(0) - 1) / 2;


            BMPfile.Pixel[,] newPixels = new BMPfile.Pixel[imageHeight, imageWidth];

            for (int height = 0; height < imageHeight; height++)
            {
                for (int width = 0; width < imageWidth; width++)
                {
                    newPixels[height, width].Channel = new byte[numOfChannels];
                    blueValue = greenValue = redValue = 0;

                    if (height >= frameWidth && width >= frameWidth && height < (imageHeight - frameWidth) && width < (imageWidth - frameWidth))
                    {
                        for (int y = -frameWidth; y <= frameWidth; y++)
                        {
                            for (int x = -frameWidth; x <= frameWidth; x++)
                            {
                                blueValue += pixels[height + y, width + x].Channel[0] * Matrix[y + frameWidth, x + frameWidth];
                                greenValue += pixels[height + y, width + x].Channel[1] * Matrix[y + frameWidth, x + frameWidth];
                                redValue += pixels[height + y, width + x].Channel[2] * Matrix[y + frameWidth, x + frameWidth];
                            }
                        }
                        newPixels[height, width].Channel[0] = ToByte(blueValue);
                        newPixels[height, width].Channel[1] = ToByte(greenValue);
                        newPixels[height, width].Channel[2] = ToByte(redValue);

                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                            newPixels[height, width].Channel[i] = pixels[height, width].Channel[i];
                    }
                    if (numOfChannels == 4)
                        newPixels[height, width].Channel[3] = pixels[height, width].Channel[3];
                }
            }
            return newPixels;
        }
    }
}
