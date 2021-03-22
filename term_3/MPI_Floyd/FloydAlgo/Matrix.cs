using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloydAlgo
{
    [Serializable()]
    public class Matrix
    {
        public long Height { get; private set; }
        public long Width { get; private set; }
        public long [,] Array { get; set; }
        public Matrix(long number, long[,] array)
        {
            Height = number;
            Width = number;
            Array = array;
        }
        public Matrix(long height, long width, long[,] array)
        {
            Height = height;
            Width = width;
            Array = array;
        }
    }
}
