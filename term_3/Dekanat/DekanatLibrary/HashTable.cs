using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DekanatLibrary
{
    public class HashTable: IExamSystem
    {
        private long hashConst;
        private List<Node>[] hashArray;
        private int size;
        private int elements;

        private volatile Mutex[] lockers;

        public HashTable(int size)
        {
            hashConst = 1;
            elements = 0;
            if (size == 0)
                size = 256;
            this.size = size;
            hashArray = new List<Node>[size];
            lockers = new Mutex[size];

            for (int i = 0; i < size; i++)
            {
                hashArray[i] = new List<Node>();
                lockers[i] = new Mutex();
            }
        }

        long GetHashKey(long student, int size)
        {
            return (student.GetHashCode() * hashConst) % size;
        }

        public void Add(long studentId, long courseId)
        {
            //Console.WriteLine("adding");
            long hash = GetHashKey(studentId, size);
            Lock(hash);
            try
            {
                if (hashArray[hash].Any(x => x.StudentID == studentId && x.CourseID == courseId))
                {
                    return;
                }

                Node newJob = new Node(studentId, courseId);
                hashArray[hash].Add(newJob);
                Interlocked.Increment(ref elements);
            }
            finally
            {
                Free(hash);
            }

            if (RequiredResize())
            {
                Resize();
            }
        }

        private bool RequiredResize()
        {
            return elements / size >= 2;   
        }

        void Lock(long hash)
        {
            lockers[hash % lockers.Length].WaitOne();
        }

        void Free(long hash)
        {
            lockers[hash % lockers.Length].ReleaseMutex();
        }

        void Resize()
        {
            //Console.WriteLine("resize");
            int oldSize = size;

            foreach (var locker in lockers)
            {
                locker.WaitOne();
            }
            if (oldSize != size)
            {
                foreach (var locker in lockers)
                {
                    locker.ReleaseMutex();
                }
                return;
            }

            size *= 2;
            List<Node>[] oldHashArray = hashArray;
            hashArray = new List<Node>[size];
            for (int i = 0; i < size; i++)
            {
                hashArray[i] = new List<Node>();
            }
            foreach (List<Node> temLst in oldHashArray)
            {
                foreach (Node tempNode in temLst)
                {
                    hashArray[GetHashKey(tempNode.StudentID, size)].Add(tempNode);
                }
            }
           // Console.WriteLine($"resize new size: {size}"); 
            foreach (var locker in lockers)
            {
                locker.ReleaseMutex();
            }
        }

        public bool Contains(long studentId, long courseId)
        {
           // Console.WriteLine("contains");
            long hash = GetHashKey(studentId, size);
            Lock(hash);
            bool result = hashArray[hash].Any(x => x.StudentID == studentId && x.CourseID == courseId);
            Free(hash);
            //Console.WriteLine($"{hashArray[hash].Count}");
            return result;
        }

        public void Remove(long studentId, long courseId)
        {
            //Console.WriteLine("removing");
            long hash = GetHashKey(studentId, size);
            Lock(hash);

            if (hashArray[hash].Any(x => x.StudentID == studentId && x.CourseID == courseId))
            {
                hashArray[hash].Remove(hashArray[hash].Find(x => x.StudentID == studentId && x.CourseID == courseId));
                Interlocked.Decrement(ref elements);
            }
            Free(hash);
        }
    }
}
