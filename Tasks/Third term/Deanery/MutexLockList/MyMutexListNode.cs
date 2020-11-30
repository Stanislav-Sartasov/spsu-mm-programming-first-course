using System;
using System.Collections.Generic;
using System.Text;

namespace MutexLockList
{
    internal class MyMutexListNode<T>
    {
        public T Value { get; private set; }
        //public MyMutexListNode<T> Next { get; set; }

        //public int IsUsing;
        public MyMutexListNode(T value)
        {
            Value = value;
            //IsUsing = 0;
        }
    }
}
