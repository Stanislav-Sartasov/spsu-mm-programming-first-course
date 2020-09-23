namespace P2PChatLibrary
{
    public enum MessageType
    {
        ConnectionRequest,
        Message,
        UsersAdding,
        Disconnection
    }
    public class MessageInfo
    {
        public MessageType MesType { get; set; }
        public string Message { get; set; }
    }
}
