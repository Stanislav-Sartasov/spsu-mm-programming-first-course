using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SpinLockList
{
    internal class MySpinLockListNode<T>
    {
        internal T Value { get; private set; }
        private volatile MySpinLockListNode<T> next = null;
        internal MySpinLockListNode<T> Next 
        { 
            get 
            {
                return next;
            }
            set 
            {
                next = value;
            } 
        }

        private volatile int isUsing;
        private static int timeToSleep = 0;
        internal MySpinLockListNode(T value)
        {
            Value = value;
            isUsing = 0;
        }

        internal MySpinLockListNode(MySpinLockListNode<T> node)
        {
            this.Value = node.Value;
            this.Next = node.Next;
        }

        internal void Dispose()
        {
            Value = default(T);
            Next = null;
            isUsing = 0;
        }

        internal void Lock()
        {
            while (Interlocked.CompareExchange(ref isUsing, 1, 0) == 1)
            { Thread.Sleep(timeToSleep); }
        }

        internal void Unlock()
        {
            isUsing = 0;
        }
    }
}
