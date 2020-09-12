using System;
using System.Collections.Generic;
using System.Text;

namespace Generics
{
    public class Node<T>
    {
        public T value;
        public int key;
        public Node<T> left;
        public Node<T> right;
        public void DisplayNode()
        {
            Console.WriteLine(key + " ");
        }
    }
}
