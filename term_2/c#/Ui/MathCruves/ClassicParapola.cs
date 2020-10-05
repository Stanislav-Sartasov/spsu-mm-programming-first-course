using System.Drawing;
using System.Collections.Generic;

namespace MathCruves
{
    public class ClassicParabola : Cruve
    {
        public override List<PointF> Raschet(List<float> lst, out bool error)
        {
            error = false;
            List<PointF> lstF = new List<PointF>();
            foreach (float f in lst)
            {
                lstF.Add(new PointF(f, f * f));
            }
            return lstF;
        }
        public ClassicParabola()
        {
            cruve = "x * x";
        }
    }
}
