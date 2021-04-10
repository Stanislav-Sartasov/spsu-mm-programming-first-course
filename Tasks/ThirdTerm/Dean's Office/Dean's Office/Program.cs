using System;
using System.Threading.Tasks;
using System.Diagnostics;
using DeansOffice.ExamSystems;

namespace DeansOffice
{
    class Program
    {
        static IExamSystem examSystem;

        static Task[] tasks;
        static Random random = new Random();

        static void Main(string[] args)
        {
            tasks = new Task[9999];
            examSystem = new TTASDeanery();

            Stopwatch stopwatch = new Stopwatch();

            TaskInitialisation();

            stopwatch.Start();
            StartTest();
            stopwatch.Stop();

            Console.WriteLine($"TTAS Deanery Table running time after 9999 requests: {stopwatch.ElapsedMilliseconds} ms");

            examSystem = new TTASListDeanery(9999);

            TaskInitialisation();

            stopwatch.Start();
            StartTest();
            stopwatch.Stop();

            Console.WriteLine($"TTAS List Deanery Table running time after 9999 requests: {stopwatch.ElapsedMilliseconds} ms");
        }

        public static void TaskInitialisation()
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                int request = random.Next(101);
                if (request == 0)
                {
                    tasks[i] = new Task(() => examSystem.Remove(random.Next(), random.Next()));
                }
                else if (request <= 10)
                {
                    tasks[i] = new Task(() => examSystem.Add(random.Next(), random.Next()));
                }
                else
                {
                    tasks[i] = new Task(() => examSystem.Contains(random.Next(), random.Next()));
                }
            }
        }

        public static void StartTest()
        {
            foreach (Task task in tasks)
            {
                task.Start();
            }
            Task.WaitAll(tasks);
        }
    }
}