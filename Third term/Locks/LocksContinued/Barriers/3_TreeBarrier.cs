using LocksContinued.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocksContinued.Barriers
{
    public class TreeBarrier : IBarrier
    {
        int childrenLimit;
        Node[] leaves;
        int leavesCount;
        ThreadLocal<bool> threadSense;

        public TreeBarrier(int n, int childrenLimit)
        {
            this.childrenLimit = childrenLimit;
            leavesCount = 0;
            this.leaves = new Node[n / childrenLimit];
            int depth = 0;
            threadSense = new ThreadLocal<Boolean>(() => true);

            // compute tree depth
            while (n > 1)
            {
                depth++;
                n = n / childrenLimit;
            }
            Node root = new Node(this.childrenLimit, threadSense);
            Build(root, depth - 1);
        }

        // recursive tree constructor
        void Build(Node parent, int depth)
        {
            if (depth == 0)
            {
                leaves[leavesCount++] = parent;
            }
            else
            {
                for (int i = 0; i < childrenLimit; i++)
                {
                    Node child = new Node(parent, childrenLimit, threadSense);
                    Build(child, depth - 1);
                }
            }
        }

        public void Await()
        {
            // диапазон от 0 до n-1
            int me = Thread.CurrentThread.ManagedThreadId;

            Node myLeaf = leaves[me / childrenLimit];
            myLeaf.Await();
        }

        private class Node
        {
            ThreadLocal<bool> threadSense;
            int childrenLimit;

            volatile int count;

            Node parent = null;
            volatile bool sense = false;

            public Node(int childrenLimit, ThreadLocal<bool> threadSense)
            {
                this.childrenLimit = childrenLimit;
                this.threadSense = threadSense;
                this.count = childrenLimit;
            }

            public Node(Node myParent, int childrenLimit, ThreadLocal<bool> threadSense) 
                : this(childrenLimit, threadSense)
            {
                parent = myParent;
            }

            public void Await()
            {
                bool mySense = threadSense.Value;
                int position = Interlocked.Decrement(ref count);

                if (position == 0)
                { // I’m last
                    if (parent != null)
                    { // Am I root?
                        parent.Await();
                    }
                    count = childrenLimit;
                    sense = mySense;
                }
                else
                {
                    while (sense != mySense) { };
                }
                threadSense.Value = !mySense;
            }
        }
    }
}
