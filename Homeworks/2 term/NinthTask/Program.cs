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
			chat.Start(args);
		}
	}
}
