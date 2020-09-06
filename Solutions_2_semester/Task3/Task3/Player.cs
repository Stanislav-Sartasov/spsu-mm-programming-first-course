
namespace Task3
{
    public class Player
    {
        public Player(PlayerSeat playerSeat)
        {
            seat = playerSeat;
        }

        PlayerSeat seat;
        public int bank
        {
            get
            {
                return seat.bank;
            }
        }
        public int bet
        {
            get
            {
                return seat.bet;
            }
        }
        public Field betField
        {
            get
            {
                return seat.betField;
            }
        }
        public bool betDone
        {
            get
            {
                return seat.betDone;
            }
        }
        public string playerName
        {
            get
            {
                return seat.playerName;
            }
        }
        public double[] gameСoefficients
        {
            get
            {
                return seat.gameСoefficients;
            }
        }
        public Field lastWinField
        {
            get
            {
                return seat.lastWinField;
            }
        }
        public bool autoKick
        {
            get
            {
                return seat.autoKick;
            }
            set
            {
                seat.autoKick = value;
            }
        }
        public bool active
        {
            get
            {
                return seat.active;
            }
        }
        public bool MakeBet(int count, Field field)
        {
            return seat.MakeBet(count, field);
        }
        public bool QuitGame()
        {
            return seat.QuitGame();
        }
    }
}
