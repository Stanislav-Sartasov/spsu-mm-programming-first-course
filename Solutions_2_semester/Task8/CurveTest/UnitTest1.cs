using NUnit.Framework;
using MyMath;
using System;

namespace CurveTest
{
	public class Tests
	{
		[Test]
		public void EllipseTest()
		{
			try
			{
				foreach (double[] inp in new double[][] { new double[] { 1, 1 }, new double[] { 4, 9 } })
				{
					var points = new Ellipse().Build(0.1, 0, inp[0], inp[1]);
					foreach (var curve in points)
						foreach (var point in curve)
							Assert.AreEqual(1, Math.Round(Math.Pow(point[0], 2) / Math.Pow(inp[0], 2) + Math.Pow(point[1], 2) / Math.Pow(inp[1], 2)), 3);
				}
			}
			catch
			{
				Assert.Fail();
			}
		}

		[Test]
		public void HyperbolTest()
		{
			try
			{
				foreach (double[] inp in new double[][] { new double[] { 1, 1 }, new double[] { 4, 9 } })
				{
					var points = new Hyperbola().Build(0.1, 10, inp[0], inp[1]);
					foreach (var curve in points)
						foreach (var point in curve)
							Assert.AreEqual(1, Math.Round(Math.Pow(point[0], 2) / Math.Pow(inp[0], 2) - Math.Pow(point[1], 2) / Math.Pow(inp[1], 2), 3));
				}
			}
			catch
			{
				Assert.Fail();
			}
		}

		[Test]
		public void ParabolaTest()
		{
			try
			{
				foreach (double[] inp in new double[][] { new double[] { 1, 1 }, new double[] { 4, 9 } })
				{
					var points = new Parabola().Build(0.1, 10, inp[0], inp[1]);
					foreach (var curve in points)
						foreach (var point in curve)
							Assert.AreEqual(Math.Round(2 * inp[0] * point[0], 3), Math.Round(Math.Pow(point[1], 2), 3));
				}
			}
			catch
			{
				Assert.Fail();
			}
		}
	}
}