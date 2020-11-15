using System.Drawing;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

namespace MathCruves
{
    public class Circle : Curve
    {
        public override List<PointF> Raschet(List<float> lst, out bool error)
        {
            float r = 1, a = 0, b = 0;

            error = false;
            List<PointF> lstF = new List<PointF>();
            float res;
            bool flagEnd = false;
            bool flagStart = true;
            foreach (float f in lst)
            {
                res = r - f * f;
                if (res >= 0)
                {
                    lstF.Add(new PointF(f, (float)Math.Sqrt(res)));
                    flagEnd = true;
                    flagStart = false;
                }
                else
                {
                    if (flagEnd)
                    {
                        flagEnd = false;
                        lstF.Add(new PointF(r, (float)(b)));
                    }
                    if (flagStart)
                    {
                        flagStart = false;
                        lstF.Add(new PointF(a - r, (float)(b)));
                    }
                }
            }
            for (int i = lst.Count - 1; i >= 0; --i)
            {
                float f = lst[i];
                res = r - f * f;
                if (res >= 0)
                    lstF.Add(new PointF(f, (float)(-Math.Sqrt(res))));
            }
            lstF.Add(lstF[0]);
            return lstF;
        }
        public Circle()
        {
            curve = "x * x + y * y = 1";
        }
    }
}
