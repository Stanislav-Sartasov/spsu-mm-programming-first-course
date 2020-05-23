using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Numerics;

namespace UI.MathCurve
{
    public class MyEllipse : Curve
    {
        public MyEllipse()
        {
            Name = "Ellipse";
        }

        public override List<List<double[]>> FindValues(double step, double offsetX, double offsetY, double scale)
        {
            A = A * scale / 100;
            B = B * scale / 100;

            List<List<double[]>> points = new List<List<double[]>>();
            double x;
            points.Add(new List<double[]>());
            for (x = 0; x < A; x += step)
            {
                double y = B * Math.Sqrt(1 - Math.Pow(x, 2) / Math.Pow(A, 2));
                if (y > 0)
                    points[0].Add(new double[] { x + offsetX, y + offsetY });
            }
            for (; x >= 0; x -= step)
            {
                double y = B * Math.Sqrt(1 - Math.Pow(x, 2) / Math.Pow(A, 2));
                if (y > 0)
                    points[0].Add(new double[] { x + offsetX, -y + offsetY });
            }
            for (x = 0; x > -A; x -= step)
            {
                double y = B * Math.Sqrt(1 - Math.Pow(x, 2) / Math.Pow(A, 2));
                if (y > 0)
                    points[0].Add(new double[] { x + offsetX, -y + offsetY });
            }
            for (; x <= 0; x += step)
            {
                
                double y = B * Math.Sqrt(1 - Math.Pow(x, 2) / Math.Pow(A, 2));
                if (y > 0)
                    points[0].Add(new double[] { x + offsetX, y + offsetY });
            }


            return points;
        }
    }
}
