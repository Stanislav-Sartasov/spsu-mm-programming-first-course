using DrawingCurves;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PointSet.Tests
{
    [TestClass]
    public class HyperbolaTests
    {
        Hyperbola hyperbola;

        [TestMethod]
        public void CorrectPointsNumber()
        {
            for (int width = 400; width < 1600; width++)
            {
                float a = 0.5f;
                while (a < width / (2 * 40))
                {
                    hyperbola = new Hyperbola(40, width, a, 2);
                    int pointsNumber = 0;
                    foreach (var points in hyperbola.GetPoints())
                        pointsNumber += points.Length;
                    Assert.AreEqual(2 * width - 4 * a * 40, pointsNumber, 6);
                    a += 0.25f;
                }
            }
        }

        [TestMethod]
        public void CorrectEquation()
        {
            for (int pixesPerSegment = 10; pixesPerSegment < 50; pixesPerSegment++)
            {
                float a = 0.5f;
                while (a < 10)
                {
                    hyperbola = new Hyperbola(pixesPerSegment, 400, a, a / 2);
                    foreach (var points in hyperbola.GetPoints())
                        foreach (var pnt in points)
                            Assert.AreEqual(pnt.Y, Math.Sign(pnt.Y) * (float)(Math.Sqrt(-1 + Math.Pow((double)pnt.X / a, 2)) * a / 2));
                    a += 0.25f;
                }
            }
        }
    }
}