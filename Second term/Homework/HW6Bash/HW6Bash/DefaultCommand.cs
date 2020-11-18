using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW6Bash
{
	class DefaultCommand
	{
		public static int Execute(string[] s, string[] localNames, string[] localValues, int totalVars, int MAXVARS)
		{
			if (s[0][0] == '$' && s[0].Length > 3)
			{
				string[] temp = s[0].Split('=');
				if (temp.Length > 1)
				{
					int indexVar = -1;
					for (int i = 0; i < totalVars; i++)
					{
						if (localNames[i].Equals(temp[0]))
						{
							indexVar = i;
							break;
						}
					}
					if (totalVars >= MAXVARS && indexVar == -1)
						Console.WriteLine("Too Much of Variables");
					else
					{
						if (indexVar == -1)
						{
							localNames[totalVars] = temp[0];
							localValues[totalVars] = temp[1];
							totalVars++;
						}
						else
						{
							localNames[indexVar] = temp[0];
							localValues[indexVar] = temp[1];
						}
					}
				}
			}
			else
				Console.WriteLine("Unknown Command");
			return totalVars;
		}
	}
}
