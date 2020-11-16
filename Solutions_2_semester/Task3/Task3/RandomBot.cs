
namespace Task3
{
    public class RandomBot : ABot
    {
        double reserveCoeff = 0.66;
        int minBet = -1;
        double minBetCoeff = 0.02;
        bool minBetCoeffChanged = false;

        public double ReserveCoeff
        {
            get
            {
                return reserveCoeff;
            }
            set
            {
                if (value >= 0 && value <= 1)
                    reserveCoeff = value;
                else if (value > 1)
                    reserveCoeff = 1;
                else if (value < 0)
                    reserveCoeff = 0;
            }
        }
        public double MinBetCoeff
        {
            get
            {
                return minBetCoeff;
            }
            set
            {
                if (value >= 0 && value <= 1)
                    minBetCoeff = value;
                else if (value > 1)
                    minBetCoeff = 1;
                else if (value < 0)
                    minBetCoeff = 0;
                minBetCoeffChanged = true;
            }
        }
        public int MinBet
        {
            get
            {
                return minBet;
            }
            set
            {
                if (value > 0)
                    minBet = value;
            }
        }

        public override bool MakeBet()
        {
            if (ConnectedPlayer == null)
                return false;
            if (minBet == -1 || minBetCoeffChanged)
            {
                minBet = (int)(startBank * minBetCoeff);
                minBetCoeffChanged = false;
            }
            int bet = rand.Next((int)(Bank * (1 - reserveCoeff)));
            if (bet < minBet)
                bet = minBet;
            if (bet > Bank)
                bet = Bank;
            ConnectedPlayer.MakeBet(bet, (Field)rand.Next(0, 3));
            return true;
        }
    }
}
