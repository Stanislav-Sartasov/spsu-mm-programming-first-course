using DrawingCurves;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PointSet.Tests
{
    [TestClass]
    public class EllipseTests
    {
        Ellipse ellipse;

        [TestMethod]
        public void CorrectPointsNumber()
        {
            for (int pixelsPerSegment = 10; pixelsPerSegment < 100; pixelsPerSegment++)
            {
                float a = 0.5f;
                float b = 10;
                while (a < 10)
                {
                    ellipse = new Ellipse(pixelsPerSegment, a, b);
                    int pointsNumber = 0;
                    foreach (var points in ellipse.GetPoints())
                        pointsNumber += points.Length;
                    Assert.AreEqual(4 * a * pixelsPerSegment, pointsNumber, 6);
                    a += 0.25f;
                    b -= 0.25f;
                }
            }
        }

        [TestMethod]
        public void CorrectEquation()
        {
            for (int pixesPerSegment = 10; pixesPerSegment < 50; pixesPerSegment++)
            {
                float a = 0.5f;
                float b = 10;
                while (a < 10)
                {
                    ellipse = new Ellipse(pixesPerSegment, a, b);
                    foreach (var points in ellipse.GetPoints())
                        foreach (var pnt in points)
                            Assert.AreEqual(pnt.Y, Math.Sign(pnt.Y) * (float)(Math.Sqrt(1 - Math.Pow((double)pnt.X / a, 2)) * b));
                    a += 0.25f;
                    b -= 0.25f;
                }
            }
        }
    }
}