using System;
using System.Collections.Generic;
using System.Text;

namespace FuturePattern.Library
{
    public static class Creator
    {
        private static Dictionary<string, IVectorLengthComputer> schemes;

        private static void Initialize()
        {
            schemes = new Dictionary<string, IVectorLengthComputer>();
            schemes["Cascade"] = new Cascade();
            schemes["ModifiedCascade"] = new ModifiedCascade();
        }
        public static IVectorLengthComputer Create(string name)
        {
            Initialize();
            if (schemes.ContainsKey(name))
                return schemes[name];
            return null;
        }
    }
}
