using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bash
{
	class Wc : Function
	{
		public override string GoFunc(string arg, Command cmd, ref int i)
		{
            string lastResult = "";
            try
            {
                long words = 0, lines = 0, bytes = 0;
                StreamReader sr = new StreamReader(arg);
                while (sr.EndOfStream == false)
                {
                    string temp = sr.ReadLine();
                    ++lines;
                    words += temp.Split(' ').Length;
                }
                bytes = (long)(new FileInfo(arg).Length);

                lastResult = "Bytes: " + bytes + ", words: " + words + ", lines: " + lines;
                //Console.WriteLine("Bytes: {0}, words: {1}, lines: {2}", bytes, words, lines);
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
