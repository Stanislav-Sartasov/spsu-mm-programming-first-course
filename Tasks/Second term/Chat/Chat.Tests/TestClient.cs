using Chat.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Chat.Tests
{
    [TestClass]
    public class TestClient
    {
        [TestMethod]
        public void TestConnection()
        {
            User petya = new User();
            User vasya = new User();
            User tim = new User();
            IPEndPoint ipPetya = null;
            IPEndPoint ipVasya = null;
            IPEndPoint ipTim = null;

            foreach (var iPAdress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                if (iPAdress.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipPetya = new IPEndPoint(iPAdress, 100);
                    break;
                }
            foreach (var iPAdress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                if (iPAdress.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipVasya = new IPEndPoint(iPAdress, 101);
                    break;
                }
            foreach (var iPAdress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                if (iPAdress.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipTim = new IPEndPoint(iPAdress, 102);
                    break;
                }
            petya.GetAddress("Petya", ipPetya);
            vasya.GetAddress("Vasya", ipVasya);
            tim.GetAddress("Tim", ipTim);
            //petya.Waiting();
            vasya.Waiting();
            tim.Waiting();

            Assert.AreEqual(100, petya.ShowYourIp().Port);
            Assert.AreEqual(101, vasya.ShowYourIp().Port);
            Assert.AreEqual(102, tim.ShowYourIp().Port);

            petya.Connect(vasya.ShowYourIp());

            List<IPEndPoint> actualPetya = petya.ShowListOfConnections();
            Assert.AreEqual(1, actualPetya.Count);
            Assert.AreEqual(101, actualPetya[0].Port);
            Assert.AreEqual(vasya.ShowYourIp().Address, actualPetya[0].Address);
            Thread.Sleep(5000);
            List<IPEndPoint> actualVasya = vasya.ShowListOfConnections();
            Assert.AreEqual(1, actualVasya.Count);
            Assert.AreEqual(100, actualVasya[0].Port);
            Assert.AreEqual(petya.ShowYourIp().Address, actualVasya[0].Address);

            petya.Connect(tim.ShowYourIp());
            Thread.Sleep(5000);
            actualPetya = petya.ShowListOfConnections();
            Assert.AreEqual(2, actualPetya.Count);
            Assert.AreEqual(101, actualPetya[0].Port);
            Assert.AreEqual(102, actualPetya[1].Port);
            Assert.AreEqual(vasya.ShowYourIp().Address, actualPetya[0].Address);
            Assert.AreEqual(tim.ShowYourIp().Address, actualPetya[1].Address);

            List<IPEndPoint> actualTim = tim.ShowListOfConnections();
            Assert.AreEqual(2, actualTim.Count);
            Assert.AreEqual(100, actualTim[0].Port);
            Assert.AreEqual(101, actualTim[1].Port);
            Assert.AreEqual(petya.ShowYourIp().Address, actualTim[0].Address);
            Assert.AreEqual(vasya.ShowYourIp().Address, actualTim[1].Address);
        }

        [TestMethod]
        public void TestDisconnection()
        {
            User petya = new User();
            User vasya = new User();
            User tim = new User();
            IPEndPoint ipPetya = null;
            IPEndPoint ipVasya = null;
            IPEndPoint ipTim = null;

            foreach (var iPAdress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                if (iPAdress.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipPetya = new IPEndPoint(iPAdress, 100);
                    break;
                }
            foreach (var iPAdress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                if (iPAdress.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipVasya = new IPEndPoint(iPAdress, 101);
                    break;
                }
            foreach (var iPAdress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                if (iPAdress.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipTim = new IPEndPoint(iPAdress, 102);
                    break;
                }
            petya.GetAddress("Petya", ipPetya);
            vasya.GetAddress("Vasya", ipVasya);
            tim.GetAddress("Tim", ipTim);
            //petya.Waiting();
            vasya.Waiting();
            tim.Waiting();

            Assert.AreEqual(100, petya.ShowYourIp().Port);
            Assert.AreEqual(101, vasya.ShowYourIp().Port);
            Assert.AreEqual(102, tim.ShowYourIp().Port);

            petya.Connect(vasya.ShowYourIp());
            petya.Connect(tim.ShowYourIp());
            Thread.Sleep(5000);

            List<IPEndPoint> actualTim = tim.ShowListOfConnections();
            Assert.AreEqual(2, actualTim.Count);
            Assert.AreEqual(100, actualTim[0].Port);
            Assert.AreEqual(101, actualTim[1].Port);
            Assert.AreEqual(petya.ShowYourIp().Address, actualTim[0].Address);
            Assert.AreEqual(vasya.ShowYourIp().Address, actualTim[1].Address);

            petya.Disconnect();
            Thread.Sleep(5000);
            actualTim = tim.ShowListOfConnections();
            Assert.AreEqual(1, actualTim.Count);
            Assert.AreEqual(101, actualTim[0].Port);
            Assert.AreEqual(vasya.ShowYourIp().Address, actualTim[0].Address);
        }
    }
}
