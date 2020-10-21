using System;
using System.Threading;

namespace Filter.Filtering
{
    public interface IFilter
    {
        byte[] Process(byte[] inputImage, int height, int width, CancellationToken token);
        int Progress();
    }
}
