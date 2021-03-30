using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;

namespace WeakReference
{

    public class Tree<T> where T : class
    {
        private Node<T> root = null;
        private int time;
        public Tree(int time)
        {
            this.time = time;
        }

        public async void Add(T value, int key)
        {
            Node<T> newNode = new Node<T>(key, value);
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
            await Task.Delay(time);
        }

        private async void Await()
        {
            await Task.Delay(time);
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
                if ((current.key == key) && current.IsAlive())
                {
                    current.GetValue();
                    Console.WriteLine(current.GetValue());
                    return current.GetValue();
                }
                if (key < current.key)
                {
                    current = current.left;
                }
                else
                {
                    current = current.right;
                }
                if (current == null)
                {
                    Console.WriteLine("nothing was found");
                    return null;
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
                        T tmpValue = minRight.GetValue();
                        Delete(minRight.key);
                        current.key = tmpKey;
                        current.SetValue(tmpValue);
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