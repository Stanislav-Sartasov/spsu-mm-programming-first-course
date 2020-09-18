using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Bash
{
    internal class VariStruct
    {
        public string name;
        public string value;
    }
    public interface IEngine
    {
        void InitInput(List<Message> start);
        bool StartCommand();
    }
    class Engine : IEngine
    {

        private List<Message> list;
        private string lastResult;
        private int len;
        private bool stop;
        private List<VariStruct> varies;
        public Engine ()
        {
            varies = new List<VariStruct>();
            varies.Clear();
        }
        public void InitInput (List<Message> start)
        {
            list = start;
            len = list.Count - 1;
        }
        private void CmdSolver(Command cmd, ref int i)
        {
            if (cmd.cmd == "pwd")
            {
                cmd.interup = Interup.inProcess;
                try
                {
                    lastResult = Directory.GetCurrentDirectory();
                    //Console.WriteLine(Directory.GetCurrentDirectory());
                }
                catch (Exception e)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }
            }
            if (cmd.cmd == "cat")
            {
                cmd.interup = Interup.inProcess;
                string path;
                if (i + 1 > len)
                    path = ArgSolver(list[i], ref i);
                else
                {
                    ++i;
                    path = ArgSolver(list[i], ref i);
                }
                try
                {
                    StreamReader sr = new StreamReader(path);
                    //Console.WriteLine(sr.ReadToEnd());
                    lastResult = sr.ReadToEnd();
                    sr.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }
            }
            if (cmd.cmd == "wc")
            {
                cmd.interup = Interup.inProcess;
                string path;
                if (i + 1 > len)
                    path = ArgSolver(list[i], ref i);
                else
                {
                    ++i;
                    path = ArgSolver(list[i], ref i);
                }
                try
                {
                    long words = 0, lines = 0, bytes = 0;
                    StreamReader sr = new StreamReader(path);
                    while (sr.EndOfStream == false)
                    {
                        string temp = sr.ReadLine();
                        ++lines;
                        words += temp.Split(' ').Length;
                    }
                    bytes = (long)(new FileInfo(path).Length);

                    lastResult = "Bytes: " + bytes + ", words: " + words + ", lines: " + lines;
                    //Console.WriteLine("Bytes: {0}, words: {1}, lines: {2}", bytes, words, lines);
                    sr.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }
            }
            if (cmd.cmd == "echo")
            {
                
                cmd.interup = Interup.inProcess;
                string args;
                if (i + 1 > len)
                    args = ArgSolver(list[i], ref i);
                else
                {
                    ++i;
                    args = ArgSolver(list[i], ref i);
                }
                //Console.WriteLine(args);
                lastResult = args;
            }
            if (cmd.cmd == "exit")
            {
                if (cmd.interup == Interup.failed)
                {
                    lastResult = "Failed Command";
                    return;
                }
                Console.WriteLine("End of Session");
                lastResult = "End of Session";
                varies.Clear();
                stop = true;
            }
        }
        private string ArgSolver(Message preArg, ref int i)
        {
            string result;
            if ((i + 1 > len) && (i - 1 >= 0) && (list[i - 1].st == Status.cmd))
                if (((Command)list[i - 1]).cmd == "|")
                    return lastResult;
            if ((i - 2 >= 0) && (list[i - 2].st == Status.cmd))
                if (((Command)list[i - 2]).cmd == "|")
                    return lastResult;
            if (preArg.st == Status.cmd)
            {
                preArg.interup = Interup.failed;
                return "";
            }
            if (preArg.st == Status.arg)
            {
                Arg arg = (Arg)preArg;
                result = arg.arg;
             //   Console.WriteLine("////" + result + "////");
            }
            else
            {
                Vari vari = (Vari)preArg;
                result = VariSolver(vari, ref i);
            }
            if ((i + 1 <= len) && (list[i+1].st != Status.cmd))
            {
                ++i;
                result = result + ArgSolver(list[i], ref i);
            }
            return result;
        }
        private string VariSolver(Vari vari, ref int i)
        {
            VariStruct tmp = new VariStruct();
            tmp.name = vari.vari;
            tmp.value = "";
            int index = -1;
            foreach (var temp in varies)
            {
                if (temp.name == tmp.name)
                {
                    index = varies.IndexOf(temp);
                    break;
                }
             //   Console.WriteLine("ds");
            }
            if (index == -1)
            {
                varies.Add(tmp);
                index = varies.IndexOf(tmp);
            }
            if ((i + 1 <= len) && (list[i + 1].st == Status.value))
            {
                ++i;
                varies[index].value = ((Vari)list[i]).vari;
            }
            return varies[index].value;

        }
        public bool StartCommand()
        {
            stop = false;
            //foreach(Message cmd in list)
            for (int i = 0; i <= len; ++i)
            {
                Message cmd = list[i];
                //Console.WriteLine(cmd);
                if (cmd.st == Status.cmd)
                {
                    CmdSolver((Command)cmd, ref i);
                }
                else
                    if (cmd.st == Status.vari)
                    {
                    VariSolver((Vari)cmd, ref i);
                    }
                if (stop)
                {
                    varies.Clear();
                    return true;
                }
            }
            Console.WriteLine(lastResult);
            lastResult = "";
            return false;
        }

    }
}
