using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloydAlgo
{
    [Serializable()]
    public class Triple<T, V>
    {
        public T first;
        public T second;
        public V value;
        public Triple(T first, T second, V value)
        {
            this.first = first;
            this.second = second;
            this.value = value;
        }
    }
}
