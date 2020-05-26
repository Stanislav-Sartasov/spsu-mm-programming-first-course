using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryWeakRef
{
    internal class TimeTree<T>
    {
        public TimeTree(Tree<T> dat)
        {
            Data = new WeakReference<Tree<T>>(dat);
            creation = DateTime.Now;
        }
        public WeakReference<Tree<T>> Data
        {
            get;
            set;
        }
        public readonly DateTime creation;
    }
}
