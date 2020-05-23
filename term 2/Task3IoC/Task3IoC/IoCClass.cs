using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using AbstractPlayer;
using System;

namespace Task3IoC
{
    [TestClass]
    public class IoCClass
    {
        [TestMethod]
        public void IoCTest()
        {
            IUnityContainer unityContainer = new UnityContainer();
            unityContainer.RegisterType<AbstractPlayer.AbstractPlayer, BotOliver>();
            unityContainer.RegisterType<AbstractPlayer.AbstractPlayer, BotStepan>();
            
            BotOliver botOliver = unityContainer.Resolve<BotOliver>();
            BotStepan botStepan = unityContainer.Resolve<BotStepan>();
            botOliver.Bet();
            botStepan.Bet();
            Console.WriteLine(botOliver.GetBalance());
            Console.WriteLine(botStepan.GetBalance());
        }
    }
}
