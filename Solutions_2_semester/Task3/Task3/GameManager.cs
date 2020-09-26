using System;
using System.Collections;
using System.Collections.Generic;

namespace Task3
{
    public class GameManager
    {
        public GameManager(int startBank, int deckCount, double[] gameСoefficients, int maxPlayerCount)
        {
            if (startBank > 0)
                this.startBank = startBank;
            else
                this.startBank = StandardStartBank;
            if (deckCount > 0)
                this.deckCount = deckCount;
            else
                this.deckCount = StandardDeckCount;
            if (gameСoefficients == null)
                this.gameСoefficients = standardGameСoefficients;
            else if (gameСoefficients.Length == 3)
                this.gameСoefficients = gameСoefficients;
            else
                this.gameСoefficients = standardGameСoefficients;
            if (maxPlayerCount > 0)
                this.maxPlayerCount = maxPlayerCount;
            else
                this.maxPlayerCount = StandardMaxPlayerCount;
        }

        const int StandardStartBank = 10000;
        const int StandardDeckCount = 4;
        const int StandardMaxPlayerCount = 5;
        static double[] standardGameСoefficients = new double[] { 2, 1.95, 10 };            /// <summary>
        public readonly double[] gameСoefficients;
        int startBank;
        int maxPlayerCount;
        public int deckCount { get; private set; }
        List<PlayerSeat> playerSeats = new List<PlayerSeat>();
        Hashtable playersBase = new Hashtable();
        public Field lastWinField { get; private set; } = Field.None;
        public bool sessionStarted { get; private set; } = false;
        public Player AddPlayer(string playerName)
        {
            return AddPlayer(playerName, startBank);
        }
        internal Player AddPlayer(string playerName, int startBank)
        {
            if (sessionStarted || playerSeats.Count >= maxPlayerCount || playerName == null || startBank < 0 || playerSeats.FindIndex((x) => { return x.playerName == playerName; }) != -1)
                return null;
            PlayerSeat newSeat = new PlayerSeat(startBank, this, playerName);
            playerSeats.Add(newSeat);
            playersBase[newSeat.player] = newSeat;
            return newSeat.player;
        }
        public bool KickPlayer(Player player)
        {
            return KickPlayer((PlayerSeat)playersBase[player]);
        }
        public bool KickPlayer(PlayerSeat playerSeat)
        {
            if (playerSeat.QuitGame() || !playerSeat.active)
            {
                playerSeats.Remove(playerSeat);
                playersBase.Remove(playerSeat.player);
                return true;
            }
            return false;
        }
        public GameLog GetPlayers()
        {
            if (sessionStarted)
                return null;
            int count = playerSeats.Count;
            string[] names = new string[count];
            int[] banks = new int[count];
            for (int i = 0; i < Math.Min(count, playerSeats.Count); i++)
            {
                names[i] = (string)playerSeats[i].playerName.Clone();
                banks[i] = playerSeats[i].bank;
            }

            return new GameLog(Math.Min(count, playerSeats.Count), names, banks, null, null, null, null, null, -1, -1, -1, -1, Field.None);
        }
        public GameLog ProduceGame()
        {
            sessionStarted = true;
            
            for (int i = 0; i < playerSeats.Count;)
                if (!playerSeats[i].active)
                    playerSeats.RemoveAt(i);
                else if (!playerSeats[i].betDone)
                {
                    sessionStarted = false;
                    return null;
                }
                else i++;

            if (playerSeats.Count == 0)
            {
                sessionStarted = false;
                return null;
            }

            Dealer dealer = new Dealer(new Deck(deckCount));

            string[] playerName = new string[playerSeats.Count];
            int[] playerBankWas = new int[playerSeats.Count];
            int[] playerBetWas = new int[playerSeats.Count];
            Field[] playerBetFieldWas = new Field[playerSeats.Count];
            bool[] playerWin = new bool[playerSeats.Count];
            int[] playerBankBecome = new int[playerSeats.Count];

            List<PlayerSeat> playersForKick = new List<PlayerSeat>();

            for (int i = 0; i < playerSeats.Count; i++)
            {
                playerName[i] = (string)playerSeats[i].playerName.Clone();
                playerBankWas[i] = playerSeats[i].bank;
                playerBetWas[i] = playerSeats[i].bet;
                playerBetFieldWas[i] = playerSeats[i].betField;                

                if (playerSeats[i].betField == dealer.winField)
                {
                    playerSeats[i].PerformResult(gameСoefficients[(int)dealer.winField]);
                    playerWin[i] = true;
                }
                else
                {
                    playerSeats[i].PerformResult(0);
                    playerWin[i] = false;
                }

                playerBankBecome[i] = playerSeats[i].bank;
                if (playerSeats[i].bank == 0 && playerSeats[i].autoKick)
                {
                    playersForKick.Add(playerSeats[i]);
                    KickPlayer(playerSeats[i]);
                }
            }

            lastWinField = dealer.winField;
            sessionStarted = false;

            while(playersForKick.Count > 0)
            {
                KickPlayer(playersForKick[0]);
                playersForKick.RemoveAt(0);
            }

            return new GameLog(
                playerSeats.Count,
                playerName,
                playerBankWas,
                playerBetWas,
                playerBetFieldWas,
                playerWin,
                playerBankBecome,
                dealer.cardPull,
                dealer.playerScoreBeforeExtraCard,
                dealer.playerScore,
                dealer.bankScoreBeforeExtraCard,
                dealer.bankScore,
                dealer.winField
                );
        }
    }
}