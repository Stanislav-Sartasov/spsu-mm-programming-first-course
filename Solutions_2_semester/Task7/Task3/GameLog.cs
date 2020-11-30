
namespace Task3
{
	public class GameLog
	{
		public GameLog(
			int count,
			string[] playerName,
			int[] playerBankWas,
			int[] playerBetWas,
			Field[] playerBetFieldWas,
			bool[] playerWin,
			int[] playerBankBecome,
			Card[] cardPull,
			int playerScoreBeforeExtraCard,
			int playerScore,
			int bankScoreBeforeExtraCard,
			int bankScore,
			Field winField
			)
		{
			Count = count;
			this.playerName = playerName;
			this.playerBankWas = playerBankWas;
			this.playerBetWas = playerBetWas;
			this.playerBetFieldWas = playerBetFieldWas;
			this.playerWin = playerWin;
			this.playerBankBecome = playerBankBecome;
			this.cardPull = cardPull;
			PlayerScoreBeforeExtraCard = playerScoreBeforeExtraCard;
			PlayerScore = playerScore;
			BankScoreBeforeExtraCard = bankScoreBeforeExtraCard;
			BankScore = bankScore;
			WinField = winField;
		}

		string[] playerName = null;
		int[] playerBankWas = null;
		int[] playerBetWas = null;
		Field[] playerBetFieldWas = null;
		bool[] playerWin = null;
		int[] playerBankBecome = null;
		Card[] cardPull = null;

		public int Count { get; private set; } = 0;
		public string[] PlayerName
		{
			get
			{
				return (string[])playerName.Clone();
			}
		}
		public int[] PlayerBankWas
		{
			get
			{
				return (int[])playerBankWas.Clone();
			}
		}
		public int[] PlayerBetWas
		{
			get
			{
				return (int[])playerBetWas.Clone();
			}
		}
		public Field[] PlayerBetFieldWas
		{
			get
			{
				return (Field[])playerBetFieldWas.Clone();
			}
		}
		public bool[] PlayerWin
		{
			get
			{
				return (bool[])playerWin.Clone();
			}
		}
		public int[] PlayerBankBecome
		{
			get
			{
				return (int[])playerBankBecome.Clone();
			}
		}
		public Card[] CardPull
		{
			get
			{
				return (Card[])cardPull.Clone();
			}
		}
		public int PlayerScoreBeforeExtraCard { get; } = 0;
		public int PlayerScore { get; } = 0;
		public int BankScoreBeforeExtraCard { get; } = 0;
		public int BankScore { get; } = 0;
		public Field WinField { get; } = Field.None;
	}
}
