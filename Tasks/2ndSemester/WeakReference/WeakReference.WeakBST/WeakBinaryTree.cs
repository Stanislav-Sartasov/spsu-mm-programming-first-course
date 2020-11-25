using System;
using System.Threading.Tasks;

namespace WeakReference.WeakBinaryTree
{
    public class Node<T> where T : class
    {
        public Node<T> LeftNode { get; set; }
        public Node<T> RightNode { get; set; }
        public WeakReference<T> Data { get; set; }
        public int Key { get; set; }
    }

    public class WeakBinaryTree<T> where T : class
    {
        public Node<T> Root { get; set; }

        public int LifeTime { get; set; }

        public WeakBinaryTree(int lifeTime)
        {
            LifeTime = lifeTime;
        }

        public void Add(WeakReference<T> value)
        {
            _Add(value, value.GetHashCode());
        }

        private async void _Add(WeakReference<T> value, int key)
        {
            Node<T> before = null, after = this.Root;

            while (after != null)
            {
                before = after;
                if (key < after.Key)
                    after = after.LeftNode;
                else
                    after = after.RightNode;
            }

            Node<T> newNode = new Node<T>();
            newNode.Data = value;
            newNode.Key = key;

            if (this.Root == null)
                this.Root = newNode;
            else
            {
                if (key < before.Key)
                    before.LeftNode = newNode;
                else
                    before.RightNode = newNode;
            }

            await Task.Delay(LifeTime);
        }


        public Node<T> Find(WeakReference<T> value)
        {
            return this.Find(value.GetHashCode(), this.Root);
        }

        public void Remove(WeakReference<T> value)
        {
            this.Root = Remove(this.Root, value.GetHashCode());
        }

        private Node<T> Remove(Node<T> parent, int key)
        {
            if (parent == null)
                return parent;

            if (key < parent.Key)
                parent.LeftNode = Remove(parent.LeftNode, key);

            else if (key > parent.Key)
                parent.RightNode = Remove(parent.RightNode, key);

            else if (parent.LeftNode != null && parent.RightNode != null)
            {
                parent.Key = MinNode(parent.RightNode).Key;
                parent.Data = MinNode(parent.RightNode).Data;
                parent.RightNode = Remove(parent.RightNode, parent.Key);
            }
            else
            {
                if (parent.LeftNode != null)
                    return parent.LeftNode;

                else if (parent.RightNode != null)
                    return parent.RightNode;
                else
                    return null;
            }

            return parent;
        }

        private Node<T> MinNode(Node<T> Node)
        {
            if (Node.LeftNode != null)
                return MinNode(Node.LeftNode);
            else
                return Node;
        }

        private Node<T> Find(int key, Node<T> parent)
        {
            if (parent != null)
            {
                if (key == parent.Key) return parent;
                if (key < parent.Key)
                    return Find(key, parent.LeftNode);
                else
                    return Find(key, parent.RightNode);
            }

            return null;
        }

        public void Traverse(Node<T> parent)
        {
            T o;
            if (parent != null)
            {
                Traverse(parent.LeftNode);
                if (parent.Data != null)
                    if (parent.Data.TryGetTarget(out o))
                        Console.WriteLine(o.ToString());
                    else
                    {
                        parent.Data = default(WeakReference<T>);
                        Console.WriteLine("Deleted");
                    }
                else
                    Console.WriteLine("Deleted");
                Traverse(parent.RightNode);
            }
        }
    }
}