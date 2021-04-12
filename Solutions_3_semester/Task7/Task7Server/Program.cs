using System;
using System.Threading.Tasks;
using Protocols;
using System.IO;

namespace Task7Server
{
	class Program
	{
		static void Main(string[] args)
		{

			while (true)
			{
				try
				{
					Console.Write("start adress: ");
					var input = Console.ReadLine().Split(':', StringSplitOptions.RemoveEmptyEntries);
					new Server(input[0], int.Parse(input[1]), GetLibMethod<IFilter>.FromDirectory(Directory.GetCurrentDirectory()).ToArray()).Start();
					break;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}
			}
		}
	}
}
