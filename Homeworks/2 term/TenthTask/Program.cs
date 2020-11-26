using System;
using TenthTask.BashDescription;

namespace TenthTask
{
	class Program
	{
                static void Main()
                {
                        var bash = new Bash();
                        while (true)
                        {
                                var str = Console.ReadLine();
                                bash.Parse(str);
                        }
                }
	}
}
