using System.Net;

namespace Task9WF
{
    public struct Message
    {
        public MessageType Type { get; set; }
        public string Text { get; set; }
        public EndPoint GetAddress()
        {
            if (Type != MessageType.Socket || Text == null)
                return null;
            try
            {
                IPAddress ipAddress = IPAddress.Parse(Text.Substring(0, Text.LastIndexOf(':')));
                int port = int.Parse(Text.Substring(Text.LastIndexOf(':') + 1));
                return new IPEndPoint(ipAddress, port);
            }
            catch
            {
                return null;
            }
        }
    }
}
