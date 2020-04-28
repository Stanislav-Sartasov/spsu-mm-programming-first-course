using System;
using System.Collections.Generic;
using System.Drawing;

namespace DrawingCurves
{

    public abstract class PointSet
    {
        public string Name { get; }
        public List<PointF[]> Points { get; protected set; }
        private int pixelsPerSegment;
        public int PixelsPerSegment
        {
            get
            {
                return pixelsPerSegment;
            }
            protected set
            {
                if (value > 0)
                    pixelsPerSegment = value;
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
