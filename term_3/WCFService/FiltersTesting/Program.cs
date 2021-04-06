using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ServiceModel;
using FilterServiceLibrary;
using System.Drawing;
using System.IO;

namespace FiltersTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadGenerator generator;
            TestRequest test;
            byte[] image = null;
            string pathWorking = Directory.GetCurrentDirectory() + "\\..\\..\\TestData\\";

            StreamReader reader = new StreamReader(pathWorking + "imageList.txt");
            List<string> images = new List<string>();
            while (!reader.EndOfStream)
            {
                images.Add(reader.ReadLine());
            }
            reader.Close();
            reader.Dispose();
            foreach (string source in images)
            {
                image = GetBytesImage(pathWorking + source);
                generator = new LoadGenerator(20, image);
                test = new TestRequest(150, image, pathWorking + source + "_Test_20perSec.txt", "requests 20 per second image file: " + source);
                generator.Start();
                Thread.Sleep(300);
                test.MakeTest();
                generator.Stop();

                Thread.Sleep(5000);

                image = GetBytesImage(pathWorking + source);
                generator = new LoadGenerator(40, image);
                test = new TestRequest(150, image, pathWorking + source + "_Test_40perSec.txt", "requests 40 per second image file: " + source);
                generator.Start();
                Thread.Sleep(300);
                test.MakeTest();
                generator.Stop();
            }
        }
        static byte[] GetBytesImage(string path)
        {
            byte[] bytesForSend = null;
            Bitmap imageSource = new Bitmap(path);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                imageSource.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                bytesForSend = memoryStream.GetBuffer();
            }
            imageSource.Dispose();
            return bytesForSend;
        }
    }
}
