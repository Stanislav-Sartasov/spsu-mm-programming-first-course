using System;
using InterfacesDescription;

namespace SecondPlugin
{
	public class PluginDesription : IPlugin
	{
		public int Operation(int a, int b)
		{
			return Math.Max(a, b);
		}
	}
}