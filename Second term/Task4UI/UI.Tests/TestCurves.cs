using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathLibrary;
using System.Collections.Generic;
using System;

namespace UI.Tests
{
    [TestClass]
    public class TestCurves
    {
        [TestMethod]
        public void TestEllipse()
        {
            Ellipse ellipse = new Ellipse(3, 2);
            double a = ellipse.GetXCoefficients();
            double b = ellipse.GetYCoefficients();
            for (double x = -a; x < a; x += 0.01)
            {
                List<double[]> solutions = ellipse.GetSolution(x);
                foreach (double[] solution in solutions)
                    Assert.AreEqual(Math.Abs(solution[1]), Math.Sqrt(b - solution[0] * solution[0] * b / a));
            }
        }
        [TestMethod]
        public void TestHyperbola()
        {
            Hyperbola hyperbola = new Hyperbola(1, 2);
            double a = hyperbola.GetXCoefficients();
            double b = hyperbola.GetYCoefficients();
            for (double x = -100; x < 100; x += 0.1)
            {
                List<double[]> solutions = hyperbola.GetSolution(x);
                foreach (double[] solution in solutions)
                    Assert.AreEqual(Math.Abs(solution[1]), Math.Sqrt(solution[0] * solution[0] * b / a - b));
            }
        }
        [TestMethod]
        public void TestParabola()
        {
            Parabola parabola = new Parabola(2);
            double p = parabola.GetXCoefficients();
            for (double x = -100; x < 100; x += 0.1)
            {
                List<double[]> solutions = parabola.GetSolution(x);
                foreach (double[] solution in solutions)
                    Assert.AreEqual(Math.Abs(solution[1]), Math.Sqrt(solution[0] * p));
            }
        }
    }
}
