using System;

namespace Generics.BST
{
    public class Node<T>
    {
        public T value; 
        public int key;
        public Node<T> left;
        public Node<T> right;
    }
    public class Tree<T>
    {
        public Node<T> Insert(Node<T> root, T value, int key)
        {
            if (root == null)
            {
                root = new Node<T>
                {
                    value = value,
                    key = key
                };
            }
            else if (key < root.key)
            {
                root.left = Insert(root.left, value, key);
            }
            else
            {
                root.right = Insert(root.right, value, key);
            }

            return root;
        }

        public Node<T> Delete(Node<T> root, int key)
        {
            if (root == null)
                return root;
            if (key < root.key)
            {
                root.left = Delete(root.left, key);
            }
            else if (key > root.key)
            {
                root.right = Delete(root.right, key);
            }
            else if (root.left != null && root.right != null)
            {
                root.key = Minimum(root.right).key;
                root.value = Minimum(root.right).value;
                root.right = Delete(root.right, root.key);
            }
            else
            {
                if (root.left != null)
                {
                    root = root.left;
                }
                else if (root.right != null)
                {
                    root = root.right;
                }
                else
                {
                    root = null;
                }
            }
            return root;
        }

        public Node<T> Search(Node<T> root, int key)
        {
            if (root == null || key == root.key)
            {
                return root;
            }
            if (key < root.key)
            {
                return Search(root.left, key);
            }
            else
            {
                return Search(root.right, key);
            }
        }

        public Node<T> Minimum(Node<T> root)
        {
            if (root.left == null)
            {
                return root;
            }
            return Minimum(root.left);
        }

        public void Traverse(Node<T> root)
        {
            if (root == null)
            {
                return;
            }
            Console.WriteLine(root.value);
            Traverse(root.left);
            Traverse(root.right);
        }
    }
}
