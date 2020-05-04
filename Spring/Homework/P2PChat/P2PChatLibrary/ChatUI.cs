using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace P2PChatLibrary
{
    public class ChatUI
    {
        private Peer peer;
        private IChatController chatController;
        private Action<string> Output;
        private bool IsWorking;

        public ChatUI()
            : this (new ConsoleChatController(), s => Console.WriteLine(s))
        {
        }

        public ChatUI(IChatController chatController) 
            : this(chatController, s => Console.WriteLine(s))
        {
        }

        public ChatUI(IChatController chatController, Action<string> output)
        {
            this.chatController = chatController;
            Output = output;
            IsWorking = true;
        }

        public UserInfo GetLocalUserInfo()
        {
            return peer.LocalUser;
        }

        public List<UserInfo> GetConnectedUsers() => peer.GetConnectedUsers();

        public void Start()
        {
            IsWorking = true;
            Console.ForegroundColor = ConsoleColor.White;
            Output("=====Console chat=====");
            Console.ResetColor();
            UserInit();
            peer.StartListening();
            Output(GetHelp());
            while (IsWorking)
            {
                string input = chatController.GetMessage();
                if (input != null && input != "")
                {
                    bool isCommand = CheckCommand(input);
                    if (isCommand)
                    {
                        input = input.Remove(0, 1);
                        ApplyCommand(input);
                    }
                    else
                    {
                        var message = new Message() { SenderName = peer.LocalUser.Name, MessageData = input };
                        var messageInfo = new MessageInfo()
                        {
                            MesType = MessageType.Message,
                            Message = JsonConvert.SerializeObject(message)
                        };
                        var serialised = JsonConvert.SerializeObject(messageInfo);
                        peer.SendAll(Encoding.Unicode.GetBytes(serialised));
                    }
                }
            }
        }

        private bool CheckCommand(string input)
        {
            var commands = new string[] { "connect", "disconnect", "help", "exit", "back", "users"};
            foreach (var command in commands)
            {
                if (input.Equals($"/{command}"))
                {
                    return true;
                }
            }
            return false;
        }

        public  void UserInit()
        {
            Output("Enter your nickname: ");
            string name = chatController.GetUsername();
            while (true)
            {
                try
                {
                    peer = new Peer(chatController.GetLocalPort(), name, Output);
                    break;
                }
                catch
                {
                    Output("This port is busy! Try again!");
                }
            }
        }

        private string GetHelp()
        {
            return "\n/connect\tconnect to chat room\n"
                + "/disconnect\tleave the chat room\n"
                + "/exit\t\texit the application\n"
                + "/users\t\tshows a list of users\n"
                + "/help\t\tshows a list of commands\n"
                + "/back\t\tterminates the current command\n";
        }

        private void ApplyCommand(string command)
        {
            switch (command)
            {
                case "connect":
                    IPEndPoint endPoint = chatController.GetEndPoint();
                    if (endPoint != null)
                        peer.Connect(new UserInfo("<unknown>", endPoint.Address.ToString(), endPoint.Port));
                    break;
                case "disconnect":
                    peer.Disconnect();
                    break;
                case "users":
                    peer.PrintUsers();
                    break;
                case "help":
                    Output(GetHelp());
                    break;
                case "exit":
                    peer.Exit();
                    IsWorking = false;
                    break;
                default:
                    Output("Something went wrong :(");
                    break;
            }
        }
    }
}
