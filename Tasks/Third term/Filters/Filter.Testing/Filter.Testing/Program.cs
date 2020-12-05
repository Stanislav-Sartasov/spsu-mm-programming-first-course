using Filter.AdditionLib;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Filter.Testing
{
    class Program
    {
        enum Test
        {
            FirstLevelFixedSize,
            SecondLevelFixedSize,
            FirstLevelNonFixedSize,
            SecondLevelNonFixedSize
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Enter server address");
            string address = Console.ReadLine();
            const int firstLevel = 20;
            const int secondLevel = 40;
            const int iterations = 60;

            StartFirstTest(iterations, firstLevel, address, (int)Test.FirstLevelFixedSize);
            Thread.Sleep(1500);
            StartFirstTest(iterations, secondLevel, address, (int)Test.SecondLevelFixedSize);

            Thread.Sleep(1500);

            StartSecondTest(iterations / 3, firstLevel, address, (int)Test.FirstLevelNonFixedSize);
            Thread.Sleep(1500);
            StartSecondTest(iterations / 3, secondLevel, address, (int)Test.SecondLevelNonFixedSize);

            StartThirdTest(address);

            Console.ReadLine();
        }

        private static void StartFirstTest(int iterations, int level, string address, int num)
        {
            TimeTester test = new TimeTester();

            int height = 1000;
            int width = 1000;
            long average;
            long standartDeviation;
            List<long> resultsOfLaunches;

            resultsOfLaunches = test.Test(level, iterations, address, height, width);

            average = SecondaryFunctions.FindAverageOfSequence(resultsOfLaunches);
            standartDeviation = SecondaryFunctions.CalculateStandartDeviation(resultsOfLaunches, average);

            string name = (num == (int)Test.FirstLevelFixedSize) ? "FirstLevelFixedSize" : "SecondLevelFixedSize";
            Print(resultsOfLaunches, average, standartDeviation, name);
        }

        private static void StartSecondTest(int iterations, int level, string address, int num)
        {
            TimeTester test = new TimeTester();
            int height = 1000;
            int width = 1000;

            long average;
            long standartDeviation;
            List<long> averageResults = new List<long>();
            List<long> medianResults = new List<long>();
            string name = (num == (int)Test.FirstLevelNonFixedSize) ? "FirstLevel" : "SecondLevel";

            for (int i = 0; i < iterations; i++)
            {
                height += 20;
                width += 20;
                List<long> temp = test.Test(level, iterations, address, height, width);
                averageResults.Add(SecondaryFunctions.FindAverageOfSequence(temp));
                medianResults.Add(SecondaryFunctions.FindMedianOfSequence(temp));
            }
            average = SecondaryFunctions.FindAverageOfSequence(averageResults);
            standartDeviation = SecondaryFunctions.CalculateStandartDeviation(averageResults, average);

            Print(averageResults, average, standartDeviation, name + "Average Test");

            average = SecondaryFunctions.FindAverageOfSequence(medianResults);
            standartDeviation = SecondaryFunctions.CalculateStandartDeviation(medianResults, average);

            Print(medianResults, average, standartDeviation, name + "Median Test");
        }

        private static void StartThirdTest(string address)
        {
            int users = 20;
            int height = 2000;
            int width = 2000;
            int threshold = 10_000;

            var tsys = new FailureTester();
            long time = tsys.Test(users, address, height, width);

            while (time <= threshold) // 10 sec.
            {
                users += 1;
                time = tsys.Test(users, address, height, width);
            }
            Console.WriteLine(users);
            string path = Directory.GetCurrentDirectory() + @"\.." + @"\.." + @"\.." + @"\..";
            path = path + $@"\resourses\FailureTest.out";

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine($"Refusal of image processing {height} * {width} at {users} users (waiting threshold - {threshold} ms)");
            }
        }

        private static void Print(List<long> resultsOfLaunches, long average, long standartDeviation, string name)
        {
            Console.WriteLine($"Average = {average}");
            Console.WriteLine($"Standart deviation = {standartDeviation}");

            resultsOfLaunches.Sort();
            List<long>[] statistic = DivideToIntervals(resultsOfLaunches, average, standartDeviation, 8);

            //Console.WriteLine("Interval distribution");
            //for (int i = 0; i < 8; i++, Console.WriteLine())
            //{
            //    foreach (long a in statistic[i])
            //        Console.Write(a + " ");
            //}

            Console.WriteLine("\n****\n");
            for (int i = 0; i < 8; i++)
            {
                Console.Write(statistic[i].Count + " ");
            }
            Console.WriteLine();
            string path = Directory.GetCurrentDirectory() + @"\.." + @"\.." + @"\.." + @"\..";
            path = path + $@"\resourses\{name}.out";

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine($"Average = {average}");
                sw.WriteLine($"Standart deviation = {standartDeviation}");
                foreach (long a in resultsOfLaunches)
                    sw.Write(a + "\n");
            }
        }
        private static List<long>[] DivideToIntervals(List<long> data, long average, long standartDeviation, int intervals)
        {
            data.Sort();
            List<long>[] result = new List<long>[intervals];

            result[0] = new List<long>();
            int j = 0;
            while (j < data.Count)
            {
                if (data[j] <= (average + standartDeviation * (-intervals / 2 + 1)))
                    result[0].Add(data[j]);
                else
                    break;
                j++;
            }

            for (int i = -intervals / 2 + 1; i < intervals / 2 - 1; i++)
            {
                result[i + intervals / 2] = new List<long>();
                while (j < data.Count)
                {
                    if ((average + standartDeviation * i) <= data[j] &&
                            data[j] <= (average + standartDeviation * (i + 1)))
                        result[i + intervals / 2].Add(data[j]);
                    else
                        break;
                    j++;
                }
            }

            result[intervals - 1] = new List<long>();
            while (j < data.Count)
            {
                if (average + standartDeviation * (intervals / 2 - 1) <= data[j])
                    result[intervals - 1].Add(data[j]);
                else
                    break;
                j++;
            }

            return result;
        }
    }
}
