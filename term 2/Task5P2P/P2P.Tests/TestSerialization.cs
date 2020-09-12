using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace P2P.Tests
{
    [TestClass]
    public class TestSerialization
    {
        [TestMethod]
        public void TestSerializeMethods()
        {
            List<EndPoint> endPoints = new List<EndPoint>();
            string[] ips = new string[] { "111.111.111.20:20", "111.151.111.20:40", "211.111.111.20:30" };
            IPEndPoint end = null;
            foreach (string ip in ips)
            {
                IPEndPoint.TryParse(ip, out end);
                endPoints.Add(end);
            }
            byte[] data = Serialization.Serialization.Serialize(endPoints, '0');
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Encoding.Unicode.GetString(data, 0, data.Length));
            List<EndPoint> newEndPoints = Serialization.Serialization.Deserialize(stringBuilder);
            foreach (EndPoint endPoint in endPoints)
                Assert.IsTrue(newEndPoints.Contains(endPoint));
            foreach (EndPoint endPoint in newEndPoints)
                Assert.IsTrue(endPoints.Contains(endPoint));
        }
        

    }
}
