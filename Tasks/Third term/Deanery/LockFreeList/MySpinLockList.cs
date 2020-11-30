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

        public MySpinLockList()
        {
            first = new MySpinLockListNode<T>(default(T));
        }

        //public void Add(T value)
        //{
        //    MySpinLockListNode<T> prev = null;
        //    MySpinLockListNode<T> curr = null;

        //    while (Interlocked.CompareExchange(ref first.IsUsing, 1, 0) == 1)
        //    { Thread.Sleep(timeToSleep); }
        //    curr = first;
        //    if (curr.Next == null)
        //    {
        //        curr.Next = new MySpinLockListNode<T>(value);
        //        Interlocked.Exchange(ref curr.IsUsing, 0);
        //        return;
        //    }

        //    while (Interlocked.CompareExchange(ref curr.Next.IsUsing, 1, 0) == 1)
        //    { Thread.Sleep(timeToSleep); }
        //    prev = curr;
        //    curr = curr.Next;

        //    while (curr.Next != null)
        //    {
        //        while (curr.Next != null && Interlocked.CompareExchange(ref curr.Next.IsUsing, 1, 0) == 1)
        //        { Thread.Sleep(timeToSleep); }
        //        Interlocked.Exchange(ref prev.IsUsing, 0);
        //        prev = curr;
        //        curr = curr.Next;
        //    }
        //    curr.Next = new MySpinLockListNode<T>(value);
        //    Interlocked.Exchange(ref prev.IsUsing, 0);
        //    Interlocked.Exchange(ref curr.IsUsing, 0);
        //}

        public void Add(T value)
        {
            MySpinLockListNode<T> temp = new MySpinLockListNode<T>(value);

            while (Interlocked.CompareExchange(ref first.IsUsing, 1, 0) == 1)
            { Thread.Sleep(timeToSleep); }

            if (first.Next == null)
            {
                first.Next = temp;
                Interlocked.Exchange(ref first.IsUsing, 0);
                return;
            }
            //while (Interlocked.CompareExchange(ref first.Next.IsUsing, 1, 0) == 1)
            //{ Thread.Sleep(timeToSleep); }

            temp.Next = first.Next;
            first.Next = temp;
            //Interlocked.Exchange(ref temp.Next.IsUsing, 0);
            Interlocked.Exchange(ref first.IsUsing, 0);
        }

        public void Remove(T value)
        {
            int i = Find(value);
            if (i == -1)
                return;
            Remove(i);
        }

        public int Find(T value)
        {
            MySpinLockListNode<T> temp = null;
            int i = -1;
            temp = first;
            //if (temp.Value.Equals(value))
            //{
            //    return i;
            //}
            while (temp.Next != null)
            {
                temp = temp.Next;
                i++;
                if (temp.Value.Equals(value))
                    return i;
            }
            return -1;
        }

        private void Remove(int index)
        {
            MySpinLockListNode<T> firstToRemove = first;
            int i = -1;
            while (i < (index - 1))
            {
                firstToRemove = firstToRemove.Next;
                i++;
            }
            MySpinLockListNode<T> secondToRemove;
            while (Interlocked.CompareExchange(ref firstToRemove.IsUsing, 1, 0) == 1)
            { Thread.Sleep(timeToSleep); }
            secondToRemove = firstToRemove;
            while (Interlocked.CompareExchange(ref secondToRemove.Next.IsUsing, 1, 0) == 1)
            { Thread.Sleep(timeToSleep); }
            firstToRemove = secondToRemove.Next;
            secondToRemove.Next = firstToRemove.Next;
            firstToRemove.Dispose();
            secondToRemove.IsUsing = 0;
        }
    }
}
