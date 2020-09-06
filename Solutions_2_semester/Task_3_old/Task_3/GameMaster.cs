using System;
using System.Collections.Generic;
using System.Text;

namespace Task3
{
    public class GameMaster
    {
        public GameMaster(int startBudget, int numberOfDecks)
        {
            if (startBudget > 0)
                this.startBudget = startBudget;
            this.numberOfDecks = numberOfDecks;
        }
        public GameMaster(int startBudget)
        {
            if (startBudget > 0)
                this.startBudget = startBudget;
        }
        public class PlayerPlace
        {
            public PlayerPlace(GameMaster gameSession)
            {
                thisGameSession = gameSession;
                coefficients = thisGameSession.coefficients;
            }
            GameMaster thisGameSession;
            internal Player currentPlayer;
            int bateIn;
            int bateTypeIn;
            bool areBatedIn = false;
            internal readonly double[] coefficients;
            internal int lastWinType = -1;
            internal void MakeBate(int count, int type)
            {
                if (!areBatedIn && count > 0)
                {
                    if (count > currentPlayer.Budget)
                        return;
                    currentPlayer.Budget -= count;
                    bateIn = count;
                    bateTypeIn = type;
                    areBatedIn = true;
                }
            }
            internal void Leave()
            {
                thisGameSession.CickPlayer(currentPlayer);
            }
            internal void Win(double count)
            {
                if (areBatedIn)
                {
                    currentPlayer.Budget += (int)(bateIn * count);
                    bateIn = 0;
                    bateTypeIn = 0;
                    areBatedIn = false;
                }
            }
            internal void Loose()
            {
                if (areBatedIn)
                {
                    bateIn = 0;
                    bateTypeIn = 0;
                    areBatedIn = false;
                }
            }
            internal int Bate
            {
                get
                {
                    return bateIn;
                }
            }
            internal int BateType
            {
                get
                {
                    return bateTypeIn;
                }
            }
            internal bool AreBated
            {
                get
                {
                    return areBatedIn;
                }
            }
        }
        public class Log
        {
            public int playerCount { get; internal set; }
            public string[] playerName{ get; internal set; }
            public int[] bate{ get; internal set; }
            public int[] bateType{ get; internal set; }
            public int[] budgetWas{ get; internal set; }
            public int[] budgetBecome{ get; internal set; }
            public bool[] win{ get; internal set; }
            public Card[] bank{ get; internal set; }
            public Card[] play{ get; internal set; }
            public int winType{ get; internal set; }
            public int bankScoreBnc{ get; internal set; }    //before new card
            public int playScoreBnc{ get; internal set; }
            public int bankScoreAnc{ get; internal set; }    //after new card
            public int playScoreAnc{ get; internal set; }
            public Log Clone()
            {
                Log newLog = new Log();
                newLog.playerCount = playerCount;
                if (playerName != null)
                {
                    newLog.playerName = new string[playerName.Length];
                    for (int i = 0; i < playerName.Length; i++)
                        newLog.playerName[i] = (string)playerName[i].Clone();
                }
                if (bate != null)
                {
                    newLog.bate = new int[bate.Length];
                    for (int i = 0; i < bate.Length; i++)
                        newLog.bate[i] = bate[i];
                }
                if (bateType != null)
                {
                    newLog.bateType = new int[bateType.Length];
                    for (int i = 0; i < bateType.Length; i++)
                        newLog.bateType[i] = bateType[i];
                }
                if (budgetWas != null)
                {
                    newLog.budgetWas = new int[budgetWas.Length];
                    for (int i = 0; i < budgetWas.Length; i++)
                        newLog.budgetWas[i] = budgetWas[i];
                }
                if (budgetBecome != null)
                {
                    newLog.budgetBecome = new int[budgetBecome.Length];
                    for (int i = 0; i < budgetBecome.Length; i++)
                        newLog.budgetBecome[i] = budgetBecome[i];
                }
                if (win != null)
                {
                    newLog.win = new bool[win.Length];
                    for (int i = 0; i < win.Length; i++)
                        newLog.win[i] = win[i];
                }
                if (bank != null)
                {
                    newLog.bank = new Card[bank.Length];
                    for (int i = 0; i < bank.Length; i++)
                        if (bank[i] == null)
                            newLog.bank[i] = null;
                        else
                            newLog.bank[i] = bank[i].Clone();
                }
                if (play != null)
                {
                    newLog.play = new Card[play.Length];
                    for (int i = 0; i < play.Length; i++)
                        if (play[i] == null)
                            newLog.play[i] = null;
                        else
                            newLog.play[i] = play[i].Clone();
                }
                newLog.winType = winType;
                newLog.bankScoreBnc = bankScoreBnc;
                newLog.playScoreBnc = playScoreBnc;
                newLog.bankScoreAnc = bankScoreAnc;
                newLog.playScoreAnc = playScoreAnc;
                return newLog;
            }
        }
        internal PlayerPlace[] PlayerList
        {
            get
            {
                return (PlayerPlace[])playerListIn.Clone();
            }
        }

