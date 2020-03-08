using System;
using BMPfilters;



namespace Filters

{
    class Program
    {
        static bool CheckInput(string[] args)
        {
            string[] filters = new string[]{ "BWfilter", "Gauss5X5", "Gauss3X3", "SobelX", "SobelY", "Median" };
            bool equal = false;
           
            if (args.Length != 3)
            { 
                Console.WriteLine("Invalid input! Enter 3 parameters!");
                return false;
            }
            if (args[0].Substring(args[0].LastIndexOf('.')) != ".bmp" || args[2].Substring(args[2].LastIndexOf('.')) != ".bmp")
            {
                Console.WriteLine("Invalid file extensions! Expected: '.bmp'");
                return false;
            }
            
            foreach (string filter in filters)
            {
                if (filter == args[1])
                    equal = true;
                    
            }

            if (!equal)
            {
                Console.WriteLine("Wrong name of filter!");
                return false;
            }

            return true;
        }


        static void Main(string[] args)
        {
            if (CheckInput(args))
            {
                BMPfile bmpFile = new BMPfile(args[0]);
                string filterName = args[1];

                if (filterName == "BWfilter")
                {
                    BWfilter bw = new BWfilter();
                    bmpFile.SetPixels(bw);
                }

                else if (filterName == "Gauss5X5")
                {
                    GaussFilter g5 = new GaussFilter("5X5");
                    bmpFile.SetPixels(g5);
                }

                else if (filterName == "Gauss3X3")
                {
                    GaussFilter g3 = new GaussFilter("3X3");
                    bmpFile.SetPixels(g3);
                }

                else if (filterName == "SobelX")
                {
                    SobelFilter sX = new SobelFilter("X");
                    bmpFile.SetPixels(sX);
                }

                else if (filterName == "SobelY")
                {
                    SobelFilter sY = new SobelFilter("Y");
                    bmpFile.SetPixels(sY);
                }

                else if (filterName == "Median")
                {
                    MedianFilter mdn = new MedianFilter();
                    bmpFile.SetPixels(mdn);
                }
                bmpFile.WriteBMP(args[2]);
                Console.WriteLine("Complete!");
            }
        }
    }
}


