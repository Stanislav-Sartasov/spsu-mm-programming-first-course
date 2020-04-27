
namespace Task3HashTable
{
    class Program
    {
        static void Main(string[] args)
        {
            Map<string, string> map = new Map<string, string>();

            map.Add("SomeKey", "123Value");
            map.Search("SomeKey");
            map.Delete("SomeKey");
            map.Search("SomeKey");



        }
    }
}
