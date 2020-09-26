
namespace Task3
{
    public class PlayerSeat
    {
        public PlayerSeat(int startBank, GameManager creator, string playerName)
        {
            if (startBank > 0)
                bank = startBank;
            else
                bank = StandardStartBank;
            currentGameSession = creator;
            if (playerName == null)
                this.playerName = "?";
            else
                this.playerName = playerName;
            active = true;
            player = new Player(this);
        }

        const int StandardStartBank = 10000;
        internal int bank { get; private set; }
        internal int bet { get; private set; } = 0;
        internal Field betField { get; private set; }
        internal bool betDone { get; private set; } = false;
        internal double[] gameСoefficients
        {
            get
            {
                return currentGameSession.gameСoefficients;
            }
        }
        internal Field lastWinField
        {
            get
            {
                return currentGameSession.lastWinField;
            }
        }
        internal readonly Player player;
        internal readonly string playerName;
        internal bool autoKick { get; set; } = true;
        internal bool active { get; private set; }

        GameManager currentGameSession;
        internal bool MakeBet(int count, Field field)
        {
            if (!active || betDone)
                return false;
            if ((int)field < -1 || (int)field > 2 || (count <= 0 && field != Field.None))
                return false;

            if (field == Field.None || bank == 0)
            {
                bet = 0;
                betField = Field.None;
                betDone = true;
                return true;
            }

            if (count <= bank)
            {
                bet = count;
                bank -= count;
            }
            else if (count > bank)
            {
                bet = bank;
                bank = 0;
            }

            betField = field;
            betDone = true;
            return true;
        }
        internal bool QuitGame()
        {
            if (currentGameSession.sessionStarted || !active)
                return false;
            if (betDone)
            {
                int betWas = bet;
                bet = 0;
                bank += betWas;
                betField = Field.None;
                betDone = false;
            }
            currentGameSession = null;
            active = false;
            return true;
        }
        internal void PerformResult(double coefficient)
        {
            bank += (int)(bet * coefficient);
            bet = 0;
            betField = Field.None;
            betDone = false;
        }
    }
}