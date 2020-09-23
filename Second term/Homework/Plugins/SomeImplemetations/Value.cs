
using SomeInterface;

namespace SomeImplemetations
{
    public class IntValue : ISomeInterface<int>
    {
        private int value = default;
        private string info = "IntValue class";
        public int Get()
        {
            return value;
        }

        public void Set(int value)
        {
            this.value = value;
        }

        public string GetInfo()
        {
            return info;
        }
    }

    public class StringValue : ISomeInterface<string>
    {
        string value = default;
        private string info = "StringValue class";
        public string Get()
        {
            return value;
        }

        public void Set(string value)
        {
            this.value = value;
        }

        public string GetInfo()
        {
            return info;
        }
    }
}
