using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Chat.Clients;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chat.Tests
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestConnection()
        {
            Client petya = new Client();
            Client vasya = new Client();
            Client tim = new Client();
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

            Assert.AreEqual(100, petya.ViewYourIp().Port);
            Assert.AreEqual(101, vasya.ViewYourIp().Port);
            Assert.AreEqual(102, tim.ViewYourIp().Port);

            petya.Connect(vasya.ViewYourIp());

            List<IPEndPoint> actualPetya = petya.ShowInterlocutors();
            Assert.AreEqual(1, actualPetya.Count);
            Assert.AreEqual(101, actualPetya[0].Port);
            Assert.AreEqual(vasya.ViewYourIp().Address, actualPetya[0].Address);
            Thread.Sleep(5000);
            List<IPEndPoint> actualVasya = vasya.ShowInterlocutors();
            Assert.AreEqual(1, actualVasya.Count);
            Assert.AreEqual(100, actualVasya[0].Port);
            Assert.AreEqual(petya.ViewYourIp().Address, actualVasya[0].Address);

            petya.Connect(tim.ViewYourIp());
            Thread.Sleep(5000);
            actualPetya = petya.ShowInterlocutors();
            Assert.AreEqual(2, actualPetya.Count);
            Assert.AreEqual(101, actualPetya[0].Port);
            Assert.AreEqual(102, actualPetya[1].Port);
            Assert.AreEqual(vasya.ViewYourIp().Address, actualPetya[0].Address);
            Assert.AreEqual(tim.ViewYourIp().Address, actualPetya[1].Address);

            List<IPEndPoint> actualTim = tim.ShowInterlocutors();
            Assert.AreEqual(2, actualTim.Count);
            Assert.AreEqual(100, actualTim[0].Port);
            Assert.AreEqual(101, actualTim[1].Port);
            Assert.AreEqual(petya.ViewYourIp().Address, actualTim[0].Address);
            Assert.AreEqual(vasya.ViewYourIp().Address, actualTim[1].Address);
        }

        [TestMethod]
        public void TestAddress()
        {
            string addressOne = "123.123.123.123:12312";
            string addressTwo = "123.123.123.123:123123";
            string addressThree = "678.687.687.687:12312";
            string addressFour = "A678.687.687.687:12312";

            Assert.AreEqual(true, Client.IsCorrectInputAddress(addressOne));
            Assert.AreEqual(false, Client.IsCorrectInputAddress(addressTwo));
            Assert.AreEqual(false, Client.IsCorrectInputAddress(addressThree));
            Assert.AreEqual(false, Client.IsCorrectInputAddress(addressFour));
        }

        [TestMethod]
        public void TestDisconnection()
        {
            Client petya = new Client();
            Client vasya = new Client();
            Client tim = new Client();
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

            Assert.AreEqual(100, petya.ViewYourIp().Port);
            Assert.AreEqual(101, vasya.ViewYourIp().Port);
            Assert.AreEqual(102, tim.ViewYourIp().Port);

            petya.Connect(vasya.ViewYourIp());
            petya.Connect(tim.ViewYourIp());
            Thread.Sleep(5000);

            List<IPEndPoint> actualTim = tim.ShowInterlocutors();
            Assert.AreEqual(2, actualTim.Count);
            Assert.AreEqual(100, actualTim[0].Port);
            Assert.AreEqual(101, actualTim[1].Port);
            Assert.AreEqual(petya.ViewYourIp().Address, actualTim[0].Address);
            Assert.AreEqual(vasya.ViewYourIp().Address, actualTim[1].Address);

            petya.Disconnect();
            Thread.Sleep(5000);
            actualTim = tim.ShowInterlocutors();
            Assert.AreEqual(1, actualTim.Count);
            Assert.AreEqual(101, actualTim[0].Port);
            Assert.AreEqual(vasya.ViewYourIp().Address, actualTim[0].Address);
        }
    }
}
