using FilterServiceContract;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;


namespace FilterServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(FilterService.FilterService), new Uri("net.tcp://localhost:8080"));
            host.Description.Behaviors.Add(new ServiceMetadataBehavior());
            host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "/mex");
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None, false);
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            host.AddServiceEndpoint(typeof(IFilterService), binding, "/srv");
            host.Open();
            Console.WriteLine("Press any button to stop");
            Console.ReadKey();
            host.Close();
        }
    }
}
