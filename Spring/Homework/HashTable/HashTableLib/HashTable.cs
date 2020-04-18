using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HashTableLib
{
    public class Hashtable<TKey, TValue> : IEnumerable<Node<TKey, TValue>>
    {
        public int NumOfLists { get; private set; } = 4;
        public int MaxLenOfList { get; private set; } = 1;
        public int Count { get; private set; } = 0;
        LinkedList<Node<TKey, TValue>>[] arrayOfNodes;

        public int HashFunc(TKey key, int arrSize) => Math.Abs((key.GetHashCode() % 67) % arrSize);

        public Hashtable()
        {
            arrayOfNodes = new LinkedList<Node<TKey, TValue>>[NumOfLists];
        }

        public void Resize()
        {

            int newNumOfLists = NumOfLists * 2;
            int newMaxLenOfList = newNumOfLists / 4;
            var newNodes = new LinkedList<Node<TKey, TValue>>[newNumOfLists];

            foreach (var node in this)
            {
                int index = HashFunc(node.Key, newNumOfLists);
                if (newNodes[index] == null)
                    newNodes[index] = new LinkedList<Node<TKey, TValue>>();
                newNodes[index].AddLast(node);
            }

            arrayOfNodes = newNodes;
            NumOfLists = newNumOfLists;
            MaxLenOfList = newMaxLenOfList;
        }


        public void AddPair(TKey key, TValue value)
        {
            if (arrayOfNodes == null)
                arrayOfNodes = new LinkedList<Node<TKey, TValue>>[NumOfLists];
            if (!ContainsKey(key))
            {
                int index = HashFunc(key, NumOfLists);
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
                int index = HashFunc(key, NumOfLists);
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
            int index = HashFunc(key, NumOfLists);
 
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
            return this.Count(node => node.Value.Equals(value)) != 0;
        }



        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default;
            if (ContainsKey(key))
            {
                int index = HashFunc(key, NumOfLists);
                if (arrayOfNodes[index] != null)
                {
                    var curNode = arrayOfNodes[index].First;
                    while (curNode != null)
                    {
                        if (curNode.Value.Key.Equals(key))
                        {
                            value = curNode.Value.Value;
                            return true;
                        }
                        curNode = curNode.Next;
                    }
                }
            }
            return false;
        }

        public void Clear()
        {
            arrayOfNodes = null;
            Count = 0;
            NumOfLists = 4;
            MaxLenOfList = 1;
        }

        public IEnumerator<Node<TKey, TValue>> GetEnumerator()
        {
            for (int i = 0; i < NumOfLists; i++)
            {
                if (arrayOfNodes[i] != null)
                {
                    var curNode = arrayOfNodes[i].First;
                    while (curNode != null)
                    {
                        yield return curNode.Value;
                        curNode = curNode.Next;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Print()
        {
            foreach (var node in this)
                Console.WriteLine(node);
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

        public override string ToString()
        {
            return $"[{Key}, {Value}]";
        }
    }
}