        double[] coefficients = new double[] { 2, 1.95, 9 };
        int startBudget = 1000;
        PlayerPlace[] playerListIn = null;
        int numberOfDecks = 4;
        Log log;

        public Log GetLog()
        {
            return log.Clone();
        }
        public int AddPlayer(Player newPlayer)
        {
            if (newPlayer.place != null)
                return -1;
            if (playerListIn == null)
            {
                playerListIn = new PlayerPlace[1];
                playerListIn[0] = new PlayerPlace(this);
                playerListIn[0].currentPlayer = newPlayer;
                newPlayer.place = playerListIn[0];
                if (!newPlayer.AreBudgeted)
                {
                    newPlayer.Budget = startBudget;
                    newPlayer.AreBudgeted = true;
                }
                return 0;
            }

            PlayerPlace[] newPlayerList = new PlayerPlace[playerListIn.Length + 1];

            for (int i = 0; i < playerListIn.Length; i++)
            {
                if (playerListIn[i].currentPlayer == newPlayer)
                    return -1;
                newPlayerList[i] = playerListIn[i];
            }

            newPlayerList[newPlayerList.Length - 1] = new PlayerPlace(this);
            newPlayerList[newPlayerList.Length - 1].currentPlayer = newPlayer;
            newPlayer.place = newPlayerList[newPlayerList.Length - 1];
            playerListIn = newPlayerList;
            if (!newPlayer.AreBudgeted)
            {
                newPlayer.Budget = startBudget;
                newPlayer.AreBudgeted = true;
            }
            return 0;
        }
        public int CickPlayer(Player currentPlayer)
        {
            if (currentPlayer.place == null)
                return 0;

            if (playerListIn.Length == 1)
            {
                playerListIn = null;
                return 0;
            }

            int pos = -1;
            for (int i = 0; i < playerListIn.Length; i++)
                if (playerListIn[i].currentPlayer == currentPlayer)
                {
                    if (playerListIn[i].AreBated)
                        return -1;
                    pos = i;
                    break;
                }
            if (pos < 0)
                return 0;

            PlayerPlace[] NewPlayerList = new PlayerPlace[playerListIn.Length - 1];

            for (int i = 0; i < pos; i++)
                NewPlayerList[i] = playerListIn[i];

            for (int i = pos + 1; i < playerListIn.Length; i++)
                NewPlayerList[i - 1] = playerListIn[i];

            playerListIn[pos].currentPlayer.place = null;
            playerListIn = NewPlayerList;
            return 0;
        }
        public int StartGame()
        {
            if (playerListIn == null)
                return -1;

            for (int i = 0; i < playerListIn.Length; i++)
                if (!playerListIn[i].AreBated)
                    return -1;

            log = new Log();

            log.bate = new int[playerListIn.Length];
            log.bateType = new int[playerListIn.Length];
            log.budgetWas = new int[playerListIn.Length];
            log.budgetBecome = new int[playerListIn.Length];
            log.win = new bool[playerListIn.Length];

            log.playerCount = playerListIn.Length;

            log.playerName = new string[log.playerCount];
            for (int i = 0; i < log.playerCount; i++)
                log.playerName[i] = (string)playerListIn[i].currentPlayer.Name.Clone();

            log.budgetWas = new int[playerListIn.Length];
            for (int i = 0; i < playerListIn.Length; i++)
            {
                log.bate[i] = playerListIn[i].Bate;
                log.budgetWas[i] = playerListIn[i].currentPlayer.Budget + log.bate[i];
            }

            Card[] bank = new Card[3] { null, null, null };
            int bankScore = 0;
            Card[] play = new Card[3] { null, null, null };
            int playScore = 0;

            Deck deck = new Deck(numberOfDecks);

            for (int i = 0; i < 2; i++)
            {
                bank[i] = deck.TakeRand();
                bankScore = (bankScore + bank[i].Cost) % 10;

                play[i] = deck.TakeRand();
                playScore = (playScore + play[i].Cost) % 10;
            }

            log.bankScoreBnc = bankScore;
            log.playScoreBnc = playScore;

            if (playScore < 8 && bankScore < 8)
            {

                if (playScore <= 5 && bankScore < 8)
                {
                    play[2] = deck.TakeRand();
                    playScore = (playScore + play[2].Cost) % 10;
                }

                if (play[2] == null)
                {
                    if (bankScore <= 5)
                    {
                        bank[2] = deck.TakeRand();
                        bankScore = (bankScore + bank[2].Cost) % 10;
                    }
                }
                else
                {
                    if (play[2].Dignity == 9 || play[2].Dignity == 10 || play[2].Dignity == 1)
                    {
                        if (bankScore <= 3)
                        {
                            bank[2] = deck.TakeRand();
                            bankScore = (bankScore + bank[2].Cost) % 10;
                        }
                    }
                    else if (play[2].Dignity == 8)
                    {
                        if (bankScore <= 2)
                        {
                            bank[2] = deck.TakeRand();
                            bankScore = (bankScore + bank[2].Cost) % 10;
                        }
                    }
                    else if (play[2].Dignity == 6 || play[2].Dignity == 7)
                    {
                        if (bankScore <= 6)
                        {
                            bank[2] = deck.TakeRand();
                            bankScore = (bankScore + bank[2].Cost) % 10;
                        }
                    }
                    else if (play[2].Dignity == 4 || play[2].Dignity == 5)
                    {
                        if (bankScore <= 5)
                        {
                            bank[2] = deck.TakeRand();
                            bankScore = (bankScore + bank[2].Cost) % 10;
                        }
                    }
                    else if (play[2].Dignity == 2 || play[2].Dignity == 3)
                    {
                        if (bankScore <= 4)
                        {
                            bank[2] = deck.TakeRand();
                            bankScore = (bankScore + bank[2].Cost) % 10;
                        }
                    }
                }
            }

            log.play = play;
            log.bank = bank;

            log.bankScoreAnc = bankScore;
            log.playScoreAnc = playScore;

            int winType;

            if (playScore > bankScore)
                winType = 0;
            else if (playScore < bankScore)
                winType = 1;
            else
                winType = 2;

            log.winType = winType;
            for (int i = 0; i < playerListIn.Length; i++)
                playerListIn[i].lastWinType = winType;

            for (int i = 0; i < playerListIn.Length; i++)
            {
                log.bateType[i] = playerListIn[i].BateType;
                if (playerListIn[i].BateType == winType)
                {
                    if (winType == 0)
                        playerListIn[i].Win(coefficients[0]);
                    else if (winType == 1)
                        playerListIn[i].Win(coefficients[1]);
                    else
                        playerListIn[i].Win(coefficients[2]);
                    log.win[i] = true;
                }
                else
                {
                    playerListIn[i].Loose();
                    log.win[i] = false;
                }

                log.budgetBecome[i] = playerListIn[i].currentPlayer.Budget;

                if (playerListIn[i].currentPlayer.Budget <= 0)
                {
                    CickPlayer(playerListIn[i].currentPlayer);
                    if (playerListIn == null)
                        break;
                }
            }

            return 0;
        }
    }
}