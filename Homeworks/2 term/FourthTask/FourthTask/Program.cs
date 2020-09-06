using System;
using TreeDescription;

namespace FourthTask // Несбалансированное бинарное дерево поиска с числовым ключом(4)
{
    class Program
    {
        static void Main()
        {
            var tree = new BinaryTree<int>();

            tree.AddElement(1, 30);
            tree.AddElement(2, 20);
            tree.AddElement(5, 42);
            tree.AddElement(12, 40);

            tree.SearchElement(key: 10/*, isPrint: 1*/);
            tree.SearchElement(key: 12/*, isPrint: 1*/);

            tree.RemoveElement(1);
            tree.SearchElement(key: 1/*, isPrint: 1*/);
        }
    }
}
