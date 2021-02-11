using ExamSystem;
using ExamSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamSystemTests
{
    public static class TestMethods
    {
        public static (long, long)[] GetTestValues(int valuesNumber)
        {
            var rnd = new Random(67);
            var testValues = new (long, long)[valuesNumber];
            for (int i = 0; i < valuesNumber; i++)
            {
                testValues[i] = (rnd.Next(), rnd.Next());
            }
            return testValues;
        }

        public static void AddValues(IExamSystem examSystem, int tasksNumber, (long, long)[] values)
        {
            var tasks = new Task[tasksNumber];
            var requestsNumber = values.Length / tasksNumber;
            for (int i = 0; i < tasksNumber; i++)
            {
                var task = i;
                tasks[task] = new Task(() =>
                {
                    var startValue = task * requestsNumber;
                    for (int req = 0; req < requestsNumber; req++)
                    {
                        examSystem.Add(values[startValue + req].Item1, values[startValue + req].Item2);
                        Thread.Sleep(10);
                    }
                }
                );
            }

            foreach (var t in tasks)
                t.Start();

            foreach (var t in tasks)
                t.Wait();
        }

        public static void RemoveValues(IExamSystem examSystem, int tasksNumber, (long, long)[] values)
        {
            var tasks = new Task[tasksNumber];
            var requestsNumber = values.Length / tasksNumber;
            for (int i = 0; i < tasksNumber; i++)
            {
                var task = i;
                tasks[task] = new Task(() =>
                {
                    var startValue = task * requestsNumber;
                    for (int req = 0; req < requestsNumber; req++)
                    {
                        examSystem.Remove(values[startValue + req].Item1, values[startValue + req].Item2);
                        Thread.Sleep(10);
                    }
                }
                );
            }

            foreach (var t in tasks)
                t.Start();

            foreach (var t in tasks)
                t.Wait();
        }
    }
}
