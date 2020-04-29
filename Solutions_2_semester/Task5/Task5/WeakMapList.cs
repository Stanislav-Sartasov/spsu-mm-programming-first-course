using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Task5
{
    public class WeakMapList<keyType, valueType> 
        where valueType : class
    {
        public WeakMapList(int lifetime)
        {
            this.lifetime = lifetime;
        }
        int lifetime;
        List<keyType> keyList = new List<keyType> { };
        List<WeakReference<valueType>> valueList = new List<WeakReference<valueType>> { };
        public int Count
        {
            get
            {
                return keyList.Count;
            }
        }

        public async void Add(keyType key, valueType value, bool wait)
        {
            Remove(key);
            keyList.Add(key);
            valueList.Add(new WeakReference<valueType>(value));
            if (wait)
                await Task.Delay(lifetime);
        }
        public bool Remove(keyType key)
        {
            bool flag = false;
            for (int i = 0; i < keyList.Count;)
                if (Equals(keyList[i], key))
                {
                    RemoveAt(i);
                    flag = true;
                }
                else if(!valueList[i].TryGetTarget(out valueType targetValue))
                    RemoveAt(i);
                else
                    i++;
            return flag;
        }
        public bool RemoveAt(int ind)
        {
            if (ind >= keyList.Count)
                return false;
            keyList.RemoveAt(ind);
            valueList.RemoveAt(ind);
            return true;
        }
        public valueType Find(keyType key)
        {
            int ind = keyList.FindIndex(x => Equals(x, key));
            valueType target = default;
            if (ind != -1)
                valueList[ind].TryGetTarget(out target);
            return target;
        }
        public keyType KeyAt(int ind)
        {
            if (ind >= keyList.Count)
                return default;
            return keyList[ind];
        }
        public valueType ValueAt(int ind)
        {
            if (ind >= keyList.Count)
                return default;
            valueType target = default;
            valueList[ind].TryGetTarget(out target);
            return target;
        }
        public bool IsAliveAt(int ind)
        {
            if (ind >= keyList.Count)
                return false;
            bool flag = valueList[ind].TryGetTarget(out valueType target);
            return flag;
        }
        public int OneHealthing()
        {
            int theNumberOfDead = 0;
            for (int i = 0; i < keyList.Count;)
                if (!valueList[i].TryGetTarget(out valueType targetValue))
                {
                    RemoveAt(i);
                    theNumberOfDead++;
                }
                else
                    i++;
            return theNumberOfDead;
        }
    }
}
