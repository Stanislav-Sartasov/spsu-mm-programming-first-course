using System;
using System.Collections.Generic;


namespace Blackjack
{
    public enum PlayerStatus
    {
        Bot,
        RealPlayer
    }

    public abstract class AGambler
    {
        public string Name { get; set; }

        public PlayerStatus Status { get; set; }

        public bool BlackJack { get; protected set; }

        public abstract void CheckBJ();

        public abstract void FreeHands();
      
        public AGambler(string name, PlayerStatus status)
        {
            Name = name;
            Status = status;
            BlackJack = false;
            
        }

        public AGambler(PlayerStatus status)
        {
            Status = status;
            BlackJack = false;
        }

        public virtual void GetInfo()
        {
            Console.WriteLine($"Player's name: {Name}"); 
        }
    }
}
