using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryJeneric
{
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
    public class JenericTree<T>// where T : IComparable
    {
        Tree<T> head;
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
        private Tree<T> Search(T dat)
        {
            int index = dat.GetHashCode();
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

            return temp;
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
        public bool IsInTree(T dat)
        {
            return Search(dat) == null ? false : true;
        }
        public bool Find(int index, ref T data)
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
        public void Delete(T dat)
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
    }
}
