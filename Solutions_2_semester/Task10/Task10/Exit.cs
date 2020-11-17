
namespace Task10
{
	class Exit : ICommand
	{
		public string Process(string input, out CommandHandler.Keys key)
		{
			key = CommandHandler.Keys.Exit;
			return null;
		}
	}
}
