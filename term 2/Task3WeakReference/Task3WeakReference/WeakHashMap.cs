using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Task3WeakReference
{
    public class HashNode<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public HashNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    
    }
    public class WeakHashNode<TNode> where TNode: class
    {
        public WeakReference<TNode> Node { get; set; }

        public void WeakNode(TNode node)
        {
            Node = new WeakReference<TNode>(node);
        }
       
    }
    public class WeakHashMap<TKey, TValue>
    {
        public int size;
        public int amountBuckets;
        public int lifeTime;
        public LinkedList<WeakHashNode<HashNode<TKey, TValue>>>[] bucketList;

        public WeakHashMap(int time)
        {
            lifeTime = time;
            size = 0;
            amountBuckets = 100;
            bucketList = new LinkedList<WeakHashNode<HashNode<TKey, TValue>>>[amountBuckets];

        }

        public int GetBucket(TKey key)
        {
            int hashCode = key.GetHashCode();
            return Math.Abs(hashCode % amountBuckets);
        }

        public void Delete(TKey key)
        {
            int bucketIndex = GetBucket(key);
            var head = bucketList[bucketIndex].First;
            
            while (head != null)
            {
                if (head.Value.Node.TryGetTarget(out HashNode<TKey, TValue> target) && target.Key.Equals(key))
                {
                    Console.WriteLine($"Deleted value: {target.Value}");
                    bucketList[bucketIndex].Remove(head);
                    size--;
                    break;
                }
                head = head.Next;
            }

            if (head == null) Console.WriteLine("Value with this key does not exist");


        }

        public bool Search(TKey key)
        {
            int bucketIndex = GetBucket(key);
            var head = bucketList[bucketIndex].First;
            while (head != null)
            {
                
                if (head.Value.Node.TryGetTarget(out HashNode<TKey, TValue> target) && target.Key.Equals(key))
                {
                    Console.WriteLine($"Searched element: {target.Value}");
                    return true;
                }
                head = head.Next;
            }
            Console.WriteLine("The key searched was not found in the hash table");
            return false;

        }

        public async void Add(TKey key, TValue value)
        {
            int bucketIndex = GetBucket(key);
            if (bucketList[bucketIndex] == null)
                bucketList[bucketIndex] = new LinkedList<WeakHashNode<HashNode<TKey, TValue>>>();
            
            var head = bucketList[bucketIndex].First;
            while (head != null)
            {
                if (head.Value.Node.TryGetTarget(out HashNode<TKey, TValue> target) && target.Key.Equals(key))
                {
                    target.Value = value;
                    
                    return;
                }
                head = head.Next;
            }
            HashNode<TKey, TValue> newNode = new HashNode<TKey, TValue>(key, value);
            WeakHashNode<HashNode<TKey, TValue>> newWeakNode = new WeakHashNode<HashNode<TKey, TValue>>();
            newWeakNode.WeakNode(newNode);
            bucketList[bucketIndex].AddLast(newWeakNode);
            size++;
            if (size / amountBuckets >= 0.75) Resize();
            await Task.Delay(lifeTime);

            size--;       
        }

        public void Resize()
        {
            amountBuckets *= 2;
            size = 0;
            var temp = bucketList;
            bucketList = new LinkedList<WeakHashNode<HashNode<TKey, TValue>>>[amountBuckets];
            for (int i = 0; i < amountBuckets / 2; i++)
            {
                if (temp[i] == null)
                    temp[i] = new LinkedList<WeakHashNode<HashNode<TKey, TValue>>>();
                foreach (var node in temp[i])
                {
                    node.Node.TryGetTarget(out HashNode<TKey, TValue> target);
                    int indexBucket = GetBucket(target.Key);
                    if (bucketList[indexBucket] == null)
                        bucketList[indexBucket] = new LinkedList<WeakHashNode<HashNode<TKey, TValue>>>();
                    bucketList[indexBucket].AddLast(node);
                }
            }
        }

        public void Clear()
        {
            bucketList = null;
            size = 0;
            amountBuckets = 100;
        }
    }

}