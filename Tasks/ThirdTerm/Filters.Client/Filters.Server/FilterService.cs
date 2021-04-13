using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.ServiceModel;

namespace Filter.Server
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class FilterService : IFilterService
    {
        private Picture picture;

        public FilterService()
        {
            picture = new Picture();
        }

        public void ApplyFilter(byte[] img, string filterName)
        {
            try
            {
                Bitmap bmp;
                using (var ms = new MemoryStream(img))
                {
                    bmp = new Bitmap(ms);
                }

                if (picture.ApplyFilter(bmp, filterName, (uint)bmp.Width))
                {
                    using (var ms = new MemoryStream())
                    {
                        bmp.Save(ms, ImageFormat.Bmp);
                        OperationContext.Current
                            .GetCallbackChannel<IFilterServiceCallback>()
                            .ImageCallback(ms.ToArray());
                    }
                }
                else
                {
                    Console.WriteLine("Stopped");
                    OperationContext.Current
                        .GetCallbackChannel<IFilterServiceCallback>()
                        .ProgressCallback(0);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public string[] GetFilters()
        {
            return ConfigurationManager.AppSettings.AllKeys;
        }

        public void StopFiltering()
        {
            picture.Stop();
        }
    }
}