using Bash.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace Bash.Tests
{
    [TestClass]
    public class Test
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
        public void TestEchoAndVariable()
        {
            MyConsole console = new MyConsole();
            string commandOne = "echo AAA BBB ccc";
            string commandTwo = "echo";
            string commandThree = "echo AAA $a";

            string resultOne = "AAA BBB ccc";
            string resultTwo = "";
            string resultThree = "AAA 4";

            string createVariable = "$a=4";
            console.Run(createVariable);
            Assert.AreEqual(resultOne, console.Run(commandOne));
            Assert.AreEqual(resultTwo, console.Run(commandTwo));
            Assert.AreEqual(resultThree, console.Run(commandThree));
        }

        [TestMethod]
        public void TestPwd()
        {
            MyConsole console = new MyConsole();
            CreateAndWriteFile("test.txt", "test\n");
            Assert.AreEqual(true, console.Run("pwd").Contains("test.txt"));
            DeleteFile("test.txt");
        }

        [TestMethod]
        public void TestCat()
        {
            MyConsole console = new MyConsole();
            CreateAndWriteFile("test.txt", "test\n");
            Assert.AreEqual(true, console.Run("cat test.txt").Contains("test\n"));
            DeleteFile("test.txt");
        }

        [TestMethod]
        public void TestWcAndStringMethods()
        {
            MyConsole console = new MyConsole();
            CreateAndWriteFile("test.txt", "test\n   t e s t\n\na\n\n");
            string result = console.Run("wc test.txt");
            int indexOfLines = result.IndexOf("l") - 1;
            int indexOfWords = result.IndexOf("w") - 1;
            int actualBytes = "test\n   t e s t\n\na\n\n".Length;
            int expectedBytes = Encoding.UTF8.GetBytes("test\n   t e s t\n\na\n\n").Length;
            Assert.AreEqual(expectedBytes, actualBytes);
            Assert.AreEqual(5, int.Parse(result.Substring(0, indexOfLines)));
            Assert.AreEqual(6, int.Parse(result.Substring(indexOfLines + 8, indexOfWords - indexOfLines - 8)));
            DeleteFile("test.txt");
        }

        [TestMethod]
        public void TestConveyor()
        {
            MyConsole console = new MyConsole();
            Assert.AreEqual("*** Shutdown. ***", console.Run("|pwd exit"));
        }
    }
}
