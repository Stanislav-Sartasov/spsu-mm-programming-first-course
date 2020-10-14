using System;
using System.IO;

namespace TaskFilters
{
    class Program
    {
        

        static void Main(string[]  args)
        {

            if (Input.CorrectInput(args))
            {
                if (File.Exists(args[0]) == true)
                {
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
                else
                {
                    Console.WriteLine($"File with name {args[0]} does not exist");
                }
            }
            
        }   
    }
}
