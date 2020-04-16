using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Filter.Tests
{

    [TestClass]
    public class FilterTests
    {
        private Picture spectrum = new Picture();
        private Picture spectrumExpected = new Picture();

        public void PicturesInitialization(string type)
        {
            string path = Directory.GetCurrentDirectory() + @"\.." + @"\.." + @"\..";
            spectrum.Read(path + @"\Resources\spectrum.bmp");
            switch (type)
            {
                case "Grey":
                    {
                        spectrumExpected.Read(path + @"\Resources\spectrumGrey.bmp");
                        break;
                    }
                case "Averaging":
                    {
                        spectrumExpected.Read(path + @"\Resources\spectrumAveraging.bmp");
                        break;
                    }
                case "Gauss3":
                    {
                        spectrumExpected.Read(path + @"\Resources\spectrumGauss3.bmp");
                        break;
                    }
                case "SobelX":
                    {
                        spectrumExpected.Read(path + @"\Resources\spectrumSobelX.bmp");
                        break;
                    }
                case "SobelY":
                    {
                        spectrumExpected.Read(path + @"\Resources\spectrumSobelY.bmp");
                        break;
                    }
            }
        }
        [TestMethod]
        public void GreyFilterValidation()
        {
            //arrange
            PicturesInitialization("Grey");

            //act
            Picture.Filter(spectrum, "Grey");

            //assert
            int diff = Picture.ImageComparison(spectrum, spectrumExpected);
            Assert.AreEqual(0, diff);
        }
        [TestMethod]
        public void SobelXFilterValidation()
        {
            //arrange
            PicturesInitialization("SobelX");

            //act
            Picture.Filter(spectrum, "SobelX");

            //assert
            int diff = Picture.ImageComparison(spectrum, spectrumExpected);
            Assert.AreEqual(0, diff);
        }
        [TestMethod]
        public void SobelYFilterValidation()
        {
            //arrange
            PicturesInitialization("SobelY");

            //act
            Picture.Filter(spectrum, "SobelY");

            //assert
            int diff = Picture.ImageComparison(spectrum, spectrumExpected);
            Assert.AreEqual(0, diff);
        }
        [TestMethod]
        public void Gauss3FilterValidation()
        {
            //arrange
            PicturesInitialization("Gauss3");

            //act
            Picture.Filter(spectrum, "Gauss3");

            //assert
            int diff = Picture.ImageComparison(spectrum, spectrumExpected);
            Assert.AreEqual(0, diff);
        }
        [TestMethod]
        public void AveragingFilterValidation()
        {
            //arrange
            PicturesInitialization("Averaging");

            //act
            Picture.Filter(spectrum, "Averaging");

            //assert
            int diff = Picture.ImageComparison(spectrum, spectrumExpected);
            Assert.AreEqual(0, diff);
        }
    }
}
