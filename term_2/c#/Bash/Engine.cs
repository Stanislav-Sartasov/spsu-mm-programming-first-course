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
        private bool nonCmdOnly;
        private int len;
        private bool stop;
        private List<VariStruct> varies;
        private Dictionary<string, Function> commands;
        public Engine ()
        {
            varies = new List<VariStruct>();
            varies.Clear();
            commands = new Dictionary<string, Function>()
            {
                {"pwd", new Pwd()},
                {"cat", new Cat()},
                {"wc", new Wc()},
                {"echo", new Echo()},
            };
            nonCmdOnly = false;
        }
        private void Exit (Command cmd, ref int i)
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
        public void InitInput (List<Message> start)
        {
            list = start;
            len = list.Count - 1;
        }
        private void ExecuteCmd (Command cmd, ref int i)
        {
            string arg = "";
            if (cmd.NeedArg)
            {
                if (i + 1 > len)
                    arg = ArgSolver(list[i], ref i);
                else
                {
                    ++i;
                    arg = ArgSolver(list[i], ref i);
                }
            }
            cmd.interup = Interup.InProcess;

            if (commands.ContainsKey(cmd.Cmd))
                lastResult = commands[cmd.Cmd].GoFunc(arg, cmd, ref i);
            nonCmdOnly = false;
            if (cmd.Cmd == "exit")
                exit(cmd, ref i);

        }
        private void NonCmd (Message cmd, ref int i)
        {
            try
            {
                nonCmdOnly = true;
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
        private string ArgSolver (Message preArg, ref int i)
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
        private string VariSolver (Vari vari, ref int i)
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
        public bool StartCommand ()
        {
            stop = false;
            for (int i = 0; i <= len; ++i)
            {
                Message cmd = list[i];
                //Console.WriteLine(cmd);
                if (cmd.st == Status.Cmd)
                {
                    ExecuteCmd((Command)cmd, ref i);
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
            if (!nonCmdOnly)
                Console.WriteLine(lastResult);
            lastResult = "";
            return false;
        }

    }
}
