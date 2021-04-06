using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using FilterServiceLibrary;
using System.Threading;

namespace FiltersTesting
{
    public class LoadGenerator
    {
        private int requestPerSecond;
        private byte[] image;
        private volatile List<Task> loadList;
        private Task taskCleaner;
        private Task taskloader;
        private CancellationTokenSource tokenSourceStop = null;
        private CancellationToken stopToken;
        private bool isRunned;

        public LoadGenerator(int amountOfReqestSecond, byte[] testImage)
        {
            requestPerSecond = amountOfReqestSecond > 1 ? amountOfReqestSecond : 1;
            loadList = new List<Task>();
            tokenSourceStop = new CancellationTokenSource();
            stopToken = tokenSourceStop.Token;
            isRunned = false;
            image = testImage;
        }
        public void Start()
        {
            if (isRunned)
            {
                return;
            }
            taskloader = new Task(() => DoLoad(stopToken));
            taskloader.Start();
            taskCleaner = new Task(() => DoCleaning(stopToken));
            taskCleaner.Start();
            isRunned = true;
        }
        public void Stop()
        {
            if (!isRunned)
            {
                return;
            }
            isRunned = false;
            tokenSourceStop.Cancel();
            taskloader.Wait();
            taskCleaner.Wait();
            taskloader.Dispose();
            taskCleaner.Dispose();
            tokenSourceStop.Dispose();
            loadList.Clear();
        }
        private void DoCleaning(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                int length = loadList.Count;
                for (int i = 0; i < length; ++i)
                {
                    
                    if (loadList[i].IsCompleted)
                    {
                        loadList[i].Dispose();
                        //loadList.RemoveAt(i);
                        //--length;
                    }
                }
                Thread.Sleep(500);
            }
        }
        private void DoLoad(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                for (int i = 0; i < requestPerSecond; ++i)
                {
                    Task temp = new Task(TaskWorking);
                    temp.Start();
                    loadList.Add(temp);
                }
                Thread.Sleep(1000);
            }
        }
        private void TaskWorking()
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None, false);
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            var factory = new ChannelFactory<IContract>(binding, new EndpointAddress("net.tcp://localhost:12345/srv"));
            var client = factory.CreateChannel();
            Task<byte[]> filterWaitTask = null;
            bool taskMade = false;
            try
            {
                string filter = client.GetFilters()[0];
                filterWaitTask = new Task<byte[]>(() => client.ApplyFilter(filter, image));
                filterWaitTask.Start();
                int progress = 0;
                while (progress < 100)
                {
                    progress = client.GetStatus();
                    Thread.Sleep(200);
                }
                filterWaitTask.Wait();
                taskMade = true;
            }
            catch
            {
                Console.WriteLine("error in requestLoad\n");
                Console.ReadLine();
            }
            factory.Close();
            if (taskMade)
                filterWaitTask.Dispose();
        }
    }
}
