using Deanery.System;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deanery.TestEnvironment
{
    public class TestSystem
    {
        IExamSystem systemUnderTest;
        Random random;
        public const double ConfidenseLevel = 0.95;
        const double confidenseCofficient = 1.96;

        public void Initialize(IExamSystem system)
        {
            systemUnderTest = system;
            random = new Random();
        }
        public void Dispose()
        {
            systemUnderTest = null;
            random = null;
        }

        private void WarmUp(int count)
        {
            int[] resultsOfRandom = Init(count);
            (int, int)[] data;
            data = FillData(count);
            Task[] users = InitUsers(resultsOfRandom, count, data);
            LaunchTasks(users);
        }
        public List<(int, int, int)> Testing(int iterations)
        {
            if (systemUnderTest == null)
                return new List<(int, int, int)>();

            List<(int, int, int)> result = new List<(int, int, int)>();
            int[] amountOfRequests = new int[] { 1_000, 1_000_0, 1_000_00, 1_000_000, 2_000_000 };
            //int[] amountOfRequests = new int[] { 1_0 };
            List<(int, int)[]> data = new List<(int, int)[]>();
            for (int i = 0; i < amountOfRequests.Length; i++)
            {
                data.Add(FillData(amountOfRequests[i]));
            }

            WarmUp(100);

            Stopwatch timer = new Stopwatch();
            for (int i = 0; i < amountOfRequests.Length; i++)
            {
                int amountOfTasks = amountOfRequests[i];
                
                List<int> resultsOfLaunches = new List<int>();
                for (int j = 0; j < iterations; j++)
                {
                    int[] resultsOfRandom = Init(amountOfTasks);
                    Task[] users = InitUsers(resultsOfRandom, amountOfTasks, data[i]);

                    GC.Collect();
                    Thread.Sleep(100);

                    timer.Restart();
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

        private (int, int)[] FillData(int amountOfTasks)
        {
            (int, int)[] result = new (int, int)[amountOfTasks];
            for (int i = 0; i < amountOfTasks; i++)
            {
                result[i] = (random.Next(), random.Next());
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

        private Task[] InitUsers(int[] resultsOfRandom, int count, (int, int)[] data)
        {
            Task[] result = new Task[count];
            for (int i = 0; i < count; i++)
            { 
                int a = data[i].Item1;
                int b = data[i].Item2;
                if (resultsOfRandom[i] == 0)
                    result[i] = new Task(() =>
                    {
                        systemUnderTest.Remove(a, b);
                    });
                else if (resultsOfRandom[i] <= 10)
                    result[i] = new Task(() =>
                    {
                        systemUnderTest.Add(a, b);
                    });
                else
                    result[i] = new Task(() =>
                    {
                        systemUnderTest.Contains(a, b);
                    });
            }
            return result;
        }

        private int[] Init(int users)
        {
            int[] result = new int[users];
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
