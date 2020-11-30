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

		public abstract bool CheckCommand(string name, string str);
		public abstract string RunCommand(string name, string str);
	}
}
