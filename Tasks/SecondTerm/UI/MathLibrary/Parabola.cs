using System;
using System.Drawing;
using System.Collections.Generic;

namespace MathLibrary
{
    public class Parabola : AbstractCurve
    {
        public Parabola(float p)
        {
            Name = "Parabola";
            A = 1;
            B = 1;
            P = p;
        }

        public override PointF GetPoint(double x)
        {
            return new PointF((float)x, (float)-Math.Sqrt(2 * P * x));
        }

        public override List<PointF> GetPoints(double height, double width, double pixelsPerUnit)
        {
            List<PointF> positivePoints = new List<PointF>();
            List<PointF> negativePoints = new List<PointF>();

            for (double x = 0; x < width / 2 * pixelsPerUnit; x += 3 / pixelsPerUnit)
            {
                PointF pointF = GetPoint(x);
                pointF.X *= (float)pixelsPerUnit;
                pointF.Y *= (float)pixelsPerUnit;
                positivePoints.Add(pointF);
                pointF.Y = -pointF.Y;
                negativePoints.Add(pointF);
            }

            positivePoints.Reverse();
            positivePoints.AddRange(negativePoints);
            return positivePoints;
        }
    }
}
