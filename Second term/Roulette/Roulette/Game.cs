using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette
{
	public class Game
	{

		public enum TypeOfBet
		{
			Single = 1,
			Split,
			Basket,
			Red,
			Black,
			Even,
			Odd,
			FirstDozen,
			SecondDozen,
			ThirdDozen,
			SnakeBet
		}

		public bool HitTheSector(int victoryCell, int min, int max)
		{
			if (victoryCell >= min && victoryCell <= max)
				return true;
			else
				return false;
		}
		public bool CheckOfWin(TypeOfBet typeOfBet, int cells)
		{
			Random rnd = new Random();
			int victoryCell = rnd.Next(37);
			Console.WriteLine($"Slot number {victoryCell} dropped");
			if (typeOfBet == TypeOfBet.Single)
			{
				if (cells == victoryCell)
					return true;
				else
					return false;
			}
			else
			{
				if (typeOfBet == TypeOfBet.Split)
				{
					if (cells == victoryCell || cells + 1 == victoryCell)
						return true;
					else
						return false;
				}
				else 
				{
					if (typeOfBet == TypeOfBet.Basket)
					{
						if (cells == victoryCell || cells + 1 == victoryCell || victoryCell == 0)
							return true;
						else
							return false;
					}
					else
					{
						if (typeOfBet == TypeOfBet.Red)
						{
							if (((victoryCell < 10 || victoryCell > 18 && victoryCell < 28) && victoryCell % 2 == 1) ||
								((victoryCell > 11 && victoryCell < 19 || victoryCell > 29) && victoryCell % 2 == 0))
							{
								return true;
							}
							else
								return false;
						}
						else
						{
							if (typeOfBet == TypeOfBet.Black)
							{
								if (!(((victoryCell < 10 || victoryCell > 18 && victoryCell < 28) && victoryCell % 2 == 1) ||
								((victoryCell > 11 && victoryCell < 19 || victoryCell > 29) && victoryCell % 2 == 0)) && victoryCell != 0)
								{
									return true;
								}
								else
									return false;
							}
							else
							{
								if (typeOfBet == TypeOfBet.Even)
								{
									if (victoryCell % 2 == 0)
										return true;
									else
										return false;
								}
								else
								{
									if (typeOfBet == TypeOfBet.Odd)
									{
										if (victoryCell % 2 == 1)
											return true;
										else
											return false;
									}
									else if (typeOfBet == TypeOfBet.FirstDozen)
										return HitTheSector(victoryCell, 1, 12);
									else if (typeOfBet == TypeOfBet.SecondDozen)
										return HitTheSector(victoryCell, 13, 24);
									else if (typeOfBet == TypeOfBet.ThirdDozen)
										return HitTheSector(victoryCell, 25, 36);
									else
									{
										if (typeOfBet == TypeOfBet.SnakeBet)
										{
											int[] snakeArray = new int[] {1, 5, 9, 12, 14, 16, 19, 23, 27, 30, 32, 34};
											for (int i = 0; i < snakeArray.Length; i++)
											{
												if (victoryCell == snakeArray[i])
													return true;
											}
											return false;
										}
										Console.WriteLine("Something wrong");
										return false;
									}
								}
							}
						}
					}
				}
			}
		}

		public int GetCoefficient(TypeOfBet typeOfBet)
		{
			int coefficient;
			if (typeOfBet >= TypeOfBet.FirstDozen)
				coefficient = 2;
			else if (typeOfBet >= TypeOfBet.Red)
				coefficient = 1;
			else if (typeOfBet == TypeOfBet.Basket)
				coefficient = 11;
			else if (typeOfBet == TypeOfBet.Split)
				coefficient = 18;
			else
				coefficient = 35;
			return coefficient;
		}
	}
}
