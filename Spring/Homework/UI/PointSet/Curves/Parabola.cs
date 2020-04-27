using System;
using System.Collections.Generic;
using System.Drawing;

namespace DrawingCurves
{
    public class Parabola : PointSet
    {
        // y^2 = 2 * p * x
        public float P { get; }

        public double ClientWidth { get; }


        public Parabola(int pixelsPerSegment, double clientWidth, float p)
            : base(pixelsPerSegment)
        {
            P = p;
            ClientWidth = clientWidth;
            Points = new List<PointF[]>();
            MakePoints();
        }

        public PointF GetPoint(float x) => new PointF(x, -(float)(Math.Sqrt(2 * P * x)));

        public override void MakePoints()
        {
            var pointSet = new List<PointF>();
            var negPointSetSet = new List<PointF>();
            float x = 0;
            while (x < ClientWidth / (2 * PixelsPerSegment))
            {
                var point = GetPoint(x);
                pointSet.Add(point);
                point.Y *= -1;
                negPointSetSet.Add(point);
                x += 1f / pointsPerSegment;
            }
            Points.Add(pointSet.ToArray());
            Points.Add(negPointSetSet.ToArray());
        }

        public override string ToString()
        {
            return $"y^2=2*{P}*x";
        }
    }
}
