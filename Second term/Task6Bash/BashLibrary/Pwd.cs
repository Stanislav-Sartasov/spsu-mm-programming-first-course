using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BashLibrary
{
    public class Pwd : ICommand
    {
        public string Execute(string args)
        {
            DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            string info = "Current directory:\n" + directory.FullName + "\n" + "Files:\n";
            StringBuilder result = new StringBuilder(info);
            foreach (FileInfo file in directory.GetFiles())
            {
                result.Append(file.Name);
                result.Append("\n");
            }
                
            
            return result.ToString();    
        }
    }
}
