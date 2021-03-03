using System;
using ChatDescription;

namespace NinthTask
{
	class Program
	{
		static void Main()
		{
			var chat = new ChatManager(new Client());
			chat.StartChating();
		}
	}
}

//TODO:
//добавить имена
//исправить ошибки
//доделать тесты