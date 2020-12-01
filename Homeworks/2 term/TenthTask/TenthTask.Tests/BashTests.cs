using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TenthTask.BashDescription;

namespace TenthTask.Tests
{
        [TestClass]
        public class BashTests
        {
                Bash bash = new Bash();
                [TestMethod]
                public void ParserTestEcho()
                {
                        var input = "echo ffff";
                        bash.Parser.Parse(input, Bash.Values);
                        //Assert.IsNotNull();

                        input = "exit 3";
                        bash.Parser.Parse(input, Bash.Values);
 
                }
        }
}
