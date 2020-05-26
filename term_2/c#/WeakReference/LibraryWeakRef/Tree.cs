using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryWeakRef
{
    internal class Tree<T> //where T : IComparable
    {
        public T Data
        {
            get;
            set;
        }
        public int Index
        {
            get;
            set;
        }
        public Tree<T> TreeRight
        {
            get;
            set;
        }
        public Tree<T> TreeLeft
        {
            get;
            set;
        }
        public Tree()
        {
            TreeLeft = null;
            TreeRight = null;
            Index = 0;
        }
        public Tree(T Data)
        {
            TreeLeft = null;
            TreeRight = null;
            this.Data = Data;
            Index = Data.GetHashCode();////////////////////////////////////////////////////////////////////////////////
        }
    }
}
