using NUnit.Framework;
using GetLib;
using Interfaces;
using System.IO;
using System.Collections.Generic;

namespace Task6Tests
{
    public class Tests
    {
        int Fibonacci(int num)
        {
            int x = 0;
            int y = 1;
            for (int i = 0; i < num; i++)
            {
                int t = x + y;
                x = y;
                y = t;
            }
            return x;
        }
        [Test]
        public void MainTest()
        {
            string way = Directory.GetCurrentDirectory();
            List<IFibonacci> objectList = GetLibMethod<IFibonacci>.FromDirectory(way);

            Assert.IsTrue(objectList != null);
            Assert.IsTrue(objectList.Count == 2);

            for (int i = 0; i < 2; i++)
                for (int num = 0; num < 10; num++)
                    Assert.AreEqual(Fibonacci(num), objectList[i].GetFibonacci(num));
        }
    }
}