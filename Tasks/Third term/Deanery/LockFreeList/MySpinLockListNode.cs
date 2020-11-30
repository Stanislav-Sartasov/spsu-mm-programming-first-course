using System;
using System.Collections.Generic;
using System.Text;

namespace SpinLockList
{
    internal class MySpinLockListNode<T>
    {
        internal T Value { get; private set; }
        internal MySpinLockListNode<T> Next { get; set; }

        internal int IsUsing;
        internal MySpinLockListNode(T value)
        {
            Value = value;
            IsUsing = 0;
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
            IsUsing = 0;
        }
    }
}
