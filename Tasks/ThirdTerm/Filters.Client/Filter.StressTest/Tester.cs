using ServiceReference1;
using System;
using System.Linq;
using System.Threading;
using System.ServiceModel;

namespace ServerTesting
{
    internal class Tester : IFilterServiceCallback
    {
        private AutoResetEvent waitHandler = new AutoResetEvent(false);

        private readonly byte[] originalBytes;

        private byte[] resultBytes;

        private volatile bool isRunning;

        private volatile bool isComplete;

        private DateTime startTime;

        private DateTime endTime;

        public Tester(byte[] bytes)
        {
            originalBytes = bytes;
            resultBytes = null;
            isComplete = false;
            isRunning = false;
            startTime = DateTime.MinValue;
            endTime = DateTime.MinValue;
            Start();
        }

        private void Start()
        {

            startTime = DateTime.Now;
            try
            {
                FilterServiceClientBase client = new FilterServiceClientBase(new InstanceContext(this));
                client.ApplyFilter(originalBytes, client.GetFilters().First());
                isRunning = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        public void ImageCallback(byte[] bytes)
        {
            resultBytes = bytes;
            endTime = DateTime.Now;
            isComplete = true;
            isRunning = false;
            waitHandler.Set();
        }

        public void ProgressCallback(int progress)
        {
            return;
        }

        public int GetTime()
        {
            if (isComplete || isRunning)
            {
                waitHandler.WaitOne(60000);
                if (resultBytes == null)
                {
                    return -1;
                }
                return (int)(endTime - startTime).TotalMilliseconds;
            }
            return 0;
        }
    }
}