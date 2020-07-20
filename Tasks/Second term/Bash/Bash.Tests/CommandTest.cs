using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.IO;
using System.Text;
using Bash.Handler;
using Bash.Manager;

namespace Bash.Tests
{
    [TestClass]
    public class CommandTest
    {
        private bool CreateAndWriteFile(string path, string text)
        {
            FileStream file = File.Create(path);
            if (file == null)
                return false;

            file.Write(Encoding.UTF8.GetBytes(text));
            file.Close();
            return true;
        }

        private void DeleteFile(string path)
        {
            File.Delete(path);
        }
        [TestMethod]
        public void TestEcho()
        {
            ICommand console = new Echo();
            string command = "echo AAA   BBB ccc";

            string result = "AAA   BBB ccc";
            
            Assert.AreEqual(result, console.Processing(command));
        }

        [TestMethod]
        public void TestPwd()
        {
            ICommand console = new Pwd();
            CreateAndWriteFile("test.txt", "text\n");
            Assert.IsTrue(console.Processing("pwd").Contains("test.txt"));
            DeleteFile("test.txt");
        }

        [TestMethod]
        public void TestCat()
        {
            ICommand console = new Cat();
            CreateAndWriteFile("test.txt", "text\n");
            Assert.IsTrue(console.Processing("cat test.txt").Contains("text\n"));
            DeleteFile("test.txt");
        }

        [TestMethod]
        public void TestWcAndStringMethods()
        {
            ICommand console = new Wc();
            CreateAndWriteFile("test.txt", "text\n");
            string result = console.Processing("wc test.txt");
            int indexOfLines = result.IndexOf("l") - 1;
            int indexOfWords = result.IndexOf("w") - 1;
            int actualBytes = "text\n".Length;
            int expectedBytes = Encoding.UTF8.GetBytes("text\n").Length;
            Assert.AreEqual(expectedBytes, actualBytes);
            Assert.AreEqual(1, int.Parse(result.Substring(0, indexOfLines)));
            Assert.AreEqual(1, int.Parse(result.Substring(indexOfLines + 8, indexOfWords - indexOfLines - 8)));
            DeleteFile("test.txt");
        }

        [TestMethod]
        public void TestPipe()
        {
            ICommand console = new Pipe();
            string input = "| echo test.txt | cat |";
            CreateAndWriteFile("test.txt", "text\n");
            Assert.IsTrue(console.Processing(input).Contains("text\n"));
            DeleteFile("test.txt");
        }
    }
}
