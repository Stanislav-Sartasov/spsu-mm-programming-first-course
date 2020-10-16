using System;


namespace Filter.Filtering
{
    public interface IFilter
    {
        byte[] Process(byte[] inputImage, int height, int width);
        int Progress();
    }
}
