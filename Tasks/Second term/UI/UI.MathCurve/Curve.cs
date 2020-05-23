using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace UI.MathCurve
{
    public abstract class Curve
    {
        public string Name { get; protected set; }


        private double a = 1;
        private double b = 1;
        private double p = 1;

        public double A
        {
            get
            {
                return a;
            }
            set
            {
                if (value > 0)
                    a = value;
            }
        }
        public double B
        {
            get
            {
                return b;
            }
            set
            {
                if (value > 0)
                    b = value;
            }
        }
        public double P
        {
            get
            {
                return p;
            }
            set
            {
                if (value > 0)
                    p = value;
            }
        }
        public abstract List<List<double[]>> FindValues(double step, double offsetX, double offsetY, double scale);
    }
}
