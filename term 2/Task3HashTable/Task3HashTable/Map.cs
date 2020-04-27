using System;
using System.Collections.Generic;

namespace Task3HashTable
{
    public class Map<TKey, TValue>
    {
        public int size;
        public int amountBuckets;
        public LinkedList<HashNode<TKey, TValue>>[] bucketList;

        public Map()
        {
            size = 0;
            amountBuckets = 100;
            bucketList = new LinkedList<HashNode<TKey, TValue>>[amountBuckets];
            
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
                if (head.Value.Key.Equals(key))
                {
                    Console.WriteLine($"Deleted value: {head.Value.Value}");
                    bucketList[bucketIndex].Remove(head);
                    size--;
                    break;
                }
                head = head.Next;
            }

            if (head == null) Console.WriteLine("Value with this key does not exist");

            
        }

        public void Search(TKey key)
        {
            
            int bucketIndex = GetBucket(key);
            var head = bucketList[bucketIndex].First;
            while (head != null)
            {
                if (head.Value.Key.Equals(key))
                {
                    Console.WriteLine($"Searched element: {head.Value.Value}");
                    return;
                }
                head = head.Next;
            }
            Console.WriteLine("The key searched was not found in the hash table");

            
        }

        public void Add(TKey key, TValue value)
        {
            int bucketIndex = GetBucket(key);
            if (bucketList[bucketIndex] == null)
                bucketList[bucketIndex] = new LinkedList<HashNode<TKey, TValue>>();

            var head = bucketList[bucketIndex].First;
            while (head != null)
            {
                if (head.Value.Key.Equals(key))
                {
                    head.Value.Value = value;
                    return;
                }
                head = head.Next;
            }

            bucketList[bucketIndex].AddLast(new HashNode<TKey, TValue>(key, value));
            size++;
            if (size / amountBuckets >= 0.75) Resize();

        }

        public void Resize()
        {
            amountBuckets *= 2;
            size = 0;
            var temp = bucketList;
            bucketList = new LinkedList<HashNode<TKey, TValue>>[amountBuckets];
            for (int i = 0; i < amountBuckets / 2; i++)
            {
                if (temp[i] == null)
                    temp[i] = new LinkedList<HashNode<TKey, TValue>>();
                foreach (var node in temp[i])
                {
                    int indexBucket = GetBucket(node.Key);
                    if (bucketList[indexBucket] == null)
                        bucketList[indexBucket] = new LinkedList<HashNode<TKey, TValue>>();
                    bucketList[indexBucket].AddLast(node);
                }
            }
        }
    }
}
