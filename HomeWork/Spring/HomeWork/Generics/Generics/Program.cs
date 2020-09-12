using System;

namespace Generics
{
    class Program
    {
        static void Main(string[] args)
        {
            Tree<int> test = new Tree<int>();
            test.Add(5, 10);
            test.Add(5, 4);
            test.Add(5, 15);
            test.Add(5, 3);
            test.Add(5, 18);
            test.Add(5, 12);
            test.Add(5, 7);

            Node<int> wtf = new Node<int>();



            test.Delete(15);
            test.Delete(4);


            int tmp = test.Find(7);
            Console.WriteLine(tmp);



        }
    }
}