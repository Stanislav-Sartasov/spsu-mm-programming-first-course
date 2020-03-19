using System;
using System.Collections.Generic;

namespace Blackjack
{
    public class RandomBot : Player, IBotPlayer
    {

        public int BotBet(int minBet, int chips)
        {
            var rand = new Random();
            int randValue = rand.Next(minBet, minBet > chips / 50 ? minBet : chips / 50);
            return randValue;
        }

        public Command BotCommand(List<Command> availableCommands, Card croupierOpenCard, int numOfHand)
        {
            var rand = new Random();
            int randomIndex = rand.Next(0, availableCommands.Count - 1);
            return availableCommands[randomIndex];
        }

        public RandomBot(int chips, string name, PlayerStatus status) :
            base(chips, name, status)
        {
            Chips = chips;
            Hands = new List<Hand>
            {
                new Hand()
            };
        }

    }
}
