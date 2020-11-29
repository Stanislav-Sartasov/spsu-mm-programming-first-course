using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathLibrary;
using System.Drawing;
using System.Collections.Generic;
using System;

namespace UI.Test
{
    [TestClass]
    public class TestMathLibrary
    {
        float eps = 0.001f;
        [TestMethod]
        public void TestEllipse()
        {
            Ellipse ellipse = new Ellipse(10, 10);
            List<PointF> points = ellipse.GetPoints(600, 800, 1);
            foreach (PointF point in points)
                    Assert.IsTrue(Math.Abs(Math.Abs(point.Y) - ellipse.B * (float)Math.Sqrt(1 - point.X * point.X / ellipse.A / ellipse.A)) < eps);
        }

        [TestMethod]
        public void TestHyperbola()
        {
            Hyperbola hyperbola = new Hyperbola(20, 10);
            List<PointF> points = hyperbola.GetPoints(600, 800, 1);
            foreach (PointF point in points)
                Assert.IsTrue(Math.Abs(Math.Abs(point.X) - Math.Abs(hyperbola.B) * (float)Math.Sqrt(1 + point.Y * point.Y / hyperbola.A / hyperbola.A)) < eps);
        }

        [TestMethod]
        public void TestParabola()
        {
            Parabola parabola = new Parabola(50);
            List<PointF> points = parabola.GetPoints(600, 800, 1);
            foreach (PointF point in points)
                Assert.IsTrue(Math.Abs(Math.Abs(point.Y) - (float)Math.Sqrt(2 * parabola.P * point.X)) < eps);
        }
    }
}
