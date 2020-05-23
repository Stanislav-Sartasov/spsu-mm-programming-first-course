using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace UI.MathCurve
{
    public class MyHyperbola : Curve
    {
        public MyHyperbola()
        {
            Name = "Hyperbola";
        }
            

        public override List<List<double[]>> FindValues(double step, double offsetX, double offsetY, double scale)
        {
            A = A * scale / 100;
            B = B * scale / 100;
            List<List<double[]>> points = new List<List<double[]>>();
            double x = -offsetX / 2;
            points.Add(new List<double[]>());
            while ((Math.Pow(x, 2) / Math.Pow(A, 2) - 1) >= 0)
            {
                double y = B * Math.Sqrt(Math.Pow(x, 2) / Math.Pow(A, 2) - 1);
                points[0].Add(new double[] { x + offsetX, y + offsetY });
                x += step;
            }
            x -= step;
            while (x >= -offsetX / 2)
            {
                double y = B * Math.Sqrt(Math.Pow(x, 2) / Math.Pow(A, 2) - 1);
                points[0].Add(new double[] { x + offsetX, -y + offsetY });
                x -= step;
            }
            x = offsetX / 2;
            points.Add(new List<double[]>());
            while ((Math.Pow(x, 2) / Math.Pow(A, 2) - 1) >= 0)
            {
                double y = B * Math.Sqrt(Math.Pow(x, 2) / Math.Pow(A, 2) - 1);
                points[1].Add(new double[] { x + offsetX, y + offsetY });
                x -= step;
            }
            x += step;
            while (x <= offsetX / 2)
            {
                double y = B * Math.Sqrt(Math.Pow(x, 2) / Math.Pow(A, 2) - 1);
                points[1].Add(new double[] { x + offsetX, -y + offsetY });
                x += step;
            }

            return points;
        }
    }
}
