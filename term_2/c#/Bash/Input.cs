using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bash
{
    enum Status
    {
        arg,
        cmd,
        vari,
        value,
        undef
    }
    internal enum Interup
    {
        queue,
        inProcess,
        failed,
    }
    public interface IInput
    {
        List<Message> GetLine();
    }
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
            Status flag = Status.undef;
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
                        if ((!needStick) && ((flag == Status.cmd) || (flag == Status.undef)))
                        {
                            list.Add(new Command("exit"));
                            flag = Status.arg;
                            needStick = true;
                        }
                        else
                        {
                            list.Clear();
                            list.Add(new Command("exit", Interup.failed));
                            break;
                        }
                        continue;
                    case "echo":
                        if ((!needStick) && ((flag == Status.cmd) || (flag == Status.undef)))
                        {
                            list.Add(new Command("echo"));
                            flag = Status.arg;
                            needStick = true;
                        }
                        else
                        {
                            list.Clear();
                            list.Add(new Command("exit", Interup.failed));
                            break;
                        }
                        continue;
                    case "pwd":
                        if ((!needStick) && ((flag == Status.cmd) || (flag == Status.undef)))
                        {
                            list.Add(new Command("pwd"));
                            needStick = true;
                        }
                        else
                        {
                            list.Clear();
                            list.Add(new Command("exit", Interup.failed));
                            break;
                        }
                        continue;
                    case "cat":
                        if ((!needStick) && ((flag == Status.cmd) || (flag == Status.undef)))
                        {
                            list.Add(new Command("cat"));
                            flag = Status.arg;
                            needStick = true;
                        }
                        else
                        {
                            list.Clear();
                            list.Add(new Command("exit", Interup.failed));
                            break;
                        }
                        continue;
                    case "wc":
                        if ((!needStick) && ((flag == Status.cmd) || (flag == Status.undef)))
                        {
                            list.Add(new Command("wc"));
                            flag = Status.arg;
                            needStick = true;
                        }
                        else
                        {
                            list.Clear();
                            list.Add(new Command("exit", Interup.failed));
                            break;
                        }
                        continue;
                    case "|":
                        if (needStick)
                        {
                            list.Add(new Command("|"));
                            flag = Status.cmd;
                            needStick = false;
                        }
                        else
                        {
                            list.Clear();
                            list.Add(new Command("exit", Interup.failed));
                            break;
                        }
                        continue;
                }
                if ((token != "") && (token[0] == '$') && ((flag == Status.arg) || (flag == Status.undef)))
                {
                    list.Add(new Vari(token, Status.vari));
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
                                list.Add(new Command("exit", Interup.failed));
                                break;
                            }
                            list.Add(new Vari(dataTokens[i], Status.value));
                        }
                        else
                            --i;
                    }
                    flag = Status.undef;
                    continue;
                }
                if ((flag == Status.arg) || (flag == Status.undef))
                {
                    list.Add(new Arg(token));
                    flag = Status.undef;
                }
                gotVari = false;
            }
            return list;
        }
    }

    public abstract class Message
    {
        internal Status st;
        internal Interup interup;
    }
    public class Vari : Message
    {
        public readonly string vari;
        internal Vari(string start, Status myStatus)
        {
            interup = Interup.queue;
            vari = start;
            st = myStatus;
        }
    }
    public class Arg : Message
    {
        public readonly string arg;
        public Arg(string start)
        {
            interup = Interup.queue;
            arg = start;
            st = Status.arg;
        }
    }
    
}
