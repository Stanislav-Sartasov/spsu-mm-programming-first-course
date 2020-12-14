using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace EighthTask.MathCurves
{
	public class Hyperbola : Curve
        {
		private float A { get; set; }
		private float B { get; set; }
		private float R { get; set; }

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