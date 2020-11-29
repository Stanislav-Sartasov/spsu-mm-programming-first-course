using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMath
{
	public class Hyperbola : ICurveBuilder
	{
		public string Name
		{
			get
			{
				return "Hyperbola";
			}
		}
		public List<List<double[]>> Build(double step, double scale, double a, double b)
		{
			if (a <= 0 || b <= 0)
				return null;

			List<List<double[]>> result = new List<List<double[]>>();

			for (int i = -1; i <= 1; i += 2)
			{
				List<double[]> newBlock = new List<double[]>();
				for (double x = -scale; x <= -a; x += step)
					newBlock.Add(new double[] { x, i * b / a * Math.Sqrt(Math.Pow(x, 2) - Math.Pow(a, 2)) });
				newBlock.Add(new double[] { -a, 0 });
				result.Add(newBlock);
				newBlock = new List<double[]>();
				newBlock.Add(new double[] { a, 0 });
				for (double x = a + step; x <= scale; x += step)
					newBlock.Add(new double[] { x, i * b / a * Math.Sqrt(Math.Pow(x, 2) - Math.Pow(a, 2)) });
				result.Add(newBlock);
			}

			return result;
		}
	}
}
