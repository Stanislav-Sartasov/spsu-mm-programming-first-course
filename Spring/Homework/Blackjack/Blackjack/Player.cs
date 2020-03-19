using System;
using System.Collections.Generic;


namespace Blackjack
{
    
    public class Player : AGambler
    {
        public List<Hand> Hands { get; set; }
        public int Chips { get; set; }

        public bool InGame;
        public Player(int chips, string name, PlayerStatus status) :
            base(name, status)
        {
            Chips = chips;
            InGame = true;
            Hands = new List<Hand>
            {
                new Hand()
            };
        }

        public int CurrentBet { get; set; } = 0;

        public override void CheckBJ()
        {
            if (Hands[0].Cards.Count == 2)
                if ((int)Hands[0].Cards[0].Value + (int)Hands[0].Cards[1].Value == 21)
                    BlackJack = true;
        }
        public override void FreeHands()
        {
            Hands.Clear();
            Hands = new List<Hand>() { new Hand() };
            BlackJack = false;
        }

        public void PrintChips()
        {
            Console.WriteLine(Chips);
        }
    }
}
