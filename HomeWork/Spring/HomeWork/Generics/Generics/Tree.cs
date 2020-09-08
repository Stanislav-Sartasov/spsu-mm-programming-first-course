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
        protected internal class Node<T>
        {
            protected internal T value;
            protected internal int key;
            protected internal Node<T> left;
            protected internal Node<T> right;
            protected internal void DisplayNode()
            {
                Console.WriteLine(key + " ");
            }
        }
        private Node<T> root = null;



        public void Add(T value, int key)
        {
            Node<T> NewNode = new Node<T>();
            NewNode.value = value;
            NewNode.key = key;
            if (root == null)
                root = NewNode;
            else
            {
                Node<T> current = root;
                Node<T> parent;
                while (true)
                {
                    parent = current;
                    if (key < current.key)
                    {
                        current = current.left;
                        if (current == null)
                        {
                            parent.left = NewNode;
                            //Console.WriteLine("L");
                            break;
                        }

                    }
                    else
                    {
                        current = current.right;
                        if (current == null)
                        {
                            parent.right = NewNode;
                            // Console.WriteLine("R");
                            break;
                        }
                    }
                }
            }

        }

        public void PritnRoot()
        {
            Console.WriteLine(root.key + " " + root.value);
            Console.WriteLine(root.left.key);
            Console.WriteLine(root.left.right.key);
        }


        public T Find(int key)
        {
            Node<T> current = root;
            while (true)
            {
                if (current.key == key)
                {
                    return current.value;
                }
                if (key < current.key)
                {
                    current = current.left;
                }
                else
                {
                    current = current.right;
                }
            }

        }

        public void Delete(int key)
        {
            Node<T> current = root;
            Node<T> parent = null;
            while (true)
            {
                if (current.key == key)
                {
                    if (current.left == null && current.right == null)
                    {
                        if (parent.left != null && parent.left.key == key)
                            parent.left = null;
                        else if (parent.right != null && parent.right.key == key)
                            parent.right = null;
                    }
                    else if (current.left != null && current.right == null)
                        parent.left = current.left;
                    else if (current.left == null && current.right != null)
                        parent.right = current.right;
                    else if (current.left != null && current.right != null)
                    {
                        Node<T> minRight = current.right;
                        while (true)
                        {
                            if (minRight.left != null)
                                minRight = minRight.left;
                            else break;
                        }
                        int tmpKey = minRight.key;
                        T tmpValue = minRight.value;
                        Delete(minRight.key);
                        current.key = tmpKey;
                        current.value = tmpValue;
                    }

                    break;
                }
                parent = current;
                if (key < current.key)
                {
                    current = current.left;
                }
                else
                {
                    current = current.right;
                }
            }
        }

    }
}