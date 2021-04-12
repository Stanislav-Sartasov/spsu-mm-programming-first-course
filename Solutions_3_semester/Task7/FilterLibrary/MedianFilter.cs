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
    public class MedianFilter : IFilter
	{
		public IFilter Use(byte[] image)
		{
			MedianFilter filter = new MedianFilter();
			filter.UseFilter(BitmapGetter.GetBitmap(image));
			return filter;
		}
        object dataLock = new object();
        double progress = 0;
        byte[] result = null;
        bool aborted = false;
        public string Name { get; } = "Median";
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
                    byte[,] maskMap = Masking.CreateCircleMask(5);
                    byte[] arr = new byte[maskMap.Length];
                    int len;
                    int counter = 0;

                    Bitmap bitmapNew = new Bitmap(image.Width, image.Height);

                    int maxX = maskMap.GetLength(0) / 2;
                    int maxY = maskMap.GetLength(1) / 2;

                    for (int y = 0; y < image.Height; y++)
                        for (int x = 0; x < image.Width; x++)
                        {
                            byte[] newColor = new byte[3];
                            for (int c = 0; c < 3; c++)
                            {
                                len = 0;
                                for (int j = -maxY; j <= maxY; j++)
                                    for (int i = -maxX; i <= maxX; i++)
                                        if ((y + j) >= 0 && (y + j) < image.Height && (x + i) >= 0 && (x + i) < image.Width && maskMap[i + maxX, j + maxY] != 0)
                                        {
                                            arr[len] = BitmapGetter.GetRGBPixel(image, x + i, y + j)[c];
                                            len++;
                                        }
                                Array.Sort(arr, 0, len);
                                newColor[c] = arr[len / 2];
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
