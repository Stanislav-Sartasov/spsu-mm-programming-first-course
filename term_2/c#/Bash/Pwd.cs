using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Bash
{
	class Pwd : Function
	{
		public override string GoFunc(string arg, Command cmd, ref int i)
		{
            string lastResult = "";
            try
            {
                lastResult = Directory.GetCurrentDirectory();
                //Console.WriteLine(Directory.GetCurrentDirectory());
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            return lastResult;
        }
	}
}
