using NUnit.Framework;
using Protocols;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Task7;
using Task7Server;
using System.Drawing;
using System.Net.Sockets;

namespace Task7Test
{
	public class Tests
	{
		const int FirstRequestCount = 100;
		const int FirstDelay = 300;

		const int SecondRequestCount = 100;
		const int SecondDelay = 500;

		const int SingleRequestCount = 100;

		[Test]
		public void StressTestFirst()
		{
			StressTest(FilterLibrary.BitmapGetter.GetArray(Properties.Resources.p64), FirstDelay, FirstRequestCount, "StressTestFirst.txt");
		}

		[Test]
		public void StressTestSecond()
		{
			StressTest(FilterLibrary.BitmapGetter.GetArray(Properties.Resources.p64), SecondDelay, SecondRequestCount, "StressTestSecond.txt");
		}

		[Test]
		public void StressTestSingle()
		{
			SingleStressTest(FilterLibrary.BitmapGetter.GetArray(Properties.Resources.p64), SingleRequestCount, "StressTestSingle.txt");
		}

		[Test]
		public void DifferentPixelsTests()
		{
			for (int i = 32; i < 256; i += 8)
			{
				byte[] image = FilterLibrary.BitmapGetter.GetArray(new Bitmap(Properties.Resources.p256, new Size(i, i)));
				StressTest(image, FirstDelay, 20, $"DifferentPixelsTestFirst_p{i * i}.txt");
				StressTest(image, SecondDelay, 20, $"DifferentPixelsTestSecond_p{i * i}.txt");
				SingleStressTest(image, 20, $"DifferentPixelsTestSingle_p{i * i}.txt");
			}
		}

		[Test]
		public void ServerKillerTest()
		{
			/*
			var server = new Server("127.0.0.1", 7777, new IFilter[] { new FilterLibrary.ShadeFilter() });
			Task.Run(server.Start);
			*/

			IPEndPoint serverIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7777);

			try
			{
				TcpClient tcp = new TcpClient();
				tcp.Connect(IPAddress.Parse("127.0.0.1"), 7777);
				tcp.Close();
			}
			catch
			{
				throw new Exception("please start Task7Server at 127.0.0.1:7777");
			}

			StreamWriter streamWriter = new StreamWriter("ServerKillerResult.txt");

			var image = FilterLibrary.BitmapGetter.GetArray(Properties.Resources.p64);

			for (int requestCount = 2;; requestCount += 2)
			{
				List<SenderHandler> tests = new List<SenderHandler>();

				for (int i = 0; i < requestCount; i++)
				{
					SenderHandler sender = new SenderHandler();
					tests.Add(sender);
					sender.Work(image, new FilterLibrary.ShadeFilter().Name, serverIP);
				}

				for (int i = 0; i < tests.Count; i++)
				{
					while (!tests[i].Finished)
						Thread.Sleep(50);

					if (tests[i].ElapsedMilliseconds < 0)
					{
						streamWriter.WriteLine($"server killed at {requestCount} requests with image 64x64");
						streamWriter.Close();

						//server.Dispose();

						return;
					}
				}
			}
		}

		void SingleStressTest(byte[] image, int requestCount, string path)
		{
			/*
			var server = new Server("127.0.0.1", 7777, new IFilter[] { new FilterLibrary.ShadeFilter() });
			Task.Run(server.Start);
			*/

			IPEndPoint serverIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7777);

			try
			{
				TcpClient tcp = new TcpClient();
				tcp.Connect(IPAddress.Parse("127.0.0.1"), 7777);
				tcp.Close();
			}
			catch
			{
				throw new Exception("please start Task7Server at 127.0.0.1:7777");
			}

			StreamWriter streamWriter = new StreamWriter(path);

			for (int i = 0; i < requestCount; i++)
			{
				SenderHandler sender = new SenderHandler();
				sender.Work(image, new FilterLibrary.ShadeFilter().Name, serverIP);

				while (!sender.Finished)
					Thread.Sleep(50);

				streamWriter.WriteLine(sender.ElapsedMilliseconds);
			}

			//server.Dispose();
			streamWriter.Close();
		}

		bool StressTest(byte[] image, int delay, int requestCount, string path)
		{
			/*
			var server = new Server("127.0.0.1", 7777, new IFilter[] { new FilterLibrary.ShadeFilter() });
			Task.Run(server.Start);
			*/

			bool result = true;

			IPEndPoint serverIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7777);

			try
			{
				TcpClient tcp = new TcpClient();
				tcp.Connect(IPAddress.Parse("127.0.0.1"), 7777);
				tcp.Close();
			}
			catch
			{
				throw new Exception("please start Task7Server at 127.0.0.1:7777");
			}

			List<SenderHandler> tests = new List<SenderHandler>();
			StreamWriter streamWriter = null;
			if (path != null)
				streamWriter = new StreamWriter(path);

			for (int i = 0; i < requestCount; i++)
			{
				SenderHandler sender = new SenderHandler();
				tests.Add(sender);
				sender.Work(image, new FilterLibrary.ShadeFilter().Name, serverIP);
				Thread.Sleep(delay);
			}

			for (int i = 0; i < tests.Count; i++)
			{
				while (!tests[i].Finished)
					Thread.Sleep(50);

				var res = tests[i].ElapsedMilliseconds;
				if (res < 0)
					result = false;
				if (streamWriter != null)
					streamWriter.WriteLine(res);
			}

			//server.Dispose();

			if (streamWriter != null)
				streamWriter.Close();

			return result;
		}
	}
}