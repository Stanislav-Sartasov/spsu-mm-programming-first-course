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
    public class GaussianFilter : IFilter
    {
        public IFilter Use(byte[] image)
        {
            GaussianFilter filter = new GaussianFilter();
            filter.UseFilter(BitmapGetter.GetBitmap(image));
            return filter;
        }
        object dataLock = new object();
        double progress = 0;
        byte[] result = null;
        bool aborted = false;
        public string Name { get; } = "Gaussian";
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
            double sigma = 0.6;
            Task.Run(() =>
            {
                try
                {
                    byte[,] maskMap = Masking.CreateCircleMask(5);
                    int maxX = maskMap.GetLength(0);
                    int maxY = maskMap.GetLength(1);
                    double[,] kern = new double[maxX, maxY];
                    maxX = maxX / 2;
                    maxY = maxY / 2;
                    double sumKern = 0;
                    for (int j = -maxY; j <= maxY; j++)
                        for (int i = -maxX; i <= maxX; i++)
                            if (maskMap[i + maxX, j + maxY] != 0)
                            {
                                kern[i + maxX, j + maxY] = Math.Exp(-(Math.Pow(i, 2.0) + Math.Pow(j, 2.0)) / (2 * Math.Pow(sigma, 2.0))) / (2 * Math.PI * Math.Pow(sigma, 2.0));
                                sumKern += kern[i + maxX, j + maxY];
                            }
                            else
                                kern[i + maxX, j + maxY] = 0;

                    Bitmap bitmapNew = new Bitmap(image.Width, image.Height);
                    int counter = 0;

                    for (int y = 0; y < image.Height; y++)
                        for (int x = 0; x < image.Width; x++)
                        {
                            byte[] newColor = new byte[3];
                            for (int c = 0; c <= 2; c++)
                                newColor[c] = (byte)Masking.UseMask(image, x, y, c, kern, sumKern);
                            bitmapNew.SetPixel(x, y, BitmapGetter.GetColor(newColor));
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
