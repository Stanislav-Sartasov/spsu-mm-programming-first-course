using System;
using ThirdTask.GameDescription;

namespace ThirdTask
{
        class Program
        {
                static void Main()
                {
                        //создание игрока/бота (дилер внутри - у него своя стратегия?)
                        var game = new Game();
                        game.Start();
                }
        }
}

//заметки на русском для себя - удалю их в финальной версии программы
//https://casino-reiting.online/блэкджек-правила-и-стратегия-игры/#2_Frank_Casino - правила для себя
