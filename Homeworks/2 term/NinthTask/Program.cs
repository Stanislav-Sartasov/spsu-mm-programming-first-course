using System;
using NinthTask.ChatDescription;

namespace NinthTask
{
	class Program
	{
		static void Main(string[] args)
		{
			var chat = new Chat();
			chat.Start(args);
		}
	}
}
