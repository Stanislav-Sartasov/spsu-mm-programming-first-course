using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryJeneric
{
    public class JenericTree<T>// where T : IComparable
    {
        private Tree<T> head;
        public JenericTree()
        {
            head = null;
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
        private Tree<T> Search(T dat)
        {
            int index = dat.GetHashCode();
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

            return temp;
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
        public bool IsInTree(T dat)
        {
            return Search(dat) == null ? false : true;
        }
        public bool Find(int index, ref T data)
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
        public void Delete(T dat)
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
    }
}
