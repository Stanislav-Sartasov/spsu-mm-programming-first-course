
namespace Task3
{
	public class Player
	{
		public Player(PlayerSeat playerSeat)
		{
			seat = playerSeat;
		}

		PlayerSeat seat;
		public int Bank
		{
			get
			{
				return seat.Bank;
			}
		}
		public int Bet
		{
			get
			{
				return seat.Bet;
			}
		}
		public Field BetField
		{
			get
			{
				return seat.BetField;
			}
		}
		public bool BetDone
		{
			get
			{
				return seat.BetDone;
			}
		}
		public string PlayerName
		{
			get
			{
				return seat.playerName;
			}
		}
		public double[] GameСoefficients
		{
			get
			{
				return seat.GameСoefficients;
			}
		}
		public Field LastWinField
		{
			get
			{
				return seat.LastWinField;
			}
		}
		public bool AutoKick
		{
			get
			{
				return seat.AutoKick;
			}
			set
			{
				seat.AutoKick = value;
			}
		}
		public bool Active
		{
			get
			{
				return seat.Active;
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
