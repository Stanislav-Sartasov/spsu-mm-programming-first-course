using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bash
{
    internal class Input : IInput
    {
        private readonly IInteraction inter;
        public Input()
        {
            inter = new Interaction();
        }
        public Input(IInteraction start)
        {
            inter = start;
        }
        public List<Message> GetLine()
        {
            bool gotVari = false, needStick = false;
            Status flag = Status.Undef;
            string data = inter.GetStr();
            string[] dataTokens = data.Split(' ');
            List<Message> list = new List<Message>();
           // foreach (string token in dataTokens)
            for (int i = 0; i < (int)dataTokens.Length; ++i)
            {
                    var token = dataTokens[i];
                switch(token)
                {
                    case "exit" :
                        if ((!needStick) && ((flag == Status.Cmd) || (flag == Status.Undef)))
                        {
                            list.Add(new Command("exit", false));
                            flag = Status.Arg;
                            needStick = true;
                        }
                        else
                        {
                            list.Clear();
                            list.Add(new Command("exit", Interup.Failed, false));
                            //break;
                        }
                        continue;
                    case "echo":
                        if ((!needStick) && ((flag == Status.Cmd) || (flag == Status.Undef)))
                        {
                            list.Add(new Command("echo", true));
                            flag = Status.Arg;
                            needStick = true;
                        }
                        else
                        {
                            list.Clear();
                            list.Add(new Command("exit", Interup.Failed, false));
                            //break;
                        }
                        continue;
                    case "pwd":
                        if ((!needStick) && ((flag == Status.Cmd) || (flag == Status.Undef)))
                        {
                            list.Add(new Command("pwd", false));
                            needStick = true;
                        }
                        else
                        {
                            list.Clear();
                            list.Add(new Command("exit", Interup.Failed, false));
                            //break;
                        }
                        continue;
                    case "cat":
                        if ((!needStick) && ((flag == Status.Cmd) || (flag == Status.Undef)))
                        {
                            list.Add(new Command("cat", true));
                            flag = Status.Arg;
                            needStick = true;
                        }
                        else
                        {
                            list.Clear();
                            list.Add(new Command("exit", Interup.Failed, false));
                            //break;
                        }
                        continue;
                    case "wc":
                        if ((!needStick) && ((flag == Status.Cmd) || (flag == Status.Undef)))
                        {
                            list.Add(new Command("wc", true));
                            flag = Status.Arg;
                            needStick = true;
                        }
                        else
                        {
                            list.Clear();
                            list.Add(new Command("exit", Interup.Failed, false));
                            //break;
                        }
                        continue;
                    case "|":
                        if ((needStick) || (flag == Status.Undef))
                        {
                            list.Add(new Command("|", false));
                            flag = Status.Cmd;
                            needStick = false;
                        }
                        else
                        {
                            list.Clear();
                            list.Add(new Command("exit", Interup.Failed, false));
                            //break;
                        }
                        continue;
                }
                if ((token != "") && (token[0] == '$') && ((flag == Status.Arg) || (flag == Status.Undef)))
                {
                    list.Add(new Vari(token, Status.Vari));
                    //gotVari = true;
                    if (i + 1 < dataTokens.Length)
                    {
                        ++i;
                        if (dataTokens[i] == "=")
                        {
                            if (i + 1 < dataTokens.Length)
                                ++i;
                            else
                            {
                                list.Clear();
                                list.Add(new Command("exit", Interup.Failed, false));
                                break;
                            }
                            list.Add(new Vari(dataTokens[i], Status.Value));
                        }
                        else
                            --i;
                    }
                    flag = Status.Undef;
                    continue;
                }
                if ((flag == Status.Arg) || (flag == Status.Undef))
                {
                    list.Add(new Arg(token));
                    flag = Status.Undef;
                }
                gotVari = false;
            }
            return list;
        }
    }
    
}
