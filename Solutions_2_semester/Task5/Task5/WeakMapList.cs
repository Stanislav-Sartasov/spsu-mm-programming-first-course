using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Task5
{
    public class WeakMapList<TKey, TValue> 
        where TValue : class
    {
        public WeakMapList(int lifetime)
        {
            this.lifetime = lifetime;
        }
        int lifetime;
        List<TKey> keyList = new List<TKey> { };
        List<WeakReference<TValue>> valueList = new List<WeakReference<TValue>> { };
        public int Count
        {
            get
            {
                return keyList.Count;
            }
        }

        public async void Add(TKey key, TValue value, bool wait)
        {
            Remove(key);
            keyList.Add(key);
            valueList.Add(new WeakReference<TValue>(value));
            if (wait)
                await Task.Delay(lifetime);
        }
        public bool Remove(TKey key)
        {
            bool flag = false;
            for (int i = 0; i < keyList.Count;)
                if (Equals(keyList[i], key))
                {
                    RemoveAt(i);
                    flag = true;
                }
                else if(!valueList[i].TryGetTarget(out TValue targetValue))
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
        public TValue Find(TKey key)
        {
            int ind = keyList.FindIndex(x => Equals(x, key));
            TValue target = default;
            if (ind != -1)
                valueList[ind].TryGetTarget(out target);
            return target;
        }
        public TKey KeyAt(int ind)
        {
            if (ind >= keyList.Count)
                return default;
            return keyList[ind];
        }
        public TValue ValueAt(int ind)
        {
            if (ind >= keyList.Count)
                return default;
            TValue target = default;
            valueList[ind].TryGetTarget(out target);
            return target;
        }
        public bool IsAliveAt(int ind)
        {
            if (ind >= keyList.Count)
                return false;
            bool flag = valueList[ind].TryGetTarget(out TValue target);
            return flag;
        }
        public int OneHealthing()
        {
            int theNumberOfDead = 0;
            for (int i = 0; i < keyList.Count;)
                if (!valueList[i].TryGetTarget(out TValue targetValue))
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
