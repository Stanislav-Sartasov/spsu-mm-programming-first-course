using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using FilterServiceLibrary;

namespace WCFService
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(Contract), new Uri("net.tcp://localhost:12345"));

            host.Description.Behaviors.Add(new ServiceMetadataBehavior());
            host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "/mex");

            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None, false);
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            host.AddServiceEndpoint(typeof(IContract), binding, "/srv");

            host.Open();

            Console.WriteLine("Server started.\nTo correctly shut down the server, enter \"close\":");
            while (Console.ReadLine().ToLower() != "close")
            {
                Console.WriteLine("Unknown command");
            }

            host.Close();
        }
    }
}
