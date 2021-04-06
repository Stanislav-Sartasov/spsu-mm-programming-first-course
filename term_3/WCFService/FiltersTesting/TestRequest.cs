using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ServiceModel;
using FilterServiceLibrary;
using System.Diagnostics;
using System.IO;

namespace FiltersTesting
{
    class TestRequest
    {
        private int requestAmount;
        private byte[] image;
        private string pathResult;
        private string firstText;

        public TestRequest(int totalRequests, byte[] testImage, string resultpath)
        {
            pathResult = resultpath;
            requestAmount = totalRequests > 1 ? totalRequests : 1;
            image = testImage;
            firstText = "";
        }
        public TestRequest(int totalRequests, byte[] testImage, string resultpath, string titleTest)
        {
            pathResult = resultpath;
            requestAmount = totalRequests > 1 ? totalRequests : 1;
            image = testImage;
            firstText = titleTest;
        }
        public int MakeTest()
        {
            StreamWriter writer = new StreamWriter(pathResult);
            writer.WriteLine(firstText);
            Stopwatch stopwatch;
            for (int i = 0; i < requestAmount; ++i)
            {
                Console.Write($"-> {i} test ");
                stopwatch = new Stopwatch();
                stopwatch.Start();
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None, false);
                binding.MaxBufferSize = int.MaxValue;
                binding.MaxReceivedMessageSize = int.MaxValue;
                var factory = new ChannelFactory<IContract>(binding, new EndpointAddress("net.tcp://localhost:12345/srv"));
                var client = factory.CreateChannel();
                string filter = client.GetFilters()[0];
                Task<byte[]> filterWaitTask = new Task<byte[]>(() => client.ApplyFilter(filter, image));
                filterWaitTask.Start();
                int progress = 0;
                try
                {
                    while (progress < 100)
                    {
                        progress = client.GetStatus();
                        //Console.WriteLine($"progress task {progress}");
                        Thread.Sleep(100);
                    }
                    filterWaitTask.Wait();
                }
                catch (Exception)
                {

                }
                finally
                {
                    stopwatch.Stop();
                }
                writer.WriteLine(stopwatch.ElapsedMilliseconds);
                factory.Close();
                filterWaitTask.Dispose();
                Console.WriteLine($"{stopwatch.ElapsedMilliseconds} time");
            }
            writer.Close();
            writer.Dispose();
            return 0;
        }
    }
}
