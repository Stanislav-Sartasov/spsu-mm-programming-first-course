using System;
using System.IO;

namespace Filter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3 || !File.Exists(args[0]) || String.Compare(args[0].Substring(args[0].Length - 4), ".bmp") != 0)
            {
                Console.WriteLine("Invalid input.Try again.");
                return;
            }

            Picture picture = new Picture();
            picture.Read(args[0]);
            Picture.Filter(picture, args[1]);
            picture.Write(args[2]);
        }
    }
}