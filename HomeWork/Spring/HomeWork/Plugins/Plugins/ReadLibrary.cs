using log4net.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Plugins
{
    public interface IPlugin
    {
        string Activate(string input);
          
    }
    public class ReadLibrary<IPlugin> where IPlugin : class
    {
       private List<IPlugin> plugins = new List<IPlugin>();
        
       public List<IPlugin> FindPlugin(string path)
        {
            plugins.Clear();
            string[] files = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                Assembly asm = Assembly.LoadFrom(file);
                var types = asm.GetTypes().
                        Where(t => t.GetInterfaces().
                        Where(i => i.FullName == typeof(IPlugin).FullName).Any());


                foreach (var type in types)
                {
                    var plugin = asm.CreateInstance(type.FullName) as IPlugin;
                    plugins.Add(plugin);
                }

            }
            return plugins;
        }
    }
}
