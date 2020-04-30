using Interfaces;
using System;

namespace SecondPlugin
{
    public class FibonacciPlugin : IFibonacci
    {
        public int GetFibonacci(int num)
        {
            double fi = (1 + Math.Sqrt(5)) / 2;
            return (int)Math.Round(Math.Pow(fi, num) / Math.Sqrt(5));
        }
    }
}
