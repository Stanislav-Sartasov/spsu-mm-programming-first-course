using System;
using System.IO;

namespace TaskFilters
{
    class Program
    {

        static void Main(string[]  args)
        {
          
            if ((args.Length != 3) || (String.Compare(args[1], "Averaging") != 0) && (String.Compare(args[1], "Gauss3") != 0)
                && (String.Compare(args[1], "Gauss5") != 0) && (String.Compare(args[1], "SobelX") != 0) &&
                (String.Compare(args[1], "SobelY") != 0) && (String.Compare(args[1], "Grayscale") != 0))
            {
                throw new ArgumentException("Incorrect input.");
            }

            FileStream input = new FileStream(args[0], FileMode.Open);
            Picture InputBMP = new Picture();
            InputBMP.BMPRead(input);
            input.Close();

            Filters.Convolution(InputBMP.pixels, InputBMP.height, InputBMP.width, args[1]);
            FileStream output = new FileStream(args[2], FileMode.Create);
            Picture OutputBMP = new Picture();
            InputBMP.BMPWrite(output);
            output.Close();
            

        }   
    }
}
