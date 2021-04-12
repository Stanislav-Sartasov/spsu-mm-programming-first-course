using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocols
{
	public interface IFilter
	{
		string Name { get; }
		double Progress { get; }
		byte[] Result { get; }
		IFilter Use(byte[] image);
		void Abort();
	}
}
