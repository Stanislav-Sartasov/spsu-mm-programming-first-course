using System;
using System.Collections.Generic;
using System.IO;
using InterfaceLibrary;


namespace Task3Plugins
{
    static class Program
    {
        static void Main()
        {
            string path = Directory.GetCurrentDirectory();
            List<IParty> parties = GetLib<IParty>.GetPlugins(path);
            for (int i = 0; i < parties.Count; i++)
            {
                Console.WriteLine(parties[i].TrustRating());
            }
            Console.ReadKey();
        }
    }
}
