using Chat.StringMethod;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Chat.Tests
{
    [TestClass]
    public class StringTest
    {
        [TestMethod]
        public void TestCompareIP()
        {
            IPEndPoint ipOne = null;
            IPEndPoint ipTwo = null;
            foreach (var iPAdress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                if (iPAdress.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipOne = new IPEndPoint(iPAdress, 100);
                    break;
                }
            foreach (var iPAdress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                if (iPAdress.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipTwo = new IPEndPoint(iPAdress, 101);
                    break;
                }

            Assert.IsFalse(Strings.CompareIp(ipOne, ipTwo));
            Assert.IsTrue(Strings.CompareIp(ipOne, ipOne));
        }

        [TestMethod]
        public void TestParseToByteList()
        {
            string[] temp = new string[] { "1", "1", "1", "1", "1" };
            List<byte> result = new List<byte>();
            for (int j = 0; j < 4; j++)
                result.Add(byte.Parse(temp[j]));
            UInt32 ip = BitConverter.ToUInt32(result.ToArray(), 0);
            UInt16 port = UInt16.Parse(temp[4]);
            IPEndPoint iP = new IPEndPoint(ip, port);

            List<byte> actual = Strings.ParseAddressToByteList(iP);

            for (int i = 0; i < 5; i++)
                Assert.AreEqual(1, actual[i]);
            Assert.AreEqual(0, actual[5]);
        }

        [TestMethod]
        public void TestParseToIPList()
        {
            byte[] address = new byte[] { 1, 1, 1, 1, 1, 0 };
            IPEndPoint iP = Strings.ParseByteListToAddress(address)[0];
            Assert.IsTrue(String.Equals(iP.ToString(), "1.1.1.1:1"));
        }

        [TestMethod]
        public void TestParseToIP()
        {
            string temp = "1.1.1.1:1";
            IPEndPoint iP;
            Assert.IsTrue(Strings.TryParseIp(temp, out iP));
            Assert.IsNotNull(iP);
            Assert.IsTrue(String.Equals(iP.ToString(), "1.1.1.1:1"));
        }

        [TestMethod]
        public void TestRemoveSpace()
        {
            string temp = "1     1";

            string actual = Strings.RemoveSpace(temp);
            string expected = "11";

            Assert.IsTrue(String.Equals(expected, actual));
        }
    }
}
