using System;
using FibersDescription;

namespace FirstTask
{
	class Program
	{
		public static int NumOfProcesses { get; set; } = 4;

		static void Main()
		{
			//Input();
			ProcessManager.IsPrioritized = true;

			for (int i = 0; i < NumOfProcesses; i++)
			{
				ProcessManager.AddProcess();
			}

			Console.WriteLine("Launched!");
			ProcessManager.Launch();

			//Thread.Sleep(10);
			ProcessManager.Dispose();
			Console.WriteLine("Disposed!");
		}

		//static void Input()
		//{
		//	Console.WriteLine("Num of processes:");
		//	while (true)
		//	{
		//		try
		//		{
		//			NumOfProcesses = Convert.ToInt32(Console.ReadLine());

		//			if (NumOfProcesses < 1)
		//			{
		//				throw new Exception();
		//			}
					
		//			break;
		//		}
		//		catch
		//		{
		//			Console.WriteLine("Incorrect num of processes, try again:");
		//		}
		//	}

		//	Console.WriteLine("Is prioritized? y/n");
		//	while (true)
		//	{
		//		try
		//		{
		//			var input = Console.ReadLine();

		//			switch (input)
		//			{
		//				case "y":
		//					ProcessManager.IsPrioritized = true;
		//					break;
		//				case "n":
		//					ProcessManager.IsPrioritized = false;
		//					break;
		//				default:
		//					throw new Exception();
		//			}

		//			break;
		//		}
		//		catch
		//		{
		//			Console.WriteLine("Invalid input, try again:");
		//		}
		//	}
		//}
	}
}