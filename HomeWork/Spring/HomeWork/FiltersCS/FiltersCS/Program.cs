using Picture;
using System;

namespace FiltersCS
{
    enum FiltersName : uint
    {
        Averaging = 1,
        Gauss = 2,
        SobelX = 3,
        SobelY = 4,
    };
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
                throw new Exception("Invalid input");

            BMP bmp = new BMP();
            bmp.ReadBMP(args[0]);

            Filters Filter = new Filters();

            if (args[2] == "Grey")
                Filter.Grey(bmp.pixels, bmp.HeaderInfoBMP.Height, bmp.HeaderInfoBMP.Width);
            else if (args[2] == "Gauss")
                Filter.Filter(bmp.pixels, bmp.HeaderInfoBMP.Height, bmp.HeaderInfoBMP.Width, (uint)FiltersName.Gauss);
            else if (args[2] == "Averaging")
                Filter.Filter(bmp.pixels, bmp.HeaderInfoBMP.Height, bmp.HeaderInfoBMP.Width, (uint)FiltersName.Averaging);
            else if (args[2] == "SobelX")
                Filter.Filter(bmp.pixels, bmp.HeaderInfoBMP.Height, bmp.HeaderInfoBMP.Width, (uint)FiltersName.SobelX);
            else if (args[2] == "SobelY")
                Filter.Filter(bmp.pixels, bmp.HeaderInfoBMP.Height, bmp.HeaderInfoBMP.Width, (uint)FiltersName.SobelY);
            else
                throw new ArgumentException("Entered wrong name of filter");

            bmp.WriteBMP(args[1]);
            Console.WriteLine("Success");

            
        }
    }
}
