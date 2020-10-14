using System.IO;
using System.Text;
using BashLibrary;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParserLibrary;

namespace Task6Bash.Tests
{
    [TestClass]
    public class CommandTest
    {
        public static void CreateFileForTest(string path, string input)
        {
            
            try
            {
                using (FileStream fileStream = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(input);
                    fileStream.Write(info, 0, info.Length);
                }
            }
            catch
            {

            }
        }
        [TestMethod]
        public void TestCat()
        {
            string path = Directory.GetCurrentDirectory() + "\\test.txt";
            string input = "test text\n";
            CreateFileForTest(path, input);
            Assert.AreEqual(input, new Cat().Execute(path));
        }
        [TestMethod]
        public void TestEcho()
        {
            string input = "   echo  test1     test2   test3  ";
            Assert.AreEqual(new Echo().Execute(Parser.GetArguments(input)), Parser.GetArguments(input));
        }
        [TestMethod]
        public void TestPwd()
        {
            string executed = new Pwd().Execute(null);
            Assert.IsTrue(executed.Contains(Directory.GetCurrentDirectory()));
            DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            foreach (FileInfo file in directory.GetFiles())
                Assert.IsTrue(executed.Contains(file.Name));
        }
        [TestMethod]
        public void TestWc()
        {
            string path = Directory.GetCurrentDirectory() + "\\test.txt";
            string input = "test text\n";
            string executed = new Wc().Execute(path);
            CreateFileForTest(path, input);

            string lines = "lines: " + Parser.CountLines(input).ToString();
            Assert.IsTrue(executed.Contains(lines));
            string words = "words: " + Parser.CountWords(input).ToString();
            Assert.IsTrue(executed.Contains(words));
            byte[] info = new UTF8Encoding(true).GetBytes(input);
            string bytes = "bytes: " + info.Length.ToString();
            Assert.IsTrue(executed.Contains(bytes));
        }
    }
}
