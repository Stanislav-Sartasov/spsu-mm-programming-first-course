using System;
using System.Collections.Generic;


namespace Blackjack
{

    public class Game
    {
        private Deck GameDeck { get; set; }
        private Croupier GameCroupier { get; set; }
        public List<Player> Players { get; set; }
        private Card OpenCroupierCard { get; set; }
        private Card ClosedCroupierCard { get; set; }
        private List<Command> AvailableCommands { get; set; }
        private BJCommands Commands { get; set; }
        private int MinBet { get; set; }
        private int CountOfRounds { get; set; }

        public Game(List<Player> players, Deck gameDeck, Croupier gameCroupier, int minBet, int countOfRounds)
        {
            Players = players;
            GameCroupier = gameCroupier;
            GameDeck = gameDeck;
            MinBet = minBet;
            CountOfRounds = countOfRounds;
            Commands = new BJCommands();
            AvailableCommands = new List<Command>();
        }

        private void DealsCards()
        {
            GameCroupier.CroupierHand.TakeCard(GameDeck.GetCard());
            GameCroupier.CroupierHand.TakeCard(GameDeck.GetCard());
            OpenCroupierCard = GameCroupier.CroupierHand.Cards[0];
            GameCroupier.CheckBJ();
            foreach (var player in Players)
            {
                if (player.InGame)
                {
                    player.Hands[0].TakeCard(GameDeck.GetCard());
                    player.Hands[0].TakeCard(GameDeck.GetCard());
                    player.CheckBJ();
                }
            }
        }

        private void CheckAvailableCommands(Player player, int numOfHand, int numOfTurn)
        {
            AvailableCommands.Add(Command.Hit);
            AvailableCommands.Add(Command.Stand);
            if (numOfTurn == 0)
                AvailableCommands.Add(Command.Surrender);
            if (player.Chips >= player.CurrentBet)
                AvailableCommands.Add(Command.Double);
            if (numOfTurn == 0)
                if (player.Hands[numOfHand].Cards[0].Value == player.Hands[numOfHand].Cards[1].Value && player.Chips >= player.CurrentBet)
                    AvailableCommands.Add(Command.Split);
        }

        private void PrintAvailableCommands()
        {
            foreach (var command in AvailableCommands)
                Console.WriteLine($"{(int)command} - {command}");
        }

        private void MakeBets()
        {
            foreach (var player in Players)
            {
                player.GetInfo();
                player.PrintChips();
                if (player.Chips >= MinBet && player.InGame)
                    for (int i = 0; i < player.Hands.Count; i++)
                    {
                        Commands.MakeBet(MinBet, player, i);
                        Console.WriteLine($"\n{player.Name} made a bet of {player.Hands[i].CurrentBet}\n");
                    }
            }
        }

