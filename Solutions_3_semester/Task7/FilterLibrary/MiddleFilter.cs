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
    public class MiddleFilter : IFilter
	{
        public IFilter Use(byte[] image)
        {
            MiddleFilter filter = new MiddleFilter();
            filter.UseFilter(BitmapGetter.GetBitmap(image));
            return filter;
        }
        object dataLock = new object();
        double progress = 0;
        byte[] result = null;
        bool aborted = false;
        public string Name { get; } = "Middle";
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
                    int count = 0;
                    byte[,] maskMap = Masking.CreateCircleMask(5);

                    double[,] mask = new double[maskMap.GetLength(0), maskMap.GetLength(1)];

                    for (int j = 0; j < maskMap.GetLength(1); j++)
                        for (int i = 0; i < maskMap.GetLength(0); i++)
                            if (maskMap[i, j] != 0)
                            {
                                mask[i, j] = 1;
                                count++;
                            }
                            else
                                mask[i, j] = 0;

                    Bitmap bitmapNew = new Bitmap(image.Width, image.Height);
                    double code;
                    int counter = 0;

                    for (int y = 0; y < image.Height; y++)
                        for (int x = 0; x < image.Width; x++)
                        {
                            byte[] newColor = new byte[3];
                            for (int c = 0; c <= 2; c++)
                            {
                                code = Masking.UseMask(image, x, y, c, mask, count);
                                if (code >= 0)
                                    newColor[c] = (byte)code;
                                else
                                    throw new Exception();
                            }
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
