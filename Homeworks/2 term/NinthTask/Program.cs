using System;
using NinthTask.ChatDescription;

namespace NinthTask
{
	class Program
	{
		static void Main(string[] args)
		{
			//Instruction
			var chat = new ChatManager();
			chat.Start(args);
		}
	}
}
