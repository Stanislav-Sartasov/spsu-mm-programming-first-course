using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bash
{
	class Function
	{
		public virtual string GoFunc(string arg, Command cmd, ref int i)
		{
			Console.WriteLine("NOT OVERRIDE");
			return "";
		}
	}
}
