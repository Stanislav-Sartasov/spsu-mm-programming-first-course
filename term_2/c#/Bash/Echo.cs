using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bash
{
	class Echo : Function
	{
		public override string GoFunc(string arg, Command cmd, ref int i)
		{
            string lastResult = arg;
            return lastResult;
        }
	}
}
