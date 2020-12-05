using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Filter.Filtering
{
    public static class Creator
    {
        static List<string> allFilters;
        static List<string> filters;

        public static void Initialize(string path)
        {
            allFilters = new List<string>();
            allFilters.Add("Gray");
            allFilters.Add("SobelX");
            allFilters.Add("SobelY");
            allFilters.Add("Averaging");

            filters = new List<string>();
            try
            {
                var file = new StreamReader(path);
                string line = file.ReadLine();
                while (line != null)
                {
                    line = line.Trim();
                    if (line[0] != '/')
                        if (allFilters.Contains(line))
                            filters.Add(line);
                    line = file.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("***");
                Console.WriteLine("File does not exist or could not be opened");
                Console.WriteLine("Applying standard filters");
                Console.WriteLine("***");
                filters.AddRange(allFilters);
            }
        }

        public static IFilter Create(string name)
        {
            if (filters.Contains(name))
            {
                if (allFilters.Contains(name))
                {
                    switch (name)
                    {
                        case "Gray":
                                return new Gray();
                        case "SobelX":
                                return new SobelX();
                        case "SobelY":
                                return new SobelY();
                        case "Averaging":
                                return new Averaging();

                        default:
                            return null;
                    }    
                }
            }
            return null;
        }

        public static string AvailableFilters()
        {
            StringBuilder result = new StringBuilder();
            foreach (string filter in filters)
                result.Append(filter + " ");
            return result.ToString() ;
        }
    }
}
