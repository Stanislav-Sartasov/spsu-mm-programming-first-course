using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMath
{
	public interface ICurveBuilder
	{
		string Name { get; }
		List<List<double[]>> Build(double step, double scale, double a, double b);
	}
}
