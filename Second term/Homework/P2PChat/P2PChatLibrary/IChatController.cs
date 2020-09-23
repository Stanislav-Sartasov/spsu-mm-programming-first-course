using System.Net;


namespace P2PChatLibrary
{
    public interface IChatController
    {
        string GetUsername();
        int GetLocalPort();
        string GetMessage();
        IPEndPoint GetEndPoint();
    }
}
