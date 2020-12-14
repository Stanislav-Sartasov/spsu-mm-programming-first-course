using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace EighthTask.MathCurves
{
	public abstract class Curve
	{
		public string CurveName { get; protected set; }
		public override string ToString()
		{
			return CurveName;
		}

		public List<PointF> Points { get; protected set; } = new List<PointF>();
		public void SetPoints(float size)
		{
			int absMax = 35;
			Points.Clear();
			for (float x = -absMax / size; x <= absMax / size; x += 0.01f)
			{
				Points.Add(new PointF(x, Function(x, 0)));
			}
			for (float x = -absMax / size; x <= absMax / size; x += 0.01f)
			{
				Points.Add(new PointF(x, Function(x, 1)));
			}
		}

		internal abstract float Function(float arg, int prm);
	}
}