using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuntoBanco
{
	class Draw : IDraw
	{
		private Card[] puntos;
		private Card[] bancos;
		public void PreRound(Gamer[] g)
		{
			int f = 0;
			foreach (var i in g)
			{
				Console.WriteLine($"index: {f}, money: {i.moneyMoment}");
				f++;
			}
		}
		public void Winner(int who)
		{
			Console.Write("The winner is ");
			if (who == 0)
				Console.WriteLine("Punto");
			if (who == 1)
				Console.WriteLine("Banco");
			if (who == 2)
				Console.WriteLine("No one (tie)");
		}
		public void BotsBets(SomeBet[] first)
		{
			string tar;
			string person;
			foreach (var bet in first)
			{
				if (bet.man == 0)
					continue;
				if (bet.target == 0)
					tar = "punto";
				else
					if (bet.target == 1)
					tar = "banco";
				else
					tar = "tie";
				person = bet.man == 0 ? "you" : ("player number " + bet.man.ToString());
				Console.WriteLine($"{person}, bet on the {tar} {bet.money} $");
			}
			Console.WriteLine("////bots bets////");
		}
		public void AllBets(SomeBet[] first)
		{
			string tar;
			string person;
			foreach (var bet in first)
			{
				if (bet.target == 0)
					tar = "punto";
				else
					if (bet.target == 1)
					tar = "banco";
				else
					tar = "tie";
				person = bet.man == 0 ? "you" : ("player number " + bet.man.ToString());
				Console.WriteLine($"{person}, bet on the {tar} {bet.money} $");
			}
			Console.WriteLine("////all bets////");
		}
		public void Natural()
		{
			Console.WriteLine("Natural combination");
		}
		public void Enough(int who)
		{
			if (who == 0)
				Console.WriteLine("Punto is enough");
			if (who == 1)
				Console.WriteLine("Banco is enough");
		}
		public void ShowFirst(SomeBet[] first, Card pf, Card ps, Card bf, Card bs)
		{
			puntos = new Card[2] { pf, ps };
			bancos = new Card[2] { bf, bs };
			Console.WriteLine($"Punto's cards {pf.GetCard()}, {ps.GetCard()}");
			Console.WriteLine($"Banco's cards {bf.GetCard()}, {bs.GetCard()}");
		}
		public void ShowSecond(SomeBet[] first, int who, Card card)
		{
			if (who == 0)
				Console.Write("Punto's");
			if (who == 1)
				Console.Write("Banco's");
			Console.WriteLine($" next card is {card.GetCard()}");
		}
		public void InitDeck()
		{
			Console.WriteLine("new deck rotation");
		}
		public void AskMoneyToGo()
		{
			Console.Write("how mutch money you will give to start the game: ");
		}
		public void ShowMyMoney(Gamer g)
		{
			Console.WriteLine($"Now you have {g.moneyMoment} $");
		}
		public void RedyToGo()
		{
			Console.Write("If you want to play 1 Round type '1' else type any other integer(0): ");
		}
		public void DoBet(int moneyMoment)
		{
			Console.WriteLine($"Now yo have {moneyMoment}$, type first target of your bet(0 = punto, 1 = banco; 2 = tie) press enter after target\nThen type sum of money\n stock target is 0, stock sum is {moneyMoment}\n");
		}
		public void IntError()
		{
			Console.WriteLine("You have entered not integer default value = 0");
		}
	}
}
