using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace HashTableLib
{
    public class Hashtable<TKey, TValue> : IEnumerable<Node<TKey, TValue>>
        where TValue : class
    {
        public int NumOfLists { get; private set; } = 4;
        public int MaxLenOfList { get; private set; } = 1;
        public int Count { get; private set; } = 0;
        public int StorageTime { get; private set; }

        LinkedList<Node<TKey, TValue>>[] arrayOfNodes;

        public int HashFunc(TKey key, int arrSize) => Math.Abs((key.GetHashCode() % 67) % arrSize);

        public Hashtable(int storageTime)
            : this()
        {
            StorageTime = storageTime;
        }

        public Hashtable()
        {
            arrayOfNodes = new LinkedList<Node<TKey, TValue>>[NumOfLists];
            StorageTime = 100000;
        }

        public void Resize()
        {

            int newNumOfLists = CountPairs() > 4 ? CountPairs() : 4;
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
                arrayOfNodes[index].AddLast(new Node<TKey, TValue>(StorageTime, key, value));
                if (arrayOfNodes[index].Count > MaxLenOfList)
                    Resize();
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
                    if (curNode.Value.Key.Equals(key) && curNode.Value.Value.TryGetTarget(out TValue _))
                        return true;
                    curNode = curNode.Next;
                }
            }
            return false;
        }


        public bool ContainsValue(TValue value)
        {
            return this
                .Count(node => node.Value.TryGetTarget(out TValue _value) && value.Equals(_value)) != 0;
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
                            if (curNode.Value.Value.TryGetTarget(out TValue target))
                            {
                                value = target;
                                return true;
                            }
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
                        if (curNode.Value.Value.TryGetTarget(out _))
                        {
                            yield return curNode.Value;
                        }
                        curNode = curNode.Next;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int CountPairs()
        {
            return this
                    .Where(node => node.Value.TryGetTarget(out _) == true)
                    .Count();
        }
        public void Print()
        {
            foreach (var node in this)
                Console.WriteLine(node);
        }
    }

    public class Node<TKey, TValue>
        where TValue : class
    {
        public TKey Key { get; private set; }
        public WeakReference<TValue> Value { get; private set; }
        private int storageTime;
        public Node(int storageTime, TKey key, TValue value)
        {
            this.storageTime = storageTime;
            SetPair(key, value);
        }

        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = new WeakReference<TValue>(value);
        }
        async public void SetPair(TKey key, TValue value) 
        {
            Key = key;
            Value = new WeakReference<TValue>(value);
            await Task.Delay(storageTime);
        }

        public override string ToString()
        {
            if (Value.TryGetTarget(out TValue target))
                return $"[{Key}, {target}]";
            else
                return $"[{Key}, collected]";
        }
    }
}
