using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileManager
{
    public static class IOManager
    {
        public static List<int> ReadFileToList(string path)
        {
            try
            {
                return File.ReadAllText(path).Split(new char[] { ' ' }).Select(x => int.Parse(x)).ToList();
            }
            catch
            {
                throw new Exception("Error while trying to read the file.");
            }
        }

        public static void WriteListToFile(string path, List<int> sortedList)
        {
            try
            {
                File.WriteAllText(path, string.Join(" ", sortedList.Select(x => x.ToString())));
            }
            catch
            {
                throw new Exception("Error while trying to write a file.");
            }
        }
    }
}
