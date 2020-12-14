using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EighthTask.MathCurves;

namespace EighthTask.Tests
{
	[TestClass]
	public class UITests // Points' lists initialization tests
	{
		[TestMethod]
		public void Circle()
		{
			var circle = new Ellipse(1, 1, 1);

			Testing(circle);
		}

		[TestMethod]
		public void Ellipse()
		{
			var ellipse = new Ellipse(1, 2, 1);

			Testing(ellipse);
		}

		[TestMethod]
		public void Parabola()
		{

			var circle = new Parabola(2, 5);

			Testing(circle);
		}

		[TestMethod]
		public void Hyperbola()
		{

			var circle = new Hyperbola(3, 2, 1);

			Testing(circle);
		}

		private void Testing(Curve curve)
		{
			curve.SetPoints(1.0f);
			foreach (var point in curve.Points)
			{
				Assert.IsNotNull(point);
			}
		}
	}
}
