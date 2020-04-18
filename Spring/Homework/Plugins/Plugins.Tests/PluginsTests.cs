using Microsoft.VisualStudio.TestTools.UnitTesting;
using SomeInterface;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Plugins.Tests
{
    [TestClass]
    public class PluginsTests
    {
        static readonly string path = string.Format("{0}SomeImplemetations\\bin", Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\")));

        LibraryFinder intFinder;
        LibraryFinder strFinder;
        Type strType = typeof(ISomeInterface<string>);
        Type intType = typeof(ISomeInterface<int>);
        IEnumerable<ISomeInterface<string>> implementingStr—lasses;
        IEnumerable<ISomeInterface<int>> implementingInt—lasses;

        [TestInitialize]
        public void PluginsTestsInit()
        {
            try
            {
                strType = typeof(ISomeInterface<string>);
                intType = typeof(ISomeInterface<int>);
                strFinder = new LibraryFinder(strType, path);
                intFinder = new LibraryFinder(intType, path);
                implementingStr—lasses = strFinder.GetImplementing—lasses().Select(obj => (ISomeInterface<string>)obj);
                implementingInt—lasses = intFinder.GetImplementing—lasses().Select(obj => (ISomeInterface<int>)obj);
            }
            catch
            {
                throw new Exception("Initialization error");
            }
        }

        [TestMethod]
        public void CorrectNumberOfStrClasses()
        {
            Assert.AreEqual(1, implementingStr—lasses.Count());
        }

        [TestMethod]
        public void CorrectNumberOfIntClasses()
        {
            Assert.AreEqual(1, implementingInt—lasses.Count());
        }

        [TestMethod]
        public void CorrectStrClassFunctionality()
        {
            foreach (var cl in implementingStr—lasses)
            {
                cl.Set("correct");
                Assert.AreEqual("correct", cl.Get());
                Assert.AreEqual("StringValue class", cl.GetInfo());
            }
        }

        [TestMethod]
        public void CorrectIntClassFunctionality()
        {
            foreach (var cl in implementingInt—lasses)
            {
                cl.Set(1);
                Assert.AreEqual(1, cl.Get());
                Assert.AreEqual("IntValue class", cl.GetInfo());
            }
        }
    }
}
