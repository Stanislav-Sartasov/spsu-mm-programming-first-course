using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Bash;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace UnitTestBash
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestEcho()
        {
            int calls = 0;
            var mock = new Mock<IInteraction>();
            mock.Setup(x => x.GetStr()).Returns(() =>
            {
                if (calls == 0)
                {
                    ++calls;
                    return "echo 123333";
                }
                else
                    return "exit";
            });

            MyBash bash = new MyBash(mock.Object);
            bash.GoBash();
        }

        [TestMethod]
        public void TestCat()
        {
            int calls = 0;
            var mock = new Mock<IInteraction>();
            mock.Setup(x => x.GetStr()).Returns(() =>
            {
                if (calls == 0)
                {
                    ++calls;
                    return "cat 123333";
                }
                else
                    return "exit";
            });

            MyBash bash = new MyBash(mock.Object);
            bash.GoBash();
        }

        [TestMethod]
        public void TestStick()
        {
            int calls = 0;
            var mock = new Mock<IInteraction>();
            mock.Setup(x => x.GetStr()).Returns(() =>
            {
                if (calls == 0)
                {
                    ++calls;
                    return "echo 123333 | cat";
                }
                else
                    return "exit";
            });

            MyBash bash = new MyBash(mock.Object);
            bash.GoBash();
        }

        [TestMethod]
        public void TestEmptyArg()
        {
            int calls = 0;
            var mock = new Mock<IInteraction>();
            mock.Setup(x => x.GetStr()).Returns(() =>
            {
                if (calls == 0)
                {
                    ++calls;
                    return "wc";
                }
                else
                    return "exit";
            });

            MyBash bash = new MyBash(mock.Object);
            bash.GoBash();
        }

        [TestMethod]
        public void TestBadCommand()
        {
            int calls = 0;
            var mock = new Mock<IInteraction>();
            mock.Setup(x => x.GetStr()).Returns(() =>
            {
                if (calls == 0)
                {
                    ++calls;
                    return "echo | | 123333";
                }
                else
                    return "exit";
            });

            MyBash bash = new MyBash(mock.Object);
            bash.GoBash();
        }

        [TestMethod]
        public void TestWc()
        {
            int calls = 0;
            var mock = new Mock<IInteraction>();
            mock.Setup(x => x.GetStr()).Returns(() =>
            {
                if (calls == 0)
                {
                    ++calls;
                    return "wc 123.txt";
                }
                else
                    return "exit";
            });

            MyBash bash = new MyBash(mock.Object);
            bash.GoBash();
        }

        [TestMethod]
        public void TestlineCommand()
        {
            int calls = 0;
            var mock = new Mock<IInteraction>();
            mock.Setup(x => x.GetStr()).Returns(() =>
            {
                if (calls == 0)
                {
                    ++calls;
                    return "echo 123333 | echo | echo";
                }
                else
                    return "exit";
            });

            MyBash bash = new MyBash(mock.Object);
            bash.GoBash();
        }

        [TestMethod]
        public void TestVaries()
        {
            int calls = 0;
            var mock = new Mock<IInteraction>();
            mock.Setup(x => x.GetStr()).Returns(() =>
            {
                if (calls == 0)
                {
                    ++calls;
                    return "echo $s = 123333";
                }
                if (calls == 1)
                {
                    ++calls;
                    return "$d";
                }
                if (calls == 2)
                {
                    ++calls;
                    return "$d = privet";
                }
                if (calls == 3)
                {
                    ++calls;
                    return "echo $d and $s";
                }
                    return "exit";
            });

            MyBash bash = new MyBash(mock.Object);
            bash.GoBash();
        }
    }
}
