using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMath
{
	public class Parabola : ICurveBuilder
	{
		public string Name
		{
			get
			{
				return "Parabola";
			}
		}
		public List<List<double[]>> Build(double step, double scale, double a, double _)
		{
			if (a <= 0)
				return null;

			List<List<double[]>> result = new List<List<double[]>>();

			for (int i = -1; i <= 1; i += 2)
			{
				List<double[]> newBlock = new List<double[]>();
				newBlock.Add(new double[] { 0, 0 });
				for (double x = step; x <= scale; x += step)
					newBlock.Add(new double[] { x, i * Math.Sqrt(2 * a * x) });
				result.Add(newBlock);
			}

			return result;
		}
	}
}
