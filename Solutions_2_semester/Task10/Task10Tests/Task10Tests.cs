using NUnit.Framework;
using Task10;
using System.IO;

namespace Task10Tests
{
    public class Tests
    {
        void CreateTestFiles()
        {
            StreamWriter sw = new StreamWriter("testFile.txt", false);
            sw.Write("testInfoFile.txt");
            sw.Close();
            sw = new StreamWriter("testInfoFile.txt", false);
            sw.Write("string 1\nstring 2\nstring 3");
            sw.Close();
        }

        void DeleteTestFies()
        {
            File.Delete("testFile.txt");
            File.Delete("testInfoFile.txt");
        }

        [Test]
        public void FunctionalTest()
        {
            CreateTestFiles();

            CommandHandler commandHandler = new CommandHandler();
            CommandHandler.Keys key;

            Assert.AreEqual("testInfoFile.txt:\nlines: 3; words: 6; bytes: 26", commandHandler.Process("cat testFile.txt | $a = | wc ", out key));
            Assert.AreEqual(CommandHandler.Keys.ok, key);
            Assert.AreEqual("testInfoFile.txt:\nlines: 3; words: 6; bytes: 26", commandHandler.Process("| echo", out key));
            Assert.AreEqual(CommandHandler.Keys.ok, key);
            Assert.AreEqual("testInfoFile.txt", commandHandler.Process("$a", out key));
            Assert.AreEqual(CommandHandler.Keys.ok, key);
            Assert.AreEqual("strings file testInfoFile.txt", commandHandler.Process("$b = strings file $a", out key));
            Assert.AreEqual(CommandHandler.Keys.ok, key);
            Assert.AreEqual("  testInfoFile.txt strings file testInfoFile.txt", commandHandler.Process("| echo   $a ", out key));
            Assert.AreEqual(CommandHandler.Keys.ok, key);

            Assert.IsTrue(commandHandler.Process("pwd", out _).Contains("testFile.txt"));
            Assert.IsTrue(commandHandler.Process("pwd", out key).Contains("testInfoFile.txt"));
            Assert.AreEqual(CommandHandler.Keys.ok, key);

            commandHandler.Process("$a = b", out _);
            commandHandler.Process("$b = c", out _);
            commandHandler.Process("$c = pointer", out _);
            Assert.AreEqual("pointer", commandHandler.Process("echo $$$a", out key));       //funny feature
            Assert.AreEqual(CommandHandler.Keys.ok, key);

            commandHandler.Process("exit", out key);
            Assert.AreEqual(CommandHandler.Keys.exit, key);

            Assert.AreEqual("", commandHandler.Process("echo |", out _));

            DeleteTestFies();
        }

        [Test]
        public void InputProtectionTest()
        {
            CreateTestFiles();

            CommandHandler commandHandler = new CommandHandler();
            CommandHandler.Keys key;

            Assert.AreEqual("testInfoFile.txt", commandHandler.Process("cat       testFile.txt", out _));
            Assert.AreEqual("testInfoFile.txt", commandHandler.Process("cat       \"testFile.txt\"", out _));

            string[] wrongCommand = new string[]
            {
                "cat \"testFile.txt\" \"testInfoFile.txt\"",
                "cat \"testFile.txt\" \"",
                "cat testFile.txt\" \"",
                "cat testFile.txt\"",
                "cat \"testFile.txt",
                "exit someParameter",
                "echo $",
                "UnidentifiedCommand"
            };

            foreach (string s in wrongCommand)
            {
                commandHandler.Process(s, out key);
                Assert.AreEqual(CommandHandler.Keys.error, key);
            }

            DeleteTestFies();
        }
    }
}