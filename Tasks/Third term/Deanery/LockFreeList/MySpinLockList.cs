using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SpinLockList
{
    public class MySpinLockList<T>
    {
        private volatile MySpinLockListNode<T> first = null;
        private const int timeToSleep = 0;

        public MySpinLockList(T value)
        {
            first = new MySpinLockListNode<T>(value);
        }

        public void Add(T value)
        {
            MySpinLockListNode<T> temp = new MySpinLockListNode<T>(value);

            while (Interlocked.CompareExchange(ref first.IsUsing, 1, 0) == 1)
            { Thread.Sleep(timeToSleep); }

            temp.Next = first.Next;
            first.Next = temp;
            Interlocked.Exchange(ref first.IsUsing, 0);
        }

        public int Find(T value)
        {
            MySpinLockListNode<T> temp = first;
            int i = -1;
            while (temp.Next != null)
            {
                temp = temp.Next;
                i++;
                if (temp.Value.Equals(value))
                    return i;
            }
            return -1;
        }

        public bool Contains(T value)
        {
            MySpinLockListNode<T> temp = first;
            while (temp.Next != null)
            {
                temp = temp.Next;
                if (temp.Value.Equals(value))
                    return true;
            }
            return false;
        }

        public void Remove(T value)
        {
            MySpinLockListNode<T> firstToRemove = first;
            bool notFound = true;
            while (firstToRemove.Next != null)
            {
                if (firstToRemove.Next.Value.Equals(value))
                {
                    notFound = false;
                    break;
                }
                firstToRemove = firstToRemove.Next;
            }
            if (notFound)
                return;

            MySpinLockListNode<T> secondToRemove;
            while (Interlocked.CompareExchange(ref firstToRemove.IsUsing, 1, 0) == 1)
            { Thread.Sleep(timeToSleep); }

            secondToRemove = firstToRemove;

            while (Interlocked.CompareExchange(ref secondToRemove.Next.IsUsing, 1, 0) == 1)
            { Thread.Sleep(timeToSleep); }

            firstToRemove = secondToRemove.Next;
            if (firstToRemove.Value.Equals(value))
            {
                secondToRemove.Next = firstToRemove.Next;
                firstToRemove.Dispose();
                secondToRemove.IsUsing = 0;
            }
        }
    }
}
