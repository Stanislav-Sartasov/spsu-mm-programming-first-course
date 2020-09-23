using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack
{
    interface IBotPlayer
    {
        Command GetBotCommand(List<Command> availableCommands, Card openCroupierCard, int numOfHand);
        int GetBotBet(int minBet, int chips);
    }
}
