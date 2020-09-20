using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MathLibrary
{
    public class Hyperbola : AbstractCurve
    {
        // x^2/a^2 - y^2/b^2 = 1
        // y = +- sqrt(-b^2 + x^2 * b^2 / a^2)
        private double a;
        private double b;

        public Hyperbola(double a, double b)
        {
            Name = "Hyperbola";
            this.a = a * a;
            this.b = b * b;
        }
        public override List<double[]> GetPointsToDraw(double pixelsPerUnit, int width, int height)
        {
            List<double[]> points = new List<double[]>();
            List<double[]> positivePointsF = new List<double[]>();
            List<double[]> negativePointsF = new List<double[]>();
            
            for (double x = width / 2; x > 0; x -= 1 / pixelsPerUnit)
            {
                points = GetSolution(x);
                foreach (double[] point in points)
                {
                    point[0] = width / 2 - point[0] * pixelsPerUnit;
                    if (point[1] > 0)
                    {
                        point[1] = height / 2 - point[1] * pixelsPerUnit;
                        positivePointsF.Add(point);
                    }
                    else
                    {
                        point[1] = height / 2 - point[1] * pixelsPerUnit;
                        negativePointsF.Add(point);
                    }

                }
            }
            negativePointsF.Reverse();
            positivePointsF.AddRange(negativePointsF);
            return positivePointsF;
        }

        public override List<double[]> GetSolution(double x)
        {
            List<double[]> solution = new List<double[]>();
            double y = (double)Math.Sqrt((x * x) * b / a - b);
            if (y == 0)
            {
                double[] point = new double[] { x, y };
                solution.Add(point);
            }
            else if (!double.IsNaN(y))
            {
                solution.Add(new double[] { x, y });
                solution.Add(new double[] { x, -y });
            }
            return solution;
        }

        public override double GetXCoefficients()
        {
            return a;
        }

        public override double GetYCoefficients()
        {
            return b;
        }
    }
}
