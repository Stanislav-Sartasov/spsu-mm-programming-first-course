using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPI
{
    public struct Block : IComparable
    {
        public int Number { get; }
        public List<int> Data { get; set; }

        public Block(int number, List<int> data)
        {
            Number = number;
            Data = data;
        }

        public int CompareTo(object block)
        {
            return Number.CompareTo(((Block)block).Number);
        }
    }
}
