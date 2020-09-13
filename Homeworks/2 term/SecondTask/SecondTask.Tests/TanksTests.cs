using Microsoft.VisualStudio.TestTools.UnitTesting;
using HeavyTankDescription;
using LightTankDescription;
using TankDestroyerDescription;

namespace SecondTask.Tests
{
	[TestClass]
	public class TanksTests
	{
		[TestMethod]
		public void TestMethodHeavy()
		{
			HeavyTank Maus = new HeavyTank("Maus", "Germany", 1944, 128, 200, 260);

			Assert.AreEqual("Maus", Maus.Name);
			Assert.AreEqual("Germany", Maus.Country);
			Assert.AreEqual(1944, Maus.ProductionYear);
			Assert.AreEqual(128, Maus.GunCaliber);
			Assert.AreEqual(200, Maus.FrontalHullArmor);
			Assert.AreEqual(260, Maus.FrontalTurretArmor);

			Assert.AreEqual("Tank description:\nName: Maus, country: Germany, production year: 1944, gun caliber: 128 mm, type: heavy tank;\nSpecific characteristics: frontal hull armor: 200 mm, frontal turret armor: 260 mm.\n", Maus.GetData());
		}

		[TestMethod]
		public void TestMethodLight()
		{
			LightTank LTTB = new LightTank("LTTB", "USSR", 1944, 85, 68, 33.95);

			Assert.AreEqual("LTTB", LTTB.Name);
			Assert.AreEqual("USSR", LTTB.Country);
			Assert.AreEqual(1944, LTTB.ProductionYear);
			Assert.AreEqual(85, LTTB.GunCaliber);
			Assert.AreEqual(68, LTTB.MaxSpeed);
			Assert.AreEqual(33.95, LTTB.SpecificPower);

			Assert.AreEqual("Tank description:\nName: LTTB, country: USSR, production year: 1944, gun caliber: 85 mm, type: light tank;\nSpecific characteristics: max speed: 68 km/h, specific power: 33,95 hp/t.\n", LTTB.GetData());
		}

		[TestMethod]
		public void TestMethodDestroyer()
		{
			TankDestroyer Charioteer = new TankDestroyer("Charioteer", "UK", 1952, 84, 2590, 360);

			Assert.AreEqual("Charioteer", Charioteer.Name);
			Assert.AreEqual("UK", Charioteer.Country);
			Assert.AreEqual(1952, Charioteer.ProductionYear);
			Assert.AreEqual(84, Charioteer.GunCaliber);
			Assert.AreEqual(2590, Charioteer.Height);
			Assert.AreEqual(360, Charioteer.TowerRotationAngles);

			Assert.AreEqual("Tank description:\nName: Charioteer, country: UK, production year: 1952, gun caliber: 84 mm, type: tank destroyer;\nSpecific characteristics: height 2590 mm, tower rotation angles: 360°.\n", Charioteer.GetData());
		}
	}
}