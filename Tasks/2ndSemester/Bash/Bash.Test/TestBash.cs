using Microsoft.VisualStudio.TestTools.UnitTesting;
using BashLibrary;
using System.Text;
using System.IO;

namespace Bash.Test
{
    [TestClass]
    public class TestBash
    {
        [TestMethod]
        public void TestEcho()
        {
            ICommand echo = new Echo();
            string command = "checking   correct    display";

            string result = "checking   correct    display\n";

            Assert.AreEqual(result, echo.Execute(command));
        }

        [TestMethod]
        public void TestPwd()
        {
            ICommand pwd = new Pwd();
            FileStream file = File.Create("test.txt");

            file.Write(Encoding.UTF8.GetBytes("test"));
            file.Close();

            Assert.IsTrue(pwd.Execute("").Contains("test.txt"));
            File.Delete("test.txt");
        }

        [TestMethod]
        public void TestCat()
        {
            ICommand cat = new Cat();
            FileStream file = File.Create("test.txt");

            file.Write(Encoding.UTF8.GetBytes("test"));
            file.Close();
            Assert.IsTrue(cat.Execute("test.txt").Contains("test"));
            File.Delete("test.txt");
        }

        [TestMethod]
        public void TestWc()
        {
            ICommand wc = new Wc();
            FileStream file = File.Create("test.txt");

            file.Write(Encoding.UTF8.GetBytes("test"));
            file.Close();
            string result = wc.Execute("test.txt");
            Assert.AreEqual(Encoding.UTF8.GetBytes("test").Length, "test".Length);
            Assert.AreEqual(1, int.Parse(result.Substring(0, result.IndexOf("l") - 1)));
            Assert.AreEqual(1, int.Parse(result.Substring(result.IndexOf("l") + 7, result.IndexOf("w") - result.IndexOf("l") - 7)));
            File.Delete("test.txt");
        }
    }
}
