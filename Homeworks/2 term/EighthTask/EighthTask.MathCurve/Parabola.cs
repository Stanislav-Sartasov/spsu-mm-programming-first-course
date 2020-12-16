using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace EighthTask.MathCurves
{
	public class Parabola : Curve
	{
		public float P { get; private set; }
		public float R { get; private set; }

		internal override float Function(float arg, int prm)
		{
			double fun = 2 * P * arg + R;

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
		public Parabola(float p, float r)
		{
			CurveName = "Parabola";
			P = p;
			R = r;
		}
	}
}