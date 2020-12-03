using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayGeneration
{
    public class Creation
    {
        public static void Generate(string path)
        {
            int capacity = 1000000;
            Random random = new Random();
            List<int> lst = new List<int>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                lst.Add(random.Next(1000001));
            }

            File.WriteAllText($"{Path.Combine(path, "input.dat")}", string.Join(" ", lst.Select(x => x.ToString())));

            lst.Sort();

            File.WriteAllText($"{Path.Combine(path, "sorted.dat")}", string.Join(" ", lst.Select(x => x.ToString())));
        }
    }
}