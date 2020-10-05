using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MathCruves
{
    public abstract class Cruve
    {
        public string cruve;
        public virtual List<PointF> Raschet(List<float> lst, out bool error)
        {
            error = false;
            return new List<PointF>();
        }
        public override string ToString()
        {
            return cruve;
        }
    }
}
