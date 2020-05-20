using System;
using System.Collections.Generic;
using System.IO;

namespace Plugins
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory() + @"\.." + @"\.." + @"\.." + @"\..";
            path = path + @"\DLLFiles\";
            LibreryAnalysis libreryAnalysis = new LibreryAnalysis(path);

            List<Type> temp = libreryAnalysis.ShowTypesInFiles();
            foreach (var o in temp)
            {
                Console.WriteLine(o.FullName);
                Console.WriteLine(o.Name);
            }
            for (int i = 0; i < libreryAnalysis.Length(); i++)
            {
                var o = libreryAnalysis.Instantiation(i);
                Console.WriteLine(o);
            }
        }
    }
}
