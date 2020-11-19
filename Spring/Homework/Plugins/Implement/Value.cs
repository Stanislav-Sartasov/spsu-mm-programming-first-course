using Interface;

namespace SomeImplemetations
{
	public class IntType : Interface<int>
	{
		private int type = default;
		private string info = "Int class";
		public int Get()
		{
			return type;
		}

		public void Set(int type)
		{
			this.type = type;
		}

		public string GetInfo()
		{
			return info;
		}
	}

	public class StringType : Interface<string>
	{
		string type = default;
		private string info = "String class";
		public string Get()
		{
			return type;
		}

		public void Set(string value)
		{
			this.type = value;
		}

		public string GetInfo()
		{
			return info;
		}
	}
}
