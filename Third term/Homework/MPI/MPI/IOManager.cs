using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPI
{
    public static class IOManager
    {
        public static List<int> ReadArray(string inputPath)
        {
            var res = File
                .ReadAllText(inputPath)
                .Split(' ')
                .Select(int.Parse)
                .ToList();
            return res;
        }
        public static void WriteArray(string outputPath, List<int> arr)
        {
            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    writer.Write(arr[i]);
                    if (i != arr.Count - 1)
                    {
                        writer.Write(' ');
                    }
                }
            }
        }
    }
}
