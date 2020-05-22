using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Task3Plugins
{
    
    public class GetLib<IPlugin> where IPlugin : class
    {
        
        private static List<IPlugin> plugins = new List<IPlugin>();
        
        public static List<IPlugin> GetPlugins(string pluginPath)
        {
            string[] pluginFiles = Directory.GetFiles(pluginPath, "*.dll", SearchOption.AllDirectories);
            foreach (string file in pluginFiles)
            {
                Assembly assembly = Assembly.LoadFile(file);
                if (assembly != null)
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (type.GetInterfaces().Contains(typeof(IPlugin))) 
                        {
                            var plugin = Activator.CreateInstance(type) as IPlugin;
                            plugins.Add(plugin);
                        }
                    }
                }
            }
            return plugins;
        }



    }
}
