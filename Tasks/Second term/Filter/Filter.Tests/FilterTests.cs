using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Filter.Picture;
using System.Drawing;
using System.Resources.Extensions;

namespace Filter.Tests
{

    [TestClass]
    public class FilterTests
    {
        private static Bitmap Spectrum = Properties.Resources.spectrum;
        private static Bitmap SpectrumGrey = Properties.Resources.spectrumGrey;
        private static Bitmap SpectrumAveraging = Properties.Resources.spectrumAveraging;
        private static Bitmap SpectrumGauss3 = Properties.Resources.spectrumGauss3;
        private static Bitmap SpectrumSobelX = Properties.Resources.spectrumSobelX;
        private static Bitmap SpectrumSobelY = Properties.Resources.spectrumSobelY;
        private static uint Height = (uint)Spectrum.Height;
        private static uint Width = (uint)Spectrum.Width;
        private byte[,,] ActualPicture = new byte[Height, Width, 3];
        private byte[,,] ExpectedPicture = new byte[Height, Width, 3];

        public void PicturesInitialization(string type)
        {
            
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    Color pixel = Spectrum.GetPixel(j, i);
                    ActualPicture[i, j, 0] = pixel.R;
                    ActualPicture[i, j, 1] = pixel.G;
                    ActualPicture[i, j, 2] = pixel.B;
                }
            Bitmap temp = new Bitmap(Spectrum);
            switch (type)
            {
                case "Grey":
                    {
                        temp = new Bitmap(SpectrumGrey);
                        break;
                    }
                case "Averaging":
                    {
                        temp = new Bitmap(SpectrumAveraging);
                        break;
                    }
                case "Gauss3":
                    {
                        temp = new Bitmap(SpectrumGauss3);
                        break;
                    }
                case "SobelX":
                    {
                        temp = new Bitmap(SpectrumSobelX);
                        break;
                    }
                case "SobelY":
                    {
                        temp = new Bitmap(SpectrumSobelY);
                        break;
                    }
            }
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    Color pixel = temp.GetPixel(j, i);
                    ExpectedPicture[i, j, 0] = pixel.R;
                    ExpectedPicture[i, j, 1] = pixel.G;
                    ExpectedPicture[i, j, 2] = pixel.B;
                }
        }
        [TestMethod]
        public void GreyFilterValidation()
        {
            //arrange
            PicturesInitialization("Grey");

            //act
            GreyFilter(ActualPicture, Height, Width);

            int diff = 0;
            //assert
            for(int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    if ((ActualPicture[i, j, 0] != ExpectedPicture[i, j, 0]) ||
                        (ActualPicture[i, j, 1] != ExpectedPicture[i, j, 1]) ||
                        (ActualPicture[i, j, 2] != ExpectedPicture[i, j, 2]))
                        diff++;
                }
            Assert.AreEqual(0, diff);
        }

        [TestMethod]
        public void SobelXFilterValidation()
        {
            //arrange
            PicturesInitialization("SobelX");

            //act
            Sobel(ActualPicture, Height, Width, "SobelX");

            int diff = 0;
            //assert
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    if ((ActualPicture[i, j, 0] != ExpectedPicture[i, j, 0]) ||
                        (ActualPicture[i, j, 1] != ExpectedPicture[i, j, 1]) ||
                        (ActualPicture[i, j, 2] != ExpectedPicture[i, j, 2]))
                        diff++;
                }
            Assert.AreEqual(0, diff);
        }
        [TestMethod]
        public void SobelYFilterValidation()
        {
            //arrange
            PicturesInitialization("SobelY");

            //act
            Sobel(ActualPicture, Height, Width, "SobelY");

            int diff = 0;
            //assert
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    if ((ActualPicture[i, j, 0] != ExpectedPicture[i, j, 0]) ||
                        (ActualPicture[i, j, 1] != ExpectedPicture[i, j, 1]) ||
                        (ActualPicture[i, j, 2] != ExpectedPicture[i, j, 2]))
                        diff++;
                }
            Assert.AreEqual(0, diff);
        }
        [TestMethod]
        public void Gauss3FilterValidation()
        {
            //arrange
            PicturesInitialization("Gauss3");

            //act
            Gauss3(ActualPicture, Height, Width);

            int diff = 0;
            //assert
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    if ((ActualPicture[i, j, 0] != ExpectedPicture[i, j, 0]) ||
                        (ActualPicture[i, j, 1] != ExpectedPicture[i, j, 1]) ||
                        (ActualPicture[i, j, 2] != ExpectedPicture[i, j, 2]))
                        diff++;
                }
            Assert.AreEqual(0, diff);
        }
        [TestMethod]
        public void AveragingFilterValidation()
        {
            //arrange
            PicturesInitialization("Averaging");

            //act
            Averaging(ActualPicture, Height, Width);

            int diff = 0;
            //assert
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    if ((ActualPicture[i, j, 0] != ExpectedPicture[i, j, 0]) ||
                        (ActualPicture[i, j, 1] != ExpectedPicture[i, j, 1]) ||
                        (ActualPicture[i, j, 2] != ExpectedPicture[i, j, 2]))
                        diff++;
                }
            Assert.AreEqual(0, diff);
        }
    }
}
