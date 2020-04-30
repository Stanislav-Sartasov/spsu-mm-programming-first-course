using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GetLib
{
    public static class GetLib<currentInterface>
    {
        public static List<currentInterface> FromDirectory(string fullFileName)
        {
            try
            {
                List<currentInterface> necessaryObjectList = new List<currentInterface> { };

                foreach (var dll in Directory.GetFiles(fullFileName, "*.dll"))
                    foreach (var currentType in Assembly.LoadFile(dll).GetTypes())
                        if (currentType.GetInterfaces().Contains(typeof(currentInterface)))
                            necessaryObjectList.Add((currentInterface)Activator.CreateInstance(currentType));

                return necessaryObjectList;
            }
            catch
            {
                return null;
            }
        }
    }
}
