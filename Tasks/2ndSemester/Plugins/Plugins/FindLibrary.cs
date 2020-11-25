using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Plugins
{
    public static class FindLibrary<IPlugin> where IPlugin : class
    {
        public static List<IPlugin> FindPlugins(string pluginPath)
        {
            List<IPlugin> plugins = new List<IPlugin>();
            string[] pluginFiles = Directory.GetFiles(pluginPath, "*.dll", SearchOption.AllDirectories);

            foreach (string file in pluginFiles)
            {
                Assembly assembly = Assembly.LoadFile(file);
                if (assembly != null)
                    foreach (Type type in assembly.GetTypes())
                        if (type.GetInterfaces().Contains(typeof(IPlugin)))
                        {
                            var plugin = Activator.CreateInstance(type) as IPlugin;
                            plugins.Add(plugin);
                        }
            }

            return plugins;
        }
    }
}
