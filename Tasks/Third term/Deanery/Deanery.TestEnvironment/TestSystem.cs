using Deanery.System;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Deanery.TestEnvironment
{
    public class TestSystem
    {
        IExamSystem systemUnderTest;
        public const double ConfidenseLevel = 0.95;
        const double confidenseCofficient = 1.96;

        public void Initialize(IExamSystem system)
        {
            systemUnderTest = system;
        }
        public void Dispose()
        {
            systemUnderTest = null;
        }

        private void WarmUp()
        {
            int[] resultsOfRandom = Init(1_000);
            Task[] users = InitUsers(resultsOfRandom, 1_000);
            LaunchTasks(users);
        }
        public List<(int, int, int)> Testing(int iterations)
        {
            if (systemUnderTest == null)
                return new List<(int, int, int)>();

            List<(int, int, int)> result = new List<(int, int, int)>();
            int[] amountOfRequests = new int[] { 1_000, 1_000_0, 1_000_00, 1_000_000, 2_000_000 };


            WarmUp();

            for (int i = 0; i < amountOfRequests.Length; i++)
            {
                int amountOfTasks = amountOfRequests[i];
                Stopwatch timer;
                List<int> resultsOfLaunches = new List<int>();

                for (int j = 0; j < iterations; j++)
                {
                    int[] resultsOfRandom = Init(amountOfTasks);
                    Task[] users = InitUsers(resultsOfRandom, amountOfTasks);
                    timer = new Stopwatch();
                    timer.Start();
                    LaunchTasks(users);
                    timer.Stop();
                    resultsOfLaunches.Add((int)timer.ElapsedMilliseconds);
                }

                int average = CalculateAverage(resultsOfLaunches);
                int standartDeviation = CalculateStandartDeviation(resultsOfLaunches, average);
                int marginOfError = (int)Math.Round(confidenseCofficient * standartDeviation / Math.Sqrt(resultsOfLaunches.Count));
                result.Add((amountOfTasks, average, marginOfError));
            }

            return result;
        }

        private int CalculateAverage(List<int> data)
        {
            return data.Sum() / data.Count;
        }

        private int CalculateStandartDeviation(List<int> data, int average)
        {
            double result = 0;
            for (int i = 0; i < data.Count; i++)
                result += Math.Pow(data[i] - average, 2);
            result *= 1.0 / (data.Count);
            return (int)Math.Round(Math.Sqrt(result));
        }

        private Task[] InitUsers(int[] resultsOfRandom, int users)
        {
            Task[] result = new Task[users];
            Random random = new Random();
            for (int i = 0; i < users; i++)
            {
                int x = resultsOfRandom[i];
                if (x == 0)
                    result[i] = new Task(() => {
                        systemUnderTest.Remove(random.Next(), random.Next());
                    });
                else if (x <= 10)
                    result[i] = new Task(() => {
                        systemUnderTest.Add(random.Next(), random.Next());
                    });
                else
                    result[i] = new Task(() => {
                        systemUnderTest.Contains(random.Next(), random.Next());
                    });
            }
            return result;
        }

        private int[] Init(int users)
        {
            int[] result = new int[users];
            Random random = new Random();
            for (int i = 0; i < users; i++)
                result[i] = random.Next(100);
            return result;
        }

        private void LaunchTasks(Task[] users)
        {
            foreach (Task task in users)
                task.Start();
            foreach (Task task in users)
                task.Wait();
        }
    }
}
