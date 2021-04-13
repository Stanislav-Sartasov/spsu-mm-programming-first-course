using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the path to the image:");
            string path = Console.ReadLine();
            Console.Write("Enter the number of requests per second:");
            int requests = GetNumber();
            Console.Write("Enter measurement time:");
            int time = GetNumber();
            Console.Write("Enter the path to the measurement results:");
            string pathOut = Console.ReadLine();

            Bitmap image = null;

            try
            {
                image = new Bitmap(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            byte[] bytes = null;

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                bytes = ms.GetBuffer();
            }

            List<Task<int>> list = new List<Task<int>>();

            for (int i = 0; i < time; i++)
            {
                for (int j = 0; j < requests; j++)
                {
                    Task<int> task = new Task<int>(new Tester(bytes).GetTime);
                    task.Start();
                    list.Add(task);
                }
                Thread.Sleep(1000);
            }

            List<int> results = new List<int>();
            foreach (Task<int> task in list)
            {
                task.Wait();
                results.Add(task.Result);
            }

            if (results.Contains(-1))
            {
                Console.WriteLine($"Denial of service occurred under load of {requests} requests per second (timeout exceeded)");
                return;
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(pathOut))
                {
                    foreach (var value in results)
                    {
                        sw.WriteLine(value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            results.Sort();

            int averageValue = (int)results.Average();
            int meadianValue = results[results.Count / 2];

            Console.WriteLine($"Average Time: {averageValue}");
            Console.WriteLine($"Median Time: {meadianValue}");
        }

        static int GetNumber()
        {
            int result;
            while (true)
            {
                bool a = int.TryParse(Console.ReadLine(), out result);

                if (!a)
                {
                    Console.WriteLine("You entered not a number, try again");
                    continue;
                }

                if (result <= 0)
                {
                    Console.WriteLine("Number must be a positive number, try again");
                    continue;
                }
                return result;
            }
        }
    }
}