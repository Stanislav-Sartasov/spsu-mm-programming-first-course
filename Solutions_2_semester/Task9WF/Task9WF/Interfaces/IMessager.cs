using System.Net.Sockets;

namespace Task9WF.Interfaces
{
    public interface IMessager
    {
        bool SendMessage(Socket socket, MessageType type, string message);
        Message ReceiveMassege(Socket socket);
    }
}
