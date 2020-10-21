using System;

namespace Filter.MagicConst
{
    public enum Protocol : byte
    {
        Image = 1,
        Filter = 2,
        Progress = 3,

        StopCode = 101
    }
}
