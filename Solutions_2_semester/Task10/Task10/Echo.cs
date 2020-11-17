
namespace Task10
{
	class Echo : ICommand
	{
		public string Process(string input, out CommandHandler.Keys key)
		{
			key = CommandHandler.Keys.Ok;
			return input;
		}
	}
}
