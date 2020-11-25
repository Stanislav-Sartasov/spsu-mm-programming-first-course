using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Plugins.Interface;

namespace Plugins
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory() + @"..\..\..\..\..\DllFiles\";
            List<IGame> games = FindLibrary<IGame>.FindPlugins(path);
            for (int i = 0; i < games.Count; i++)
            {
                Console.WriteLine(games[i].GetMetacriticRating());
            }
            Console.ReadKey();
        }
    }
}
