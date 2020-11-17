using System;
using System.Threading;

namespace Task3
{
	internal static class Visual
	{
		static char[,] CreateCard(Card card)
		{
			if (card == null || card.value == 0)
				return CreateCard();

			char[,] heartsTMask = new char[7, 6]
			{
				{ ' ', '|', '|', '|', '|', '|' },
				{ '_', 'A', '(', ' ', ' ', '_' },
				{ '_', '_', ' ', '\\', ' ', '_' },
				{ '_', ' ', 'v', ' ', '.', '_' },
				{ '_', '_', ' ', '/', ' ', '_' },
				{ '_', ' ', ')', ' ', ' ', 'A' },
				{ ' ', '|', '|', '|', '|', '|' }
			};
			char[,] diamondsTMask = new char[7, 6]
			{
				{ ' ', '|', '|', '|', '|', '|' },
				{ '_', 'A', ' ', ' ', ' ', '_' },
				{ '_', ' ', '/', '\\', ' ', '_' },
				{ '_', '^', ' ', ' ', '.', '_' },
				{ '_', ' ', '\\', '/', ' ', '_' },
				{ '_', ' ', ' ', ' ', ' ', 'A' },
				{ ' ', '|', '|', '|', '|', '|' }
			};
			char[,] clubsTMask = new char[7, 6]
			{
				{ ' ', '|', '|', '|', '|', '|' },
				{ '_', 'A', ' ', '(', ' ', '_' },
				{ '_', ' ', '(', '_', ' ', '_' },
				{ '_', '_', ' ', '\'', '|', '_' },
				{ '_', ' ', ')', '_', ' ', '_' },
				{ '_', ' ', ' ', ')', ' ', 'A' },
				{ ' ', '|', '|', '|', '|', '|' }
			};
			char[,] spadesTMask = new char[7, 6]
			{
				{ ' ', '|', '|', '|', '|', '|' },
				{ '_', 'A', ' ', '(', ' ', '_' },
				{ '_', ' ', '/', '_', ' ', '_' },
				{ '_', '.', '.', '.', '|', '_' },
				{ '_', ' ', '\\', '_', ' ', '_' },
				{ '_', ' ', ' ', ')', ' ', 'A' },
				{ ' ', '|', '|', '|', '|', '|' }
			};

			char[,] jack = new char[7, 6]
			{
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', '0', '0', '%' },
				{ '0', 'w', '{', '%', '%', '%' },
				{ '0', 'w', ')', '0', '0', '0' },
				{ '0', '0', '0', '0', '0', '0' }
			};
			char[,] queen = new char[7, 6]
			{
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', '0', '0', '%' },
				{ '0', '0', '0', '0', '%', '%' },
				{ '0', 'w', '{', '%', '%', '%' },
				{ '0', 'w', '(', '%', '%', '0' },
				{ '0', '0', '0', '0', '0', '0' }
			};
			char[,] king = new char[7, 6]
			{
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', '0', '0', '%' },
				{ '0', '0', '0', '0', '%', '%' },
				{ '0', 'W', '{', '%', '%', '%' },
				{ '0', 'W', ')', '%', '%', '0' },
				{ '0', '0', '0', '0', '0', '0' }
			};

			char[,] heartsMod = new char[7, 6]
			{
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', '(', '0', '0' },
				{ '0', '0', '0', 'v', 'v', '0' },
				{ '0', '0', '0', ')', '0', '0' },
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', '0', '0', '0' }
			};
			char[,] diamondsMod = new char[7, 6]
			{
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '/', '\\', '0', '0' },
				{ '0', '0', '\\', '/', '0', '0' },
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', '0', '0', '0' }
			};
			char[,] clubsMod = new char[7, 6]
			{
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', 'o', '0', '0' },
				{ '0', '0', 'o', '.', '|', '0' },
				{ '0', '0', '0', 'o', '0', '0' },
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', '0', '0', '0' }
			};
			char[,] spadesMod = new char[7, 6]
			{
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', '(', '0', '0' },
				{ '0', '0', '^', '.', '|', '0' },
				{ '0', '0', '0', ')', '0', '0' },
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', '0', '0', '0' },
				{ '0', '0', '0', '0', '0', '0' }
			};

			static char[,] overlay(char[,] first, char[,] second)
			{
				for (int j = 0; j < 6; j++)
					for (int i = 0; i < 7; i++)
						if (second[i, j] != '0')
							first[i, j] = second[i, j];
				return first;
			}

			char[,] outCard = new char[7, 6];

			for (int j = 0; j < 6; j++)
				for (int i = 0; i < 7; i++)
					if (j == 0 && i > 0 && i < 6)
						outCard[i, j] = '_';
					else if (j != 0 && (i == 0 || i == 6))
						outCard[i, j] = '|';
					else if (j == 5)
						outCard[i, j] = '_';
					else
						outCard[i, j] = ' ';

			var sym = card.suit switch
			{
				Card.Suits.Hearts => 'v',
				Card.Suits.Diamonds => 'o',
				Card.Suits.Clubs => '&',
				Card.Suits.Spades => '^',
				_ => '?',
			};

			switch (card.value)
			{
				case 1:
					switch (card.suit)
					{
						case Card.Suits.Hearts:
							return (char[,])heartsTMask.Clone();
						case Card.Suits.Diamonds:
							return (char[,])diamondsTMask.Clone();
						case Card.Suits.Clubs:
							return (char[,])clubsTMask.Clone();
						case Card.Suits.Spades:
							return (char[,])spadesTMask.Clone();
					}
					break;
				case 11:
					outCard[1, 1] = 'J';
					break;
				case 12:
					outCard[1, 1] = 'Q';
					break;
				case 13:
					outCard[1, 1] = 'K';
					break;
				case 10:
					outCard[1, 1] = '1';
					outCard[2, 1] = '0';
					outCard[4, 5] = '1';
					outCard[5, 5] = '0';
					outCard[4, 1] = sym;
					break;
				default:
					outCard[1, 1] = (char)(card.value + '0');
					break;
			}
			if (card.value != 10)
				outCard[5, 5] = outCard[1, 1];

			switch (card.value)
			{
				case 11:
					outCard = overlay(outCard, jack);
					break;
				case 12:
					outCard = overlay(outCard, queen);
					break;
				case 13:
					outCard = overlay(outCard, king);
					break;
				default:
					if (card.value == 2 || card.value == 3 || card.value == 8 || card.value == 9 || card.value == 10)
					{
						if (card.value != 3)
							outCard[3, 2] = sym;
						outCard[3, 4] = sym;
					}

					if (card.value >= 3 && card.value <= 7)
					{
						outCard[2, 2] = sym;
						outCard[4, 2] = sym;
						if (card.value != 3)
						{
							outCard[2, 4] = sym;
							outCard[4, 4] = sym;
						}
					}

					if (card.value == 5 || card.value == 7 || card.value == 9 || card.value == 10)
						outCard[3, 3] = sym;

					if (card.value == 6 || card.value == 8)
					{
						outCard[2, 3] = sym;
						outCard[4, 3] = sym;
					}

					if (card.value == 7 || card.value == 9 || card.value == 10)
					{
						outCard[1, 3] = sym;
						outCard[5, 3] = sym;
					}

					if (card.value >= 8 && card.value <= 10)
					{
						outCard[1, 2] = sym;
						outCard[5, 2] = sym;
						outCard[1, 4] = sym;
						outCard[5, 4] = sym;
					}
					break;
			}

			if (card.value > 10)
				switch (card.suit)
				{
					case Card.Suits.Hearts:
						outCard = overlay(outCard, heartsMod);
						break;
					case Card.Suits.Diamonds:
						outCard = overlay(outCard, diamondsMod);
						break;
					case Card.Suits.Clubs:
						outCard = overlay(outCard, clubsMod);
						break;
					case Card.Suits.Spades:
						outCard = overlay(outCard, spadesMod);
						break;
				}
			return outCard;
		}
		static char[,] CreateCard()
		{
			return new char[7, 6]
			{
				{ ' ', '|', '|', '|', '|', '|' },
				{ '_', '%', '_', '%', '_', '%' },
				{ '_', '_', '%', '_', '%', '_' },
				{ '_', '%', '_', '%', '_', '%' },
				{ '_', '_', '%', '_', '%', '_' },
				{ '_', '%', '_', '%', '_', '%' },
				{ ' ', '|', '|', '|', '|', '|' }
			};
		}
		static void DrawCardPos(int pos, Card card)
		{
			char[,] cardForDraw = CreateCard(card);
			int delta = (80 / 2 - 7 * 3) / 4 + (pos % 3) * (7 + (80 / 2 - 7 * 3) / 4) + (pos / 3) * 80 / 2 + 2;
			for (int j = 0; j < 6; j++)
			{
				Console.SetCursorPosition(delta, j + 2);
				for (int i = 0; i < 7; i++)
					Console.Write(cardForDraw[i, j]);
			}
		}
		static void FreeCardPos(int pos)
		{
			int delta = (80 / 2 - 7 * 3) / 4 + (pos % 3) * (7 + (80 / 2 - 7 * 3) / 4) + (pos / 3) * 80 / 2 + 2;
			for (int j = 0; j < 6; j++)
			{
				Console.SetCursorPosition(delta, j + 2);
				for (int i = 0; i < 7; i++)
					Console.Write(' ');
			}
		}
		static void DrawScorePos(int pos, int score)
		{
			if (pos >= 0 && pos <= 1)
			{
				Console.SetCursorPosition(((80 / 2) - "SCORE: ?".Length) / 2 + pos * 80 / 2, 1);
				Console.Write("SCORE: " + score);
			}
		}
		static void DrawPlayerPos(int pos, string name, int bank)
		{
			int place = 80 * (pos + 1) / 5 - 80 * pos / 5 - 1;
			if (pos == 4)
				place--;
			Console.SetCursorPosition(80 * pos / 5 + 1, 18);
			if (name.Length > 15)
				Console.Write(name.Substring(0, 15));
			else
				Console.Write(name);
			for (int i = 0; i < place - name.Length; i++)
				Console.Write(' ');
			Console.SetCursorPosition(80 * pos / 5 + 1, 19);
			Console.Write("$: " + bank);
			for (int i = 0; i < place - bank.ToString().Length - 3; i++)
				Console.Write('_');
		}
		static void DrawBet(Field field, int pos, string name, int count)
		{
			int col;
			if (field == Field.Draw)
				col = 1;
			else
				col = (int)field * 2;

			Console.SetCursorPosition(80 * col / 3 + 1, 9 + pos);
			int cut = count.ToString().Length + name.Length - 21;
			if (cut > 0)
				Console.WriteLine(name.Substring(0, cut) + ": " + count);
			else
				Console.WriteLine(name + ": " + count);
		}
		static internal void PreDraw(GameLog players, bool delay)
		{
			Console.Clear();
			Console.SetWindowSize(80, 25);
			Console.SetBufferSize(80, 80);
			Console.CursorVisible = false;
			Console.SetCursorPosition(3, 1);
			Console.Write("PLAYER");
			Console.SetCursorPosition(80 / 2 + 3, 1);
			Console.Write("BANK");

			DrawScorePos(0, 0);
			DrawScorePos(1, 0);

			Console.SetCursorPosition(0, 8);
			for (int i = 0; i < 80; i++)
				Console.Write('_');
			Console.SetCursorPosition(0, 19);
			for (int i = 0; i < 80; i++)
				Console.Write('_');

			for (int i = 0; i < 20; i++)
			{
				Console.SetCursorPosition(0, i);
				Console.Write('|');
				if (i < 9)
				{
					Console.SetCursorPosition(80 / 2, i);
					Console.Write('|');
				}
				if (i >= 9 && i <= 13)
				{
					Console.SetCursorPosition(80 / 3, i);
					Console.Write('|');
					Console.SetCursorPosition(80 * 2 / 3, i);
					Console.Write('|');
				}
				if (i >= 18)
					for (int j = 0; j < 4; j++)
					{
						Console.SetCursorPosition(80 * (j + 1) / 5, i);
						Console.Write('|');
					}
				Console.SetCursorPosition(79, i);
				Console.Write('|');
			}

			for (int i = 0; i < Math.Min(5, players.count); i++)
				DrawPlayerPos(i, players.playerName[i], players.playerBankWas[i]);

			for (int i = 0; i < 5; i++)
				if (i != 2)
				{
					if (delay)
						Thread.Sleep(500);
					DrawCardPos(i, null);
				}

			Console.CursorVisible = true;
			Console.SetCursorPosition(0, 21);
		}
		static internal void LogDraw(GameLog log)
		{
			Console.SetWindowSize(80, 25);
			Console.SetBufferSize(80, 80);
			Console.CursorVisible = false;

			int[] typePoses = new int[3] { 0, 0, 0 };

			for (int i = 0; i < Math.Min(5, log.count); i++)
			{
				Thread.Sleep(250);
				if (log.playerBetFieldWas[i] == Field.None)
					continue;
				DrawBet(log.playerBetFieldWas[i], typePoses[(int)log.playerBetFieldWas[i]]++, log.playerName[i], log.playerBetWas[i]);
				DrawPlayerPos(i, log.playerName[i], log.playerBankWas[i]);
			}

			Thread.Sleep(250);

			for (int i = 0; i < 2; i++)
			{
				Thread.Sleep(750);
				DrawCardPos(i, log.cardPull[i]);
			}

			DrawScorePos(0, log.playerScoreBeforeExtraCard);

			Thread.Sleep(250);

			for (int i = 3; i < 5; i++)
			{
				Thread.Sleep(1000);
				DrawCardPos(i, log.cardPull[i]);
			}

			DrawScorePos(1, log.bankScoreBeforeExtraCard);

			Thread.Sleep(500);

			if (log.cardPull[2] != null)
			{
				Console.SetCursorPosition(80 / 2 - 5, 1);
				Console.Write('+');

				Thread.Sleep(1000);
				DrawCardPos(2, log.cardPull[2]);

				DrawScorePos(0, log.playerScore);

				Console.SetCursorPosition(80 / 2 - 5, 1);
				Console.Write(' ');
			}

			if (log.cardPull[5] != null)
			{
				Console.SetCursorPosition(80 - 5, 1);
				Console.Write('+');

				Thread.Sleep(1000);
				DrawCardPos(5, log.cardPull[5]);

				DrawScorePos(1, log.bankScore);

				Console.SetCursorPosition(80 - 5, 1);
				Console.Write(' ');
			}

			Thread.Sleep(2500);

			for (int i = 0; i < 6; i++)
				FreeCardPos(i);

			if (log.winField == Field.Player)
			{
				Console.SetCursorPosition((80 / 2 - "WIN".Length) / 2, 5);
				Console.Write("WIN");
				Console.SetCursorPosition((80 / 2 - "LOOSE".Length + 80) / 2, 5);
				Console.Write("LOOSE");
			}
			else if (log.winField == Field.Bank)
			{
				Console.SetCursorPosition((80 / 2 - "WIN".Length + 80) / 2, 5);
				Console.Write("WIN");
				Console.SetCursorPosition((80 / 2 - "LOOSE".Length) / 2, 5);
				Console.Write("LOOSE");
			}
			else
			{
				for (int j = 0; j <= 3; j++)
				{
					Console.SetCursorPosition(80 / 2 - ("DRAW".Length + 2) / 2, j + 2);
					for (int i = 0; i < "DRAW".Length + 2; i++)
						if (j == 0 && i > 0 && i < "DRAW".Length + 1)
							Console.Write('_');
						else if (j != 0 && (i == 0 || i == "DRAW".Length + 1))
							Console.Write('|');
						else if (j == 3)
							Console.Write('_');
						else
							Console.Write(' ');
				}
				Console.SetCursorPosition(80 / 2 - "DRAW".Length / 2, 4);
				Console.Write("DRAW");
				Console.SetCursorPosition((80 / 2 - "LOOSE".Length) / 2, 5);
				Console.Write("LOOSE");
				Console.SetCursorPosition((80 / 2 - "LOOSE".Length + 80) / 2, 5);
				Console.Write("LOOSE");
			}

			Thread.Sleep(1000);

			for (int i = 0; i < Math.Min(5, log.count); i++)
			{
				Console.SetCursorPosition((80 / 5 - (1 + Math.Abs(log.playerBankBecome[i] - log.playerBankWas[i] - log.playerBetWas[i]).ToString().Length)) / 2 + 80 * i / 5, 16);
				if (log.playerWin[i])
					Console.Write('+');
				else if (log.playerBetWas[i] != 0)
					Console.Write('-');
				Console.Write(Math.Abs(log.playerBankBecome[i] - log.playerBankWas[i] - log.playerBetWas[i]));
				DrawPlayerPos(i, log.playerName[i], log.playerBankBecome[i]);
			}

			Console.SetCursorPosition(0, 21);

			for (int i = 0; i < 160; i++)
				Console.Write(' ');
		}
	}
}