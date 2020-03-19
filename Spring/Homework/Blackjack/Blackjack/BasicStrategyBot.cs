using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack
{
    public class BasicStrategyBot : Player, IBotPlayer
    {

        public int BotBet(int minBet, int chips)
        {
            return (minBet > chips / 50) ? minBet : chips / 50;
        }

        public Command BotCommand(List<Command> availableCommands, Card croupierOpenCard, int numOfHand)
        {
            int points = Hands[numOfHand].Points;
            int croupierPoints = (int)croupierOpenCard.Value;
            switch (points)
            {
                case 4:
                    if (availableCommands.Contains(Command.Split) && croupierPoints <= 7)
                        return Command.Split;
                    else
                        return Command.Hit;
                case 5:
                    return Command.Hit;
                case 6:
                    if (availableCommands.Contains(Command.Split) && croupierPoints <= 7)
                        return Command.Split;
                    else
                        return Command.Hit;
                case 7:
                case 8:
                    return Command.Hit;
                case 9:
                    if (croupierPoints >= 3 && croupierPoints <= 6 && availableCommands.Contains(Command.Double))
                        return Command.Double;
                    else
                        return Command.Hit;
                case 10:
                    if (croupierPoints == 10 || croupierPoints == 11)
                        return Command.Hit;
                    else
                        return Command.Double;
                case 11:
                    if (croupierPoints == 11)
                        return Command.Hit;
                    else
                        return Command.Double;

                case 12:
                    if (Hands[numOfHand].Aces == 2 && croupierPoints == 11)
                        return Command.Hit;
                    else if (Hands[numOfHand].Aces == 2)
                        return Command.Split;
                    else if (availableCommands.Contains(Command.Split) && croupierPoints <= 6)
                        return Command.Split;
                    else
                    {
                        if (croupierPoints >= 4 && croupierPoints <= 6)
                            return Command.Stand;
                        else
                            return Command.Hit;
                    }
                case 13:
                case 14:
                    if (availableCommands.Contains(Command.Split) && croupierPoints <= 6)
                        return Command.Split;
                    else if (Hands[numOfHand].AcesAs11 == 0)
                    {
                        if (croupierPoints <= 6)
                            return Command.Stand;
                        else
                            return Command.Hit;
                    }
                    else
                    {
                        if (croupierPoints >= 5 && croupierPoints <= 6 && availableCommands.Contains(Command.Double))
                            return Command.Double;
                        else
                            return Command.Hit;
                    }
                case 15:
                case 16:
                    if (availableCommands.Contains(Command.Split) && croupierPoints == 11)
                        return Command.Hit;
                    else if (availableCommands.Contains(Command.Split))
                        return Command.Split;
                    else
                    {
                        if (Hands[numOfHand].AcesAs11 == 0)
                        {
                            if (croupierPoints >= 10 || croupierPoints <= 6)
                                return Command.Stand;
                            else
                                return Command.Hit;
                        }
                        else
                        {
                            if (croupierPoints >= 5 && croupierPoints <= 6 && availableCommands.Contains(Command.Double))
                                return Command.Double;
                            else
                                return Command.Hit;
                        }
                    }
                case 17:
                    if (Hands[numOfHand].AcesAs11 != 0)
                    {
                        if (croupierPoints >= 5 && croupierPoints <= 6 && availableCommands.Contains(Command.Double))
                            return Command.Double;
                        else
                            return Command.Hit;
                    }
                    else
                    {
                        return Command.Stand;
                    }
                case 18:
                    if (availableCommands.Contains(Command.Split) && (croupierPoints == 11 || croupierPoints == 10 || croupierPoints == 7))
                        return Command.Stand;
                    else if (availableCommands.Contains(Command.Split))
                        return Command.Split;
                    else
                    {
                        if (Hands[numOfHand].AcesAs11 != 0)
                        {
                            if (croupierPoints >= 9)
                                return Command.Hit;
                            else
                                return Command.Stand;
                        }
                        else
                        {
                            return Command.Stand;
                        }
                    }
                default:
                    return Command.Stand;
            }
        }

        public BasicStrategyBot(int chips, string name, PlayerStatus status) :
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
