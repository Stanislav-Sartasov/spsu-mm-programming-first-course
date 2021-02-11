using ExamSystem.Interfaces;
using ExamSystem.Locks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamSystem
{
    class Program
    {
        static Random rnd;
        public struct RequestsRatio
        {
            public int RemoveReqNumber { get; }
            public int AddReqNumber { get; }
            public int ContainsReqNumber { get; }

            public RequestsRatio(int removeReqNumber, int addReqNumber, int containsReqNumber)
            {
                RemoveReqNumber = removeReqNumber;
                AddReqNumber = addReqNumber;
                ContainsReqNumber = containsReqNumber;
            }
        }

        public static List<Task> InitTasks(RequestsRatio ratio, IExamSystem examSystem)
        {
            var tasks = new List<Task>();
            for (int i = 0; i < ratio.RemoveReqNumber; i++)
            {
                tasks.Add(new Task(() => examSystem.Remove(rnd.Next(), rnd.Next())));
            }

            for (int i = 0; i < ratio.AddReqNumber; i++)
            {
                tasks.Add(new Task(() => examSystem.Add(rnd.Next(), rnd.Next())));
            }

            for (int i = 0; i < ratio.ContainsReqNumber; i++)
            {
                tasks.Add(new Task(() => examSystem.Contains(rnd.Next(), rnd.Next())));
            }

            return tasks.OrderBy(i => Guid.NewGuid()).ToList(); //shuffle
        }

        public static RequestsRatio GetRatio(int total)
        {
            var removeNumber = (int)Math.Floor(total * 0.01);
            var addNumber = (int)Math.Floor(total * 0.09);
            var containsNumber = total - removeNumber - addNumber;
            return new RequestsRatio(removeNumber, addNumber, containsNumber);
        }

        public static double GetResult(List<Task> tasks)
        {
            var start = DateTime.Now;

            tasks.ForEach(t => t.Start());
            tasks.ForEach(t => t.Wait());

            return (DateTime.Now - start).TotalMilliseconds;
        }

        public static void PrintResult(string examSystem, RequestsRatio requests, 
            int roundsNumber, List<double> results)
        {
            Console.WriteLine($"\n{examSystem}: ");
            Console.WriteLine($"Number of add requests per round: {requests.AddReqNumber}");
            Console.WriteLine($"Number of remove requests per round: {requests.RemoveReqNumber}");
            Console.WriteLine($"Number of contains requests per round: {requests.ContainsReqNumber}");
            Console.WriteLine($"Number of rounds: {roundsNumber}");
            Console.WriteLine($"Average processing time for all requests: " +
                $"{results.Average()}");
            results.Sort();
            Console.WriteLine($"Median processing time for all requests: " +
                $"{results[results.Count / 2]}");
        }


        static void Main(string[] args)
        {
            rnd = new Random(67);
            var requestsNumber = 10000;
            var roundsNumber = 10;
            var lazyExamSystemResults = new List<double>();
            var stripedExamSystemResults = new List<double>();
            var ratio = GetRatio(requestsNumber);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Performance check\n");
            Console.ResetColor();

            IExamSystem examSystem = new LazyExamSystem(16, new MutexLockFactory());
            
            for (int i = 0; i < roundsNumber; i++)
            {
                var tasks = InitTasks(ratio, examSystem);
                lazyExamSystemResults.Add(GetResult(tasks));
            }

            PrintResult("LazyExamSystem", ratio, roundsNumber, lazyExamSystemResults);

            examSystem = new StripedExamSystem(16, new TASLockFactory());

            for (int i = 0; i < roundsNumber; i++)
            {
                var tasks = InitTasks(ratio, examSystem);
                stripedExamSystemResults.Add(GetResult(tasks));
            }

            PrintResult("StripedExamSystem", ratio, roundsNumber, stripedExamSystemResults);

            Console.ReadKey();
        }
    }
}
