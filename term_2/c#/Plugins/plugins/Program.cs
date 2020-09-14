using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using InterfacePlugin;

namespace plugins
{
    class Program
    {
        static void Main(string[] args)
        {
            String s = Console.ReadLine();
            string[] FileNames = { "", "" };
            if ((!File.Exists(s)) & (Directory.Exists(s)))
            {
                FileNames = Directory.GetFiles(s, "*.dll");
            }
            else
            {
                Console.WriteLine("BAD PATH");
                Console.ReadKey();
                return;
            }

            List<IPlugin> MyPlugins = new List<IPlugin>();
            MyPlugins.Clear();
            try
            {
                foreach (var Name in FileNames)
                {


                    Assembly asm = Assembly.LoadFrom(Name);

                    var types = asm.GetTypes().Where(ttype => ttype.GetInterfaces().Where(iinterface => iinterface.FullName == typeof(IPlugin).FullName).Any());

                    foreach (var type in types)
                    {
                        IPlugin plugin = asm.CreateInstance(type.FullName) as IPlugin;
                        MyPlugins.Add(plugin);
                    }
                }

                foreach (var plugin in MyPlugins)
                    plugin.StartAction();
            }
            catch(System.BadImageFormatException)
            {
                Console.WriteLine("BAD FILE, probably it is not C# dll");
            }
            Console.ReadKey();


        }
    }
}
