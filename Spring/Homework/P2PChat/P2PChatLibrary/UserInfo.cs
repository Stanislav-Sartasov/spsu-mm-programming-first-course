namespace P2PChatLibrary
{
    public class UserInfo
    {
        public UserInfo(string name, string address, int port)
        {
            Name = name;
            Address = address;
            Port = port;
        }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is UserInfo))
                return false;
            return Equals((UserInfo)obj);
        }

        public bool Equals(UserInfo other)
        {
            if (Address != other.Address)
                return false;

            return Port == other.Port;
        }
    }
}
