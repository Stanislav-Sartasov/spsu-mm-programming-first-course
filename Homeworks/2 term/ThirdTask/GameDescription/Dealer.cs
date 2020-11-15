using System;
using System.Collections.Generic;
using System.Text;

namespace ThirdTask.GameDescription
{
	public class Dealer : Player
	{
		public int[] cards = new int[12] { 0, 0, 32, 32, 32, 32, 32, 32, 32, 32, 128, 32 };
		public void Turn(Player player)
		{
			int playerCard = player.Turn();
			while (cards[playerCard] == 0) // если пустая колода
			{
				playerCard = player.Turn();
			}

			if (player.FirstCard == 0)
			{
				player.FirstCard = playerCard;
			}
			else if (player.SecondCard == 0)
			{
				player.SecondCard = playerCard;
			}
			else
			{
				if (playerCard == 11)
				{
					player.NumOfAces++;
				}
				else
				{
					player.OtherCards += playerCard;
				}
			}
			cards[playerCard]--;
		}
		public override void ChangeStatus()
		{
			if (FirstCard + SecondCard == 21) // блэкджек (выиграл)
			{
				GameStatus = 1;
			}
			else if (SumOfAllCards() > 21) // >21 (выбыл, берёт остаток)
			{
				GameStatus = 0; 
			}
			else if (SumOfAllCards() < 17) // <17 (берёт)
			{
				GameStatus = 3;
			}
			else // от 17 до 21 (не берёт)
			{
				GameStatus = 2;
			}
		}
	}
}
