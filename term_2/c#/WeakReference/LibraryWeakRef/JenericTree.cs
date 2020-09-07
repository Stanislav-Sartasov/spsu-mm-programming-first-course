using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryWeakRef
{
    public class JenericTree<T>// where T : IComparable
    {
        private Tree<T> head;
        private readonly List<TimeTree<T>> list;
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
            if (temp.Index == index)
                return null;
            bool flag = true;
            while (flag)
            {
                if (temp.TreeRight != null)
                {
                    if (temp.TreeRight.Index == index)
                        return temp;
                    if (index > temp.Index)
                    {
                        temp = temp.TreeRight;
                        continue;
                    }
                }
                if (temp.TreeLeft != null)
                {
                    if (temp.TreeLeft.Index == index)
                        return temp;
                    if (index <= temp.Index)
                    {
                        temp = temp.TreeLeft;
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
            while ((temp != null) && (temp.Index != index))
            {
                if (index > temp.Index)
                {
                    temp = temp.TreeRight;
                }
                else
                {
                    temp = temp.TreeLeft;
                }
            }

            data = temp == null ? data : temp.Data;
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
            while (((index > temp.Index) && (temp.TreeRight != null)) || ((index <= temp.Index) && (temp.TreeLeft != null)))
            {
                if (index > temp.Index)
                    temp = temp.TreeRight;
                else
                    temp = temp.TreeLeft;
            };
            if (index > temp.Index)
            {
                temp.TreeRight = new Tree<T>(dat);
            }
            else
            {
                temp.TreeLeft = new Tree<T>(dat);
            }
        }
        public void WeakDelete(T dat)
        {
            int index = dat.GetHashCode();
            Tree<T> condidat;
            Tree<T> parent = SearchParent(dat);
            bool flagRight = true, flagLeft = true;
            if (head.TreeRight != null)
                if (head.TreeRight.Index == index)
                    flagRight = false;
            if (head.TreeLeft != null)
                if (head.TreeLeft.Index == index)
                    flagLeft = false;

            if ((head.Index != index) && (parent == null) && (flagLeft) && (flagRight))
                return;

            if (parent != null)
            {
                condidat = index > parent.Index ? parent.TreeRight : parent.TreeLeft;
            }
            else
            {
                condidat = head;
            }
            /////////////////////////////////
            list.Add(new TimeTree<T>(condidat));
            if ((condidat.TreeRight == null) || (condidat.TreeLeft == null))
            {
                if (condidat.TreeRight != null)
                {
                    if (parent != null)
                    {
                        if (parent.TreeRight.Index == index)
                        {
                            parent.TreeRight = condidat.TreeRight;
                        }
                        else
                            parent.TreeLeft = condidat.TreeRight;
                    }
                    else
                        head = condidat.TreeRight;
                }
                else
                {
                    if (condidat.TreeLeft != null)
                    {
                        if (parent != null)
                        {
                            if (parent.TreeRight.Index == index)
                            {
                                parent.TreeRight = condidat.TreeLeft;
                            }
                            else
                                parent.TreeLeft = condidat.TreeLeft;
                        }
                        else
                            head = condidat.TreeLeft;
                    }

                    else
                    {
                        if (parent != null)
                        {
                            if (parent.TreeRight.Index == index)
                            {
                                parent.TreeRight = null;
                            }
                            else
                                parent.TreeLeft = null;
                        }
                        else
                            head = null;
                    }
                }
            }
            ////////////////////////////////////////////////////////////////////
            else
            {
                Tree<T> temp = condidat.TreeRight;
                while (temp.TreeLeft != null)
                {
                    temp = temp.TreeLeft;
                }
                temp.TreeLeft = condidat.TreeLeft;
                if (parent != null)
                {
                    if (parent.TreeRight.Index == index)
                    {
                        parent.TreeRight = condidat.TreeRight;
                    }
                    else
                        parent.TreeLeft = condidat.TreeRight;
                }
                else
                {
                    head = condidat.TreeRight;
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
                if (a.Data.TryGetTarget(out temp))
                {
                    if (temp.Index == index)
                    {
                        result = true;
                        break;
                    }
                }
            }

            if (result)
            {
                data = temp.Data;
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
