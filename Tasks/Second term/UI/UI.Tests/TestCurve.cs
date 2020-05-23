using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using UI.MathCurve;

namespace UI.Tests
{
    [TestClass]
    public class TestCurve
    {
        [TestMethod]
        public void TestEllipse()
        {
            MyEllipse myEllipse = new MyEllipse();
            myEllipse.A = 100;
            myEllipse.B = 100;
            var points = myEllipse.FindValues(1, 0, 0, 100);
            foreach(var partOfEllipse in points)
                foreach(var point in partOfEllipse)
                {
                    Assert.AreEqual(Math.Abs(point[1]), myEllipse.B * Math.Sqrt(1 - Math.Pow(point[0], 2) / Math.Pow(myEllipse.A, 2))); // y = f(x)
                }
            Assert.AreEqual(1, points.Count); // part of shape
            Assert.AreEqual(400, points[0].Count); // amount of points
        }

        [TestMethod]
        public void TestHyperbola()
        {
            MyHyperbola myHyperbola = new MyHyperbola();
            myHyperbola.A = 1;
            myHyperbola.B = 1;
            var points = myHyperbola.FindValues(1, 200, 0, 100);
            foreach (var partOfEllipse in points)
                foreach (var point in partOfEllipse)
                {
                    Assert.AreEqual(Math.Abs(point[1]), myHyperbola.B * Math.Sqrt(Math.Pow(point[0] - 200, 2) / Math.Pow(myHyperbola.A, 2) - 1)); // y = f(x)
                }
            Assert.AreEqual(2, points.Count); // part of shape
            Assert.AreEqual(400, points[0].Count + points[1].Count); // amount of points
        }

        [TestMethod]
        public void TestParabola()
        {
            MyParabola myParabola = new MyParabola();
            myParabola.P = 1;
            var points = myParabola.FindValues(1, 400, 0, 100);
            foreach (var partOfEllipse in points)
                foreach (var point in partOfEllipse)
                {
                    Assert.AreEqual(Math.Abs(point[1]), Math.Sqrt(2 * myParabola.P * (point[0] - 400))); // y = f(x)
                }
            Assert.AreEqual(1, points.Count); // part of shape
            Assert.AreEqual(402, points[0].Count); // amount of points, 2 points extreme, x = offsetX / 2
        }
    }
}
