using SomeInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Plugins
{
    public class LibraryFinder
    {
        private IEnumerable<object> implementingСlasses = null;
        private string path;
        private Type desiredType;

        public LibraryFinder(Type desiredType, string path)
        {
            SetPath(path);
            this.desiredType = desiredType;
        }

        private void LoadImplementingСlasses()
        {
            try
            {
                implementingСlasses = from lib in Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories)
                                      from type in Assembly.LoadFrom(lib).GetExportedTypes()
                                      where type.GetInterfaces().Contains(desiredType)
                                      select type.GetConstructor(new Type[] { }).Invoke(new object[] { });
            }
            catch
            {
                throw new Exception("Failed to read files");
            }
        }

        public void SetDesiredType(Type desiredType)
        {
            if (!Directory.Exists(path))
            {
                throw new Exception("This directory doesn't exist!");
            }
            this.desiredType = desiredType;
        }

        public void SetPath(string path)
        {
            this.path = path;
        }

        public IEnumerable<object> GetImplementingСlasses()
        {
            LoadImplementingСlasses();
            return implementingСlasses;
        }
    }
}
