using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Task5
{
    public class WeakHashTable<keyType, valueType>
        where valueType : class
    {
        public WeakHashTable(int startSize, int overflowSize, int lifetime)
            : this(lifetime)
        {
            if (startSize > 0)
                this.startSize = startSize;
            if (overflowSize > 0)
                this.overflowSize = overflowSize;
        }
        public WeakHashTable(int lifetime)
        {
            table = new WeakMapList<keyType, valueType>[startSize];
            this.lifetime = lifetime;
        }

        int startSize = 256;
        int growthCoeff = 2;
        int overflowSize = 4;
        bool balanced = false;
        int lifetime;
        WeakMapList<keyType, valueType>[] table;

        int HashFunc(keyType obj, int len)
        {
            return Math.Abs(obj.GetHashCode()) % len;
        }
        public void Add(keyType key, valueType value)
        {
            Add(key, value, table);
        }
        void Add(keyType key, valueType value, WeakMapList<keyType, valueType>[] table)       //override
        {
            int code = HashFunc(key, table.Length);
            if (table[code] == null)
                table[code] = new WeakMapList<keyType, valueType>(lifetime);
            if (balanced)
                table[code].Add(key, value, false);
            else
                table[code].Add(key, value, true);
            int i = 0;
            while (table[code].Count >= overflowSize && i++ < 5 && !balanced)
            {
                balanced = true;
                Balance();
                balanced = false;
                code = HashFunc(key, table.Length);
                if (table[code] == null)
                    table[code] = new WeakMapList<keyType, valueType>(lifetime);
            }
        }
        void Balance()
        {
            int newSize = table.Length * growthCoeff;
            var newTable = new WeakMapList<keyType, valueType>[newSize];
            for (int i = 0; i < table.Length; i++)
            {
                if (table[i] != null)
                    for (int j = 0; j < table[i].Count; j++)
                        if (table[i].IsAliveAt(j))
                            Add(table[i].KeyAt(j), table[i].ValueAt(j), newTable);
                table[i] = null;
            }
            table = newTable;
        }
        public bool Remove(keyType key)
        {
            while (balanced) { };
            int code = HashFunc(key, table.Length);
            return table[code].Remove(key);
        }
        public valueType Find(keyType key)
        {
            return table[HashFunc(key, table.Length)].Find(key);
        }
        public int OneHealthing()
        {
            GC.Collect();
            int answer = 0;
            for (int i = 0; i < table.Length; i++)
                if (table[i] != null)
                    answer += table[i].OneHealthing();
            return answer;
        }
    }
}