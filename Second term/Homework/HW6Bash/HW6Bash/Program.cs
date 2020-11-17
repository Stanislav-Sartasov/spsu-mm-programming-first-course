using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace HW6Bash
{
	class Program
	{
		static void Main(string[] args)
		{
			const int MAXVARS = 50;
			string[] localNames = new string[MAXVARS];
			string[] localValues = new string[MAXVARS];
			int totalVars = 0;
			string buffer = "";
			int commandsLeft = 0;

			string[] commands = null;
			string realCommand = null;
			bool stopFlag = false;
			while (!stopFlag)
			{
				if (commandsLeft > 0)
				{
					realCommand = commands[commands.Length - commandsLeft];
				}
				else
				{

					Console.Write(">");
					string input = Console.ReadLine();
					int indexOfPipe = input.IndexOf('|');
					realCommand = input;
					if (indexOfPipe >= 0)
					{
						commands = input.Trim().Split('|');
						commandsLeft = commands.Length;
						realCommand = commands[0];
					}
				}
				
				string[] s = realCommand.Trim().Split(' ');
				switch (s[0])
				{
					case "echo":
						if (buffer.Length > 0)
						{
							string[] s2 = buffer.Split(' ');
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
							buffer = "";
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
						commandsLeft--;
						break;

					case "pwd":
						string curDir = Directory.GetCurrentDirectory();
						Console.WriteLine("Current directory is " + curDir);
						string[] listOfFiles = Directory.GetFiles(curDir);
						for (int i = 0; i < listOfFiles.Length; i++)
						{
							Console.WriteLine(listOfFiles[i]);
						}
						break;

					case "cat":
						if (s.Length < 2)
							Console.WriteLine("Error 1");
						else
						{
							string fName = s[1];
							if (File.Exists(s[1]))
							{
								string[] allLines = File.ReadAllLines(fName);
								for (int i = 0; i < allLines.Length; i++)
								{
									Console.WriteLine(allLines[i]);
									buffer += allLines[i];
								}
							}
							else
								Console.WriteLine("File not Found");
						}
						commandsLeft--;
						break;

					case "wc":
						if (s.Length < 2)
							Console.WriteLine("Error 1");
						else
						{
							string fName = s[1];
							if (File.Exists(s[1]))
							{
								long fSize = new FileInfo(fName).Length;
								Console.WriteLine("Size of File = " + fSize + " bytes");

								using (StreamReader sr = File.OpenText(fName))
								{
									int lineCount = 1;
									int wordCount = 0;
									bool lastCharLetter = false;
									for (int i = 0; ; i++)
									{
										int k = sr.Read();
										if (k >= 65 && k <= 90 || k >= 97 && k <= 122)
											lastCharLetter = true;
										else
										{
											if (lastCharLetter)
												wordCount++;

											lastCharLetter = false;

											if(k == '\n')
												lineCount++;
											if (k < 0)
												break;
										}
									}
									Console.WriteLine("Total words = " + wordCount);
									Console.WriteLine("Total lines = " + lineCount);
								}
							}
							else
								Console.WriteLine("File not Found");
						}
						break;

					case "exit":
						stopFlag = true;
						break;

					default: // пробуем определить заведение новой переменной
						if(s[0][0] == '$' && s[0].Length > 3)
						{
							string[] temp = s[0].Split('=');
							if (temp.Length > 1)
							{
								int indexVar = -1;
								for (int i = 0; i < totalVars;  i++)
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
						break;
				}
			}
		}
	}
}
