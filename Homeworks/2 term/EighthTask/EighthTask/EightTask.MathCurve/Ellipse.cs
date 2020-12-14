using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace EighthTask.MathCurves
{
	public class Ellipse : Curve
	{
		private float A { get; set; }
		private float B { get; set; }
		private float R { get; set; }

		internal override float Function(float arg, int prm)
		{
			double fun = B * B * (R - arg * arg / (A * A));

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
		public Ellipse(float a, float b, float r)
		{
			if (a == 1 && b == 1)
			{
				CurveName = "Circle";
				A = B = a;
			}
			else
			{
				CurveName = "Ellipse";
				A = a;
				B = b;
			}
		
			R = r;
		}
	}
}