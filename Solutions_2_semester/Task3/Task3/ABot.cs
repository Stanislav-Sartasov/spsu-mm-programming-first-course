using System;

namespace Task3
{
	public abstract class ABot
	{
		protected static Random rand = new Random();
		double leaveBankCoeffUp = 5;
		double leaveBankCoeffDown = 0.01;
		bool AutoLeaving 
		{
			get
			{
				return ConnectedPlayer.AutoKick;
			}
			set
			{
				ConnectedPlayer.AutoKick = value;
			} 
		}
		protected int startBank = -1;
		public Player ConnectedPlayer { get; protected set; }
		public int Bank
		{
			get
			{
				return ConnectedPlayer.Bank;
			}
		}

		public void Connect(Player player)
		{
			ConnectedPlayer = player;
			ResetStartBank();
		}
		public void Disconnect()
		{
			ConnectedPlayer = null;
		}

		public double LeaveBankCoeffUp
		{
			get
			{
				return leaveBankCoeffUp;
			}
			set
			{
				if (ConnectedPlayer == null && value >= 1)
					leaveBankCoeffUp = value;
			}
		}
		public double LeaveBankCoeffDown
		{
			get
			{
				return leaveBankCoeffDown;
			}
			set
			{
				if (ConnectedPlayer == null && value >= 0 && value <= 1)
					leaveBankCoeffDown = value;
			}
		}
		public void ResetStartBank()
		{
			startBank = Bank;
		}
		protected bool NeedToLeave()
		{
			return (AutoLeaving && (Bank > startBank * leaveBankCoeffUp || Bank < startBank * leaveBankCoeffDown)) || Bank <= 0;
		}
		public abstract bool MakeBet();
	}
}