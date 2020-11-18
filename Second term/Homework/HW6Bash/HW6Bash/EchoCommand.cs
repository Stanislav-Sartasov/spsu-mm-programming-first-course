using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW6Bash
{
	class EchoCommand
	{
		public static void Execute(string[] s, string[] buffer, string[] localNames, string[] localValues, int totalVars)
		{

			if (buffer[0].Length > 0)
			{
				string[] s2 = buffer[0].Split(' ');
				for (int i = 0; i < s2.Length; i++)
				{
					if (s2[i].Length != 0)
					{
						// проверяем наличие переменной с таким именем
						bool nameFound = false;
						for (int j = 0; j < totalVars; j++)
						{
							if (localNames[j].Equals(s2[i]))
							{
								Console.Write(localValues[j] + " ");
								nameFound = true;
								break;
							}
						}
						if (!nameFound)
							Console.Write("[" + s2[i] + "] ");
					}
				}
				buffer[0] = "";
			}

			for (int i = 1; i < s.Length; i++)
			{
				if (s[i].Length != 0)
				{
					// проверяем наличие переменной с таким именем
					bool nameFound = false;
					for (int j = 0; j < totalVars; j++)
					{
						if (localNames[j].Equals(s[i]))
						{
							Console.Write(localValues[j] + " ");
							nameFound = true;
							break;
						}
					}
					if (!nameFound)
						Console.Write("[" + s[i] + "] ");
				}
			}
			Console.WriteLine();
			//commandsLeft--;
		}
	}
}
