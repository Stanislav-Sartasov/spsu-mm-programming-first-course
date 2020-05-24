using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryWeakRef
{
    public static class LibraryWeakRef
    {
        
    }
    internal class Tree<T> //where T : IComparable
    {
        public T data;
        public int index;
        public Tree<T> treeRight;
        public Tree<T> treeLeft;
        public Tree()
        {
            treeLeft = null;
            treeRight = null;
            index = 0;
        }
        public Tree(T data)
        {
            treeLeft = null;
            treeRight = null;
            this.data = data;
            index = data.GetHashCode();////////////////////////////////////////////////////////////////////////////////
        }
    }
    internal class TimeTree<T>
    {
        public TimeTree(Tree<T> dat)
        {
            data = new WeakReference<Tree<T>>(dat);
            creation = DateTime.Now;
        }
        public WeakReference<Tree<T>> data;
        public readonly DateTime creation;
    }
    public class JenericTree<T>// where T : IComparable
    {
        private Tree<T> head;
        private List<TimeTree<T>> list;
        private TimeSpan maxLive;

        public JenericTree()
        {
            head = null;
            list = new List<TimeTree<T>>();
            maxLive = new TimeSpan(0, 0, 0);
        }
        public JenericTree(TimeSpan maxTimeToLive):this()
        {
            maxLive = maxTimeToLive;
        }
        private Tree<T> SearchParent(T dat)
        {
            int index = dat.GetHashCode();
            Tree<T> temp = head;
            if (temp.index == index)
                return null;
            bool flag = true;
            while (flag)
            {
                if (temp.treeRight != null)
                {
                    if (temp.treeRight.index == index)
                        return temp;
                    if (index > temp.index)
                    {
                        temp = temp.treeRight;
                        continue;
                    }
                }
                if (temp.treeLeft != null)
                {
                    if (temp.treeLeft.index == index)
                        return temp;
                    if (index <= temp.index)
                    {
                        temp = temp.treeLeft;
                        continue;
                    }
                }
                flag = false;
            }

            return null;
        }
        private bool Find(int index, ref T data)
        {
            Tree<T> temp = head;
            while ((temp != null) && (temp.index != index))
            {
                if (index > temp.index)
                {
                    temp = temp.treeRight;
                }
                else
                {
                    temp = temp.treeLeft;
                }
            }

            data = temp == null ? data : temp.data;
            return temp == null ? false : true;
        }
        public void Insert(T dat)
        {
            if (head == null)
            {
                head = new Tree<T>(dat);
                return;
            }
            int index = dat.GetHashCode();
            Tree<T> temp = head;
            while (((index > temp.index) && (temp.treeRight != null)) || ((index <= temp.index) && (temp.treeLeft != null)))
            {
                if (index > temp.index)
                    temp = temp.treeRight;
                else
                    temp = temp.treeLeft;
            };
            if (index > temp.index)
            {
                temp.treeRight = new Tree<T>(dat);
            }
            else
            {
                temp.treeLeft = new Tree<T>(dat);
            }
        }
        public void WeakDelete(T dat)
        {
            int index = dat.GetHashCode();
            Tree<T> condidat;
            Tree<T> parent = SearchParent(dat);
            bool flagRight = true, flagLeft = true;
            if (head.treeRight != null)
                if (head.treeRight.index == index)
                    flagRight = false;
            if (head.treeLeft != null)
                if (head.treeLeft.index == index)
                    flagLeft = false;

            if ((head.index != index) && (parent == null) && (flagLeft) && (flagRight))
                return;

            if (parent != null)
            {
                condidat = index > parent.index ? parent.treeRight : parent.treeLeft;
            }
            else
            {
                condidat = head;
            }
            /////////////////////////////////
            list.Add(new TimeTree<T>(condidat));
            if ((condidat.treeRight == null) || (condidat.treeLeft == null))
            {
                if (condidat.treeRight != null)
                {
                    if (parent != null)
                    {
                        if (parent.treeRight.index == index)
                        {
                            parent.treeRight = condidat.treeRight;
                        }
                        else
                            parent.treeLeft = condidat.treeRight;
                    }
                    else
                        head = condidat.treeRight;
                }
                else
                {
                    if (condidat.treeLeft != null)
                    {
                        if (parent != null)
                        {
                            if (parent.treeRight.index == index)
                            {
                                parent.treeRight = condidat.treeLeft;
                            }
                            else
                                parent.treeLeft = condidat.treeLeft;
                        }
                        else
                            head = condidat.treeLeft;
                    }

                    else
                    {
                        if (parent != null)
                        {
                            if (parent.treeRight.index == index)
                            {
                                parent.treeRight = null;
                            }
                            else
                                parent.treeLeft = null;
                        }
                        else
                            head = null;
                    }
                }
            }
            ////////////////////////////////////////////////////////////////////
            else
            {
                Tree<T> temp = condidat.treeRight;
                while (temp.treeLeft != null)
                {
                    temp = temp.treeLeft;
                }
                temp.treeLeft = condidat.treeLeft;
                if (parent != null)
                {
                    if (parent.treeRight.index == index)
                    {
                        parent.treeRight = condidat.treeRight;
                    }
                    else
                        parent.treeLeft = condidat.treeRight;
                }
                else
                {
                    head = condidat.treeRight;
                }
            }
        }
        public bool WeakFind(int index, ref T data, ref bool inWeakList)
        {
            inWeakList = false;
            bool result = Find(index, ref data);
            if (result)
                return result;
            WeakClean();
            //result = false;
            Tree<T> temp = null;
            foreach(TimeTree<T> a in list)
            {
                temp = null;
                if (a.data.TryGetTarget(out temp))
                {
                    if (temp.index == index)
                    {
                        result = true;
                        break;
                    }
                }
            }

            if (result)
            {
                data = temp.data;
                inWeakList = true;
            }
            return result;
        }
        public void WeakClean()
        {
            DateTime certainTime = DateTime.Now;
            TimeTree<T> b = null;
            foreach (var a in list)
            {
                if (b != null)
                {
                    list.Remove(b);
                    b = null;
                }
                if (certainTime - a.creation >= maxLive)
                {
                    b = a;
                }
            }
            if (b != null)
                list.Remove(b);
        }
    }

}
