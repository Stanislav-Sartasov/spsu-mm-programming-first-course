using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SpinLockList
{
    public class MySpinLockList<T>
    {
        private volatile MySpinLockListNode<T> first = null;

        public MySpinLockList(T value)
        {
            first = new MySpinLockListNode<T>(value);
        }

        public void Add(T value)
        {
            MySpinLockListNode<T> temp = new MySpinLockListNode<T>(value);

            first.Lock();

            temp.Next = first.Next;
            first.Next = temp;
            first.Unlock();
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
            firstToRemove.Lock();

            secondToRemove = firstToRemove;
            firstToRemove = secondToRemove.Next;

            firstToRemove.Lock();

            if (firstToRemove.Value.Equals(value))
            {
                secondToRemove.Next = firstToRemove.Next;
                firstToRemove.Dispose();
                secondToRemove.Unlock();
            }
        }
    }
}
