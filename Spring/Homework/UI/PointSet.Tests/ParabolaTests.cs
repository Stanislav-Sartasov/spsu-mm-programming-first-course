using DrawingCurves;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PointSet.Tests
{
    [TestClass]
    public class ParabolaTests
    {
        Parabola parabola;

        [TestMethod]
        public void CorrectPointsNumber()
        {
            for (int pixesPerSegment = 10; pixesPerSegment < 50; pixesPerSegment++)
                for (int width = 400; width < 1600; width++)
                {
                    parabola = new Parabola(pixesPerSegment, width, 5);
                    int pointsNumber = 0;
                    foreach (var points in parabola.GetPoints())
                        pointsNumber += points.Length;
                    Assert.AreEqual(width, pointsNumber, 2);
                }
        }

        [TestMethod]
        public void CorrectEquation()
        {
            for (int pixesPerSegment = 10; pixesPerSegment < 50; pixesPerSegment++)
            {
                float p = 0.5f;
                while (p < 10)
                {
                    parabola = new Parabola(pixesPerSegment, 1600, p);
                    foreach (var points in parabola.GetPoints())
                        foreach (var pnt in points)
                            Assert.AreEqual(pnt.Y, Math.Sign(pnt.Y) * (float)(Math.Sqrt(2 * p * pnt.X)));
                    p += 0.25f;
                }
            }
        }
    }
}
