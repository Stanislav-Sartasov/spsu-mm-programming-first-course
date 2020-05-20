using Plugins.InterfaceLibrery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;


namespace Plugins
{
    public class LibreryAnalysis
    {
        public LibreryAnalysis(string path)
        {
            FileInfo[] files = new DirectoryInfo(path).GetFiles("*.dll");
            for (int i = 0; i < files.Length; i++)
            {
                Type[] type = Assembly.LoadFile(files[i].FullName).GetTypes();
                foreach (Type o in type)
                {
                    var arrayOfInterfaces = o.GetInterfaces();
                    foreach (var typeInterface in arrayOfInterfaces)
                        if (typeInterface.Equals(typeof(ICream)))
                            suitableTypes.Add(o);
                }
            }
        }

        private List<Type> suitableTypes = new List<Type>();
        public List<Type> ShowTypesInFiles()
        {
            return suitableTypes;
        }

        public int Length()
        {
            int i = 0;
            foreach (var o in suitableTypes)
                i++;
            return i;
        }
        public object Instantiation(int index)
        {
            var result = suitableTypes[index].GetConstructors()[0].Invoke(null);
            return result;
        }
    }
}
