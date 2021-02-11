using StressTest.FilterService;
using System;
using System.Linq;
using System.ServiceModel;

namespace StressTest
{
    public class MockClient : IFilterServiceCallback
    {
        private byte[] input;
        private TimeSpan responseTime;
        private DateTime start;
        private string filterName;
        private volatile bool isRunning;
        private volatile bool isResponseInvalid;

        public MockClient(byte[] img, string filterName)
        {
            this.filterName = filterName;
            input = img;
            isRunning = false;
            isResponseInvalid = true;
        }

        private bool Start()
        {
            try
            {
                var client = new FilterServiceClient(new InstanceContext(this));
                var filters = client.GetFilters();
                if (!filters.Contains(filterName))
                    return false;
                start = DateTime.Now;
                client.ApplyFilter(input, filterName);
                isRunning = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void ImageCallback(byte[] img)
        {
            responseTime = DateTime.Now - start;
            isResponseInvalid = img is null;
            isRunning = false;
        }

        public double GetResponseTime()
        {
            if (!Start())
                return double.NaN;
            while (isRunning) { };
            return isResponseInvalid ? 0 : responseTime.TotalMilliseconds;
        }

        public void ProgressCallback(int progress) { }
    }
}
