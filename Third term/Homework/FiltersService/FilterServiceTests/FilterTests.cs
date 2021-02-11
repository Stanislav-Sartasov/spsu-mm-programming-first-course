using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;


namespace FilterServiceTests
{
    [TestClass]
    public class FilterTests
    {
        static Process host;
        static readonly string path = Path.Combine(Path.GetFullPath(@"..\..\..\"),
            @"FilterServiceHost\bin\Debug\FilterServiceHost.exe");

        private bool PixelCompare(Bitmap exp, Bitmap act)
        {
            for (int height = 0; height < exp.Height; height++)
            {
                for (int width = 0; width < exp.Width; width++)
                {
                    var expColor = exp.GetPixel(width, height);
                    var actColor = act.GetPixel(width, height);
                    if (!expColor.Equals(actColor))
                        return false;
                }
            }
            return true;
        }

        [ClassInitialize]
        public static void Init(TestContext _)
        {
            host = new Process();
            host.StartInfo.FileName = path;
            host.Start();
        }


        [TestMethod]
        public void CorrectAverage()
        {
            var mock = new MockClient(TestImages.input, "Average");
            var res = mock.GetResult();
            Assert.IsFalse(res is null);
            Assert.IsTrue(PixelCompare(TestImages.expAverage, res));
        }

        [TestMethod]
        public void CorrectGauss3x3()
        {
            var mock = new MockClient(TestImages.input, "Gauss3x3");
            var res = mock.GetResult();
            Assert.IsFalse(res is null);
            Assert.IsTrue(PixelCompare(TestImages.expGauss3x3, res));
        }

        [TestMethod]
        public void CorrectGauss5x5()
        {
            var mock = new MockClient(TestImages.input, "Gauss5x5");
            var res = mock.GetResult();
            Assert.IsFalse(res is null);
            Assert.IsTrue(PixelCompare(TestImages.expGauss5x5, res));
        }

        [TestMethod]
        public void CorrectSobelX()
        {
            var mock = new MockClient(TestImages.input, "SobelX");
            var res = mock.GetResult();
            Assert.IsFalse(res is null);
            Assert.IsTrue(PixelCompare(TestImages.expSobelX, res));
        }

        [TestMethod]
        public void CorrectSobelY()
        {
            var mock = new MockClient(TestImages.input, "SobelY");
            var res = mock.GetResult();
            Assert.IsFalse(res is null);
            Assert.IsTrue(PixelCompare(TestImages.expSobelY, res));
        }

        [TestMethod]
        public void CorrectBW()
        {
            var mock = new MockClient(TestImages.input, "BlackAndWhite");
            var res = mock.GetResult();
            Assert.IsFalse(res is null);
            Assert.IsTrue(PixelCompare(TestImages.expBlackAndWhite, res));
        }

        [ClassCleanup]
        public static void Cleanup() => host.Kill();
    }
}
