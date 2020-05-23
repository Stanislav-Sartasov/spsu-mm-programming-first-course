using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace UI.MathCurve
{
    public class MyParabola : Curve
    {
        public MyParabola()
        {
            Name = "Parabola";
        }

        public override List<List<double[]>> FindValues(double step, double offsetX, double offsetY, double scale)
        {
            P = P * scale / 100;
            List<List<double[]>> points = new List<List<double[]>>();
            points.Add(new List<double[]>());
            for (double x = offsetX / 2; x >= 0; x -= step)
            {
                double y = -Math.Sqrt(2 * P * x);
                points[0].Add(new double[] { x + offsetX, y + offsetY });
            }
            for (double x = 0; x <= offsetX / 2; x += step)
            {
                double y = Math.Sqrt(2 * P * x);
                points[0].Add(new double[] { x + offsetX, y + offsetY });
            }

            return points;
        }
    }
}
