using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FirstTask;
using FirstTask.FiltersDescription;
using FirstTask.ImageDescription;

namespace FirstTask.Tests
{
	[TestClass]
	public class FilterTests
	{
		private string Path { get; set; } = String.Concat(AppDomain.CurrentDomain.BaseDirectory, @"\..", @"\..");
		private BitMapFile ExpectedPicture { get; set; } = new BitMapFile();
		private BitMapFile RealPicture { get; set; } = new BitMapFile();

		[TestMethod]
		public void Gauss3Test()
		{
			FilterTestImplementation("gauss3");
		}

		[TestMethod]
		public void Gauss5Test()
		{
			FilterTestImplementation("gauss5");
		}

		[TestMethod]
		public void MedianTest()
		{
			FilterTestImplementation("median");
		}

		[TestMethod]
		public void GreyTest()
		{
			FilterTestImplementation("grey");
		}

		[TestMethod]
		public void SobelXTest()
		{
			FilterTestImplementation("sobelX");
		}

		[TestMethod]
		public void SobelYTest()
		{
			FilterTestImplementation("sobelY");
		}

		public void FilterTestImplementation(string filter)
		{
			Path = String.Concat(Path, @"\Samples\");

			RealPicture.FileRead(String.Concat(Path, "test.bmp"));
			Program.FilterSelect(RealPicture, filter);
			//realPicture.FileWrite(String.Concat(Path, "image_is_saved_correctly.bmp")); // The program is not crashing when recording a new image

			ExpectedPicture.FileRead(String.Concat(Path, "test_", filter, ".bmp"));

			Assert.AreEqual(RealPicture.SizeOfImage, ExpectedPicture.SizeOfImage);
			long sizeOfImage = RealPicture.SizeOfImage;
			for (long i = 0; i < sizeOfImage; i++)
			{
				Assert.AreEqual(ExpectedPicture.PixelsBytes[i], RealPicture.PixelsBytes[i]);
			}
		}
	}
}
