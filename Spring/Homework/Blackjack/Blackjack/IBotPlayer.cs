using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack
{
    interface IBotPlayer
    {
        Command BotCommand(List<Command> availableCommands, Card openCroupierCard, int numOfHand);
        int BotBet(int minBet, int chips);
    }
}
