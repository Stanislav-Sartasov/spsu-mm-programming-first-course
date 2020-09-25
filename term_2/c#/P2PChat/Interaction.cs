using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChat
{
    class Interaction : IInteraction
    {
        public string GetStr()
        {
            return Console.ReadLine();
        }
        public string GetIp()
        {
            string str;
            bool flag;
            do
            {
                Show("Input ip: ");
                str = GetStr();
                string[] ip = str.Split('.');
                int i;
                flag = true;
                if (ip.Length == 4)
                {

                    foreach (string tmp in ip)
                    {
                        bool b = Int32.TryParse(tmp, out i);
                        if (b)
                        {
                            if (!((0 <= i) && (255 >= i)))
                            {
                                flag = false;
                                break;
                            }
                        }
                        else
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                else
                    flag = false;
                if (!flag)
                    Show("ip is 4 numbers from 0 up to 255. Example: 127.0.0.1\n");
            } while (!flag);
            return str;
        }
        public int GetPort()
        {
            string str;
            int port;
            bool flag;
            do
            {
                Show("Input port: ");
                str = GetStr();
                flag = Int32.TryParse(str, out port);
                if (flag)
                    if (port < 0)
                        flag = false;
                if (!flag)
                    Show("port is integer number > 0. Example: 8811\n");
            } while (!flag);
            return port;
        }
        public void Show(string str)
        {
            Console.Write(str);
        }
        public void SystemShow(string str)
        {
           // Console.WriteLine("<System>" + str + "<System>");
        }
        public void SystemShow(int code, string str)
        {
            Console.WriteLine("<System>" + str + "<System>");
        }
        public void ShowSender(RecivedMessage re)
        {
            string[] strs = re.message.Split('/');
            if (strs.Length >= 4)
            {
                string outStr = "<" + strs[1] + ":" + strs[2] + ">";
                Console.Write(outStr + ": ");
            }
            else
            {
                Console.Write($"<??{re.ip}:{re.port}??>: ");
            }
        }
        public void ShowMessage(RecivedMessage re)
        {
            string[] strs = re.message.Split('/');
            if (strs.Length >= 4)
            {
                string outStr = "";
                for (int i = 3; i < strs.Length; ++i)
                {
                    outStr = outStr + strs[i];
                }
                Console.WriteLine(outStr);
            }
            else
            {
                Console.WriteLine("<" + re.message + "!not standart packet!");
            }
        }
    }
}
