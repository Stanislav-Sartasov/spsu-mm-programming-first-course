using System;
using System.Collections.Generic;
using System.IO;
using MyInterface;

namespace Plugins
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory();
            List<IDisplayText> textes = new List<IDisplayText>();
            ReadLibrary<IDisplayText> tmp = new ReadLibrary<IDisplayText>();
            textes = tmp.FindPlugin(path);
            for (int i = 0; i < textes.Count; i++)
                Console.WriteLine(textes[i].Text());
        }
    }
}
