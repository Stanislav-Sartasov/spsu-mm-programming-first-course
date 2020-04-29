using NUnit.Framework;
using Task5;
using System;

namespace Task5Tests
{
    public static class DateTimeExtension
    {
        public static long ToMilliseconds(this TimeSpan moment)
        {
            return moment.Milliseconds + moment.Seconds * 1000 + moment.Minutes * 60000 + moment.Hours * 320000;
        }
    }
    public class Tests
    {
        [Test]
        public void TestIntFrom0To200000()
        {
            int count = 2000000;
            int lifetime = 5000;
            WeakHashTable<int, int[]> hashTable = new WeakHashTable<int, int[]>(lifetime);
            DateTime[] addTime = new DateTime[count];

            DateTime startTime = DateTime.Now;
            Console.WriteLine("Data lifetime: " + lifetime.ToString() + " milliseconds");            
            Console.WriteLine("Writing started: " + DateTime.Now.Subtract(startTime).ToMilliseconds().ToString() + " milliseconds");
            for (int i = 0; i < count; i++)
            {
                addTime[i] = DateTime.Now;
                hashTable.Add(i, new int[] { i });
            }

            hashTable.OneHealthing();
            DateTime checkTime = DateTime.Now;
            Console.WriteLine("Ñhecking started after OneHealthing: " + checkTime.Subtract(startTime).ToMilliseconds().ToString() + " milliseconds");
            bool flagExistence = false;
            bool flagDots = false;
            int[] value;
            for (int i = 0; i < count; i++)
            {
                value = hashTable.Find(i);
                if (DateTime.Now.Subtract(addTime[i]).ToMilliseconds() < lifetime)
                    if (value == null)
                        Assert.Fail("Something go wrong");
                    else
                        Assert.AreEqual(i, value[0]);
                if (value == null)
                {
                    if (i == 0 || flagExistence || i == count - 1)
                    {
                        Console.WriteLine("[" + i.ToString() + "] : dead; time from creation to checking = " + checkTime.Subtract(addTime[i]).ToMilliseconds().ToString() + " milliseconds");
                        flagExistence = false;
                        flagDots = false;
                    }
                    else if (!flagDots)
                    {
                        Console.WriteLine("...");
                        flagDots = true;
                    }
                }
                else
                {
                    Assert.AreEqual(i, value[0]);
                    if (i == 0 || !flagExistence || i == count - 1)
                    {
                        Console.WriteLine("[" + i.ToString() + "] : alive; time from creation to checking = " + checkTime.Subtract(addTime[i]).ToMilliseconds().ToString() + " milliseconds");
                        flagExistence = true;
                        flagDots = false;
                    }
                    else if (!flagDots)
                    {
                        Console.WriteLine("...");
                        flagDots = true;
                    }
                }
            }
        }
    }
}