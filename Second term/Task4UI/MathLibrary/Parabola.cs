using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary
{
    public class Parabola : AbstractCurve
    {
        // y^2 = px
        // y = +-sqrt(px)


        private double yCoefficient;
        private double p;
        public Parabola(double p)
        {
            Name = "Parabola";
            this.p = p;
            yCoefficient = 1;
        }

        
        public override List<double[]> GetPointsToDraw(double pixelsPerUnit, int width, int height)
        {
            List<double[]> points = new List<double[]>();
            List<double[]> positivePointsF = new List<double[]>();
            List<double[]> negativePointsF = new List<double[]>();
            for (double x = 0; x < width / pixelsPerUnit; x += 1 / pixelsPerUnit)
            {
                points = GetSolution(x);
                foreach (double[] point in points)
                {
                    point[0] = width / 2 + point[0] * pixelsPerUnit;
                    
                    if (point[1] > 0)
                    {
                        point[1] = height / 2 + point[1] * pixelsPerUnit;
                        positivePointsF.Add(point);
                    }
                    else
                    {
                        point[1] = height / 2 + point[1] * pixelsPerUnit;
                        negativePointsF.Add(point);
                    }
                    
                }
            }
            positivePointsF.Reverse();
            positivePointsF.AddRange(negativePointsF);
            return positivePointsF;
        }
        public override List<double[]> GetSolution(double x)
        {
            List<double[]> solution = new List<double[]>();
            if (x > 0)
            {
                double y = (double)Math.Sqrt(x * p);
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
            }
            return solution;
        }

        public override double GetXCoefficients()
        {
            return p;
        }
        public override double GetYCoefficients()
        {
            return yCoefficient;
        }

    }
}
