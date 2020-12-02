using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Plugins.Interface;

namespace Plugins.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void Test()
        {
            string path = Directory.GetCurrentDirectory() + @"..\..\..\..\..\DllFiles\";
            List<IGame> games = FindLibrary<IGame>.FindPlugins(path);

            Assert.AreEqual(2, games.Count);

            for (int i = 0; i < games.Count; i++)
            {
                switch (games[i].GetType().FullName)
                {
                    case "Plugins.FirstDll.Doka2":
                        Assert.AreEqual(90, games[i].GetMetacriticRating());
                        break;
                    case "Plugins.SecondDll.HW40":
                        Assert.AreEqual(87, games[i].GetMetacriticRating());
                        break;
                }
            }
        }
    }
}
