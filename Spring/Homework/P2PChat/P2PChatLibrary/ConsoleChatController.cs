using System;
using System.Net;
using System.Text.RegularExpressions;

namespace P2PChatLibrary
{
    public class ConsoleChatController : IChatController
    {
        public IPEndPoint GetEndPoint()
        {
            bool isIncorrect = true;
            string address;
            IPEndPoint endPoint = null;
            while (isIncorrect)
            {
                Console.WriteLine("Enter remote IP-address and port: [ip]:[port]");
                address = Console.ReadLine();
                string pattern = @"^\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}:\d{1,5}";
                if (address.Equals("/back"))
                    break;
                if (Regex.IsMatch(address, pattern, RegexOptions.IgnoreCase))
                {
                    var substr = address.Split('.', ':');
                    int.TryParse(substr[4], out int res);
                    if (res >= 1 && res <= 65535)
                    {
                        isIncorrect = false;
                        for (int i = 0; i < substr.Length - 1; i++)
                        {
                            bool parsed = int.TryParse(substr[i], out res);
                            if (!(res >= 0 && res <= 255) || !parsed)
                                isIncorrect = true;
                        }
                    }
                }
                if (isIncorrect)
                {
                    Console.WriteLine("Incorrect address or format!");
                }
                else
                {
                    var substr = address.Split(':');
                    endPoint = new IPEndPoint(IPAddress.Parse(substr[0]), int.Parse(substr[1]));
                }
            }
            return endPoint;
        }

        public int GetLocalPort()
        {
            int res;
            while (true)
            {
                Console.WriteLine("Enter your port:");
                string port = Console.ReadLine();
                int.TryParse(port, out res);
                if (res >= 1 && res <= 65535)
                    break;
                else
                    Console.WriteLine("Incorrect input!");
            }
            return res;
        }

        public string GetMessage()
        {
            return Console.ReadLine();
        }

        public string GetUsername()
        {
            string name;
            while (true)
            {
                name = Console.ReadLine();
                if (name.Equals("<unknown>"))
                {
                    Console.WriteLine("Incorrect name!");
                }
                else
                    break;
            }
            return name;
        }
    }
}
