using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace Task3
{
    internal static class Visualizer
    {
        static char[,] CreateCard(Card card)
        {
            if (card == null)
                return CreateCard();

            char[,] chrevTMask = new char[7, 6]
            {
                { ' ', '|', '|', '|', '|', '|' },
                { '_', 'A', '(', ' ', ' ', '_' },
                { '_', '_', ' ', '\\', ' ', '_' },
                { '_', ' ', 'v', ' ', '.', '_' },
                { '_', '_', ' ', '/', ' ', '_' },
                { '_', ' ', ')', ' ', ' ', 'A' },
                { ' ', '|', '|', '|', '|', '|' }
            };
            char[,] bubTMask = new char[7, 6]
            {
                { ' ', '|', '|', '|', '|', '|' },
                { '_', 'A', ' ', ' ', ' ', '_' },
                { '_', ' ', '/', '\\', ' ', '_' },
                { '_', '^', ' ', ' ', '.', '_' },
                { '_', ' ', '\\', '/', ' ', '_' },
                { '_', ' ', ' ', ' ', ' ', 'A' },
                { ' ', '|', '|', '|', '|', '|' }
            };
            char[,] krestTMask = new char[7, 6]
            {
                { ' ', '|', '|', '|', '|', '|' },
                { '_', 'A', ' ', '(', ' ', '_' },
                { '_', ' ', '(', '_', ' ', '_' },
                { '_', '_', ' ', '\'', '|', '_' },
                { '_', ' ', ')', '_', ' ', '_' },
                { '_', ' ', ' ', ')', ' ', 'A' },
                { ' ', '|', '|', '|', '|', '|' }
            };
            char[,] picTMask = new char[7, 6]
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

            char[,] chrevMod = new char[7, 6]
            {
                { '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '(', '0', '0' },
                { '0', '0', '0', 'v', 'v', '0' },
                { '0', '0', '0', ')', '0', '0' },
                { '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '0', '0', '0' }
            };
            char[,] bubMod = new char[7, 6]
            {
                { '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '0', '0', '0' },
                { '0', '0', '/', '\\', '0', '0' },
                { '0', '0', '\\', '/', '0', '0' },
                { '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '0', '0', '0' }
            };
            char[,] krestMod = new char[7, 6]
            {
                { '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', 'o', '0', '0' },
                { '0', '0', 'o', '.', '|', '0' },
                { '0', '0', '0', 'o', '0', '0' },
                { '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '0', '0', '0' }
            };
            char[,] picMod = new char[7, 6]
            {
                { '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '(', '0', '0' },
                { '0', '0', '^', '.', '|', '0' },
                { '0', '0', '0', ')', '0', '0' },
                { '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '0', '0', '0' }
            };

            static char[,] overlay (char[,] first, char[,] second)
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

            char sym;
            switch (card.Suit)
            {
                case 'c':
                    sym = 'v';
                    break;
                case 'b':
                    sym = 'o';
                    break;
                case 'k':
                    sym = '&';
                    break;
                case 'p':
                    sym = '^';
                    break;
                default:
                    sym = '?';
                    break;
            }

            switch (card.Dignity)
            {
                case 1:
                    switch (card.Suit)
                    {
                        case 'c':
                            return (char[,])chrevTMask.Clone();
                        case 'b':
                            return (char[,])bubTMask.Clone();
                        case 'k':
                            return (char[,])krestTMask.Clone();
                        case 'p':
                            return (char[,])picTMask.Clone();
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
                    outCard[1, 1] = (char)(card.Dignity + '0');
                    break;
            }
            if (card.Dignity != 10)
                outCard[5, 5] = outCard[1, 1];

            switch (card.Dignity)
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
                    if (card.Dignity == 2 || card.Dignity == 3 || card.Dignity == 8 || card.Dignity == 9 || card.Dignity == 10)
                    {
                        if (card.Dignity != 3)
                            outCard[3, 2] = sym;
                        outCard[3, 4] = sym;
                    }

                    if (card.Dignity >= 3 && card.Dignity <= 7)
                    {
                        outCard[2, 2] = sym;
                        outCard[4, 2] = sym;
                        if (card.Dignity != 3)
                        {
                            outCard[2, 4] = sym;
                            outCard[4, 4] = sym;
                        }
                    }

                    if (card.Dignity == 5 || card.Dignity == 7 || card.Dignity == 9 || card.Dignity == 10)
                        outCard[3, 3] = sym;

                    if (card.Dignity == 6 || card.Dignity == 8)
                    {
                        outCard[2, 3] = sym;
                        outCard[4, 3] = sym;
                    }

                    if (card.Dignity == 7 || card.Dignity == 9 || card.Dignity == 10)
                    {
                        outCard[1, 3] = sym;
                        outCard[5, 3] = sym;
                    }

                    if (card.Dignity >= 8 && card.Dignity <= 10)
                    {
                        outCard[1, 2] = sym;
                        outCard[5, 2] = sym;
                        outCard[1, 4] = sym;
                        outCard[5, 4] = sym;
                    }
                    break;
            }

            if (card.Dignity > 10)
                switch (card.Suit)
                {
                    case 'c':
                        outCard = overlay(outCard, chrevMod);
                        break;
                    case 'b':
                        outCard = overlay(outCard, bubMod);
                        break;
                    case 'k':
                        outCard = overlay(outCard, krestMod);
                        break;
                    case 'p':
                        outCard = overlay(outCard, picMod);
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
        static void DrowCardPos(int pos, Card card)
        {
            char[,] cardForDrow = CreateCard(card);
            int delta = (80 / 2 - 7 * 3) / 4 + (pos % 3) * (7 + (80 / 2 - 7 * 3) / 4) + (pos / 3) * 80 / 2 + 2;
            for (int j = 0; j < 6; j++)
            {
                Console.SetCursorPosition(delta, j + 2);
                for (int i = 0; i < 7; i++)
                    Console.Write(cardForDrow[i, j]);
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
        static void DrowScorePos(int pos, int score)
        {
            if (pos >= 0 && pos <= 1)
            {
                Console.SetCursorPosition(((80 / 2) - "SCORE: ?".Length) / 2 + pos * 80 / 2, 1);
                Console.Write("SCORE: " + score);
            }
        }
        static void DrowPlayerPos(int pos, string name, int bank)
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
        static void DrowBate(int type, int pos, string name, int count)
        {
            int col;
            if (type == 2)
                col = 1;
            else
                col = type * 2;

            Console.SetCursorPosition(80 * col / 3 + 1, 9 + pos);
            int cut = count.ToString().Length + name.Length  - 21;
            if (cut > 0)
                Console.WriteLine(name.Substring(0, cut) + ": " + count);
            else
                Console.WriteLine(name + ": " + count);
        }
        static internal void PreDrow(GameMaster.PlayerPlace[] players, bool delay)
        {
            Console.Clear();
            Console.SetWindowSize(80, 25);
            Console.SetBufferSize(80, 80);
            Console.CursorVisible = false;
            Console.SetCursorPosition(3, 1);
            Console.Write("PLAYER");
            Console.SetCursorPosition(80 / 2 + 3, 1);
            Console.Write("BANK");

            DrowScorePos(0, 0);
            DrowScorePos(1, 0);
            
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

            for (int i = 0; i < 5; i++)
                if (i < players.Length)
                    DrowPlayerPos(i, players[i].currentPlayer.Name, players[i].currentPlayer.Budget);

            for (int i = 0; i < 5; i++)
                if (i != 2)
                {
                    if (delay)
                        Thread.Sleep(500);
                    DrowCardPos(i, null);
                }

            Console.CursorVisible = true;
            Console.SetCursorPosition(0, 21);
        }
        static internal void LogDrow(GameMaster.Log log)
        {
            Console.SetWindowSize(80, 25);
            Console.SetBufferSize(80, 80);
            Console.CursorVisible = false;

            int[] typePoses = new int[3] { 0, 0, 0 };

            for (int i = 0; i < log.playerCount; i++)
            {
                Thread.Sleep(250);
                DrowBate(log.bateType[i], typePoses[log.bateType[i]]++, log.playerName[i], log.bate[i]);
                DrowPlayerPos(i, log.playerName[i], log.budgetWas[i] - log.bate[i]);
            }

            Thread.Sleep(250);

            for (int i = 0; i < 2; i++)
            {
                Thread.Sleep(750);
                DrowCardPos(i, log.play[i]);
            }

            DrowScorePos(0, log.playScoreBnc);

            Thread.Sleep(250);

            for (int i = 0; i < 2; i++)
            {
                Thread.Sleep(1000);
                DrowCardPos(i + 3, log.bank[i]);
            }

            DrowScorePos(1, log.bankScoreBnc);

            Thread.Sleep(500);

            if (log.play[2] != null)
            {
                Console.SetCursorPosition(80 / 2 - 5, 1);
                Console.Write('+');

                Thread.Sleep(1000);
                DrowCardPos(2, log.play[2]);

                DrowScorePos(0, log.playScoreAnc);

                Console.SetCursorPosition(80 / 2 - 5, 1);
                Console.Write(' ');
            }

            if (log.bank[2] != null)
            {
                Console.SetCursorPosition(80 - 5, 1);
                Console.Write('+');

                Thread.Sleep(1000);
                DrowCardPos(5, log.bank[2]);

                DrowScorePos(1, log.bankScoreAnc);

                Console.SetCursorPosition(80 - 5, 1);
                Console.Write(' ');
            }

            Thread.Sleep(2500);

            for (int i = 0; i < 6; i++)
                FreeCardPos(i);

            if (log.winType == 0)
            {
                Console.SetCursorPosition((80 / 2 - "WIN".Length) / 2, 5);
                Console.Write("WIN");
                Console.SetCursorPosition((80 / 2 - "LOOSE".Length + 80) / 2, 5);
                Console.Write("LOOSE");
            }
            else if (log.winType == 1)
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
                    Console.SetCursorPosition(80 / 2 - ("TIE".Length + 2) / 2, j + 2);
                    for (int i = 0; i < "TIE".Length + 2; i++)
                        if (j == 0 && i > 0 && i < "TIE".Length + 1)
                            Console.Write('_');
                        else if (j != 0 && (i == 0 || i == "TIE".Length + 1))
                            Console.Write('|');
                        else if (j == 3)
                            Console.Write('_');
                        else
                            Console.Write(' ');
                }
                Console.SetCursorPosition(80 / 2 - "TIE".Length / 2, 4);
                Console.Write("TIE");
                Console.SetCursorPosition((80 / 2 - "LOOSE".Length) / 2, 5);
                Console.Write("LOOSE");
                Console.SetCursorPosition((80 / 2 - "LOOSE".Length + 80) / 2, 5);
                Console.Write("LOOSE");
            }

            Thread.Sleep(1000);

            for (int i = 0; i < log.playerCount; i++)
            {
                Console.SetCursorPosition((80 / 5 - (1 + Math.Abs(log.budgetBecome[i] - log.budgetWas[i]).ToString().Length)) / 2 + 80 * i / 5, 16);
                if (log.win[i])
                    Console.Write('+');
                else
                    Console.Write('-');
                Console.Write(Math.Abs(log.budgetBecome[i] - log.budgetWas[i]));
                DrowPlayerPos(i, log.playerName[i], log.budgetBecome[i]);
            }

            Console.SetCursorPosition(0, 21);

            for (int i = 0; i < 160; i++)
                Console.Write(' ');
        }
    }
}