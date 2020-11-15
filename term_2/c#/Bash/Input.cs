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
                            list.Add(new Command("exit"));
                            flag = Status.Arg;
                            needStick = true;
                        }
                        else
                        {
                            list.Clear();
                            list.Add(new Command("exit", Interup.Failed));
                            break;
                        }
                        continue;
                    case "echo":
                        if ((!needStick) && ((flag == Status.Cmd) || (flag == Status.Undef)))
                        {
                            list.Add(new Command("echo"));
                            flag = Status.Arg;
                            needStick = true;
                        }
                        else
                        {
                            list.Clear();
                            list.Add(new Command("exit", Interup.Failed));
                            break;
                        }
                        continue;
                    case "pwd":
                        if ((!needStick) && ((flag == Status.Cmd) || (flag == Status.Undef)))
                        {
                            list.Add(new Command("pwd"));
                            needStick = true;
                        }
                        else
                        {
                            list.Clear();
                            list.Add(new Command("exit", Interup.Failed));
                            break;
                        }
                        continue;
                    case "cat":
                        if ((!needStick) && ((flag == Status.Cmd) || (flag == Status.Undef)))
                        {
                            list.Add(new Command("cat"));
                            flag = Status.Arg;
                            needStick = true;
                        }
                        else
                        {
                            list.Clear();
                            list.Add(new Command("exit", Interup.Failed));
                            break;
                        }
                        continue;
                    case "wc":
                        if ((!needStick) && ((flag == Status.Cmd) || (flag == Status.Undef)))
                        {
                            list.Add(new Command("wc"));
                            flag = Status.Arg;
                            needStick = true;
                        }
                        else
                        {
                            list.Clear();
                            list.Add(new Command("exit", Interup.Failed));
                            break;
                        }
                        continue;
                    case "|":
                        if (needStick)
                        {
                            list.Add(new Command("|"));
                            flag = Status.Cmd;
                            needStick = false;
                        }
                        else
                        {
                            list.Clear();
                            list.Add(new Command("exit", Interup.Failed));
                            break;
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
                                list.Add(new Command("exit", Interup.Failed));
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
