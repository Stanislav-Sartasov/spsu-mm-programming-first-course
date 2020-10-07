using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Filter;

namespace Filters.Tests
{
    [TestClass]
    public class FiltersTests
    {
        private Picture picture = new Picture();
        private Picture correctPicture = new Picture();

        [TestMethod]
        public void ColorWBFilterValidation()
        {
            picture.Read((Directory.GetCurrentDirectory() + @"\.." + @"\.." + @"\..") + @"\Resources\fileIn.bmp");
            correctPicture.Read((Directory.GetCurrentDirectory() + @"\.." + @"\.." + @"\..") + @"\Resources\ColorWB.bmp");
            Picture.Filter(picture, "ColorWB");
            Assert.AreEqual(1, Picture.PictureComparsion(picture, correctPicture));
        }

        [TestMethod]
        public void AveragingFilterValidation()
        {
            picture.Read((Directory.GetCurrentDirectory() + @"\.." + @"\.." + @"\..") + @"\Resources\fileIn.bmp");
            correctPicture.Read((Directory.GetCurrentDirectory() + @"\.." + @"\.." + @"\..") + @"\Resources\Averaging.bmp");
            Picture.Filter(picture, "Averaging");
            Assert.AreEqual(1, Picture.PictureComparsion(picture, correctPicture));
        }

        [TestMethod]
        public void Gauss3FilterValidation()
        {
            picture.Read((Directory.GetCurrentDirectory() + @"\.." + @"\.." + @"\..") + @"\Resources\fileIn.bmp");
            correctPicture.Read((Directory.GetCurrentDirectory() + @"\.." + @"\.." + @"\..") + @"\Resources\Gauss3.bmp");
            Picture.Filter(picture, "Gauss3");
            Assert.AreEqual(1, Picture.PictureComparsion(picture, correctPicture));
        }

        [TestMethod]
        public void SobelXFilterValidation()
        {
            picture.Read((Directory.GetCurrentDirectory() + @"\.." + @"\.." + @"\..") + @"\Resources\fileIn.bmp");
            correctPicture.Read((Directory.GetCurrentDirectory() + @"\.." + @"\.." + @"\..") + @"\Resources\SobelX.bmp");
            Picture.Filter(picture, "SobelX");
            Assert.AreEqual(1, Picture.PictureComparsion(picture, correctPicture));
        }

        [TestMethod]
        public void SobelYFilterValidation()
        {
            picture.Read((Directory.GetCurrentDirectory() + @"\.." + @"\.." + @"\..") + @"\Resources\fileIn.bmp");
            correctPicture.Read((Directory.GetCurrentDirectory() + @"\.." + @"\.." + @"\..") + @"\Resources\SobelY.bmp");
            Picture.Filter(picture, "SobelY");
            Assert.AreEqual(1, Picture.PictureComparsion(picture, correctPicture));
        }
    }
}
