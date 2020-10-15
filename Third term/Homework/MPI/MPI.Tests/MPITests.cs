using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using ArrayGeneration;


namespace MPI.Tests
{
    [TestClass]
    public class MPITests
    {
        const int processorsNumber = 4;
        static readonly string input = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "input.txt");
        static readonly string actOutput = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "actOutput.txt");
        static readonly string expOutput = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "expOutput.txt");
        static readonly string appPath = string
            .Format("{0}MPI\\bin\\Debug\\MPI.exe", 
            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\")));

        public bool CompareFiles(string path1, string path2)
        {
            string contents1 = File.ReadAllText(path1);
            string contents2 = File.ReadAllText(path2);

            if (contents1.Length != contents2.Length)
            {
                Console.WriteLine("File sizes are different");
                return false;
            }

            for (int i = 0; i < contents1.Length; i++)
            {
                if (contents1[i] != contents2[i])
                {
                    return false;
                }
            }
            return true;
        }

        [TestInitialize]
        public void Init()
        {
            var array = ArrayGenerator.GenerateArray(2000000);
            IOManager.WriteArray(input, array);
            array.Sort();
            IOManager.WriteArray(expOutput, array);
            var args = $"mpiexec -n {processorsNumber} {appPath} {input} {actOutput}";
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(args);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
        }

        [TestMethod]
        public void CorrectSorting()
        {
            Assert.IsTrue(CompareFiles(actOutput, expOutput));
        }
    }
}
