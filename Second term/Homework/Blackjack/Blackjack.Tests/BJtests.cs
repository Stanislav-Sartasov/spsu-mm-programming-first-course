using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace Blackjack.Tests
{
    [TestClass]
    public class BlackJackTests
    {

        private int averagedScoreRandom;
        private int averagedScoreBasic;
        readonly int numOfRounds = 400;
        readonly int numOfGames = 10;

        [TestMethod]
        public void BlackJackTest()
        {
            for (int i = 0; i < numOfGames; i++)
            {
                Game game;
                game = new Game(new List<Player> { new RandomBot(5000, "Random_Bot", PlayerStatus.Bot), new BasicStrategyBot(5000, "BSBot", PlayerStatus.Bot) },
                new Deck(), new Croupier(PlayerStatus.Bot), 10, numOfRounds);
                game.StartGame();
                averagedScoreRandom += game.Players[0].Chips;
                averagedScoreBasic += game.Players[1].Chips;
            }
            Console.WriteLine($"Average scores after {numOfGames} games:");
            Console.WriteLine($"Random_Bot: {averagedScoreRandom / numOfGames}\nBSBot: {averagedScoreBasic / numOfGames}");
        }
    }
}
