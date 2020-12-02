using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using Deanery.System;
using Deanery.TestEnvironment;

namespace Deanery
{
    class Program
    {
        static void Main(string[] args)
        {
            IExamSystem system;
            int size = 9999;
            TestSystem tsys = new TestSystem();
            List<(int, int, int)> results;
            int iterations = 10;
            Console.WriteLine($"Testing system at {iterations} iterations");

            system = new ListExamSystem(size);
            tsys.Initialize(system);
            results = tsys.Testing(iterations);
            Console.WriteLine($"List exam system (size of table - {size}):");
            for (int i = 0; i < results.Count; i++)
                Print(results[i].Item1, results[i].Item2, results[i].Item3);
            tsys.Dispose();

            Console.WriteLine("***************");

            system = new DefaultExamSystem(size);
            tsys.Initialize(system);
            results = tsys.Testing(iterations);
            Console.WriteLine($"Default exam system (size of table - {size}):");
            for (int i = 0; i < results.Count; i++)
                Print(results[i].Item1, results[i].Item2, results[i].Item3);
            tsys.Dispose();

            Console.WriteLine("***************");

            system = new MutexExamSystem(size);
            tsys.Initialize(system);
            results = tsys.Testing(iterations);
            Console.WriteLine($"Mutex exam system (size of table - {size}):");
            for (int i = 0; i < results.Count; i++)
                Print(results[i].Item1, results[i].Item2, results[i].Item3);
            tsys.Dispose();
        }

        private static void Print(int amountOfRequests, int average, int marginOfError)
        {
            Console.WriteLine($"Confidence interval with confidense level = {TestSystem.ConfidenseLevel} " +
                    $"at amount of requests = {amountOfRequests} of execution time" +
                    $" is [{average - marginOfError}; {average + marginOfError}] ms");
        }
    }
}
