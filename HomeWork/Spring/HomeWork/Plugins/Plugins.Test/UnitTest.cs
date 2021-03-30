using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using Plugins;
using HelloWorld;
using MyInterface;
using Quote;
using System;

namespace PluginsTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestReadLibrary()
        {
            string path = Directory.GetCurrentDirectory();
            Console.WriteLine(path);
            List<IDisplayText> textes = new List<IDisplayText>();
            ReadLibrary<IDisplayText> tmp = new ReadLibrary<IDisplayText>();
            textes = tmp.FindPlugin(path);
            Assert.IsTrue(textes.Count == 2);
            Assert.AreEqual("Hello world", textes[0].Text());
            Assert.AreEqual("Don't know. Dont' care. Not my problem", textes[1].Text());


        }
    }
}