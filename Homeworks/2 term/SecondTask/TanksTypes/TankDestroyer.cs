using System;
using TankTemplateDescription;

namespace TankDestroyerDescription
{
	public class TankDestroyer : Tank
	{
		public int Height { get; set; }
		public int TowerRotationAngles { get; set; }
		public TankDestroyer(string name, string country, int year, int caliber, int height, int degrees) : base(name, country, year, caliber)
		{
			Height = height;
			TowerRotationAngles = degrees;
		}
		public override string GetData()
		{
			string output = base.GetData() + $"type: tank destroyer;\nSpecific characteristics: height {Height} mm, tower rotation angles: {TowerRotationAngles}°.\n";
			Console.WriteLine($"type: tank destroyer;\nSpecific characteristics: height {Height} mm, tower rotation angles: {TowerRotationAngles}°.\n");
			return output;
		}
	}
}