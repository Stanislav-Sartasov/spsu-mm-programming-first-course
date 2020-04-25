using System;
using System.Collections.Generic;
using System.Text;

namespace Task4
{
    public class HashTable<keyType, valueType>
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
            table = new MapList<keyType, valueType>[startSize];
        }

        int startSize = 256;
        int growthCoeff = 2;
        int overflowSize = 4;
        bool balanced = false;
        MapList<keyType, valueType>[] table;

        int HashFunc<T>(T obj)
        {
            return Math.Abs(obj.GetHashCode()) % table.Length;
        }
        public void Add(keyType key, valueType value)       //override
        {
            int code = HashFunc(key);
            if (table[code] == null)
                table[code] = new MapList<keyType, valueType>();
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
                    table[code] = new MapList<keyType, valueType>();
            }

            table[code].Add(key, value);
        }
        void Balance()
        {
            int newSize = table.Length * growthCoeff;
            MapList<keyType, valueType>[] oldTable = table;
            table = new MapList<keyType, valueType>[newSize];
            for (int i = 0; i < oldTable.Length; i++)
            {
                if (oldTable[i] != null)
                    for (int j = 0; j < oldTable[i].Count; j++)
                        Add(oldTable[i].FindIndKey(j), oldTable[i].FindIndValue(j));
                oldTable[i] = null;
            }
        }
        public bool Remove(keyType key)
        {
            int code = HashFunc(key);
            return table[code].Remove(key);
        }
        public valueType Find(keyType key)
        {
            return table[HashFunc(key)].Find(key);
        }
    }
}
