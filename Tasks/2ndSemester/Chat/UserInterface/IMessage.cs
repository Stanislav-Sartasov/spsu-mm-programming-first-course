using System.Net;
using System.Collections.Generic;

namespace UserInterface
{
    public interface IMessage
    {
        void ConnectMessage(EndPoint connectedUserEndPoint);
        void ConnectAnswerMessage(List<EndPoint> connectedUsers);
        void DisconnectMessage(EndPoint disconnectedUserEndPoint);
        void WriteMessage(string message);
    }
}
