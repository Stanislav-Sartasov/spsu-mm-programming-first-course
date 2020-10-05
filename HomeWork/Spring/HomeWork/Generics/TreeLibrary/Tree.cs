using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;

namespace Generics
{
    
    public class Tree<T>
    {
        private class Node<T>
        {
            public T Value { get; internal set; }
            public int Key { get; internal set; }
            public Node<T> Left { get; internal set; }
            public Node<T> Right { get; internal set; }
            public void DisplayNode()
            {
                Console.WriteLine(Key + " ");
            }
        }
        private Node<T> root = null;

        public void Add(T value, int key)
        {
            Node<T> NewNode = new Node<T>();
            NewNode.Value = value;
            NewNode.Key = key;
            if (root == null)
                root = NewNode;
            else
            {
                Node<T> current = root;
                Node<T> parent;
                while (true)
                {
                    parent = current;
                    if (key < current.Key)
                    {
                        current = current.Left;
                        if (current == null)
                        {
                            parent.Left = NewNode;
                            //Console.WriteLine("L");
                            break;
                        }

                    }
                    else
                    {
                        current = current.Right;
                        if (current == null)
                        {
                            parent.Right = NewNode;
                            // Console.WriteLine("R");
                            break;
                        }
                    }
                }
            }

        }

        public void PritnRoot()
        {
            Console.WriteLine(root.Key + " " + root.Value);
            Console.WriteLine(root.Left.Key);
            Console.WriteLine(root.Left.Right.Key);
        }


        public T Find(int key)
        {
            Node<T> current = root;
            while (true)
            {
                if (current.Key == key)
                {
                    return current.Value;
                }
                if (key < current.Key)
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }

        }

        public void Delete(int key)
        {
            Node<T> current = root;
            Node<T> parent = null;
            while (true)
            {
                if (current.Key == key)
                {
                    if (current.Left == null && current.Right == null)
                    {
                        if (parent.Left != null && parent.Left.Key == key)
                            parent.Left = null;
                        else if (parent.Right != null && parent.Right.Key == key)
                            parent.Right = null;
                    }
                    else if (current.Left != null && current.Right == null)
                        parent.Left = current.Left;
                    else if (current.Left == null && current.Right != null)
                        parent.Right = current.Right;
                    else if (current.Left != null && current.Right != null)
                    {
                        Node<T> minRight = current.Right;
                        while (true)
                        {
                            if (minRight.Left != null)
                                minRight = minRight.Left;
                            else break;
                        }
                        int tmpKey = minRight.Key;
                        T tmpValue = minRight.Value;
                        Delete(minRight.Key);
                        current.Key = tmpKey;
                        current.Value = tmpValue;
                    }

                    break;
                }
                parent = current;
                if (key < current.Key)
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }
        }

    }
}