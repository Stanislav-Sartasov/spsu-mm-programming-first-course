using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System;
using System.IO;
using Picture;


namespace FiltersTest
{
    enum FiltersName : uint
    {
        Averaging = 1,
        Gauss = 2,
        SobelX = 3,
        SobelY = 4,
    };


    [TestClass]
    public class FiltersTest
    {
        BMP BmpInput = new BMP();
        BMP BmpExpected = new BMP();
        Filters Filtration = new Filters();

        string path = Directory.GetCurrentDirectory() + @"\.." + @"\.." + @"\..";

        private void Check(BMP expected, BMP actual)
        {

            Assert.AreEqual(expected.HeaderInfoBMP.Height, actual.HeaderInfoBMP.Height);
            Assert.AreEqual(expected.HeaderInfoBMP.Width, actual.HeaderInfoBMP.Width);

            byte[,,] ExpectedPixels = new byte[expected.HeaderInfoBMP.Height, expected.HeaderInfoBMP.Width, 3];
            byte[,,] ActualPixels = new byte[actual.HeaderInfoBMP.Height, actual.HeaderInfoBMP.Width, 3];

            for (int i = 0; i < expected.HeaderInfoBMP.Height; i++)
                for (int j = 0; j < expected.HeaderInfoBMP.Width; j++)
                    for (int k = 0; k < 3; k++)
                        Assert.AreEqual(ExpectedPixels[i, j, k], ActualPixels[i, j, k]);
        }


        [TestMethod]
        public void TestGrey()
        {
            BmpInput.ReadBMP(path + @"\Resources\TestIn.bmp");
            BmpExpected.ReadBMP(path + @"\Resources\Grey.bmp");

            Filtration.Grey(BmpInput.pixels, BmpInput.HeaderInfoBMP.Height, BmpInput.HeaderInfoBMP.Width);
            Check(BmpInput, BmpExpected);
        }

        [TestMethod]
        public void TestGauss()
        {
            BmpInput.ReadBMP(path + @"\Resources\TestIn.bmp");
            BmpExpected.ReadBMP(path + @"\Resources\Gauss.bmp");

            Filtration.Filter(BmpInput.pixels, BmpInput.HeaderInfoBMP.Height, BmpInput.HeaderInfoBMP.Width, (uint)FiltersName.Gauss);
            Check(BmpInput, BmpExpected);
        }
        [TestMethod]
        public void TestAveraging()
        {
            BmpInput.ReadBMP(path + @"\Resources\TestIn.bmp");
            BmpExpected.ReadBMP(path + @"\Resources\Averaging.bmp");

            Filtration.Filter(BmpInput.pixels, BmpInput.HeaderInfoBMP.Height, BmpInput.HeaderInfoBMP.Width, (uint)FiltersName.Averaging);
            Check(BmpInput, BmpExpected);
        }

        [TestMethod]
        public void TestSobelX()
        {
            BmpInput.ReadBMP(path + @"\Resources\TestIn.bmp");
            BmpExpected.ReadBMP(path + @"\Resources\SobelX.bmp");

            Filtration.Filter(BmpInput.pixels, BmpInput.HeaderInfoBMP.Height, BmpInput.HeaderInfoBMP.Width, (uint)FiltersName.SobelX);
            Check(BmpInput, BmpExpected);
        }

        [TestMethod]
        public void TestSobelY ()
        {
            BmpInput.ReadBMP(path + @"\Resources\TestIn.bmp");
            BmpExpected.ReadBMP(path + @"\Resources\SobelY.bmp");

            Filtration.Filter(BmpInput.pixels, BmpInput.HeaderInfoBMP.Height, BmpInput.HeaderInfoBMP.Width, (uint)FiltersName.SobelY);
            Check(BmpInput, BmpExpected);
        }

 
    }
}
