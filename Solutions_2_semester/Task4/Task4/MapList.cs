using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Task4
{
    public class MapList<keyType,valueType>
    {
        List<keyType> keyList = new List<keyType> { };
        List<valueType> valueList = new List<valueType> { };
        public int Count
        {
            get
            {
                return keyList.Count;
            }
        }
        public void Add(keyType key, valueType value)
        {
            int ind = keyList.FindIndex(x => Equals(x, key));
            if (ind != -1)
                RemoveAt(ind);
            keyList.Add(key);
            valueList.Add(value);
        }
        public bool Remove(keyType key)
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
        public valueType Find(keyType key)
        {
            int ind = keyList.FindIndex(x => Equals(x, key));
            if (ind != -1)
                return valueList[ind];
            return default;
        }
        public keyType FindIndKey(int ind)
        {
            if (ind >= Count)
                return default;
            return keyList[ind];
        }
        public valueType FindIndValue(int ind)
        {
            if (ind >= Count)
                return default;
            return valueList[ind];
        }
    }
}
