using UserInterface;

namespace Users
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            IUser user = new User();
            user.Start();
        }
    }
}
