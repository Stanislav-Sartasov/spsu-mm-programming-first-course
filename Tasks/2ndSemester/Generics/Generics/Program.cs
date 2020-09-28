using System;
using Generics.BST;

namespace Generics
{
    class Program
    {
        static void Main(string[] args)
        {
            Node<int> root = null;
            Tree<int> tree = new Tree<int>();

            for (int i = 0; i < 100; i++)
                root = tree.Insert(root, i * i, i);

            Node<int> search = tree.Search(root, 0);
            if (search != null)
                Console.WriteLine(search.value);
            else
                Console.WriteLine("Not Found.");

            tree.Traverse(root);

            for (int i = 0; i < 50; i++)
                root = tree.Delete(root, i);

            search = tree.Search(root, 0);
            if (search != null)
                Console.WriteLine(search.value);
            else
                Console.WriteLine("Not Found.");

            tree.Traverse(root);
        }
    }
}
