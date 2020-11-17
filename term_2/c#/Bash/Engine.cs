using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Bash
{
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
            if (cmd.Cmd == "pwd")
            {
                cmd.interup = Interup.InProcess;
                try
                {
                    lastResult = Directory.GetCurrentDirectory();
                    //Console.WriteLine(Directory.GetCurrentDirectory());
                }
                catch (Exception e)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }
                return;
            }
            if (cmd.Cmd == "cat")
            {
                cmd.interup = Interup.InProcess;
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
                    lastResult = "";
                    StreamReader sr = new StreamReader(path);
                    //Console.WriteLine(sr.ReadToEnd());
                    lastResult = sr.ReadToEnd();
                    sr.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }
                return;
            }
            if (cmd.Cmd == "wc")
            {
                cmd.interup = Interup.InProcess;
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
                    lastResult = "";
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
                return;
            }
            if (cmd.Cmd == "echo")
            {
                
                cmd.interup = Interup.InProcess;
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
                return;
            }
            if (cmd.Cmd == "exit")
            {
                if (cmd.interup == Interup.Failed)
                {
                    lastResult = "Failed Command";
                    list.Clear();
                    i = len + 1;
                    return;
                }
                Console.WriteLine("End of Session");
                lastResult = "End of Session";
                varies.Clear();
                stop = true;
                return;
            }

        }
        private void NonCmd(Message cmd, ref int i)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = cmd.Cmd;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                lastResult = "";
                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine();
                    Console.WriteLine(line);
                    lastResult += line;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{cmd.Cmd} does not exist in Path");
            }
        }
        private string ArgSolver(Message preArg, ref int i)
        {
            string result;
            if ((i + 1 > len) && (i - 1 >= 0) && (list[i - 1].st == Status.Cmd))
                if (((Command)list[i - 1]).Cmd == "|")
                    return lastResult;
            if ((i - 2 >= 0) && (list[i - 2].st == Status.Cmd))
                if (((Command)list[i - 2]).Cmd == "|")
                    return lastResult;
            if (preArg.st == Status.Cmd)
            {
                preArg.interup = Interup.Failed;
                return "";
            }
            if (preArg.st == Status.Arg)
            {
                Arg arg = (Arg)preArg;
                result = arg.Cmd;
             //   Console.WriteLine("////" + result + "////");
            }
            else
            {
                Vari vari = (Vari)preArg;
                result = VariSolver(vari, ref i);
            }
            if ((i + 1 <= len) && (list[i+1].st != Status.Cmd))
            {
                ++i;
                result = result + ArgSolver(list[i], ref i);
            }
            return result;
        }
        private string VariSolver(Vari vari, ref int i)
        {
            VariStruct tmp = new VariStruct();
            tmp.Name = vari.Cmd;
            tmp.Value = "";
            int index = -1;
            foreach (var temp in varies)
            {
                if (temp.Name == tmp.Name)
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
            if ((i + 1 <= len) && (list[i + 1].st == Status.Value))
            {
                ++i;
                varies[index].Value = ((Vari)list[i]).Cmd;
            }
            return varies[index].Value;

        }
        public bool StartCommand()
        {
            stop = false;
            for (int i = 0; i <= len; ++i)
            {
                Message cmd = list[i];
                //Console.WriteLine(cmd);
                if (cmd.st == Status.Cmd)
                {
                    CmdSolver((Command)cmd, ref i);
                }
                else
                    if (cmd.st == Status.Vari)
                    {
                    VariSolver((Vari)cmd, ref i);
                    }
                if (cmd.st == Status.Arg)
                {
                    NonCmd(cmd, ref i);
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
