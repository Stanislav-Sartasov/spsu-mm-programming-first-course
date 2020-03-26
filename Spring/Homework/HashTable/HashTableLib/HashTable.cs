using System;
using System.Collections.Generic;

namespace HashTableLib
{
    public class Hashtable<TKey, TValue>
    {
        public int NumOfLists { get; private set; } = 4;
        public int MaxLenOfList { get; private set; } = 1;
        public int Count { get; private set; } = 0;
        LinkedList<Node<TKey, TValue>>[] arrayOfNodes;

        public int HashFunc(TKey key) => Math.Abs((key.GetHashCode() % 67) % NumOfLists);

        public Hashtable()
        {
            arrayOfNodes = new LinkedList<Node<TKey, TValue>>[NumOfLists];
        }

        public void Resize()
        {
            LinkedList<Node<TKey, TValue>>[] oldNodes = arrayOfNodes;
            arrayOfNodes = new LinkedList<Node<TKey, TValue>>[NumOfLists * 2];
            NumOfLists *= 2;
            MaxLenOfList = NumOfLists / 4;
            Count = 0;

            for (int i = 0; i < oldNodes.Length; i++)
            {
                if (oldNodes[i] != null)
                {
                    while (oldNodes[i].Count != 0)
                    {
                        TKey key = oldNodes[i].Last.Value.Key;
                        TValue value = oldNodes[i].Last.Value.Value;
                        AddPair(key, value);
                        oldNodes[i].RemoveLast();
                    }
                }
            }
        }

        public void AddPair(TKey key, TValue value)
        {
            if (arrayOfNodes == null)
                arrayOfNodes = new LinkedList<Node<TKey, TValue>>[NumOfLists];
            if (!ContainsKey(key))
            {
                int index = HashFunc(key);
                if (arrayOfNodes[index] == null)
                    arrayOfNodes[index] = new LinkedList<Node<TKey, TValue>>();
                arrayOfNodes[index].AddLast(new Node<TKey, TValue>(key, value));
                Count++;
                if (arrayOfNodes[index].Count > MaxLenOfList)
                {
                    Resize();
                }
            }
        }

        public void DeleteByKey(TKey key)
        {
            if (ContainsKey(key))
            {
                int index = HashFunc(key);
                var curNode = arrayOfNodes[index].First;
                while (curNode != null)
                {
                    if (curNode.Value.Key.Equals(key))
                    {
                        arrayOfNodes[index].Remove(curNode);
                        Count--;
                        break;
                    }
                    curNode = curNode.Next;
                }
            }
        }

        public bool ContainsKey(TKey key)
        {
            int index = HashFunc(key);
 
            if (arrayOfNodes[index] != null)
            {
                var curNode = arrayOfNodes[index].First;
                while (curNode != null)
                {
                    if (curNode.Value.Key.Equals(key))
                    {
                        return true;
                    }
                    curNode = curNode.Next;
                }
            }
            return false;
        }

        public bool ContainsValue(TValue value)
        {
            for (int i = 0; i < arrayOfNodes.Length; i++)
            {
                if (arrayOfNodes[i] != null)
                {
                    var curNode = arrayOfNodes[i].First;
                    while (curNode != null)
                    {
                        if (curNode.Value.Value.Equals(value))
                        {
                            return true;
                        }
                        curNode = curNode.Next;
                    }
                }
            }
            return false;
        }

        public TValue TryGetValue(TKey key, out TValue value)
        {
            value = default;
            if (ContainsKey(key))
            {
                int index = HashFunc(key);
                if (arrayOfNodes[index] != null)
                {
                    var curNode = arrayOfNodes[index].First;
                    while (curNode != null)
                    {
                        if (curNode.Value.Key.Equals(key))
                        {
                            value = curNode.Value.Value;
                            break;
                        }
                        curNode = curNode.Next;
                    }
                }
            }
            return value;
        }

        public void Clear()
        {
            for (int i = 0; i < NumOfLists; i++)
            {
                arrayOfNodes[i].Clear();
            }
            arrayOfNodes = null;
            Count = 0;
            NumOfLists = 4;
            MaxLenOfList = 1;
        }
    }

    public class Node<TKey, TValue>
    {
        public TKey Key { get; private set; }
        public TValue Value { get; private set; }
        public Node(TKey key, TValue value) 
        {
            Key = key;
            Value = value;
        }
    }
}
