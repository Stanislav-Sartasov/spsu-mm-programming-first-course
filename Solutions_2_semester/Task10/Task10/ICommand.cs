
namespace Task10
{
	interface ICommand
	{
		public string Process(string input, out CommandHandler.Keys key);
	}
}
