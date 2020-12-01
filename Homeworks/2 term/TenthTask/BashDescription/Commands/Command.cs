using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenthTask.BashDescription
{
	public abstract class Command
	{
		public string Str { get; set; } // Input string

		public abstract string RunCommand(string str, Values values = null);
	}
}
