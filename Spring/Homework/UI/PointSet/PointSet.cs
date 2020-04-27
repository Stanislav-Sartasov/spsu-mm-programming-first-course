using System;
using System.Collections.Generic;
using System.Drawing;

namespace DrawingCurves
{

    public abstract class PointSet
    {
        //(x/a)^2 + (y/b)^2 = 1"
        public string Name { get; }
        public List<PointF[]> Points { get; protected set; }
        protected int pointsPerSegment;
        public int PixelsPerSegment
        {
            get
            {
                return pointsPerSegment;
            }
            protected set
            {
                if (value > 0)
                    pointsPerSegment = value;
                else
                    throw new ArgumentException();
            }
        }

        public List<PointF[]> GetPoints() => Points;

        public PointSet(int pixelsPerSegment)
        {
            PixelsPerSegment = pixelsPerSegment;
        }

        public abstract void MakePoints();
    }
}
