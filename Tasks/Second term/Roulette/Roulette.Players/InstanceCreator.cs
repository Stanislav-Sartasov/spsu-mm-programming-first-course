using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Players
{
    public static class InstanceCreator
    {
        public static IPlayer SelectBot(int level)
        {
            Bot bot;
            level %= 2;
            if (level == 0)
                bot = new Beginner();
            else
                bot = new Martingejl();
            return bot;
        }
    }
}
