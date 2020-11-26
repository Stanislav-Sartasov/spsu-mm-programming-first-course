using System;
using System.Drawing;
using System.Collections.Generic;

namespace MathLibrary
{
    public class Hyperbola : AbstractCurve
    {
        public Hyperbola(float a, float b)
        {
            Name = "Hyperbola";
            A = a;
            B = b;
            P = 1;
            if (a * b == 0)
            {
                A = 1;
                B = 1;
            }
        }

        public override PointF GetPoint(double x)
        {
            return new PointF(-B * (float)Math.Sqrt(1 + x * x / A / A), (float)x);
        }

        public override List<PointF> GetPoints(double height, double width, double pixelsPerUnit)
        {
            List<PointF> positivePoints = new List<PointF>();
            List<PointF> negativePoints = new List<PointF>();

            for (double x = -2 * A; x <= 2 * A; x += 1 / pixelsPerUnit)
            {
                PointF pointF = GetPoint(x);
                pointF.X *= (float)pixelsPerUnit;
                pointF.Y *= (float)pixelsPerUnit;
                positivePoints.Add(pointF);
                pointF.X = -pointF.X;
                negativePoints.Add(pointF);
            }

            negativePoints.Reverse();
            positivePoints.AddRange(negativePoints);
            return positivePoints;
        }
    }
}
