using System;

namespace LocksContinued
{
    public class LockFreeSet<T>
    {
        private AtomicNode<T> _tail = new AtomicNode<T>();
        private AtomicNode<T> _head = new AtomicNode<T>();

        public LockFreeSet()
        {
            _head.Next = new AtomicMarkableReference<AtomicNode<T>>(_tail);
        }

        public bool Add(T item)
        {
            int key = item.GetHashCode();
            while (true)
            {
                //Tuple<AtomicNode<T>, AtomicNode<T>>
                Window<T> window = Find(_head, key);
                AtomicNode<T> pred = window.Pred, curr = window.Curr;
                if (curr.Key == key)
                {
                    return false;
                }
                else
                {
                    AtomicNode<T> node = new AtomicNode<T>()
                    {
                        Key = key,
                        Value = item,
                        Next = new AtomicMarkableReference<AtomicNode<T>>(curr, false)
                    };
                    if (pred.Next.CompareAndSet(curr, node, false, false))
                    {
                        return true;
                    }
                }
            }
        }

        public bool Remove(T item)
        {
            int key = item.GetHashCode();
            bool snip;
            while (true)
            {
                Window<T> window = Find(_head, key);
                AtomicNode<T> pred = window.Pred, curr = window.Curr;
                if (curr.Key != key)
                {
                    return false;
                }
                else
                {
                    AtomicNode<T> succ = curr.Next.GetReference();
                    snip = curr.Next.CompareAndSet(succ, succ, false, true);
                    if (!snip)
                        continue;
                    pred.Next.CompareAndSet(curr, succ, false, false);
                    return true;
                }
            }
        }

        public bool Contains(T item)
        {
            bool marked = false;
            int key = item.GetHashCode();
            AtomicNode<T> curr = _head;
            while (curr.Key < key)
            {
                curr = curr.Next.GetReference();
                AtomicNode<T> succ = curr.Next.Get(out marked);
            }
            return (curr.Key == key && !marked);
        }

        private Window<T> Find(AtomicNode<T> head, int key)
        {
            AtomicNode<T> pred = null, curr = null, succ = null;
            bool marked;
            bool snip;

            while (true)
            {

                pred = head;
                curr = pred.Next.GetReference();
                while (true)
                {
                    bool proceedWithNextCycle = false;

                    succ = curr.Next.Get(out marked);
                    while (marked)
                    {
                        snip = pred.Next.CompareAndSet(curr, succ, false, false);
                        if (!snip)
                        {
                            proceedWithNextCycle = true;
                            break;
                        }
                        curr = succ;
                        succ = curr.Next.Get(out marked);
                    }

                    if (proceedWithNextCycle) break;

                    if (curr.Key >= key)
                        return new Window<T>(pred, curr);

                    pred = curr;
                    curr = succ;
                }
            }
        }
    }

}
   
