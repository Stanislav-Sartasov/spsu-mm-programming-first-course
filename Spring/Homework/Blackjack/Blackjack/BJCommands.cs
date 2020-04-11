using System;
using System.Collections.Generic;


namespace Blackjack
{
    public enum Command
    {
        Hit = 1, Stand = 2, Surrender = 3, Double = 4, Split = 5
    }

    public class BJCommands
    {
        public int CorrectInput()
        {
            while (true)
            {
                string Input = Console.ReadLine();
                if (int.TryParse(Input, out int number))
                {
                    return number;
                }
                else
                    Console.WriteLine("Not an integer! Try again!\n");
            }
        }

        public Command GetCommand(Player player, List<Command> availableCommands, int numOfHand, Card openCroupierCard)
        {
            switch (player.Status)
            {
                case PlayerStatus.RealPlayer:
                    Console.WriteLine("\nEnter number of command: ");
                    int numOfCommand;
                    while (true)
                    {
                        numOfCommand = CorrectInput();
                        if (availableCommands.Contains((Command)numOfCommand))
                            return (Command)numOfCommand;
                        else
                            Console.WriteLine("Incorrect number of command!\n");
                    }
                case PlayerStatus.Bot:
                    IBotPlayer bot = (IBotPlayer)player;
                    Command botCommand = bot.BotCommand(availableCommands, openCroupierCard, numOfHand);
                    return botCommand;
                default:
                    throw new Exception();
            }
        }

        public void MakeBet(int minBet, Player player, int numOfHand)
        {
            switch (player.Status)
            {
                case PlayerStatus.RealPlayer:
                    int bet;
                    do
                    {
                        Console.WriteLine($"\nMinimal bet is {minBet}");
                        Console.WriteLine($"Please enter sum of bet for hand #{numOfHand}: ");
                        bet = CorrectInput();
                        if (player.Chips < bet)
                            Console.WriteLine("Not enough chips! Change the amount!");
                    } while (bet < minBet && player.Chips >= bet);
                    player.Hands[numOfHand].CurrentBet += bet;
                    player.Chips -= bet;
                    break;
                case PlayerStatus.Bot:
                    IBotPlayer bot = (IBotPlayer)player;
                    bet = bot.BotBet(minBet, player.Chips);
                    player.Hands[numOfHand].CurrentBet += bet;
                    player.Chips -= bet;
                    break;
                default:
                    throw new Exception();
            }

        }

        private void Hit(Player player, int numOfHand, Deck deck)
        {
            player.Hands[numOfHand].TakeCard(deck.GetCard());
        }

        private void Double(Player player, int numOfHand, Deck deck)
        {
            Hit(player, numOfHand, deck);
            player.Chips -= player.Hands[numOfHand].CurrentBet;
            player.Hands[numOfHand].CurrentBet += player.Hands[numOfHand].CurrentBet;
        }

        private void Split(Player player, int numOfHand, Deck deck)
        {
            var splitCard = player.Hands[numOfHand].PopCard();
            player.Hands[numOfHand].TakeCard(deck.GetCard());
            var newHand = new Hand();
            newHand.TakeCard(splitCard);
            newHand.TakeCard(deck.GetCard());
            player.Chips -= player.Hands[numOfHand].CurrentBet;
            newHand.CurrentBet += player.Hands[numOfHand].CurrentBet;
            player.Hands.Add(newHand);
        }

        private void Surrender(Player player)
        {
            player.Hands[0].Surrender = true;
        }

        public void ApplyCommand(Player player, Command command, int numOfHand, Deck deck)
        {
            switch (command)
            {
                case Command.Double:
                    Double(player, numOfHand, deck);
                    break;
                case Command.Hit:
                    Hit(player, numOfHand, deck);
                    break;
                case Command.Split:
                    Split(player, numOfHand, deck);
                    break;
                case Command.Surrender:
                    Surrender(player);
                    break;
                default:
                    throw new Exception();
            }
        }
    }
}
