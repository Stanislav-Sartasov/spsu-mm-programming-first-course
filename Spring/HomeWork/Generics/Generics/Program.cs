using System;

namespace Generics
{
    class Program
    {
        static void Main(string[] args)
        {
            Tree<int> test = new Tree<int>();
            test.add(5, 10);
            test.add(5, 4);
            test.add(5, 15);
            test.add(5, 3);
            test.add(5, 18);
            test.add(5, 12);
            test.add(5, 7);
            

            test.delete(15);
            test.delete(4);

            Console.WriteLine(test.root.left.key);
           

            test.find(7);

           

        }
    }
}
