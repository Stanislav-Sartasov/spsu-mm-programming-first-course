using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Filter.Filtering
{
    public static class Creator
    {
        static Dictionary<string, IFilter> filters;

        public static void Initialize(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }

        // переписать на строки и словарь
        public static IFilter Create(int number)
        {
            if (number == 1)
                return new Gray();
            return null;
        }

        public static string AvailableFilters()
        {
            return "Gray";
        }
    }
}
