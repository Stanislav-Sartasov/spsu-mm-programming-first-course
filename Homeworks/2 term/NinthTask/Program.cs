using System;
using NinthTask.ChatDescription;

namespace NinthTask
{
	class Program
	{
		#region usage

		/*
		No parameters - only server
		Connection: tasksocket.exe <ipaddress> <port>
		tasksocket.exe <port> == tasksocket.exe 127.0.0.1 <port>
		 */

		#endregion
		static void Main(string[] args)
		{
			var chat = new ChatManager();

			if (args.Length == 0)
			{
				chat.StartServer(); //сервер - для себя, поменяю
			}
			else if (args.Length == 1)
			{
				chat.StartChatting(args[0]);
			}
			else if (args.Length == 2)
			{
				chat.StartChatting(args[0], args[1]);
			}
			else
			{
				Console.WriteLine("Incorrect input format.");
			}
		}
	}
}