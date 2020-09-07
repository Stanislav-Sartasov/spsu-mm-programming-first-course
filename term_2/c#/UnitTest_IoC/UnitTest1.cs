using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using Unity.Injection;
using PuntoBanco;
using Moq;

namespace UnitTest_IoC
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethodIo_C()
        {
            SomeBet bet;
            bet.man = 0;
            bet.money = 0;
            bet.target = 1;
            int Rounds = 401;
            int callsBet = 0;
            int callsInput = 0;
            int callsReady = 0;
            
            var mock = new Mock<Iinteraction>();
            mock.Setup(x => x.ready()).Returns(() =>
            {
                callsReady++;
                if (callsReady != Rounds + 1)
                {
                    Console.WriteLine("test 1");
                    return true;
                }
                Console.WriteLine("test 0");
                return false;
            });
            mock.Setup(x => x.getInt()).Returns(() =>
            {
                callsInput++;
                if (callsInput == 1)
                {
                    Console.WriteLine("test 25");
                    return 25;
                }
                Console.WriteLine("test 1");
                return 1;
            });
            
            mock.Setup(x => x.doBet(It.IsAny<SomeBet>(), It.IsAny<int>())).Returns(() =>
            {
                callsBet++;
                Console.WriteLine($"test: money{bet.money}");
                return bet;
            });
            //
            IUnityContainer container = new UnityContainer();
            container.RegisterInstance(mock.Object);
            container.RegisterType<UserInterface>("User", new InjectionConstructor(typeof(Iinteraction)));
            UserInterface user = container.Resolve<UserInterface>("User");
            user.goGame();
        }
    }
}
