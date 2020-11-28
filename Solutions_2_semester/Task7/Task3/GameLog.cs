
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
			this.count = count;
			this.playerName = playerName;
			this.playerBankWas = playerBankWas;
			this.playerBetWas = playerBetWas;
			this.playerBetFieldWas = playerBetFieldWas;
			this.playerWin = playerWin;
			this.playerBankBecome = playerBankBecome;
			this.cardPull = cardPull;
			this.playerScoreBeforeExtraCard = playerScoreBeforeExtraCard;
			this.playerScore = playerScore;
			this.bankScoreBeforeExtraCard = bankScoreBeforeExtraCard;
			this.bankScore = bankScore;
			this.winField = winField;
		}

		public readonly int count = 0;
		public readonly string[] playerName = null;
		public readonly int[] playerBankWas = null;
		public readonly int[] playerBetWas = null;
		public readonly Field[] playerBetFieldWas = null;
		public readonly bool[] playerWin = null;
		public readonly int[] playerBankBecome = null;
		public readonly Card[] cardPull = null;
		public readonly int playerScoreBeforeExtraCard = 0;
		public readonly int playerScore = 0;
		public readonly int bankScoreBeforeExtraCard = 0;
		public readonly int bankScore = 0;
		public readonly Field winField = Field.None;
	}
}
