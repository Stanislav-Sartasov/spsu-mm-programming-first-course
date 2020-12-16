using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace EighthTask.MathCurves
{
	public class Hyperbola : Curve
        {
		public float A { get; private set; }
		public float B { get; private set; }
		public float R { get; private set; }

		internal override float Function(float arg, int prm)
		{
			double fun = B * B * (arg * arg / (A * A) - R);

			if (fun >= 0)
			{
				if (prm == 0)
				{
					fun = Math.Sqrt(fun);
				}
				else
				{
					fun = -Math.Sqrt(fun);
				}

				return (float)Math.Round(fun, 3);
			}
			else
			{
				return 0;
			}
		}
		public Hyperbola(float a, float b, float r)
		{
			CurveName = "Hyperbola";
			A = a;
			B = b;
			R = r;
		}
	}
}