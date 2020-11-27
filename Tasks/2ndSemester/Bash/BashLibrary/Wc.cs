using System.IO;
using System.Linq;

namespace BashLibrary
{
    public class Wc : ICommand
    {
        public string Execute(string input)
        {
            try
            {
                return $"{File.ReadLines(input).Count()} lines, {File.ReadAllText(input).Split(new char[] { ' ', '\n' }).Count()} words, {File.ReadAllBytes(input).Count()} bytes.\n";
            }
            catch
            {
                return "Error. Try again.\n";
            }
        }
    }
}
