using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Protocols;

namespace FilterLibrary
{
    public class SobelFilter : IFilter
    {
        public IFilter Use(byte[] image)
        {
            SobelFilter filter = new SobelFilter();
            filter.UseFilter(BitmapGetter.GetBitmap(image));
            return filter;
        }
        object dataLock = new object();
        double progress = 0;
        byte[] result = null;
        bool aborted = false;
        public string Name { get; } = "Sobel";
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
                try
                {
                    double gX = 0;
                    double gY = 0;
                    double[,] gXMask = new double[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
                    double[,] gYMask = new double[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };

                    Bitmap bitmapNew = new Bitmap(image.Width, image.Height);
                    int counter = 0;

                    for (int y = 0; y < image.Height; y++)
                        for (int x = 0; x < image.Width; x++)
                        {
                            if (y > 0 && y < image.Height - 1 && x > 0 && x < image.Width - 1)
                            {
                                byte[] newColor = new byte[3];
                                for (int c = 0; c <= 2; c++)
                                {
                                    gX = Math.Abs(Masking.UseMask(image, x, y, c, gXMask, 1));
                                    gY = Math.Abs(Masking.UseMask(image, x, y, c, gYMask, 1));
                                    newColor[c] = (byte)Math.Sqrt(Math.Pow(gX, 2) + Math.Pow(gY, 2));
                                }
                                bitmapNew.SetPixel(x, y, BitmapGetter.GetColor(newColor));
                            }
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
