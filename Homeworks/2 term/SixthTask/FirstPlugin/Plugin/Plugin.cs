using System;
using InterfacesDescription;

namespace FirstPlugin
{
	public class PluginDesription : IPlugin
	{
		public int Operation(int a, int b)
		{
			return Math.Min(a, b);
		}
	}
}