using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace DekanatLibrary
{
    public class CuckooHash: IExamSystem
    {
        private const int THRESHOLD = 20;
        private const int PROBE_SIZE = 30;
        private const int LIMIT = 6;
        private volatile Thread owner;
        private volatile Mutex ownerMutex;
        private volatile Mutex[,] locks;

        private int capacity;
        private List<Node>[,] table;
        private delegate int Hashing(Node x, int capacity);
        private Hashing[] hash;
        public CuckooHash(int size)
        {
            capacity = size;
            table = new List<Node>[2, capacity];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < capacity; j++)
                {
                    table[i, j] = new List<Node>(PROBE_SIZE);
                }
            }
            hash = new Hashing[2];
            hash[0] = new Hashing((x, capacity) =>(int)(x.StudentID.GetHashCode() % capacity));
            hash[1] = new Hashing((x, capacity) => (int)((x.StudentID.GetHashCode() + 5) % capacity));
            locks = new Mutex[2, capacity];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < capacity; j++)
                {
                    locks[i, j] = new Mutex();
                }
            }
            owner = (Thread)null;
            ownerMutex = new Mutex();
        }

        public bool Contains(long studentID, long courseID)
        {
            Node searchNode = new Node(studentID, courseID);
            Acquire(searchNode);
            try
            {
                List<Node> set0 = table[0, hash[0](searchNode, capacity)];
                if (set0.Any(x => x.StudentID == studentID && x.CourseID == courseID))
                {
                    return true;
                }
                else
                {
                    List<Node> set1 = table[1, hash[1](searchNode, capacity)];
                    if (set1.Any(x => x.StudentID == studentID && x.CourseID == courseID))
                    {
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                Release(searchNode);
            }
        }

        public void Remove(long studentID, long courseID)
        {
            Node deleteNode = new Node(studentID, courseID);
            Acquire(deleteNode);
            try
            {
                List<Node> set0 = table[0, hash[0](deleteNode, capacity)];
                if (set0.Any(x => x.StudentID == studentID && x.CourseID == courseID))
                {
                    set0.Remove(set0.Find(x => x.StudentID == studentID && x.CourseID == courseID));
                    return; //true
                }
                else
                {
                    List<Node> set1 = table[1, hash[1](deleteNode, capacity)];
                    if (set1.Any(x => x.StudentID == studentID && x.CourseID == courseID))
                    {
                        set1.Remove(set1.Find(x => x.StudentID == studentID && x.CourseID == courseID));
                        return;// true
                    }
                }
                return;// false
            }
            finally
            {
                Release(deleteNode);
            }
        }

        public void Add(long studentID, long courseID)
        {
            Node AddingNode = new Node(studentID, courseID);
            Node y = default(Node);
            Acquire(AddingNode);
            int h0 = hash[0](AddingNode, capacity), h1 = hash[1](AddingNode, capacity);
            int i = -1, h = -1;
            bool mustResize = false;
            try
            {
                if (Contains(studentID, courseID))
                    return;// false
                List<Node> set0 = table[0, h0];
                List<Node> set1 = table[1, h1];
                if (set0.Count < THRESHOLD)
                {
                    set0.Add(AddingNode);
                    return;// true
                }
                else if (set1.Count < THRESHOLD)
                {
                    set1.Add(AddingNode);
                    return;// true
                }
                else if (set0.Count < PROBE_SIZE)
                {
                    set0.Add(AddingNode); i = 0; h = h0;
                }
                else if (set1.Count < PROBE_SIZE)
                {
                    set1.Add(AddingNode); i = 1; h = h1;
                }
                else
                {
                    mustResize = true;
                }
            }
            finally
            {
                Release(AddingNode);
            }
            if (mustResize)
            {
                Resize();
                Add(studentID, courseID);
            }
            else if (!Relocate(i, h))
            {
                Resize();
            }
            return;// true x must have been present
        }
        protected bool Relocate(int i, int hi)
        {
            int hj = 0;
            int j = 1 - i;
            for (int round = 0; round < LIMIT; round++)
            {
               // if (capacity > 16000)
                //    Console.WriteLine("reloc");
                List<Node> iSet = table[i, hi];
                Node y = iSet[0];
                hj = hash[i](y, capacity);
                Acquire(y);
                List<Node> jSet = table[j, hj];
                try
                {
                    if (iSet.Remove(y))
                    {
                        if (jSet.Count < THRESHOLD)
                        {
                            jSet.Add(y);
                            return true;
                        }
                        else if (jSet.Count < PROBE_SIZE)
                        {
                            jSet.Add(y);
                            i = 1 - i;
                            hi = hj;
                            j = 1 - j;
                        }
                        else
                        {
                            iSet.Add(y);
                            return false;
                        }
                    }
                    else if (iSet.Count >= THRESHOLD)
                    {
                        continue;
                    }
                    else
                    {
                        //if (capacity > 16000)
                         //   Console.WriteLine("reloc END true");
                        return true;
                    }
                }
                finally
                {
                    Release(y);
                }
            }
           // if (capacity > 16000)
            //    Console.WriteLine("reloc END false");
            return false;
        }


        private void Acquire(Node x)
        {
            Thread me = Thread.CurrentThread;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (true)
            {
                do
                { // wait until not resizing
                } while ((owner != (Thread)null) && (owner != me));
                Mutex[,] oldLocks = locks;
                Mutex oldLock0 = oldLocks[0, hash[0](x, capacity)];
                Mutex oldLock1 = oldLocks[1, hash[1](x, capacity)];
                oldLock0.WaitOne();
                oldLock1.WaitOne();
                if (((owner == (Thread)null) || (owner == me)) && locks == oldLocks)
                {
                    stopwatch.Stop();
                    return;
                }
                else
                {
                    oldLock0.ReleaseMutex();
                    oldLock1.ReleaseMutex();
                }
            }
        }

        private void Release(Node x)
        {
            locks[0, hash[0](x, capacity)].ReleaseMutex();
            locks[1, hash[1](x, capacity)].ReleaseMutex();
        }

        private void Resize()
        {
            int oldCapacity = capacity;
            Thread me = Thread.CurrentThread;
            ownerMutex.WaitOne();
            if (owner == (Thread)null)
            {
                owner = Thread.CurrentThread;
                ownerMutex.ReleaseMutex();
                try
                {
                    if (capacity != oldCapacity)
                    { // someone else resized first
                        return;
                    }
                    for (int i = 0; i < locks.GetLength(1); i++)
                    {
                        locks[0, i].WaitOne();
                        locks[0, i].ReleaseMutex();
                    }
                    capacity = 2 * capacity;
                    List<Node>[,] oldTable = table;
                    table = new List<Node>[2, capacity];
                    locks = new Mutex[2, capacity];
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < capacity; j++)
                        {
                            locks[i, j] = new Mutex();
                        }
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < capacity; j++)
                        {
                            table[i, j] = new List<Node>(PROBE_SIZE);
                        }
                    }

                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < oldCapacity; j++)
                        {
                            foreach (Node z in oldTable[i, j])
                            {
                                Add(z.StudentID, z.CourseID);
                            }
                        }
                    }
                }
                finally
                {
                    ownerMutex.WaitOne();
                    owner = (Thread)null;
                    ownerMutex.ReleaseMutex();
                }
            }
            else
            {
                ownerMutex.ReleaseMutex();
            }
        }
    }
}
