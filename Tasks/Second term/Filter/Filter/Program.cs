using System;
using System.IO;

namespace Filter
{
    class MainBody
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Invalid number of parameters.");
                return;
            }
            if (!File.Exists(args[0]))
            {
                Console.WriteLine("Invalid path to the picture.");
                return;
            }
            if (String.Compare(args[0].Substring(args[0].Length - 4), ".bmp") != 0)
            {
                Console.WriteLine("Invalid input file format.");
                return;
            }

            Picture picture = new Picture();
            picture.Read(args[0]);
            Picture.Filter(picture, args[1]);
            picture.Write(args[2]);
        }
    }
}
