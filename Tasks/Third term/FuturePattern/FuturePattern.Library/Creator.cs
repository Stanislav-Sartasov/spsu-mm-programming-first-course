using System;
using System.Collections.Generic;
using System.Text;

namespace FuturePattern.Library
{
    public static class Creator
    {
        public static IVectorLengthComputer Create(int number)
        {
            if (number == 0)
                return new Cascade();
            if (number == 1)
                return new ModifiedCascade();
            return null;
        }
    }
}
