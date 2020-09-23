using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack
{
    public class Croupier : AGambler
    {
        public Hand CroupierHand { get; set; }

        public override void FreeHands()
        {

            CroupierHand.Reset();
            BlackJack = false;
        }

        public override void CheckBJ()
        {
            if (CroupierHand.Cards.Count == 2)
                if ((int)CroupierHand.Cards[0].Value + (int)CroupierHand.Cards[1].Value == 21)
                    BlackJack = true;
        }
        public Croupier(PlayerStatus status) :
            base(status)
        {
            CroupierHand = new Hand();
        }
    }
}
