using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;

namespace StressTest
{
    class Program
    {
        static int PositiveIntInput()
        {
            string input;
            while (true)
            {
                input = Console.ReadLine();
                if (int.TryParse(input, out int number))
                    return number;
                else
                    Console.WriteLine("Please enter a positive integer!");
            }
        }

        static void Main(string[] args)
        {
            string inputPath;
            string outPath;
            string filterName;
            string[] validExtensions = new string[] { ".bmp", ".png", ".jpg" };
            string[] validFilters = new string[]
            {
                "BlackAndWhite", "Average", "Gauss5x5", "Gauss3x3", "SobelX", "SobelY"
            };

            while (true)
            {
                Console.WriteLine("Enter path to image");
                inputPath = Console.ReadLine();
                if (File.Exists(inputPath) && validExtensions.Contains(Path.GetExtension(inputPath)))
                    break;
                else
                    Console.WriteLine("Invalid path or extension");
            }
            
            while (true)
            {
                Console.WriteLine("Enter path to output file");
                outPath = Console.ReadLine();
                if (Directory.Exists(Path.GetDirectoryName(outPath)) && 
                    !string.IsNullOrWhiteSpace(Path.GetFileName(outPath)))
                    break;
                else
                    Console.WriteLine("Invalid directory");
            }
            
            while (true)
            {
                Console.WriteLine("Enter filter name");
                filterName = Console.ReadLine();
                if (validFilters.Contains(filterName))
                    break;
                else
                    Console.WriteLine("Invalid filter name");
            }
            Console.WriteLine("Enter number of requests per second");
            int requestsPerSecond = PositiveIntInput();
            Console.WriteLine("Enter number of seconds");
            int seconds = PositiveIntInput();
            var clients = new List<MockClient>();
            var requests = new List<Task<double>>();
            var results = new List<double>();

            var img = new Bitmap(inputPath);

            for (int second = 0; second < seconds; second++)
            {
                for (int client = 0; client < requestsPerSecond; client++)
                {
                    try 
                    {
                        using (var ms = new MemoryStream())
                        {
                            img.Save(ms, ImageFormat.Bmp);
                            var input = ms.ToArray();
                            clients.Add(new MockClient(input, filterName));
                            var cur = clients.Last();
                            requests.Add(new Task<double>(cur.GetResponseTime));
                            requests.Last().Start();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                     
                }
                Thread.Sleep(1000);
            }

            requests.ForEach(t => t.Wait());

            results = requests.Select(r => r.Result).ToList();
            try
            {
                if (results.Any(r => double.IsNaN(r)) || requests.Any(t => (t.Status == TaskStatus.Faulted)))
                {
                    Console.WriteLine("Denial");
                    File.WriteAllText(outPath, "Denial");
                }
                else
                {
                    Console.WriteLine("Ok");
                    File.WriteAllLines(outPath, results.Select(x => x.ToString()));
                    results.Sort();
                    File.AppendAllText(outPath, $"Average: {results.Average()}\n");
                    File.AppendAllText(outPath, $"Median: {results[results.Count / 2]}");
                }
            }
            catch
            {
                Console.WriteLine("Failed to write to file");
            }
            
            Console.ReadKey();
        }
    }
}
