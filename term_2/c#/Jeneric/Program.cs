using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryJeneric;

namespace Jeneric
{
    class Program
    {
        static void Main(string[] args)
        {
            JenericTree<int> t = new JenericTree<int>();
            int j = 0;
            int d = 0;
            for (int i = 0; i < 10; ++i)
                t.Insert(i);

            t.Delete(d);
            for (int i = 0; i < 10; ++i)
            {
                if (i == d)
                    Console.WriteLine(t.Find(d.GetHashCode(), ref j));
                else
                    Console.WriteLine(t.Find(i.GetHashCode(), ref j));
            }
            Console.ReadKey();
        }
    }
}
