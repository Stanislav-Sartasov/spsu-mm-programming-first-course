using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ServiceModel;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Configuration;

namespace FilterServiceLibrary
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession)]
    public class Contract : IContract
    {
        private volatile int progress;
        private volatile string str;
        /// <summary>
        private void RunCmdFilter(string fileIn, string filter, string fileOut)
        {
            string program = "filters.exe";
            string command = $"{program} {fileIn} {filter} {fileOut}";
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = false;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.WriteLine($"cd {Directory.GetCurrentDirectory()}\\..\\..\\WorkingFolder");
            cmd.StandardInput.WriteLine(command);
            for (int i = 0; i < 7; ++i)
            {
                string temp = cmd.StandardOutput.ReadLine();
                //Console.WriteLine(temp);
            }
            //Console.WriteLine("END--------------");
            while (!cmd.StandardOutput.EndOfStream)
            {
                string line = cmd.StandardOutput.ReadLine();
                //Console.WriteLine($"{line} from image");
                progress = int.Parse(line);
                if (progress == 100)
                    break;
            }

            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
        }
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public byte[] ApplyFilter(string filter, byte[] image)
        {
            progress = 0;
            //Console.WriteLine("ApplyFilter is running");
            str = filter;
            string pred = Guid.NewGuid().ToString();
            string pathFile = Directory.GetCurrentDirectory() + "\\..\\..\\WorkingFolder\\";
            string fileIn = pred + "In.bmp";
            string fileOut = pred + "Out.bmp";
            Bitmap imageToFilter;
            byte[] bytesOut = null;
            try 
            {
                using (MemoryStream memoryStream = new MemoryStream(image))
                {
                    imageToFilter = (Bitmap)Bitmap.FromStream(memoryStream);
                    imageToFilter.Save(pathFile + fileIn, System.Drawing.Imaging.ImageFormat.Bmp);
                }
                RunCmdFilter(fileIn, filter, fileOut);
                Bitmap doneImage = new Bitmap(pathFile + fileOut);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    doneImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                    bytesOut = memoryStream.GetBuffer();
                }
                doneImage.Dispose();
                File.Delete(pathFile + fileOut);
            }
            catch (Exception e)
            {
                Console.WriteLine("error in apply filter\n" + e.Message);
            }
            finally
            {
                File.Delete(pathFile + fileIn);
            }
            //for (int i = 0; i < 10; ++i)
            //{
            //    Thread.Sleep(500);
            //    Console.WriteLine($"sleem times {i}, progress is {progress}, tread is {str}");
            //    progress += 10;
            //}
            progress = 100;
            //Console.WriteLine($"is filtred");
            return bytesOut;//image;
        }

        public string[] GetFilters()
        {
            string[] str = { "sobelX", "allowed" };
            //Console.WriteLine("GetFilters called");
            return ConfigurationManager.AppSettings.AllKeys;
        }
        public int GetStatus()
        {
            //Console.WriteLine($"progress is {progress}, tread is {str}");
            return progress;
        }
    }
}
