using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace TaskFilters.Tests
{
    [TestClass]
    public class UnitTest
    {
        private static Bitmap input = TaskFilters.Tests.Resources.input;
        private static Bitmap gauss3x3 = TaskFilters.Tests.Resources.Gauss3;
        private static Bitmap gauss5x5 = TaskFilters.Tests.Resources.Gauss5;
        private static Bitmap gray = TaskFilters.Tests.Resources.Grayscale;
        private static Bitmap sobelx = TaskFilters.Tests.Resources.SobelX;
        private static Bitmap sobely = TaskFilters.Tests.Resources.SobelY;
        private static Bitmap average = TaskFilters.Tests.Resources.Averaging;

        private static byte[,,] expected = new byte[input.Height, input.Width, 3];

        private static byte[,,] TakeImage(Bitmap file)
        {
            for (int i = 0; i < file.Height; i++)
            {
                for (int j = 0; j < file.Width; j++)
                {
                    expected[i, j, 0] = file.GetPixel(j, i).R;
                    expected[i, j, 1] = file.GetPixel(j, i).G;
                    expected[i, j, 2] = file.GetPixel(j, i).B;
                }
            }
            return expected;
        }

        private static byte[,,] original;
        private static byte[,,] copy = TakeImage(input);
        
        private void Check(byte[,,] expected, byte[,,] actual)
        {
            for (int i = 0; i < expected.GetLength(0); i++)
            {
                for (int j = 0; j < expected.GetLength(1); j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        Assert.AreEqual(expected[i, j, k], actual[i, j, k]);
                    }
                }
            }
        }


        [TestMethod]
        public void TestMethod()
        {
            string[] types = { "Averaging", "Gauss3", "Gauss5", "SobelX", "SobelY", "Grayscale" };
            for (int i = 0; i < 6; i++)
            {
                original = copy;
                switch (types[i])
                {
                    case "Averaging":
                        TaskFilters.Filters.Convolution(original, (uint)input.Height, (uint)input.Width, types[i]);
                        TakeImage(average);
                        Check(expected, original);
                        break;
                    case "Gauss3":
                        TaskFilters.Filters.Convolution(original, (uint)input.Height, (uint)input.Width, types[i]);
                        TakeImage(gauss3x3);
                        Check(expected, original);
                        break;
                    case "Gauss5":
                        TaskFilters.Filters.Convolution(original, (uint)input.Height, (uint)input.Width, types[i]);
                        TakeImage(average);
                        Check(expected, original);
                        break;
                    case "SobelX":
                        TaskFilters.Filters.Convolution(original, (uint)input.Height, (uint)input.Width, types[i]);
                        TakeImage(sobelx);
                        Check(expected, original);
                        break;
                    case "SobelY":
                        TaskFilters.Filters.Convolution(original, (uint)input.Height, (uint)input.Width, types[i]);
                        TakeImage(sobely);
                        Check(expected, original);
                        break;
                    case "Grayscale":
                        TaskFilters.Filters.Convolution(original, (uint)input.Height, (uint)input.Width, types[i]);
                        TakeImage(gray);
                        Check(expected, original);
                        break;
                }
            }
        }
    }
}
