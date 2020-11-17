using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bash
{
	class Cat : Function
	{
        public override string GoFunc(string arg, Command cmd, ref int i)
        {
            string lastResult = "";
            try
            {
                StreamReader sr = new StreamReader(arg);
                //Console.WriteLine(sr.ReadToEnd());
                lastResult = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            return lastResult;
        }
    }
}
