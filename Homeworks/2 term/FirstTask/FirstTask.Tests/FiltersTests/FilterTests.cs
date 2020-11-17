using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using FirstTask;
using FirstTask.FiltersDescription;
using FirstTask.ImageDescription;

namespace FirstTask.Tests
{
	[TestClass]
	public class FilterTests
	{
		private string path = String.Concat(AppDomain.CurrentDomain.BaseDirectory, @"\..", @"\..");
		private BitMapFile expectedPicture = new BitMapFile();
		private BitMapFile realPicture = new BitMapFile();

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
			path = String.Concat(path, @"\Samples\");

			realPicture.FileRead(String.Concat(path, "test.bmp"));
			Program.FilterSelect(realPicture, filter);
			//realPicture.FileWrite(String.Concat(path, "image_is_saved_correctly.bmp")); // The program is not crashing when recording a new image.

			expectedPicture.FileRead(String.Concat(path, "test_", filter, ".bmp"));

			Assert.AreEqual(realPicture.SizeOfImage, expectedPicture.SizeOfImage);
			long sizeOfImage = realPicture.SizeOfImage;
			for (long i = 0; i < sizeOfImage; i++)
			{
				Assert.AreEqual(expectedPicture.PixelsBytes[i], realPicture.PixelsBytes[i]);
			}
		}
	}
}
