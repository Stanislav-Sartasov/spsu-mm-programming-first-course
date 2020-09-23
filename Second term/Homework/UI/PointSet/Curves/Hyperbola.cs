using System;
using System.Collections.Generic;
using System.Drawing;


namespace DrawingCurves
{
    public class Hyperbola : PointSet
    {
        public float A { get; }
        public float B { get; }
        public double ClientWidth { get; }

        public Hyperbola(int pixelsPerSegment, double clientWidth, float a, float b)
            : base(pixelsPerSegment)
        {
            if (a * b == 0)
                throw new DivideByZeroException();
            A = (float)Math.Abs(a);
            B = (float)Math.Abs(b);
            ClientWidth = clientWidth;
            Points = new List<PointF[]>();
            MakePoints();
        }

        public PointF GetPoint(float x) => new PointF(x, -(float)(Math.Sqrt(-1 + Math.Pow((double)x / A, 2)) * B));

        public override void MakePoints()
        {
            var firstPointSet = new List<PointF>();
            var secondPointSetSet = new List<PointF>();
            var thirdPointSetSet = new List<PointF>();
            var fourthPointSet = new List<PointF>();
            float x = A;
            while (x < ClientWidth / (2 * PixelsPerSegment))
            {
                var point = GetPoint(x);
                firstPointSet.Add(point);
                point.Y *= -1;
                secondPointSetSet.Add(point);
                point.X *= -1;
                thirdPointSetSet.Add(point);
                point.Y *= -1;
                fourthPointSet.Add(point);
                x += 1f / PixelsPerSegment;
            }
            Points.Add(firstPointSet.ToArray());
            Points.Add(secondPointSetSet.ToArray());
            Points.Add(thirdPointSetSet.ToArray());
            Points.Add(fourthPointSet.ToArray());
        }

        public override string ToString()
        {
            return $"(x/{A})^2 - (y/{B})^2 = 1";
        }
    }
}
