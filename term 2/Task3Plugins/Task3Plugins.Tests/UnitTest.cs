using System;
using Task3Plugins;
using InterfaceLibrary;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using PartyOfRussia;

namespace Task3Plugins.Tests
{
    [TestClass]
    public class UnitTest
    {
        
        [TestMethod]
        public void TestGetPluginsMethod()
        {
            string path = Directory.GetCurrentDirectory();
            List<IParty> parties = GetLib<IParty>.GetPlugins(path);
            Assert.IsTrue(parties.Count == 4);
            foreach (IParty party in parties)
            {
                switch (party.GetType().FullName)
                {
                    case "PartyOfRussia.LDPR":
                        Assert.AreEqual(14, party.TrustRating());
                        break;
                    case "PartyOfRussia.UnitedRussia":
                        Assert.AreEqual(100, party.TrustRating());
                        break;
                    case "PartyOfUSA.Democratic":
                        Assert.AreEqual(49, party.TrustRating());
                        break;
                    case "PartyOfUSA.Republican":
                        Assert.AreEqual(51, party.TrustRating());
                        break;
                }
            }
        }
    }
}
