using System.Collections.Generic;
using ExamSystem.Interfaces;
using System;


namespace ExamSystem.ConcurrentCollections
{
    public class LazySet<T>
        where T : IComparable<T>, IEquatable<T>
    {
        Node<T> Head { get; }
        Node<T> Tail { get; }

        private ILockFactory lockFactory;
        public ILockFactory LockFactory
        {
            get => lockFactory;
            set => lockFactory = value ?? 
                throw new ArgumentNullException("Lock factory cannot be null");
        }

        public LazySet(ILockFactory lockFactory, T tailValue, T headValue)
        {
            if (tailValue.CompareTo(headValue) <= 0)
                throw new ArgumentException("tailValue must be greater than headValue");
            Tail = new Node<T>(tailValue, null, lockFactory);
            Head = new Node<T>(headValue, Tail, lockFactory);
            LockFactory = lockFactory;
        }

        private bool Validate(Node<T> pred, Node<T> curr) => 
            !pred.Marked && !curr.Marked && pred.Next.Equals(curr);

        public bool FindAndProcess(T item, Func<Node<T>, Node<T>, bool> process)
        {
            if (item.Equals(Head.Value) || item.Equals(Tail.Value))
                throw new ArgumentException("You cannot change head or tail elements");
            while (true)
            {
                Node<T> pred = Head;
                Node<T> curr = Head.Next;
                while (curr.Value.CompareTo(item) < 0)
                {
                    pred = curr;
                    curr = curr.Next;
                }
                pred.Lock();
                try
                {
                    curr.Lock();
                    try
                    {
                        if (Validate(pred, curr))
                            return process(pred, curr);
                    }
                    finally
                    {
                        curr.Unlock();
                    }
                }
                finally
                {
                    pred.Unlock();
                }
            }
        }

        public bool Add(T item) => FindAndProcess(item, (pred, curr) =>
        {
            if (curr.Value.Equals(item))
                return false;
            Node<T> node = new Node<T>(item, curr, lockFactory);
            pred.Next = node;
            return true;
        });

        public bool Remove(T item) => FindAndProcess(item, (pred, curr) =>
        {
            if (!curr.Value.Equals(item))
                return false;
            curr.Marked = true;
            pred.Next = curr.Next;
            return true;
        });

        public bool Сontains(T item)
        {
            Node<T> curr = Head;
            while (curr.Value.CompareTo(item) < 0)
                curr = curr.Next;
            return curr.Value.Equals(item) && !curr.Marked;
        }

        //only for test
        public List<T> GetAllData()
        {
            var lst = new List<T>();
            Node<T> curr = Head.Next;
            while (!curr.Value.Equals(Tail.Value))
            {
                if (!curr.Marked)
                    lst.Add(curr.Value);
                curr = curr.Next;
            }
            return lst;
        }
    }

    public class Node<T>
        where T: IComparable<T>, IEquatable<T>
    {
        ILock lck;
        private volatile bool marked;
        public bool Marked 
        {
            get => marked;
            set => marked = value;
        }
        public T Value { get; }
        public Node<T> Next { get; set; }

        public Node(T value, Node<T> next, ILockFactory factory)
        {
            Value = value;
            Next = next;
            lck = factory.CreateLock();
            Marked = false;
        }

        public void Lock() => lck.Lock();

        public void Unlock() => lck.Unlock();
    }
}
