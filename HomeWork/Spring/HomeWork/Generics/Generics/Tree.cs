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
            public T value;
            public int key;
            public Node<T> left;
            public Node<T> right;
            public void DisplayNode()
            {
                Console.WriteLine(key + " ");
            }
        }
        private Node<T> root = null;



        public void Add(T value, int key)
        {
            Node<T> newNode = new Node<T>();
            newNode.value = value;
            newNode.key = key;
            if (root == null)
                root = newNode;
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
                            parent.left = newNode;
                            //Console.WriteLine("L");
                            break;
                        }

                    }
                    else
                    {
                        current = current.right;
                        if (current == null)
                        {
                            parent.right = newNode;
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