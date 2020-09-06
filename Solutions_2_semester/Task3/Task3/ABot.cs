using System;
using System.Collections.Generic;
using System.Text;

namespace Task3
{
    public abstract class ABot
    {
        protected static Random rand = new Random();
        double leaveBankCoeffUp = 5;
        double leaveBankCoeffDown = 0.01;
        bool autoLeaving 
        {
            get
            {
                return connectedPlayer.autoKick;
            }
            set
            {
                connectedPlayer.autoKick = value;
            } 
        }
        protected int startBank = -1;
        public Player connectedPlayer { get; protected set; }
        public int bank
        {
            get
            {
                return connectedPlayer.bank;
            }
        }

        public void Connect(Player player)
        {
            connectedPlayer = player;
            ResetStartBank();
        }
        public void Disconnect()
        {
            connectedPlayer = null;
        }

        public double LeaveBankCoeffUp
        {
            get
            {
                return leaveBankCoeffUp;
            }
            set
            {
                if (connectedPlayer == null && value >= 1)
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
                if (connectedPlayer == null && value >= 0 && value <= 1)
                    leaveBankCoeffDown = value;
            }
        }
        public void ResetStartBank()
        {
            startBank = bank;
        }
        protected bool NeedToLeave()
        {
            return (autoLeaving && (bank > startBank * leaveBankCoeffUp || bank < startBank * leaveBankCoeffDown)) || bank <= 0;
        }
        public abstract bool MakeBet();
    }
}