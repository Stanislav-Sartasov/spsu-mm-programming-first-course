using FilterServiceTests.FilterService;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;


namespace FilterServiceTests
{
    public class MockClient : IFilterServiceCallback
    {
        private Bitmap input;
        private Bitmap output;
        private FilterServiceClient client;
        private string filterName;
        private volatile bool isRunning;

        public MockClient(Bitmap img, string filterName)
        {
            this.filterName = filterName;
            input = img;
            isRunning = false;
        }

        private bool Start()
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    client = new FilterServiceClient(new System.ServiceModel.InstanceContext(this));
                    var filters = client.GetFilters();
                    if (!filters.Contains(filterName))
                        return false;        
                    input.Save(ms, ImageFormat.Bmp);
                    client.ApplyFilter(ms.ToArray(), filterName);
                    isRunning = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void ImageCallback(byte[] img)
        {
            using (var ms = new MemoryStream(img))
            {
                output = new Bitmap(ms);
                isRunning = false;
            }
        }


        public Bitmap GetResult()
        {
            if (!Start())
                return null;
            while (isRunning) { };
            return output;
        }

        public void ProgressCallback(int progress) { }       
    }
}
