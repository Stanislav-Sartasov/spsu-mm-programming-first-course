using System;
using System.Linq;
using FirstTask.ImageDescription;
using FirstTask.FiltersDescription;

namespace FirstTask
{
	public class Program
	{
		private static void Main(string[] args)
		{
			InputCheck(args);
			var image = new BitMapFile();

			image.FileRead(args[0]);

			FilterSelect(image, args[1]);
			Console.WriteLine("Success!\n");

			image.FileWrite(args[2]);
		}

		private static void InputCheck(string[] args)
		{
			int argc = args.Count();
			if (argc == 1 && String.Compare(args[0], "help") == 0)
			{
				Console.WriteLine("Commands: \"help\", \"median\", \"grey\", \"gauss3\", \"gauss5\", \"sobelX\", \"sobelY\"\nPattern of input: \"input.bmp\" \"command\" \"output.bmp\"\nExample of input: input.bmp gauss3 output.bmp\n");
				Environment.Exit(0);
			}
			else
			{
				if (argc != 3)
				{
					Console.WriteLine("Wrong number of the incoming parameters\nPlease, try again or use \"help\"\n");
					Environment.Exit(-1);
				}
				else
				{
					byte fl = 0;
					if (!(String.Compare(args[1], "median") == 0
					    || String.Compare(args[1], "gauss3") == 0
					    || String.Compare(args[1], "gauss5") == 0
					    || String.Compare(args[1], "sobelX") == 0
					    || String.Compare(args[1], "sobelY") == 0
					    || String.Compare(args[1], "grey") == 0))
					{
						Console.WriteLine("Wrong name of the filter\n");
						fl = 1;
					}

					string bmp = ".bmp";
					for (int i = 1; i < 5; i++)
					{
						if (args[0][args[0].Length - i] != bmp[4 - i]
						    || args[2][args[2].Length - i] != bmp[4 - i])
						{
							Console.WriteLine("Wrong name of the input/output file\n");
							fl = 1;
							break;
						}
					}

					if (fl == 1)
					{
						Console.WriteLine("Please, try again or use \"help\"\n");
						Environment.Exit(-1);
					}
				}
			}
		}

		public static void FilterSelect(BitMapFile image, string filterName)
		{
			switch (filterName)
			{
				case "median":
					var medianFilter = new Median(3);
					medianFilter.FilterImplementation(image);
					return;

				case "gauss3":
					var gauss3Filter = new Gauss(3);
					gauss3Filter.FilterImplementation(image);
					return;

				case "gauss5":
					var gauss5Filter = new Gauss(5);
					gauss5Filter.FilterImplementation(image);
					return;

				case "sobelX":
					var sobelXFilter = new Sobel(3, 0);
					sobelXFilter.FilterImplementation(image);
					return;

				case "sobelY":
					var sobelYFilter = new Sobel(3, 1);
					sobelYFilter.FilterImplementation(image);
					return;

				case "grey":
					var greyFilter = new Grey(0);
					greyFilter.FilterImplementation(image);
					return;

			}
		}
	}
}