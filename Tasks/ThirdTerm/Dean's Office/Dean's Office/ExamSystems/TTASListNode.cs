using System.Threading;

namespace DeansOffice.ExamSystems
{
    internal class TTASListNode<T>
    {
        internal T Value { get; private set; }
        private volatile TTASListNode<T> next = null;

        internal TTASListNode<T> Next
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

        private volatile int state;

        internal TTASListNode(T value)
        {
            Value = value;
            state = 0;
        }

        internal TTASListNode(TTASListNode<T> node)
        {
            this.Value = node.Value;
            this.Next = node.Next;
        }

        internal void Dispose()
        {
            Value = default;
            Next = null;
            state = 0;
        }

        internal void Lock()
        {
            while (Interlocked.CompareExchange(ref state, 1, 0) == 1)
            {
                Thread.Sleep(0);
            }
        }

        internal void Unlock()
        {
            state = 0;
        }
    }
}