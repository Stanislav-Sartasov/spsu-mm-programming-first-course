using System;
using System.Collections.Generic;
using System.Drawing;


namespace DrawingCurves
{
    public class Ellipse : PointSet
    {
        public float A { get; }
        public float B { get; }

        public Ellipse(int pixelsPerSegment, double a, double b)
            : base(pixelsPerSegment)
        {
            if (a * b == 0)
                throw new DivideByZeroException();
            A = (float)Math.Abs(a);
            B = (float)Math.Abs(b);
            Points = new List<PointF[]>();
            MakePoints();
        }

        public PointF GetPoint(float x) => new PointF(x, -(float)(Math.Sqrt(1 - Math.Pow((double)x / A, 2)) * B));

        public override void MakePoints()
        {
            var pointSet = new List<PointF>();
            var negPointSetSet = new List<PointF>();
            float x = -A;
            while (x < A)
            {
                var point = GetPoint(x);
                pointSet.Add(point);
                point.Y *= -1;
                negPointSetSet.Add(point);
                x += 1f / pointsPerSegment;
            }
            var endPoint = GetPoint(A);
            pointSet.Add(endPoint);
            endPoint.Y *= -1;
            negPointSetSet.Add(endPoint);

            Points.Add(pointSet.ToArray());
            Points.Add(negPointSetSet.ToArray());
        }

        public override string ToString()
        {
            return $"(x/{A})^2+(y/{B})^2=1";
        }
    }
}
