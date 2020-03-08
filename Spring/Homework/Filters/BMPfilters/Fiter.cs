namespace BMPfilters
{
    public abstract class Filter
    {

        abstract public BMPfile.Pixel[,] ApplyFilter(BMPfile.Pixel[,] pixels);
        public static byte ToByte(double value)
        {
            if (value > 255)
                return 255;
            else if (value < 0)
                return 0;
            else
                return (byte)value;
        }
    }
}
