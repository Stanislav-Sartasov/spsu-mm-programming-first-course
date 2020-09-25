using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChat
{
    public interface IInteraction
    {
        string GetStr();
        string GetIp();
        int GetPort();
        void Show(string str);
        void SystemShow(string str);
        void SystemShow(int code, string str);
        void ShowSender(RecivedMessage re);
        void ShowMessage(RecivedMessage re);
    }
}
