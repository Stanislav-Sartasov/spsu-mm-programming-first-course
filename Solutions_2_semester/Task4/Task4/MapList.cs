using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Task4
{
    public class MapList<TKey,TValue>
    {
        List<TKey> keyList = new List<TKey> { };
        List<TValue> valueList = new List<TValue> { };
        public int Count
        {
            get
            {
                return keyList.Count;
            }
        }
        public void Add(TKey key, TValue value)
        {
            int ind = keyList.FindIndex(x => Equals(x, key));
            if (ind != -1)
                RemoveAt(ind);
            keyList.Add(key);
            valueList.Add(value);
        }
        public bool Remove(TKey key)
        {
            int ind = keyList.FindIndex(x => Equals(x, key));
            if (ind != -1)
            {
                RemoveAt(ind);
                return true;
            }
            return false;
        }
        public bool RemoveAt(int ind)
        {
            if (ind >= Count)
                return false;
            keyList.RemoveAt(ind);
            valueList.RemoveAt(ind);
            return true;
        }
        public TValue Find(TKey key)
        {
            int ind = keyList.FindIndex(x => Equals(x, key));
            if (ind != -1)
                return valueList[ind];
            return default;
        }
        public TKey FindIndKey(int ind)
        {
            if (ind >= Count)
                return default;
            return keyList[ind];
        }
        public TValue FindIndValue(int ind)
        {
            if (ind >= Count)
                return default;
            return valueList[ind];
        }
    }
}
