using System;
using System.Drawing;
using System.Collections.Generic;

namespace MathLibrary
{
    public abstract class AbstractCurve
    {
        public string Name { get; set;  }
        public float A { get; set; }
        public float B { get; set; }
        public float P { get; set; }
        public abstract PointF GetPoint(double x);
        public abstract List<PointF> GetPoints(double height, double width, double pixelsPerUnit);
    }
}
