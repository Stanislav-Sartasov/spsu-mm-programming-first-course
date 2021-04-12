using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocols;
using System.Drawing;
using System.IO;

namespace FilterLibrary
{
    public class ShadeFilter : IFilter
    {
        public IFilter Use(byte[] image)
        {
            ShadeFilter filter = new ShadeFilter();
            filter.UseFilter(BitmapGetter.GetBitmap(image));
            return filter;
        }

        object dataLock = new object();
        double progress = 0;
        byte[] result = null;
        double[] shadeCoefficient = { 0.299, 0.587, 0.114 };
        bool aborted = false;
        public string Name { get; } = "Shade";
        public double Progress
        {
            get
            {
                lock (dataLock)
                    return progress;
            }
        }
        public byte[] Result
        {
            get
            {
                lock (dataLock)
                    return result;
            }
        }
        void UseFilter(Bitmap image)
        {
            Task.Run(() =>
            {
                Bitmap bitmapNew = new Bitmap(image.Width, image.Height);
                int counter = 0;

                try
                {
                    double shade;
                    for (int y = 0; y < image.Height; y++)
                        for (int x = 0; x < image.Width; x++)
                        {
                            shade = 0;
                            for (int c = 0; c <= 2; c++)
                                shade += BitmapGetter.GetRGBPixel(image, x, y)[c] * shadeCoefficient[c];
                            bitmapNew.SetPixel(x, y, Color.FromArgb((int)shade, (int)shade, (int)shade));
                            if (counter++ % 1000 == 0)
                                lock (dataLock)
                                {
                                    if (aborted)
                                        return;
                                    progress = counter * 100 / (image.Height * image.Width);
                                }
                        }

                    lock (dataLock)
                    {
                        progress = 100;
                        result = BitmapGetter.GetArray(bitmapNew);
                    }
                }
                catch
                {
                    lock (dataLock)
                        progress = -1;
                }
            });
        }

        public void Abort()
		{
            lock (dataLock)
                aborted = true;
		}
    }
}
