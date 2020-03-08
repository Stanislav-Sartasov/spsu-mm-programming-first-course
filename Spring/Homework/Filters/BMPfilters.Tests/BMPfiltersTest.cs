using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using BMPfilters;
using System;
using System.IO;

namespace BMPfilters.Tests
{
    [TestClass]
    public class BMPfiltersTest
    {
        static readonly string filePath = string.Format("{0}Resources\\test.bmp", Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\")));
        private readonly BMPfile actualInput = new BMPfile(filePath);
        private readonly Bitmap expectedInput = new Bitmap(TestImages.test);
        private readonly Bitmap expectedBW = new Bitmap(TestImages.expectedBWfilter);
        private readonly Bitmap expectedGauss3X3 = new Bitmap(TestImages.expectedGauss3X3);
        private readonly Bitmap expectedGauss5X5 = new Bitmap(TestImages.expectedGauss5X5);
        private readonly Bitmap expectedMedian = new Bitmap(TestImages.expectedMedian);
        private readonly Bitmap expectedSobelY = new Bitmap(TestImages.expectedSobelY);
        private readonly Bitmap expectedSobelX = new Bitmap(TestImages.expectedSobelX);

        private bool PixelCompare(Bitmap exp, BMPfile.Pixel[,] pixel)
        {
            int numOfChannels = pixel[0, 0].Channel.Length;
            for (int height = 0; height < exp.Height; height++)
            {
                for (int width = 0; width < exp.Width; width++)
                {
                    byte[] rgb1 = { exp.GetPixel(width, exp.Height - 1 - height).B, exp.GetPixel(width, exp.Height - 1 - height).G, exp.GetPixel(width, exp.Height - 1 - height).R };
                    byte[] rgb2 = { pixel[height, width].Channel[0], pixel[height, width].Channel[1], pixel[height, width].Channel[2] };

                    for (int channel = 0; channel < 3; channel++)
                        if (rgb1[channel] != rgb2[channel])
                            return false;
                    if (numOfChannels == 4)
                        if (exp.GetPixel(width, exp.Height - 1 - height).A != pixel[height, width].Channel[3])
                            return false;
                }
            }
            return true;
        }

        [TestMethod]
        public void CorrectFormat()
        {
            Assert.AreEqual(0x42, actualInput.HeaderBMP.B1);
            Assert.AreEqual(0x4d, actualInput.HeaderBMP.B2);
        }

        [TestMethod]
        public void CorrectImageSize()
        {
            Assert.AreEqual(expectedInput.Height, (int)actualInput.HeaderInfoBMP.Height);
            Assert.AreEqual(expectedInput.Width, (int)actualInput.HeaderInfoBMP.Width);
        }

        [TestMethod]
        public void CorrectBitmap()
        {
            Assert.IsTrue(PixelCompare(expectedInput, actualInput.Pixels));
        }

        [TestMethod]
        public void CorrectBWfilter()
        {
            BWfilter bw = new BWfilter();
            BMPfile.Pixel[,] actualPixels = bw.ApplyFilter(actualInput.Pixels);
            Assert.IsTrue(PixelCompare(expectedBW, actualPixels));
        }

        [TestMethod]
        public void CorrectGauss5X5filter()
        {
            GaussFilter g5 = new GaussFilter("5X5");
            BMPfile.Pixel[,] actualPixels = g5.ApplyFilter(actualInput.Pixels);
            Assert.IsTrue(PixelCompare(expectedGauss5X5, actualPixels));
        }

        [TestMethod]
        public void CorrectGauss3X3filter()
        {
            GaussFilter g3 = new GaussFilter("3X3");
            BMPfile.Pixel[,] actualPixels = g3.ApplyFilter(actualInput.Pixels);
            Assert.IsTrue(PixelCompare(expectedGauss3X3, actualPixels));
        }

        [TestMethod]
        public void CorrectSobelXfilter()
        {
            SobelFilter sX = new SobelFilter("X");
            BMPfile.Pixel[,] actualPixels = sX.ApplyFilter(actualInput.Pixels);
            Assert.IsTrue(PixelCompare(expectedSobelX, actualPixels));
        }

        [TestMethod]
        public void CorrectSobelYfilter()
        {
            SobelFilter sY = new SobelFilter("Y");
            BMPfile.Pixel[,] actualPixels = sY.ApplyFilter(actualInput.Pixels);
            Assert.IsTrue(PixelCompare(expectedSobelY, actualPixels));
        }

        [TestMethod]
        public void CorrectMedianfilter()
        {
            MedianFilter mdn = new MedianFilter();
            BMPfile.Pixel[,] actualPixels = mdn.ApplyFilter(actualInput.Pixels);
            Assert.IsTrue(PixelCompare(expectedMedian, actualPixels));
        }
    }
}
