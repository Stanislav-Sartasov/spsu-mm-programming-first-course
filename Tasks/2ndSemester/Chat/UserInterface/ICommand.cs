using System.Net;

namespace UserInterface
{
    public interface ICommand
    {
        public void Connect(EndPoint ipForConnection);
        public void SendMessage(string message);
        public void Disconnect();
        public void Exit();
    }
}
