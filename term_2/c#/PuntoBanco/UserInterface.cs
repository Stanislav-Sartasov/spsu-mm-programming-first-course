using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuntoBanco
{
	public class UserInterface
	{
		private int Bots = 2;
		private Deck myDeck;
		private Gamer[] gamers;
		private IInteraction inter;
		public UserInterface(IInteraction YourInter)
		{
			Bots = 2;
			inter = YourInter;
			myDeck = new Deck();
		}
		public UserInterface()
		{
			Bots = 2;
			inter = new Interaction(new Draw());
			myDeck = new Deck();
		}
		private void GetPlayers()
		{
			gamers = new Gamer[3];
			gamers[0] = new Person(inter);
			gamers[1] = new GamerFirst(20);
			gamers[2] = new GamerSecond(40);
		}
		private void Round()
		{
			Draw table = new Draw();
			table.PreRound(gamers);
			SomeBet[] bets = new SomeBet[3];

			for (int i = 1; i <= Bots; ++i)
				bets[i] = gamers[i].MakeBet();

			table.BotsBets(bets);

			bets[0] = gamers[0].MakeBet();

			table.AllBets(bets);

			PuntoBanco punto = new PuntoBanco();
			PuntoBanco banco = new PuntoBanco();
			Card card1, card2, card3, card4;
			card1 = myDeck.GetCard();
			card2 = myDeck.GetCard();
			card3 = myDeck.GetCard();
			card4 = myDeck.GetCard();
			

			punto.GetCard(card1, card3);
			banco.GetCard(card2, card4);
			table.ShowFirst(bets, card1, card3, card2, card4);

			if ((!punto.IsNatural()) && (!banco.IsNatural()))
			{
				if (punto.CanTake())
				{
					Card puntoCard = myDeck.GetCard(); 
					punto.GetCard(puntoCard);
					table.ShowSecond(bets, 0, puntoCard);
				}
				else
					table.Enough(0);
				if (banco.CanTake())
				{
					Card bancoCard = myDeck.GetCard();
					banco.GetCard(bancoCard);
					table.ShowSecond(bets, 1, bancoCard);
				}
				else
					table.Enough(1);
			}
			else
			{
				table.Natural();
			}

			if (punto > banco)
			{
				for (int i = 0; i < 3; i++)
				{
					if (bets[i].target == 0)
						gamers[bets[i].man].Recive(bets[i].money * 2);
				}
				table.Winner(0);
			}
			else if (banco > punto)
			{
				for (int i = 0; i < 3; i++)
				{
					if (bets[i].target == 1)
						gamers[bets[i].man].Recive(bets[i].money * 2);
				}
				table.Winner(1);
			}
			else
			{
				for (int i = 0; i < 3; i++)
				{
					if (bets[i].target == 2)
						gamers[bets[i].man].Recive(bets[i].money * 9);
				}
				table.Winner(2);
			}

			table.PreRound(gamers);

		}

		public void GoGame()
		{
			GetPlayers();
			Draw user = new Draw();
			int end = 0;
			do
			{
				user.RedyToGo();
				if (inter.Ready() == true)
				{
					Round();
				}
				else
				{
					user.ShowMyMoney(gamers[0]);
					end = 1;
				}
			} while (end == 0);
		}
	}

}
