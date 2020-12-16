using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EighthTask.MathCurves;

namespace EighthTask.Tests
{
	[TestClass]
	public class UITests // Points' lists correctness tests (for 1.0f)
	{
		[TestMethod]
		public void Circle()
		{
			var circle = new Ellipse(1, 1, 1);

			Init(circle);

			foreach (var point in circle.Points)
			{
				//Assert.IsNotNull(point);
				float fun = circle.B * circle.B * (circle.R * circle.R - point.X * point.X / (circle.A * circle.A));
				if (fun < 0)
				{
					fun = 0;
				}
				else
				{
					fun = (float)Math.Round(Math.Sqrt(fun), 3);
				}

				Assert.AreEqual(fun, Math.Abs(point.Y));
			}
		}

		[TestMethod]
		public void Ellipse()
		{
			var ellipse = new Ellipse(1, 2, 1);

			Init(ellipse);

			foreach (var point in ellipse.Points)
			{
				//Assert.IsNotNull(point);
				float fun = ellipse.B * ellipse.B * (ellipse.R  - point.X * point.X / (ellipse.A * ellipse.A));
				if (fun < 0)
				{
					fun = 0;
				}
				else
				{
					fun = (float)Math.Round(Math.Sqrt(fun), 3);
				}

				Assert.AreEqual(fun, Math.Abs(point.Y));
			}
		}

		[TestMethod]
		public void Parabola()
		{

			var parabola = new Parabola(2, 5);

			Init(parabola);

			foreach (var point in parabola.Points)
			{
				//Assert.IsNotNull(point);
				float fun = 2 * parabola.P * point.X + parabola.R;
				if (fun < 0)
				{
					fun = 0;
				}
				else
				{
					fun = (float)Math.Round(Math.Sqrt(fun), 3);
				}

				Assert.AreEqual(fun, Math.Abs(point.Y));
			}
		}

		[TestMethod]
		public void Hyperbola()
		{

			var hyperbola = new Hyperbola(3, 2, 1);

			Init(hyperbola);

			foreach (var point in hyperbola.Points)
			{
				//Assert.IsNotNull(point);
				float fun = hyperbola.B * hyperbola.B * (point.X * point.X / (hyperbola.A * hyperbola.A) - hyperbola.R);
				if (fun < 0)
				{
					fun = 0;
				}
				else
				{
					fun = (float)Math.Round(Math.Sqrt(fun), 3);
				}

				Assert.AreEqual(fun, Math.Abs(point.Y));
			}
		}

		private void Init(Curve curve)
		{
			int abs = 35;
			float size = 1.0f;

			curve.SetPoints(size);
			Assert.AreEqual(abs * 2 * (2 / 1.0f) * 100, curve.Points.Count); // Count of points
		}
	}
}
