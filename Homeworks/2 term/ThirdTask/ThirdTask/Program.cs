using System;
using ThirdTask.GameDescription;

namespace ThirdTask
{
        class Program
        {
                static void Main()
                {
                        var player = new UserPlayer();
                        var game = new Game();

                        game.Start(player);
                }
        }
}