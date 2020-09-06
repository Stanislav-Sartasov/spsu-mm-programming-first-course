
namespace Task3
{
    public class Dealer
    {
        public Dealer(Deck deck)
        {
            cardPull = new Card[6];

            for (int i = 0; i < 6; i++)
                if (i == 2 || i == 5)
                    cardPull[i] = null;
                else
                    cardPull[i] = deck.Next();

            playerScore = (cardPull[0].cost + cardPull[1].cost) % 10;
            bankScore = (cardPull[3].cost + cardPull[4].cost) % 10;
            playerScoreBeforeExtraCard = playerScore;
            bankScoreBeforeExtraCard = bankScore;

            if (playerScore < 8 && bankScore < 8)
                if (playerScore < 6)
                {
                    cardPull[2] = deck.Next();
                    playerScore = (playerScore + cardPull[2].cost) % 10;
                    if (bankScore <= 2)
                        cardPull[5] = deck.Next();
                    else if (bankScore == 3)
                    {
                        if (cardPull[2].value != 8)
                            cardPull[5] = deck.Next();
                    }
                    else if (bankScore == 4)
                    {
                        if (cardPull[2].value >= 2 && cardPull[2].value <= 7)
                            cardPull[5] = deck.Next();
                    }
                    else if (bankScore == 5)
                    {
                        if (cardPull[2].value >= 4 && cardPull[2].value <= 7)
                            cardPull[5] = deck.Next();
                    }
                    else if (bankScore == 6)
                        if (cardPull[2].value == 6 || cardPull[2].value == 7)
                            cardPull[5] = deck.Next();
                }
                else if (bankScore < 6)
                    cardPull[5] = deck.Next();

            if (cardPull[5] != null)
                bankScore = (bankScore + cardPull[5].cost) % 10;

            if (playerScore > bankScore)
                winField = Field.player;
            else if (bankScore > playerScore)
                winField = Field.bank;
            else
                winField = Field.draw;
        }

        public readonly Card[] cardPull;
        public readonly int playerScoreBeforeExtraCard;
        public readonly int playerScore;
        public readonly int bankScoreBeforeExtraCard;
        public readonly int bankScore;
        public readonly Field winField;
    }
}