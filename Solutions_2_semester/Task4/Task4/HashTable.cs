using System;
using System.Collections.Generic;
using System.Text;

namespace Task4
{
    public class HashTable<TKey, TValue>
    {
        public HashTable(int startSize, int overflowSize)
            : this()
        {
            if (startSize > 0)
                this.startSize = startSize;
            if (overflowSize > 0)
                this.overflowSize = overflowSize;
        }
        public HashTable()
        {
            table = new MapList<TKey, TValue>[startSize];
        }

        int startSize = 256;
        int growthCoeff = 2;
        int overflowSize = 4;
        bool balanced = false;
        MapList<TKey, TValue>[] table;

        int HashFunc<T>(T obj)
        {
            return Math.Abs(obj.GetHashCode()) % table.Length;
        }
        public void Add(TKey key, TValue value)       //override
        {
            int code = HashFunc(key);
            if (table[code] == null)
                table[code] = new MapList<TKey, TValue>();
            else
                table[code].Remove(key);
            int i = 0;
            while (table[code].Count >= overflowSize && i++ < 5 && !balanced)
            {
                balanced = true;
                Balance();
                balanced = false;
                code = HashFunc(key);
                if (table[code] == null)
                    table[code] = new MapList<TKey, TValue>();
            }

            table[code].Add(key, value);
        }
        void Balance()
        {
            int newSize = table.Length * growthCoeff;
            MapList<TKey, TValue>[] oldTable = table;
            table = new MapList<TKey, TValue>[newSize];
            for (int i = 0; i < oldTable.Length; i++)
            {
                if (oldTable[i] != null)
                    for (int j = 0; j < oldTable[i].Count; j++)
                        Add(oldTable[i].FindIndKey(j), oldTable[i].FindIndValue(j));
                oldTable[i] = null;
            }
        }
        public bool Remove(TKey key)
        {
            int code = HashFunc(key);
            return table[code].Remove(key);
        }
        public TValue Find(TKey key)
        {
            return table[HashFunc(key)].Find(key);
        }
    }
}
