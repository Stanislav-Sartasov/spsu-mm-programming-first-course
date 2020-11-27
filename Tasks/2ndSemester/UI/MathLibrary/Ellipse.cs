using System;
using System.Drawing;
using System.Collections.Generic;

namespace MathLibrary
{
    public class Ellipse : AbstractCurve
    {
        public Ellipse(float a, float b)
        {
            Name = "Ellipse";
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
            return new PointF((float)x, -Math.Abs(B) * (float)Math.Sqrt(1 - x * x / A / A));
        }

        public override List<PointF> GetPoints(double height, double width, double pixelsPerUnit)
        {
            List<PointF> positivePoints = new List<PointF>();
            List<PointF> negativePoints = new List<PointF>();

            for (double x = -Math.Abs(A); x <= Math.Abs(A); x += 1 / pixelsPerUnit / 4)
            {
                PointF pointF = GetPoint(x);
                pointF.X *= (float)pixelsPerUnit;
                pointF.Y *= (float)pixelsPerUnit;
                positivePoints.Add(pointF);
                pointF.Y = -pointF.Y;
                negativePoints.Add(pointF);
            }

            positivePoints.Add(new PointF(Math.Abs(A) * (float)pixelsPerUnit, 0));
            negativePoints.Add(new PointF(Math.Abs(A) * (float)pixelsPerUnit, 0));

            negativePoints.Reverse();
            positivePoints.AddRange(negativePoints);
            return positivePoints;
        }
    }
}
