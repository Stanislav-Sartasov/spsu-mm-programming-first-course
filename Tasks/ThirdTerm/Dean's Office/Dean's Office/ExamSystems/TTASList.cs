using System;
using System.Collections.Generic;
using System.Text;

namespace DeansOffice.ExamSystems
{
    public class TTASList<T>
    {
        private volatile TTASListNode<T> first = null;

        public TTASList(T value)
        {
            first = new TTASListNode<T>(value);
        }

        public void Add(T value)
        {
            TTASListNode<T> temp = new TTASListNode<T>(value);

            first.Lock();

            temp.Next = first.Next;
            first.Next = temp;

            first.Unlock();
        }

        public int Find(T value)
        {
            first.Lock();
            TTASListNode<T> temp = first;
            int i = -1;

            while (temp.Next != null)
            {
                temp.Unlock();
                temp = temp.Next;
                temp.Lock();
                i++;
                if (temp.Value.Equals(value))
                {
                    temp.Unlock();
                    return i;
                }
            }
            temp.Unlock();

            return -1;
        }

        public bool Contains(T value)
        {
            first.Lock();
            TTASListNode<T> temp = first;

            while (temp.Next != null)
            {
                first.Unlock();
                temp = temp.Next;
                if (temp.Value.Equals(value))
                    return true;
            }
            temp.Unlock();

            return false;
        }

        public void Remove(T value)
        {
            TTASListNode<T> firstToRemove = first;

            if (this.Contains(value))
            {
                int order = this.Find(value);
                for (int i = 0; i < order; i++)
                {
                    firstToRemove = firstToRemove.Next;
                }
            }
            else
            {
                return;
            }

            TTASListNode<T> secondToRemove;
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
