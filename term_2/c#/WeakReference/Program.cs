using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibraryWeakRef;

namespace WeakReference
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeSpan interval = new TimeSpan(0, 0, 3);
            JenericTree<int> t = new JenericTree<int>(interval);
            int j = 0;
            int d = 4;
            bool weak = false;
            for (int i = 0; i < 10; ++i)
                t.Insert(i);

            t.WeakDelete(d);
            Thread.Sleep(5000); //////////////////////////////////////// test weakReference
            for (int i = 0; i < 10; ++i)
            {
                j = -1;
                if (i == d)
                    Console.WriteLine(t.WeakFind(d.GetHashCode(), ref j, ref weak));
                else
                {
                    Console.WriteLine(t.WeakFind(i.GetHashCode(), ref j, ref weak));
                }
            }

            Console.ReadKey();
        }
    }
}
