using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary
{
    public abstract class AbstractCurve
    {
        public string Name { get; set; }
        public abstract double GetXCoefficients();
        public abstract double GetYCoefficients();

        public abstract List<double[]> GetSolution(double x);
        public abstract List<double[]> GetPointsToDraw(double pixelsPerUnit, int width, int height);
    }
}