        private void PlayerTurn(Player player)
        {
            Console.WriteLine($"Player's turn: {player.Name}");
            for (int i = 0; i < player.Hands.Count; i++)
            {
                player.PrintChips();
                int numOfTurn = 0;
                
                Command command;

                Console.WriteLine($"\nThe first croupier card: ");
                OpenCroupierCard.PrintCard();


                while (true)
                {
                    Console.WriteLine($"\nYour cards on hand #{i}:");
                    player.Hands[i].PrintCards();
                    Console.WriteLine($"\nYour points on hand #{i}:");
                    player.Hands[i].PrintPoints();
                    Console.WriteLine($"\nYour bet on hand #{i}:");
                    player.Hands[i].PrintBet();
                    Console.WriteLine($"\nYour bank is:");
                    player.PrintChips();

                    if (GameCroupier.BlackJack)
                    {
                        break;
                    }

                    if (player.Hands[i].Boosted)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Boosted!\n");
                        Console.ResetColor();
                        break;
                    }

                    if (player.BlackJack)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("BlackJack!\n");
                        Console.ResetColor();
                        break;
                    }

                    AvailableCommands.Clear();
                    CheckAvailableCommands(player, i, numOfTurn);
                    Console.WriteLine($"\nAvailable commands for hand #{i}:");
                    PrintAvailableCommands();
                    
                    command = Commands.GetCommand(player, AvailableCommands, i, OpenCroupierCard);
                    Console.WriteLine($"\nYour command: {command}\n");

                    if (Command.Stand.CompareTo(command) == 0)
                        break;
                    Commands.ApplyCommand(player, command, i, GameDeck);

                    if (Command.Surrender.CompareTo(command) == 0)
                    {
                        break;
                    }
                    numOfTurn++;
                }
            }
        }

        private void CroupierTurn()
        {
            ClosedCroupierCard = GameCroupier.CroupierHand.Cards[1];
            Console.WriteLine($"\nThe second croupier card: ");
            ClosedCroupierCard.PrintCard();

            if (GameCroupier.BlackJack)
            {
                Console.WriteLine("\nCroupier have Black Jack!");
            }

            else if (GameCroupier.CroupierHand.Points < 17)
            {
                Console.WriteLine("\nCroupier turn: ");
                while (GameCroupier.CroupierHand.Points < 17)
                {
                    var newCard = GameDeck.GetCard();
                    GameCroupier.CroupierHand.TakeCard(newCard);
                    Console.WriteLine("\nCroupier took card: ");
                    newCard.PrintCard();
                }
            }

            Console.WriteLine($"\nCroupier's final score: {GameCroupier.CroupierHand.Points}\n");
        }

        private void RoundOutcome()
        {
            foreach (var player in Players)
            {
                for (int i = 0; i < player.Hands.Count; i++)
                {
                    int returnedChips = 0;
                    Outcome outcome = Outcome.lose;

                    if (player.Hands[i].Boosted)
                    {
                        returnedChips = 0;
                    }
                    else if (player.Hands[i].Surrender)
                    {
                        returnedChips = player.Hands[0].CurrentBet / 2;
                        outcome = Outcome.surrender;
                    }
                    else
                    {
                        if (player.BlackJack && !GameCroupier.BlackJack)
                        {
                            returnedChips = (int)(2.5 * player.Hands[i].CurrentBet);
                            outcome = Outcome.win;
                        }
                        else if (player.BlackJack && GameCroupier.BlackJack)
                        {
                            returnedChips = player.Hands[i].CurrentBet;
                            outcome = Outcome.push;
                        }
                        else if (GameCroupier.CroupierHand.Boosted)
                        {
                            returnedChips = 2 * player.Hands[i].CurrentBet;
                            outcome = Outcome.win;
                        }
                        else if (player.Hands[i].Points > GameCroupier.CroupierHand.Points)
                        {
                            returnedChips = 2 * player.Hands[i].CurrentBet;
                            outcome = Outcome.win;
                        }
                        else if (player.Hands[i].Points == GameCroupier.CroupierHand.Points)
                        {
                            returnedChips = player.Hands[i].CurrentBet;
                            outcome = Outcome.push;
                        }
                    }
                    player.Chips += returnedChips;
                    player.Hands[i].CurrentBet = 0;
                    OutcomeStatus(outcome, returnedChips, player.Name, i);
                }
                player.FreeHands();
            }
           
        }

        private void OutcomeStatus(Outcome outcome, int returnedChips, string name, int numOfHand)
        {
            Console.WriteLine($"\nPlayer's name: {name}\nOutcome on hand #{numOfHand}: {outcome}\nChips returned: {returnedChips}");               
        }

        public void StartGame()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n========= BlackJack Console Game =========\n");
            Console.ResetColor();
            int eliminatedPlayers = 0;
            for (int numOfRound = 0 ; numOfRound < CountOfRounds; numOfRound++)
            {
                if (GameDeck.Cards.Count > (Players.Count + 1 - eliminatedPlayers) * 4)
                {
                    GameDeck.Reset();
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n========= New round starts! =========\n");
                Console.ResetColor();
                Console.WriteLine($"\nCards remaining {GameDeck.Cards.Count}");
                if (eliminatedPlayers == Players.Count)
                    break;
                Console.WriteLine("\nMake your bets!\n");
                MakeBets();
                DealsCards();
                foreach (var player in Players)
                {
                    if (player.Chips >= MinBet && player.InGame)
                        PlayerTurn(player);
                    else
                    {
                        if (player.InGame)
                        {
                            eliminatedPlayers++;
                            player.InGame = false;
                        }

                    }
                }
                CroupierTurn();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n========= Round results =========\n");
                Console.ResetColor();
                RoundOutcome();
           
                foreach (var player in Players)
                {
                    if (player.InGame)
                        player.FreeHands();
                }
                GameCroupier.FreeHands();
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n========= End of the game! =========\n");
            Console.ResetColor();
            
            foreach (Player player in Players)
            {
                player.GetInfo();
                player.PrintChips();
            }
        }
    }
}
