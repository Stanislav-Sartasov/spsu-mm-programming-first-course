using System;
using HeavyTankDescription;
using LightTankDescription;
using TankDestroyerDescription;

namespace SecondTask
{
	class Program
	{
		static void Main()
		{
			HeavyTank Maus = new HeavyTank("Maus", "Germany", 1944, 128, 200, 260);
			Maus.GetData();

			LightTank LTTB = new LightTank("LTTB", "USSR", 1944, 85, 68, 33.95);
			LTTB.GetData();

			TankDestroyer Charioteer = new TankDestroyer("Charioteer", "UK", 1952, 84, 2590, 360);
			Charioteer.GetData();
		}
	}
}